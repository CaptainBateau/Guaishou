using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterDetectionEvent))]
public class spiderMovement : MonoBehaviour
{
    [Header("References")]
    public Transform _body;
    public List<LegStep> leftLegs;
    public List<LegStep> rightLegs;

    [Header("Parameters")]
    public float stepShift = 2f;
    public float speed = 1;
    public float _stunDuration = 0.05f;

    public AnimationCurve breathCurve = null;
    public float distanceToGround = 2f;

    [Header("Optional")]
    [SerializeField] bool inversed = false;
    [SerializeField] bool shiftToFacePlayer = true;
    [SerializeField] bool changeSpeedOnPlayerDetected = true;
    [SerializeField] [Range(0, 300)] float bodySpeedOnPlayerDetected = 100;
    [SerializeField] [Range(0, 300)] float stepDistanceOnPlayerDetected = 100;
    [SerializeField] [Range(0, 300)] float stepDurationOnPlayerDetected = 100;
    [SerializeField] bool _randomSpeed;
    [SerializeField] float _randomSpeedRange;
    [SerializeField] float _speedCurveDuration;
    [SerializeField] AnimationCurve _speedCurvePoints;

    [Header("Debugs")]
    [SerializeField] AnimationCurve _randomSpeedCurve;

    float breathTimer = 0;
    Vector2 _dir = Vector2.left;
    float heightPosition = 0;
    MonsterDetectionEvent detectionEvent = null;
    float _initialSpeed = 0;
    bool newRandomCurve = true;
    float speedCurveTimer = 0;
    bool gotHit = false;
    float hitTimer = 0;

    private void Start()
    {
        detectionEvent = GetComponent<MonsterDetectionEvent>();
        detectionEvent.OnWallIsNextBy += OnWallIsNextHandler;
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;
        detectionEvent.OnPlayerNotDetectedAnymore += OnPlayerNotDetectedAnymoreHandler;
        detectionEvent.OnMonsterHit += OnMonsterHitHandler;
        heightPosition = _body.position.y;

        _initialSpeed = speed;
        detectionEvent.ShiftDirection(new MonsterDetectionEvent.ShiftDirectionEventArgs { newDir = _dir });
    }

    private void OnMonsterHitHandler(object sender, MonsterDetectionEvent.MonsterHitEventArgs e)
    {
        gotHit = true;
    }

    private void OnPlayerNotDetectedAnymoreHandler(object sender, MonsterDetectionEvent.PlayerNotDetectedAnymoreEventArgs e)
    {
        if (changeSpeedOnPlayerDetected)
        {
            speed = _initialSpeed;
            List<LegStep> legs = new List<LegStep>();
            legs.AddRange(leftLegs);
            legs.AddRange(rightLegs);
            foreach (LegStep leg in legs)
            {
                leg.distanceToStep = leg.initialDistanceToStep;
                leg.stepDuration = leg.initialStepDuration;
            }
        }
    }

    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {
        if (changeSpeedOnPlayerDetected)
        {
            speed = _initialSpeed * bodySpeedOnPlayerDetected / 100;
            List<LegStep> legs = new List<LegStep>();
            legs.AddRange(leftLegs);
            legs.AddRange(rightLegs);
            foreach (LegStep leg in legs)
            {
                leg.distanceToStep = leg.initialDistanceToStep * stepDistanceOnPlayerDetected / 100;
                leg.stepDuration = leg.initialStepDuration * stepDurationOnPlayerDetected / 100;
            }
        }
        if (shiftToFacePlayer)
        {
            if (e.player.transform.position.x < detectionEvent._center.x && _dir == Vector2.right)
            {
                ShiftDirection(_dir);
            }
            if (e.player.transform.position.x > detectionEvent._center.x && _dir == Vector2.left)
            {
                ShiftDirection(_dir);
            }
        }
    }

    private void OnWallIsNextHandler(object sender, MonsterDetectionEvent.WallIsNextByEventArgs e)
    {
        ShiftDirection(_dir);
        detectionEvent.ShiftDirection(new MonsterDetectionEvent.ShiftDirectionEventArgs { newDir = _dir });
    }

    void Update()
    {
        CheckHeightPosition();
        Breath();
        Move();

    }
    private void CheckHeightPosition()
    {
        RaycastHit2D hit;
        if (inversed)
        {
            hit = Physics2D.Raycast(new
        Vector2(_body.position.x, _body.position.y),
        Vector2.up, 50f, LayerMask.GetMask("Ground"));
        }
        else
        {
            hit = Physics2D.Raycast(new
        Vector2(_body.position.x, _body.position.y),
        Vector2.down, 50f, LayerMask.GetMask("Ground"));
        }

        float projectedHeight = -1;
        if (hit)
        {
            if (inversed)
                projectedHeight = hit.point.y - distanceToGround;
            else
                projectedHeight = hit.point.y + distanceToGround;
            heightPosition = Mathf.Lerp(heightPosition, projectedHeight, 0.1f);
        }
    }
    private void Breath()
    {
        breathTimer += Time.deltaTime;
        if (breathTimer > breathCurve.keys[breathCurve.length - 1].time)
        {
            breathTimer = 0;
        }
        _body.position = new Vector3(_body.position.x, heightPosition + breathCurve.Evaluate(breathTimer));
    }
    private void ShiftDirection(Vector2 dir)
    {
        if (dir == Vector2.left)
        {
            foreach (LegStep target in rightLegs)
            {
                Transform desiredTarget = target.desiredTarget.transform;
                desiredTarget.position = new Vector3(desiredTarget.position.x + stepShift, desiredTarget.position.y, desiredTarget.position.y);
            }
            foreach (LegStep target in leftLegs)
            {
                Transform desiredTarget = target.desiredTarget.transform;
                desiredTarget.position = new Vector3(desiredTarget.position.x + stepShift, desiredTarget.position.y, desiredTarget.position.y);
            }
            _dir = Vector2.right;
        }
        else
        {
            foreach (LegStep target in rightLegs)
            {
                Transform desiredTarget = target.desiredTarget.transform;
                desiredTarget.position = new Vector3(desiredTarget.position.x - stepShift, desiredTarget.position.y, desiredTarget.position.y);
            }
            foreach (LegStep target in leftLegs)
            {
                Transform desiredTarget = target.desiredTarget.transform;
                desiredTarget.position = new Vector3(desiredTarget.position.x - stepShift, desiredTarget.position.y, desiredTarget.position.y);
            }
            _dir = Vector2.left;
        }
        detectionEvent.ShiftDirection(new MonsterDetectionEvent.ShiftDirectionEventArgs { newDir = _dir });
    }

    AnimationCurve RandomSpeedCurve()
    {
        AnimationCurve _randomStep = NewRandomizeKeys(_speedCurvePoints, _randomSpeedRange);
        return _randomStep;
    }


    AnimationCurve NewRandomizeKeys(AnimationCurve curve, float randomRange)
    {
        // skip first and last keys
        AnimationCurve tempCurve;
        tempCurve = new AnimationCurve();
        for (int i = 0; i < curve.keys.Length; i++)
        {
            if (i == 0 || i == curve.keys.Length - 1)
            {
                tempCurve.AddKey(curve[i]);
            }
            else
            {
                tempCurve.AddKey(new Keyframe(curve.keys[i].time, UnityEngine.Random.Range(curve.keys[i].value - randomRange / 2, curve.keys[i].value + randomRange / 2)));
            }

        }


        return tempCurve;
    }

    private void Move()
    {
        if (_randomSpeed)
        {
            if (newRandomCurve)
            {
                _randomSpeedCurve = RandomSpeedCurve();
                speedCurveTimer = 0;
                newRandomCurve = false;
            }
            if (speedCurveTimer < _speedCurveDuration)
            {
                speedCurveTimer += Time.deltaTime;
                _body.Translate(_dir * (speed + _randomSpeedCurve.Evaluate(speedCurveTimer / _speedCurveDuration)) * Time.deltaTime);
            }
            else
            {
                newRandomCurve = true;
            }

        }
        else
        {
            _body.Translate(_dir * speed * Time.deltaTime);
        }
        if (gotHit && hitTimer < _stunDuration)
        {
            hitTimer += Time.deltaTime;
            _body.Translate(-_dir * speed * Time.deltaTime);
        }
        else
        {
            gotHit = false;
            hitTimer = 0;
        }
    }
}
