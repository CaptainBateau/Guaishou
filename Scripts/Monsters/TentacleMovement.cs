using System.Collections;
using UnityEngine;

public class TentacleMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BoxCollider2D _movementZone;
    [SerializeField] protected Transform _movingObject;
    [SerializeField] MonsterDetectionEvent detectionEvent;

    [Header("Parameters")]
    [SerializeField] protected float _TimeToTarget = 2;
    [SerializeField] float _TimeToTargetPlayer = 1;
    [SerializeField] protected AnimationCurve _moveCurve;

    


    GameObject _target;
    Vector2 topRight;
    Vector2 bottomLeft;
    protected bool attackPlayer;
    Transform player;
    Bounds boxBounds;

    void Start()
    {
        boxBounds = _movementZone.bounds;
        topRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
        bottomLeft = new Vector2(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y);


        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;
        detectionEvent.OnPlayerNotDetectedAnymore += OnPlayerNotDetectedAnymoreHandler;
        StartCoroutine(moveToNextTarget());
    }
    void Update()
    {
        boxBounds = _movementZone.bounds;
        topRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
        bottomLeft = new Vector2(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y);
    }

    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {       
        attackPlayer = true;
        player = e.player.transform;
    }
    private void OnPlayerNotDetectedAnymoreHandler(object sender, MonsterDetectionEvent.PlayerNotDetectedAnymoreEventArgs e)
    {
        attackPlayer = false;
    }

    protected virtual IEnumerator moveToNextTarget()
    {
        if (attackPlayer)
        {
            float moveDuration = Vector2.Distance(_movingObject.transform.position, player.position) * _TimeToTargetPlayer;
            IEnumerator attack = Movements.Move(_movingObject.transform, player, _moveCurve, moveDuration, 1.5f, 2);
            StartCoroutine(attack);
            yield return new WaitForSeconds(moveDuration);
            StartCoroutine(moveToNextTarget());
        }
        else
        {
            GenerateRandomTarget();

            float moveDuration = Vector2.Distance(_movingObject.transform.position, _target.transform.position) * _TimeToTarget;
            IEnumerator move = Movements.Move(_movingObject.transform, _target.transform, _moveCurve, moveDuration);
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

    void GenerateRandomTarget()
    {
        GameObject targetGO = new GameObject();
        Destroy(_target);
        _target = Instantiate(targetGO, new Vector2(UnityEngine.Random.Range(bottomLeft.x, topRight.x), UnityEngine.Random.Range(topRight.y, bottomLeft.y)), Quaternion.identity, transform);
        _target.name = "target";
        Destroy(targetGO);
    }
}
