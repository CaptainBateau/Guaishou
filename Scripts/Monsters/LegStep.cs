using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegStep : MonoBehaviour
{
    [Header("References")]
    public Transform foot;
    MonsterDetectionEvent detectionEvent;
    public Transform currentTarget;
    public TargetStep desiredTarget;

    [Header("Parameters")]
    public AnimationCurve yCurve;
    public float stepDuration;
    public float distanceToStep = 1;


    [Header("Editing Not Necessary")]
    public float initialDistanceToStep;
    public float initialStepDuration;
    float timer = 0;
    Vector2 startPos;
    bool moving;
    private void Start()
    {
        detectionEvent = gameObject.GetComponentInParent(typeof(MonsterDetectionEvent)) as MonsterDetectionEvent;
        if (detectionEvent == null)
            Debug.LogWarning(this + " doesn't have a MonsterDetectionEvent in its parent, footstep won't be registered in events");
        initialDistanceToStep = distanceToStep;
        initialStepDuration = stepDuration;
        startPos = foot.position;
        currentTarget.position = foot.position;
    }

    // have a button generating currentTarget;
    void Update()
    {
        // make the feet stay at their place when we move the body
        //foot.transform.position = Vector2.MoveTowards(foot.transform.position, currentTarget.position, speed * Time.deltaTime);


        float distToTarget = Vector2.Distance(foot.position, currentTarget.position);
        // Move the foot
        if (distToTarget > 0.05f)
        {
            if (!moving)
            {
                startPos = foot.position;
                timer = 0;
                moving = true;
            }

            if (timer < stepDuration && moving)
            {
                foot.position = Vector2.Lerp(startPos, new Vector2(currentTarget.position.x, currentTarget.position.y + yCurve.Evaluate(timer / stepDuration)), timer / stepDuration);
                timer += Time.deltaTime;
            }
            else
            {
                detectionEvent.TakeStep(new MonsterDetectionEvent.TakeStepEventArgs { });
                foot.position = currentTarget.position;
                moving = false;
            }
        }
        else
        {
            moving = false;
            foot.position = currentTarget.position;
        }


        // Update current target if desiredTarget is far enough
        float desiredTargetDist = Vector2.Distance(currentTarget.position, desiredTarget.transform.position);
        if (desiredTargetDist > distanceToStep)
        {
            currentTarget.transform.position = desiredTarget.transform.position;
        }
    }
}
