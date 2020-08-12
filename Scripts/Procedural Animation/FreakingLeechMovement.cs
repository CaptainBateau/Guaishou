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
    [SerializeField] float _stepDistance;
    [SerializeField] List<AnimationCurve> _stepMovements = new List<AnimationCurve>();
    [SerializeField] AnimationCurve _stepMovementBase;
    [SerializeField] float delay;

    float _distanceBetweenPart;
    float _stepDuration;
    float _stepDurationBase;




    // Start is called before the first frame update
    void Start()
    {
        
        _stepDurationBase = _stepMovementBase.keys[_stepMovementBase.length - 1].time;
        _distanceBetweenPart = Vector2.Distance(_bigPartSolver.transform.position, _endPartSolver.transform.position) - 1;
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
        return _stepMovements[Random.Range(0, _stepMovements.Count - 1)];
    }
    IEnumerator TakeStep()
    {
        float timer = 0;
        Vector2 startPos = _endPartSolver.transform.position;
        Vector2 target = getTargetPos(_endPartSolver.transform);
        Vector2 targetBigPart = new Vector2(target.x, target.y + _distanceBetweenPart);
        AnimationCurve stepMovement = RandomStepMovement();
        _stepDuration = stepMovement.keys[stepMovement.length - 1].time;
        while (timer < _stepDuration)
        {
            timer += Time.deltaTime;
            _endPartSolver.transform.position = Vector2.Lerp(startPos, new Vector2(target.x, target.y + stepMovement.Evaluate(timer)), timer / _stepDuration);
            _bigPartSolver.transform.position = Vector2.Lerp(startPos, new Vector2(targetBigPart.x, targetBigPart.y + stepMovement.Evaluate(timer)), timer / _stepDuration);
            
            yield return null;
        }
        StartCoroutine(FollowStep());
    }


    IEnumerator FollowStep()
    {
        float timer = 0;
        Vector2 startPos = _base.transform.position;
        Vector2 target = getTargetPos(_base.transform);
        while (timer < _stepDuration)
        {
            timer += Time.deltaTime;
            _base.transform.position = Vector2.Lerp(startPos, new Vector2(target.x, target.y + _stepMovementBase.Evaluate(timer)), timer / _stepDurationBase);            
            yield return null;
        }
        StartCoroutine(TakeStep());
    }
    Vector2 getTargetPos(Transform initialPos)
    {
        Vector2 targetPos = initialPos.position - new Vector3(_stepDistance, 0, 0);
        RaycastHit2D hit = Physics2D.Raycast(new
            Vector2(targetPos.x, targetPos.y + 5),
            Vector2.down, 12f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            targetPos = hit.point;
        }
        Debug.Log(targetPos);
        return targetPos;
    }
}
