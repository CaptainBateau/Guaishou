using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class TentacleMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BoxCollider2D _movementZone;
    [SerializeField] CCDSolver2D _solver;
    [SerializeField] MonsterDetectionEvent detectionEvent;

    [Header("Parameters")]
    [SerializeField] float _TimeToTarget = 2;
    [SerializeField] float _TimeToTargetPlayer = 1;
    [SerializeField] AnimationCurve _moveCurve;

    


    GameObject _target;
    Vector2 topRight;
    Vector2 bottomLeft;
    bool attackPlayer;
    Transform player;

    void Start()
    {
        Bounds boxBounds = _movementZone.bounds;
        topRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
        bottomLeft = new Vector2(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y);

        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;
        detectionEvent.OnPlayerNotDetectedAnymore += OnPlayerNotDetectedAnymoreHandler;
        //movementZone.bounds;
        StartCoroutine(moveToNextTarget());
    }   

    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {       
        Debug.Log("player detected");
        attackPlayer = true;
        player = e.player.transform;
    }
    private void OnPlayerNotDetectedAnymoreHandler(object sender, MonsterDetectionEvent.PlayerNotDetectedAnymoreEventArgs e)
    {
        Debug.Log("player not detected anymore");
        attackPlayer = false;
    }

    IEnumerator moveToNextTarget()
    {
        if (attackPlayer)
        {
            float moveDuration = Vector2.Distance(_solver.transform.position, player.position) * _TimeToTargetPlayer;
            IEnumerator attack = Movements.Move(_solver.transform, player, _moveCurve, moveDuration, 1.5f, 2);
            StartCoroutine(attack);
            yield return new WaitForSeconds(moveDuration);
            StartCoroutine(moveToNextTarget());
        }
        else
        {
            GenerateRandomTarget();

            float moveDuration = Vector2.Distance(_solver.transform.position, _target.transform.position) * _TimeToTarget;
            IEnumerator move = Movements.Move(_solver.transform, _target.transform, _moveCurve, moveDuration);
            StartCoroutine(move);
            float timer = 0;
            while(timer < moveDuration)
            {
                if (attackPlayer)
                {
                    StopCoroutine(move);
                    timer = moveDuration;
                }
                timer += Time.deltaTime;
                yield return null;
            }
            StartCoroutine(moveToNextTarget());
        }        
    }

    [ContextMenu("createRandomTarget")]
    void GenerateRandomTarget()
    {
        GameObject targetGO = new GameObject();
        Destroy(_target);
        _target = Instantiate(targetGO, new Vector2(UnityEngine.Random.Range(bottomLeft.x, topRight.x), UnityEngine.Random.Range(topRight.y, bottomLeft.y)), Quaternion.identity, transform);
        _target.name = "target";
        Destroy(targetGO);
    }
}
