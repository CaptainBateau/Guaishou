using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingMonsterBehavior : TentacleMovement
{
    [Header("References")]
    [SerializeField] Transform _body;
    [SerializeField] Transform _basePosition;

    [Header("Parameters")]
    [SerializeField] AnimationCurve _floatCurve = null;
    [SerializeField] float breathDuration = 5;
    [SerializeField] [Range(0, 100)] float chanceToGoToInterestPoints;
    [SerializeField] [Range(0, 10)] float timeStayingAtInterestPoints;
    [SerializeField] List<Transform> _interestPoints;

    float breathTimer;

    // Update is called once per frame
    void Update()
    {
        breathTimer += Time.deltaTime;
        if (breathTimer > breathDuration)
        {
            breathTimer = 0;
        }
        _body.transform.position = new Vector3(_body.transform.position.x, _basePosition.position.y + _floatCurve.Evaluate(breathTimer / breathDuration));
    }

    protected override IEnumerator moveToNextTarget()
    {
        float rng = Random.Range(0, 100);
        if (rng <= chanceToGoToInterestPoints && _interestPoints.Count > 0 && !attackPlayer)
        {
            Transform targetPoint = _interestPoints[0];
            bool targetFound = false;
            if (_interestPoints.Count == 1)
            {
                targetFound = true;
            }
            int iterationDebug = 0;
            while (!targetFound)
            {
                targetPoint = _interestPoints[Random.Range(0, _interestPoints.Count)];
                if (Vector2.Distance(targetPoint.position, _movingObject.position) > 0.1f)
                {
                    targetFound = true;
                }
                iterationDebug++;
                if (iterationDebug > 100)
                    targetFound = true;
            }
            float moveDuration = Vector2.Distance(_movingObject.transform.position, targetPoint.position) * _TimeToTarget;
            IEnumerator move = Movements.Move(_movingObject.transform, targetPoint, _moveCurve, moveDuration);
            StartCoroutine(move);
            yield return new WaitForSeconds(moveDuration);
            yield return new WaitForSeconds(timeStayingAtInterestPoints);
            StartCoroutine(moveToNextTarget());
        }
        else
        {
            yield return StartCoroutine(base.moveToNextTarget());
        }
        yield return null;
    }
}
