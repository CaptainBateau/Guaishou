using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStep : MonoBehaviour
{
    public TargetStep desiredTarget;
    public AnimationCurve stepHighCurve;
    public float speed = 100;
    public float distanceToStep;
    bool moving;
    Vector2 currentTarget;
    float timer;
    void Update()
    {
        float dist;
        dist = Vector2.Distance(transform.position, desiredTarget.transform.position);

        if (dist > distanceToStep)
        {
            moving = true;
            currentTarget = desiredTarget.transform.position;
            
            // animation to make it to this point
            
            //transform.position = desiredTarget.transform.position;
        }
        if(moving)
        {
            if(Vector2.Distance(transform.position, currentTarget) < 0.01)
            {
                moving = false;
                timer = 0;
            }
            timer += Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentTarget.x, currentTarget.y + stepHighCurve.Evaluate(timer)), speed * Time.deltaTime);
        }
    }
}
