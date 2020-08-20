using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class WeaponController : MonoBehaviour
{
   // public Light2D _pointLight;
    public LightParameters[] _pointLightStruct;

    [System.Serializable]
    public struct LightParameters
    {
        public Light2D _light;
        public float _minLightAngle;
        public float _maxLightAngle;
        public float _minDistance;
        public float _maxDistance;
        public float _intensity;
    }


    private float _toAdd;

    [Range(0, 360f)]
    public float _minSpreadAngle, _maxSpreadAngle;

    Camera _camera;
    Vector3 _orientation;
    Vector3 _direction;
    public Transform _parent;
    public float _aimAngleRange = 120f;

    public GameObject _projectile;
    public float _firePower;

    public Transform _spawner;

    public int _pelletNumber = 3;

    float _spreadAngle;
    bool _canShoot = false;

    public float _reloadTime;
    public int _magazineCapacity = 4;
    public int _maxCapacity = 4;
    public Light2D _magazineLight;
    public float _maxMagazineIntensity;
    public Light2D _emptyMagazineLight;


    public Animator _animator; 


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
            if (_orientation.y < transform.localPosition.y)
                transform.eulerAngles = new Vector3(0, 0, (Mathf.Clamp(tempAngle, -180,-180 + _aimAngleRange/2))-180);
            else
                transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, 180 - _aimAngleRange / 2, 180)-180);
        }
        else
        {

            GameState._isCharacterFlipped = false;

            _parent.transform.localScale = new Vector3(1, 1, 1);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, - _aimAngleRange / 2, _aimAngleRange / 2));
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if(_magazineCapacity>0)
                _canShoot = true;
            

            if (_pointLightStruct.Length > 0)
            {
                for(int i = 0; i < _pointLightStruct.Length; i++)
                {
                    _toAdd = (_pointLightStruct[i]._maxLightAngle - _pointLightStruct[i]._minLightAngle) * Time.deltaTime;
                    _pointLightStruct[i]._light.pointLightInnerAngle = Mathf.Clamp(_pointLightStruct[i]._light.pointLightInnerAngle -= _toAdd, _pointLightStruct[i]._minLightAngle, _pointLightStruct[i]._maxLightAngle);
                   // _pointLightStruct[i]._light.pointLightOuterAngle = Mathf.Clamp(_pointLightStruct[i]._light.pointLightOuterAngle -= _toAdd, _pointLightStruct[i]._minLightAngle + 10, _pointLightStruct[i]._maxLightAngle);
                    float tempRadius = Mathf.InverseLerp(_pointLightStruct[i]._minLightAngle, _pointLightStruct[i]._maxLightAngle, _pointLightStruct[i]._light.pointLightInnerAngle);
                    _pointLightStruct[i]._light.pointLightInnerRadius = Mathf.Lerp(_pointLightStruct[i]._maxDistance, _pointLightStruct[i]._minDistance, tempRadius);
                }
                _spreadAngle = _pointLightStruct[0]._light.pointLightInnerAngle;
            }


        }
        else
        {
            
            if (_pointLightStruct.Length > 0)
            {
                for (int i = 0; i < _pointLightStruct.Length; i++)
                {

                    _toAdd = (_pointLightStruct[i]._maxLightAngle - _pointLightStruct[i]._minLightAngle) * Time.deltaTime;
                    _pointLightStruct[i]._light.pointLightInnerAngle = Mathf.Clamp(_pointLightStruct[i]._light.pointLightInnerAngle += _toAdd/3, _pointLightStruct[i]._minLightAngle, _pointLightStruct[i]._maxLightAngle);
                    //_pointLightStruct[i]._light.pointLightOuterAngle = Mathf.Clamp(_pointLightStruct[i]._light.pointLightOuterAngle += _toAdd/3, _pointLightStruct[i]._minLightAngle + 10, _pointLightStruct[i]._maxLightAngle);
                    float tempRadius = Mathf.InverseLerp(_pointLightStruct[i]._minLightAngle, _pointLightStruct[i]._maxLightAngle, _pointLightStruct[i]._light.pointLightInnerAngle);
                    _pointLightStruct[i]._light.pointLightInnerRadius = Mathf.Lerp(_pointLightStruct[i]._maxDistance, _pointLightStruct[i]._minDistance, tempRadius);
                }
                 _spreadAngle = _pointLightStruct[0]._light.pointLightInnerAngle;
            }
            if( _magazineCapacity <= 0)
            {
                _canShoot = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canShoot)
        {
            float tempValue = Mathf.InverseLerp(_maxSpreadAngle, _minSpreadAngle, _spreadAngle);
            StartCoroutine(ShootWithSpread(_spreadAngle, _pelletNumber, transform.rotation, _spawner.position, Mathf.Lerp(.3f,1f,tempValue)));
            _magazineCapacity--;
            if (_magazineCapacity <= 0)
            {
                StartCoroutine(Reloading());
            }
            if (_pointLightStruct.Length > 0)
            {
                for (int i = 0; i < _pointLightStruct.Length; i++)
                {
                    _pointLightStruct[i]._light.pointLightInnerAngle = _pointLightStruct[i]._maxLightAngle;
                    //_pointLightStruct[i]._light.pointLightOuterAngle = _pointLightStruct[i]._maxLightAngle;
                    _pointLightStruct[i]._light.pointLightInnerRadius = _pointLightStruct[i]._minDistance;
                }
            }
        }
        SetMagazineLight();
    }
    private void OnValidate()
    {
        if (_minSpreadAngle > _maxSpreadAngle)
            _minSpreadAngle = _maxSpreadAngle;
    }

    void SetMagazineLight()
    {
        _magazineLight.intensity = (_magazineCapacity/ (float)_maxCapacity)*_maxMagazineIntensity;
        _emptyMagazineLight.intensity = _maxMagazineIntensity - _magazineLight.intensity;
    }

    IEnumerator ShootWithSpread(float spreadAngle, int numberOfPellets, Quaternion transformRotation, Vector3 spawnerPosition,float powerMulti = 1f, float duration = 1f)
    {
        for(int i = 0; i < numberOfPellets; i++)
        {
            GameObject pellet = Instantiate(_projectile, spawnerPosition, transformRotation);
            if(GameState._isCharacterFlipped)
                pellet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transformRotation.eulerAngles.z + Random.Range(-spreadAngle, spreadAngle)+180));
            else
                pellet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transformRotation.eulerAngles.z + Random.Range(-spreadAngle, spreadAngle)));
            pellet.GetComponent<Rigidbody2D>().AddForce(pellet.transform.right * _firePower * powerMulti);
            Destroy(pellet, duration);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(.01f);

        }
    }

    IEnumerator Reloading()
    {
        GameState._isReloading = true;
        _animator.SetBool("reloading", true);
        _canShoot = false;
        yield return new WaitForSeconds(_reloadTime);
        _canShoot = true;
        _magazineCapacity = _maxCapacity;
        SetMagazineLight();
        _animator.SetBool("reloading", false);
        GameState._isReloading = false;
    }
}
