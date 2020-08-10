using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpiderDetectionsEvent))]
public class spiderMovement : MonoBehaviour
{
    public List<TargetStep> leftTargetSteps;
    public List<TargetStep> rightTargetSteps;
    public float stepShift = 2f;
    public float speed;
    public AnimationCurve breathCurve;
    public float distanceToGround = 2f;
    float breathTimer;
    Vector2 _dir = Vector2.left;
    float heightPosition;
    [SerializeField] bool inversed;
    [SerializeField] bool shiftToFacePlayer;
    SpiderDetectionsEvent detectionEvent;

    private void Start()
    {
        detectionEvent = GetComponent<SpiderDetectionsEvent>();
        detectionEvent.OnWallIsNextBy += OnWallIsNextHandler;
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;        
        heightPosition = transform.position.y;
        if (inversed)
            _dir = Vector2.right;

        detectionEvent.ShiftDirection(new SpiderDetectionsEvent.ShiftDirectionEventArgs { newDir = _dir });
    }

    private void OnPlayerDetectedHandler(object sender, SpiderDetectionsEvent.PlayerDetectedEventArgs e)
    {
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
            foreach (TargetStep target in rightTargetSteps)
            {
                target.transform.position = new Vector3(target.transform.position.x + stepShift, target.transform.position.y, target.transform.position.y);
            }
            foreach (TargetStep target in leftTargetSteps)
            {
                target.transform.position = new Vector3(target.transform.position.x + stepShift, target.transform.position.y, target.transform.position.y);
            }
            _dir = Vector2.right;
        }
        else
        {
            foreach (TargetStep target in rightTargetSteps)
            {
                target.transform.position = new Vector3(target.transform.position.x - stepShift, target.transform.position.y, target.transform.position.y);
            }
            foreach (TargetStep target in leftTargetSteps)
            {
                target.transform.position = new Vector3(target.transform.position.x - stepShift, target.transform.position.y, target.transform.position.y);
            }
            _dir = Vector2.left;
        }
    }
    private void Move()
    {
            transform.Translate(_dir * speed * Time.deltaTime);
    }
}
