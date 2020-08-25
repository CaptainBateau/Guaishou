using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class TentacleMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BoxCollider2D _movementZone;
    [SerializeField] CCDSolver2D _solver;

    [Header("Parameters")]
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _moveCurve;



    GameObject _target;
    Vector2 topRight;
    Vector2 bottomLeft;

    void Start()
    {
        Bounds boxBounds = _movementZone.bounds;
        topRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
        bottomLeft = new Vector2(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y);

        //movementZone.bounds;
        StartCoroutine(moveToNextTarget());
    }

    IEnumerator moveToNextTarget()
    {
        GenerateRandomTarget();
        
        float moveDuration = Vector2.Distance(_solver.transform.position, _target.transform.position) * _speed;
        //Debug.Log("new target pos is " + _target.transform.position + " and duration is " + moveDuration);

        //float timer = 0;
        //Vector3 solverInitialPos = _solver.transform.position;
        //while (timer < moveDuration)
        //{
        //    timer += Time.deltaTime;
        //        Vector2 target = _target.transform.position;

        //    _solver.transform.position = Vector2.Lerp(solverInitialPos, target, _moveCurve.Evaluate(timer / moveDuration));
        //    yield return null;
        //}
        StartCoroutine(Movements.Move(_solver.transform, _target.transform, _moveCurve, moveDuration));
        yield return new WaitForSeconds(moveDuration);
        StartCoroutine(moveToNextTarget());
    }

    [ContextMenu("createRandomTarget")]
    void GenerateRandomTarget()
    {
        GameObject targetGO = new GameObject();
        Destroy(_target);
        _target = Instantiate(targetGO, new Vector2(Random.Range(bottomLeft.x, topRight.x), Random.Range(topRight.y, bottomLeft.y)), Quaternion.identity, transform);
        _target.name = "target";
        Destroy(targetGO);
    }
}
