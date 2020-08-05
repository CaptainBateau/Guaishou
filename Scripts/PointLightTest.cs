using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PointLightTest : MonoBehaviour
{
    public Light2D _pointLight;
    public float _toAdd;
    [Range(0,360f)]
    public float _minAngle, _maxAngle;
    public float _rotationSpeed = 1;

    Camera _camera;
    public Vector3 _orientation;
    public Vector3 _direction;
    public Transform _parent;
    float _lightAngleRange = 120f; 

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
        _direction = (_camera.ScreenToWorldPoint(temp) - transform.position);
        float tempAngle= Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90;
        Debug.Log(tempAngle);
        if (_orientation.x < 0)
        {
            _parent.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            if(_orientation.y<transform.position.y)
                transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle,-270,-270+_lightAngleRange/2));
            else
                transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle,90- _lightAngleRange / 2, 90));
        }
        else
        {
            _parent.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle,-90- _lightAngleRange / 2, -90+ _lightAngleRange / 2));
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            _pointLight.pointLightInnerAngle = Mathf.Clamp(_pointLight.pointLightInnerAngle -=_toAdd, _minAngle, _maxAngle);
            _pointLight.pointLightOuterAngle = Mathf.Clamp(_pointLight.pointLightOuterAngle -=_toAdd, _minAngle, _maxAngle);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {

            _pointLight.pointLightInnerAngle =  _maxAngle;
            _pointLight.pointLightOuterAngle = _maxAngle;
        }

        if (Input.GetAxisRaw("Vertical") > 0f)
        {
            transform.Rotate(transform.forward * _rotationSpeed);
        }
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            transform.Rotate(transform.forward * -_rotationSpeed);
        }
    }
    private void OnValidate()
    {
        if (_minAngle > _maxAngle)
            _minAngle = _maxAngle;
    }
}
