using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class FreakingLeechMovement : MonoBehaviour
{
    [Header("References")]

    [SerializeField] Solver2D _bigPartSolver = null;
    [SerializeField] Solver2D _endPartSolver = null;
    [SerializeField] GameObject _base = null;

    [Header("Parameters")]
    
    [SerializeField] float _stepDistance = 1;
    [SerializeField] float _stepDurationHead = 1;
    [SerializeField] float _stepDurationBase = 1;
    [SerializeField] float _jumpMaxHigh = 2f;
    [SerializeField] AnimationCurve _stepMovementHead = null;
    [SerializeField] float _randomMovementRange = 0;
    [SerializeField] AnimationCurve _turningAround = null;
    [SerializeField] AnimationCurve _stepMovementBase = null;
    [SerializeField] float delay = 0;
    [SerializeField] bool directionIsLeft = true;
    //[SerializeField] AnimationCurve _attackingCurve = null;
    //[SerializeField] float _attackDuration = 1;
    public bool DirectionIsLeft { get => directionIsLeft; set { directionIsLeft = value; directionJustChanged = true; } }


    [Header("Debugs")]
    [SerializeField] AnimationCurve debugCurve;   
    [SerializeField] float _distanceBetweenHeadAndBase;
    [SerializeField] bool directionJustChanged;


    float _distanceBetweenPart;
    [SerializeField] MonsterDetectionEvent detectionEvent = null;


    void ChangeDirection()
    {
        DirectionIsLeft = !DirectionIsLeft;
        Vector2 dir;
        if (directionIsLeft)
            dir = Vector2.left;
        else
            dir = Vector2.right;
        detectionEvent.ShiftDirection(new MonsterDetectionEvent.ShiftDirectionEventArgs { newDir = dir });   
    }


    // Start is called before the first frame update
    void Start()
    {
        detectionEvent.OnWallIsNextBy += OnWallIsNextHandler;
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;



        _distanceBetweenPart = Vector2.Distance(_bigPartSolver.transform.position, _endPartSolver.transform.position) * 0.65f;
        _distanceBetweenHeadAndBase = Vector2.Distance(_base.transform.position, _endPartSolver.transform.position);
        Invoke("StartMoving", delay);
    }

    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {
        if(e.player.transform.position.x < transform.position.x && !directionIsLeft)
        {
            ChangeDirection();
        }
        if (e.player.transform.position.x > detectionEvent._center.x && directionIsLeft)
        {
            ChangeDirection();         
        }
    }

    private void OnWallIsNextHandler(object sender, MonsterDetectionEvent.WallIsNextByEventArgs e)
    {
        ChangeDirection();
    }

    void StartMoving()
    {
        StartCoroutine(TakeStep());
    }
    AnimationCurve RandomStepMovement()
    {
        AnimationCurve _randomStep = NewRandomizeKeys(_stepMovementHead, _randomMovementRange);
        return _randomStep;
    }


    AnimationCurve NewRandomizeKeys(AnimationCurve curve, float randomRange)
    {
        // skip first and last keys
        AnimationCurve tempCurve;
        tempCurve = new AnimationCurve();
        for (int i = 0; i < curve.keys.Length; i++)
        {
            if(i == 0 || i == curve.keys.Length - 1)
            {
                tempCurve.AddKey(curve[i]);
            }
            else
            {
                tempCurve.AddKey(new Keyframe(curve.keys[i].time, UnityEngine.Random.Range(curve.keys[i].value - randomRange / 2, curve.keys[i].value + randomRange / 2)));
            }
            
        }
        debugCurve = tempCurve;

        return tempCurve;
    }
    IEnumerator TakeStep()
    {
        float timer = 0;
        AnimationCurve stepMovement;
        if (directionJustChanged)
            stepMovement = _turningAround;
        else
            stepMovement = RandomStepMovement();

        Vector2 startPos = _endPartSolver.transform.position;
        Vector2 startPosBig = _bigPartSolver.transform.position;
        Vector2 target = getTargetPos(_endPartSolver.transform, DirectionIsLeft, true);
        Vector2 targetBigPart = new Vector2(target.x, target.y + _distanceBetweenPart);
        
        while (timer < _stepDurationHead)
        {
            timer += Time.deltaTime;
            _endPartSolver.transform.position = Vector2.Lerp(startPos, new Vector2(target.x, target.y + stepMovement.Evaluate(timer / _stepDurationHead)), timer / _stepDurationHead);
            _bigPartSolver.transform.position = Vector2.Lerp(startPosBig, new Vector2(targetBigPart.x, targetBigPart.y + stepMovement.Evaluate(timer / _stepDurationHead)), timer / _stepDurationHead);
            yield return null;
        }
        detectionEvent.TakeStep(new MonsterDetectionEvent.TakeStepEventArgs { });
        StartCoroutine(FollowStep());
    }


    IEnumerator FollowStep()
    {
        float timer = 0;
        Vector2 startPos = _base.transform.position;
        Vector2 target = getTargetPos(_base.transform, DirectionIsLeft);
        while (timer < _stepDurationBase)
        {
            timer += Time.deltaTime;
            _base.transform.position = Vector2.Lerp(startPos, new Vector2(target.x, target.y + _stepMovementBase.Evaluate(timer / _stepDurationBase)), timer / _stepDurationBase);            
            yield return null;
        }
        detectionEvent.TakeStep(new MonsterDetectionEvent.TakeStepEventArgs { });
        StartCoroutine(TakeStep());
    }

    //IEnumerator Attack()
    //{
    //    Movements.Move(_endPartSolver.transform, player, _attackingCurve, _attackDuration);
    //    yield return new WaitForSeconds(_attackDuration / 10);
    //    Movements.Move(_bigPartSolver.transform, player, _attackingCurve, _attackDuration);
    //    yield return new WaitForSeconds(_attackDuration / 3);
    //    Movements.Move(_base.transform, player, _attackingCurve, _attackDuration);

    //}

    Vector2 getTargetPos(Transform initialPos, bool movingDirLeft, bool isHead = false)
    {
        Vector2 targetPos;
        
        if (directionJustChanged)
        {
            
            // move toward new direction
            if (isHead)
            {
                directionJustChanged = false;
                if (movingDirLeft)
                    targetPos = initialPos.position - new Vector3(_stepDistance + _distanceBetweenHeadAndBase * 2, 0, 0);
                else
                    targetPos = initialPos.position + new Vector3(_stepDistance + _distanceBetweenHeadAndBase * 2, 0, 0);
            }
            // keep it like normal movement in the wrong direction
            else
            {
                if (movingDirLeft)
                    targetPos = initialPos.position + new Vector3(_stepDistance, 0, 0);
                else
                    targetPos = initialPos.position - new Vector3(_stepDistance, 0, 0);
            }            
        }
        // move based on Direction
        else
        {
            if (movingDirLeft)
                targetPos = initialPos.position - new Vector3(_stepDistance, 0, 0);
            else
                targetPos = initialPos.position + new Vector3(_stepDistance, 0, 0);
        }
        


        RaycastHit2D hit = Physics2D.Raycast(new
            Vector2(targetPos.x, targetPos.y + _jumpMaxHigh),
            Vector2.down, 12f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            targetPos = hit.point;
        }
        return targetPos;
    }
}
