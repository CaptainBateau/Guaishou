using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStep : MonoBehaviour
{
    [SerializeField] bool inversed = false;
    [SerializeField] float maxStepHigh = 1.5f;
    float desiredYPosition;

    void Update()
    {
        // Only for leg on ground for now
        // Cast a ray
        RaycastHit2D hit;
        if (inversed)
        {
            hit = Physics2D.Raycast(new
            Vector2(transform.position.x, transform.position.y - maxStepHigh),
            Vector2.up, 12f, LayerMask.GetMask("Ground"));
        }
        else
        {
            hit = Physics2D.Raycast(new
            Vector2(transform.position.x, transform.position.y + maxStepHigh),
            Vector2.down, 12f, LayerMask.GetMask("Ground"));
        }
        

        // If we hit a collider, set the desiredYPosition to the hit Y point.        
        if (hit.collider != null)
        {
            desiredYPosition = hit.point.y;
        }
        else
        {
            desiredYPosition = transform.position.y;
        }

        transform.position = new Vector2(transform.position.x,
        desiredYPosition);
    }
}
