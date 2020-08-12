using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class WeaponController : MonoBehaviour
{
    public Light2D _pointLight;
    public Light2D[] _pointLights;
    public float _toAdd;

    [Range(0,360f)]
    public float _minAngle, _maxAngle;

    Camera _camera;
    Vector3 _orientation;
    Vector3 _direction;
    public Transform _parent;
    float _lightAngleRange = 120f;

    public GameObject _projectile;
    public float _firePower;

    public Transform _spawner;

    public int _pelletNumber=3;

    float _spreadAngle;
    bool _canShoot = false;

    public float _reloadTime;
    public int _magazineCapacity = 4;
    public int _maxCapacity = 4;
    public Light2D _magazineLight;
    public float _maxMagazineIntensity;
    public Light2D _emptyMagazineLight;



    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = -_camera.transform.position.z;
        _orientation = _camera.ScreenToWorldPoint(temp);
        _orientation = _orientation - _parent.position;
        _direction = (_camera.ScreenToWorldPoint(temp) - transform.position).normalized;
        float tempAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        if (_orientation.x < 0)
        {
            GameState._isCharacterFlipped = true;
            _parent.transform.localScale = new Vector3(-1, 1, 1);
            //_parent.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            if (_orientation.y < transform.localPosition.y)
                transform.eulerAngles = new Vector3(0, 0, (Mathf.Clamp(tempAngle, -180,-180 + _lightAngleRange/2))-180);
                //transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, -180, -180 + _lightAngleRange/2));
            else
                transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, 180 - _lightAngleRange / 2, 180)-180);
        }
        else
        {

            GameState._isCharacterFlipped = false;

            _parent.transform.localScale = new Vector3(1, 1, 1);
            //_parent.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, - _lightAngleRange / 2, _lightAngleRange / 2));
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if(_magazineCapacity>0)
                _canShoot = true;
            if (_pointLights.Length > 0)
            {
                for (int i = 0; i < _pointLights.Length; i++)
                {
                    _pointLights[i].pointLightInnerAngle = Mathf.Clamp(_pointLights[i].pointLightInnerAngle -= _toAdd, _minAngle, _maxAngle);
                    _pointLights[i].pointLightOuterAngle = Mathf.Clamp(_pointLights[i].pointLightOuterAngle -= _toAdd, _minAngle + 10, _maxAngle);
                }
            }
            else 
            { 
                _pointLight.pointLightInnerAngle = Mathf.Clamp(_pointLight.pointLightInnerAngle -= _toAdd, _minAngle, _maxAngle);
                _pointLight.pointLightOuterAngle = Mathf.Clamp(_pointLight.pointLightOuterAngle -= _toAdd, _minAngle + 10, _maxAngle);
            }
            _spreadAngle = _pointLight.pointLightInnerAngle;
        }
        else
        {
            if (_pointLights.Length > 0)
            {
                for (int i = 0; i < _pointLights.Length; i++)
                {
                    _pointLights[i].pointLightInnerAngle = Mathf.Clamp(_pointLight.pointLightInnerAngle += _toAdd / 3f, _minAngle, _maxAngle);
                    _pointLights[i].pointLightOuterAngle = Mathf.Clamp(_pointLight.pointLightOuterAngle += _toAdd / 3f, _minAngle + 10, _maxAngle);
                }
            }
            else
            {
                _pointLight.pointLightInnerAngle = Mathf.Clamp(_pointLight.pointLightInnerAngle += _toAdd / 3f, _minAngle, _maxAngle);
                _pointLight.pointLightOuterAngle = Mathf.Clamp(_pointLight.pointLightOuterAngle += _toAdd / 3f, _minAngle + 10, _maxAngle);
            }
            _spreadAngle = _pointLight.pointLightInnerAngle;
            if(_pointLight.pointLightInnerAngle == _maxAngle || _magazineCapacity <= 0)
            {
                _canShoot = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canShoot)
        {
            StartCoroutine(ShootWithSpread(_spreadAngle, _pelletNumber, transform.rotation, _spawner.position));
            _magazineCapacity--;
            if (_magazineCapacity <= 0)
            {
                StartCoroutine(Reloading());
            }
            _canShoot = false;
            if (_pointLights.Length > 0)
            {
                for (int i = 0; i < _pointLights.Length; i++)
                {
                    _pointLights[i].pointLightInnerAngle = _maxAngle;
                    _pointLights[i].pointLightOuterAngle = _maxAngle;
                }
            }
            else
            {
                _pointLight.pointLightOuterAngle = _maxAngle;
                _pointLight.pointLightInnerAngle = _maxAngle;
            }
        }
        SetMagazineLight();
    }
    private void OnValidate()
    {
        if (_minAngle > _maxAngle)
            _minAngle = _maxAngle;
    }

    void SetMagazineLight()
    {
        _magazineLight.intensity = (_magazineCapacity/ (float)_maxCapacity)*_maxMagazineIntensity;
        _emptyMagazineLight.intensity = _maxMagazineIntensity - _magazineLight.intensity;
    }

    IEnumerator ShootWithSpread(float spreadAngle, int numberOfPellets, Quaternion transformRotation, Vector3 spawnerPosition)
    {
        for(int i = 0; i < numberOfPellets; i++)
        {
            GameObject pellet = Instantiate(_projectile, spawnerPosition, transformRotation);
            if(GameState._isCharacterFlipped)
                pellet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transformRotation.eulerAngles.z + Random.Range(-spreadAngle, spreadAngle)+180));
            else
                pellet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transformRotation.eulerAngles.z + Random.Range(-spreadAngle, spreadAngle)));
            pellet.GetComponent<Rigidbody2D>().AddForce(pellet.transform.right * _firePower);
            Destroy(pellet, 2f);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(.01f);

        }
    }

    IEnumerator Reloading()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_reloadTime);
        _canShoot = true;
        _magazineCapacity = _maxCapacity;
    }
}
