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
    public AnimationCurve breathCurve = null;
    public float distanceToGround = 2f;

    [Header("Optional")]
    [SerializeField] bool inversed = false;
    [SerializeField] bool shiftToFacePlayer = true;
    [SerializeField] bool changeSpeedOnPlayerDetected = true;
    [SerializeField] [Range(0, 300)] float bodySpeedOnPlayerDetected = 100;
    [SerializeField] [Range(0, 300)] float stepDistanceOnPlayerDetected = 100;
    [SerializeField] [Range(0, 300)] float stepDurationOnPlayerDetected = 100;

    float breathTimer;
    Vector2 _dir = Vector2.left;
    float heightPosition;
    MonsterDetectionEvent detectionEvent;
    float _initialSpeed;

    private void Start()
    {
        detectionEvent = GetComponent<MonsterDetectionEvent>();
        detectionEvent.OnWallIsNextBy += OnWallIsNextHandler;
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;        
        detectionEvent.OnPlayerNotDetectedAnymore += OnPlayerNotDetectedAnymoreHandler;        
        heightPosition = _body.position.y;

        _initialSpeed = speed;
        detectionEvent.ShiftDirection(new MonsterDetectionEvent.ShiftDirectionEventArgs { newDir = _dir });
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
        detectionEvent.ShiftDirection(new MonsterDetectionEvent.ShiftDirectionEventArgs { newDir = _dir});        
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
            if(inversed)
                projectedHeight = hit.point.y - distanceToGround;
            else
                projectedHeight = hit.point.y + distanceToGround;
            heightPosition = Mathf.Lerp(heightPosition, projectedHeight, 0.1f);
        }
    }
    private void Breath()
    {
        breathTimer += Time.deltaTime;
        if (breathTimer > breathCurve.keys[breathCurve.length -1].time)
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
    private void Move()
    {
        _body.Translate(_dir * speed * Time.deltaTime);
    }
}
