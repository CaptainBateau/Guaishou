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
        [Header("Minimum light angle when aiming")]
        public float _minLightAngle;
        [Header("Maximum light angle when aiming")]
        public float _maxLightAngle;
        [Header("Distance of light when NOT aiming")]
        public float _minDistance;
        [Header("Distance of light when aiming")]
        public float _maxDistance;
        public float _intensity;
    }


    private float _toAdd;

    [Range(0, 360f)]
    float _minSpreadAngle, _maxSpreadAngle;

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


    public Light2D[] _playerSpriteLights;
    public float _initialIntensity = 1;
    public float _targetIntensity;
    public float _timeToRecover;
    float _timeWhenShoot;



    public PlayerEvent _playerEvent;

    public ParticleSystem _shootFX;
    Transform _shootFXTransform;

    public Vector3 _recoilOffset;
    Vector3 _recoilOffsetLerped;

    void Start()
    {
        _camera = Camera.main;
        if (_pointLightStruct.Length > 0)
        {
            _minSpreadAngle = _pointLightStruct[0]._minLightAngle;
            _maxSpreadAngle = _pointLightStruct[0]._maxLightAngle;
        }

        for(int i = 0; i < _pointLightStruct.Length; i++)
        {
            _pointLightStruct[i]._light.intensity = _pointLightStruct[i]._intensity;
        }

        if(_shootFX!=null)
            _shootFXTransform = _shootFX.gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = -_camera.transform.position.z;
        _orientation = _camera.ScreenToWorldPoint(temp);
        _orientation = _orientation - _parent.position;

        if(Time.time <= _timeWhenShoot+0.02f)
            RecoilMovement();
        _direction = (_camera.ScreenToWorldPoint(temp) - transform.position + _recoilOffsetLerped).normalized;
        float tempAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        if (_orientation.x < 0)
        {
            GameState._isCharacterFlipped = true;
            _parent.transform.localScale = new Vector3(-1, 1, 1);
            if(_shootFX!=null)
                _shootFXTransform.localEulerAngles = new Vector3(0, 180, 0);
            if (_orientation.y < transform.localPosition.y)
                transform.eulerAngles = new Vector3(0, 0, (Mathf.Clamp(tempAngle, -180,-180 + _aimAngleRange/2))-180);
            else
                transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(tempAngle, 180 - _aimAngleRange / 2, 180)-180);
        }
        else
        {

            GameState._isCharacterFlipped = false;

            if(_shootFX!=null)
                _shootFXTransform.localEulerAngles = new Vector3(0, 0, 0);
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
            _playerEvent.PlayerShoot(new PlayerEvent.PlayerShootEventArgs { });
            StartCoroutine(ShootWithSpread(_spreadAngle, _pelletNumber, transform.rotation, _spawner.position, Mathf.Lerp(.3f,1f,tempValue)));

            _timeWhenShoot = Time.time + _timeToRecover;


            _magazineCapacity--;
            if (_magazineCapacity <= 0)
            {
                _playerEvent.PlayerReload(new PlayerEvent.PlayerReloadEventArgs { });
                StartCoroutine(Reloading());
            }
            if (_pointLightStruct.Length > 0)
            {
                for (int i = 0; i < _pointLightStruct.Length; i++)
                {
                    _pointLightStruct[i]._light.pointLightInnerAngle = _pointLightStruct[i]._maxLightAngle;
                    _pointLightStruct[i]._light.pointLightInnerRadius = _pointLightStruct[i]._minDistance;
                }
            }
        }
        if (Time.time < _timeWhenShoot)
        {
            SetIntensityPlayer();
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

    void SetIntensityPlayer()
    {
        for(int i= 0; i < _playerSpriteLights.Length; i++)
        {
            _playerSpriteLights[i].intensity = Mathf.Lerp( _initialIntensity, _targetIntensity, (_timeWhenShoot - Time.time) * _timeToRecover);
        }
    }

    void RecoilMovement() 
    {
        _recoilOffsetLerped = new Vector3(_recoilOffset.x, Mathf.Lerp(0, _recoilOffset.y, (_timeWhenShoot - Time.time) * _timeToRecover), _recoilOffset.z);
        //if (_orientation.x < 0 && _orientation.y < transform.localPosition.y)
        //{
        //    //_recoilOffsetLerped = -_recoilOffsetLerped;
        //}
    }

    IEnumerator ShootWithSpread(float spreadAngle, int numberOfPellets, Quaternion transformRotation, Vector3 spawnerPosition,float powerMulti = 1f, float duration = 1f)
    {
        if(_shootFX!=null)
            _shootFX.Play();
        for (int i = 0; i < numberOfPellets; i++)
        {
            GameObject pellet = Instantiate(_projectile, spawnerPosition, transformRotation);
            if(GameState._isCharacterFlipped)
                pellet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transformRotation.eulerAngles.z + Random.Range(-spreadAngle, spreadAngle)+180));
            else
                pellet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transformRotation.eulerAngles.z + Random.Range(-spreadAngle, spreadAngle)));
            pellet.GetComponent<Rigidbody2D>().AddForce(pellet.transform.right * _firePower * powerMulti);
            Destroy(pellet, duration);
            yield return new WaitForSeconds(.01f);

        }
        if(_shootFX!=null)
            _shootFX.Stop();
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
