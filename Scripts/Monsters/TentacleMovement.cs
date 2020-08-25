using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class TentacleMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BoxCollider2D _movementZone;
    [SerializeField] LimbSolver2D _solver;

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

        GenerateRandomTarget();
        //movementZone.bounds;
        float moveDuration = Vector2.Distance(_solver.transform.position, _target.transform.position) * _speed;
        Movements.Move(_solver.transform, _target.transform, _moveCurve, moveDuration);
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
