using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class FreakingLeechMovement : MonoBehaviour
{
    [Header("References")]

    [SerializeField] Solver2D _bigPartSolver;
    [SerializeField] Solver2D _endPartSolver;
    [SerializeField] GameObject _base;

    [Header("Parameters")]
    
    [SerializeField] float _stepDistance = 1;
    [SerializeField] float _stepDurationHead = 1;
    [SerializeField] float _stepDurationBase = 1;
    // configure so if it it is higher will go back instead of jumping
    [SerializeField] float _maxJump = 1;

    [SerializeField] AnimationCurve _stepMovementHead;
    [SerializeField] float _randomMovementRange;
    [SerializeField] AnimationCurve _stepMovementBase;
    [SerializeField] float delay;
    [SerializeField] bool directionIsLeft = true;
    public bool DirectionIsLeft { get => directionIsLeft; set { directionIsLeft = value; directionJustChanged = true; } }


    [Header("Debugs")]
    [SerializeField] AnimationCurve debugCurve;

    float _distanceBetweenPart;
    public float _distanceBetweenHeadAndBase;
    public bool directionJustChanged;

   
    [ContextMenu("Change Direction")]
    void ChangeDirection()
    {
        DirectionIsLeft = !DirectionIsLeft;
    }


    // Start is called before the first frame update
    void Start()
    {
        _distanceBetweenPart = Vector2.Distance(_bigPartSolver.transform.position, _endPartSolver.transform.position) * 0.65f;
        _distanceBetweenHeadAndBase = Vector2.Distance(_base.transform.position, _endPartSolver.transform.position);
        Invoke("StartMoving", delay);
    }

    void StartMoving()
    {
        StartCoroutine(TakeStep());
    }
    // Update is called once per frame
    void Update()
    {

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
                tempCurve.AddKey(new Keyframe(curve.keys[i].time, Random.Range(curve.keys[i].value - randomRange / 2, curve.keys[i].value + randomRange / 2)));
            }
            
        }
        debugCurve = tempCurve;

        return tempCurve;
    }
    IEnumerator TakeStep()
    {
        float timer = 0;
        Vector2 startPos = _endPartSolver.transform.position;
        Vector2 startPosBig = _bigPartSolver.transform.position;
        Vector2 target = getTargetPos(_endPartSolver.transform, DirectionIsLeft, true);
        Vector2 targetBigPart = new Vector2(target.x, target.y + _distanceBetweenPart);
        AnimationCurve stepMovement = RandomStepMovement();
        while (timer < _stepDurationHead)
        {
            timer += Time.deltaTime;
            _endPartSolver.transform.position = Vector2.Lerp(startPos, new Vector2(target.x, target.y + stepMovement.Evaluate(timer / _stepDurationHead)), timer / _stepDurationHead);
            _bigPartSolver.transform.position = Vector2.Lerp(startPosBig, new Vector2(targetBigPart.x, targetBigPart.y + stepMovement.Evaluate(timer / _stepDurationHead)), timer / _stepDurationHead);
            yield return null;
        }
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
        StartCoroutine(TakeStep());
    }
    Vector2 getTargetPos(Transform initialPos, bool movingDirLeft, bool isHead = false)
    {
        Vector2 targetPos;
        
        if (directionJustChanged)
        {
            directionJustChanged = false;
            // move toward new direction
            if (isHead)
            {
                if (movingDirLeft)
                    targetPos = initialPos.position - new Vector3(_stepDistance + _distanceBetweenHeadAndBase, 0, 0);
                else
                    targetPos = initialPos.position + new Vector3(_stepDistance + _distanceBetweenHeadAndBase, 0, 0);
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
            Vector2(targetPos.x, targetPos.y + 5),
            Vector2.down, 12f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            targetPos = hit.point;
        }
        return targetPos;
    }
}
