using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpiderDetectionsEvent))]
public class spiderMovement : MonoBehaviour
{
    [Header("References")]
    public List<LegStep> leftLegs;
    public List<LegStep> rightLegs;

    [Header("Parameters")]
    public float stepShift = 2f;
    public float speed;
    public AnimationCurve breathCurve;
    public float distanceToGround = 2f;

    [Header("Optional")]
    [SerializeField] bool inversed;
    [SerializeField] bool shiftToFacePlayer;
    [SerializeField] bool changeSpeedOnPlayerDetected;
    [SerializeField] [Range(0, 300)] float bodySpeedOnPlayerDetected;
    [SerializeField] [Range(0, 300)] float stepDistanceOnPlayerDetected;

    float breathTimer;
    Vector2 _dir = Vector2.left;
    float heightPosition;
    SpiderDetectionsEvent detectionEvent;
    float _initialSpeed;

    private void Start()
    {
        detectionEvent = GetComponent<SpiderDetectionsEvent>();
        detectionEvent.OnWallIsNextBy += OnWallIsNextHandler;
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;        
        detectionEvent.OnPlayerNotDetectedAnymore += OnPlayerNotDetectedAnymoreHandler;        
        heightPosition = transform.position.y;
        if (inversed)
            _dir = Vector2.right;

        _initialSpeed = speed;
        detectionEvent.ShiftDirection(new SpiderDetectionsEvent.ShiftDirectionEventArgs { newDir = _dir });
    }

    private void OnPlayerNotDetectedAnymoreHandler(object sender, SpiderDetectionsEvent.PlayerNotDetectedAnymoreEventArgs e)
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
            }
        }
    }

    private void OnPlayerDetectedHandler(object sender, SpiderDetectionsEvent.PlayerDetectedEventArgs e)
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
            }

        }
        if (shiftToFacePlayer)
        {
            if (e.player.transform.position.x < transform.position.x && _dir == Vector2.right)
            {
                ShiftDirection(_dir);
                detectionEvent.ShiftDirection(new SpiderDetectionsEvent.ShiftDirectionEventArgs { newDir = _dir });
            }
            if (e.player.transform.position.x > transform.position.x && _dir == Vector2.left)
            {
                ShiftDirection(_dir);
                detectionEvent.ShiftDirection(new SpiderDetectionsEvent.ShiftDirectionEventArgs { newDir = _dir });
            }
        }        
    }

    private void OnWallIsNextHandler(object sender, SpiderDetectionsEvent.WallIsNextByEventArgs e)
    {
        ShiftDirection(_dir);
        detectionEvent.ShiftDirection(new SpiderDetectionsEvent.ShiftDirectionEventArgs { newDir = _dir});        
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
        Vector2(transform.position.x, transform.position.y),
        Vector2.up, 50f, LayerMask.GetMask("Ground"));
        }
        else
        {
            hit = Physics2D.Raycast(new
        Vector2(transform.position.x, transform.position.y),
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
        transform.position = new Vector3(transform.position.x, heightPosition + breathCurve.Evaluate(breathTimer));
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
    }
    private void Move()
    {
            transform.Translate(_dir * speed * Time.deltaTime);
    }
}
