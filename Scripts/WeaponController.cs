using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class WeaponController : MonoBehaviour
{
    public Light2D _pointLight;
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
            _parent.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            if(_orientation.y < transform.localPosition.y)
                transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, -180, -180 + _lightAngleRange/2));
            else
                transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, 180 - _lightAngleRange / 2, 180));
        }
        else
        {

            GameState._isCharacterFlipped = false;
            _parent.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, - _lightAngleRange / 2, _lightAngleRange / 2));
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            _pointLight.pointLightInnerAngle = Mathf.Clamp(_pointLight.pointLightInnerAngle -=_toAdd, _minAngle, _maxAngle);
            _pointLight.pointLightOuterAngle = Mathf.Clamp(_pointLight.pointLightOuterAngle -=_toAdd, _minAngle + 10, _maxAngle);
        }
        else if(_pointLight.pointLightInnerAngle != _maxAngle) 
        {
            _spreadAngle = _pointLight.pointLightInnerAngle;
            _pointLight.pointLightInnerAngle =  _maxAngle;
            _pointLight.pointLightOuterAngle =  _maxAngle;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StartCoroutine(ShootWithSpread(_spreadAngle, _pelletNumber, transform.rotation, _spawner.position));
        }
    }
    private void OnValidate()
    {
        if (_minAngle > _maxAngle)
            _minAngle = _maxAngle;
    }


    IEnumerator ShootWithSpread(float spreadAngle, int numberOfPellets, Quaternion transformRotation, Vector3 spawnerPosition)
    {
        for(int i = 0; i < numberOfPellets; i++)
        {
            GameObject pellet = Instantiate(_projectile, spawnerPosition, transformRotation);
            pellet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transformRotation.eulerAngles.z + Random.Range(-spreadAngle, spreadAngle)));
            pellet.GetComponent<Rigidbody2D>().AddForce(pellet.transform.right * _firePower);
            Destroy(pellet, 2f);
            yield return new WaitForEndOfFrame();

        }
    }
}
