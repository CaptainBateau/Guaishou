using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegStep : MonoBehaviour
{
    [Header("Reference")]
    public Transform foot;

    public Transform currentTarget;
    public TargetStep desiredTarget;

    [Header("Parameters")]
    public AnimationCurve yCurve;
    public float distanceToStep = 1;
    public float speed = 10;

    float timer;
    bool moving;

    // have a button generating currentTarget;
    void Update()
    {
        // make the feet stay at their place when we move the body
        //foot.transform.position = Vector2.MoveTowards(foot.transform.position, currentTarget.position, speed * Time.deltaTime);


        float distToTarget = Vector2.Distance(foot.position, currentTarget.position);
        // Move the foot
        if (distToTarget > 0.1f)
        {
            if (!moving)
            {
                timer = 0;
                moving = true;
            }
            
            timer += Time.deltaTime;
            // Move towards desired target position
            foot.position = Vector2.MoveTowards(foot.position, new Vector2(currentTarget.position.x, currentTarget.position.y + yCurve.Evaluate(timer)), speed * Time.deltaTime);

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
