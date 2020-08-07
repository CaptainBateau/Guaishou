using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class spiderMovement : MonoBehaviour
{
    public List<TargetStep> leftTargetSteps;
    public List<TargetStep> rightTargetSteps;
    public float stepShift = 2f;
    public float speed;
    public AnimationCurve breathCurve;
    public float distanceToChangeDistance = 3f;
    public float distanceToGround = 2f;
    float breathTimer;
    Vector2 dir = Vector2.left;
    float heightPosition;
    [SerializeField] bool inversed;

    private void Start()
    {
        if (inversed)
            dir = Vector2.right;
    }
    void Update()
    {
        CheckHeightPosition();
        CheckWall();
        Breath();
        Move();
    }
    private void CheckHeightPosition()
    {
        RaycastHit2D hit;
        if (inversed)
        {
            hit = Physics2D.Raycast(new
        Vector2(transform.position.x, transform.position.y),
        Vector2.up, 50f, LayerMask.GetMask("Ground"));
        }
        else
        {
            hit = Physics2D.Raycast(new
        Vector2(transform.position.x, transform.position.y),
        Vector2.down, 50f, LayerMask.GetMask("Ground"));
        }
        
        float projectedHeight = -1;
        if (hit)
        {
            if(inversed)
                projectedHeight = hit.point.y - distanceToGround;
            else
                projectedHeight = hit.point.y + distanceToGround;
            heightPosition = Mathf.Lerp(heightPosition, projectedHeight, 0.1f);
        }
    }
    private void Breath()
    {
        breathTimer += Time.deltaTime;
        if (breathTimer > breathCurve.length)
        {
            breathTimer = 0;
        }
        transform.position = new Vector3(transform.position.x, heightPosition + breathCurve.Evaluate(breathTimer));
    }
    private void CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(transform.position.x, transform.position.y),
        dir, distanceToChangeDistance, LayerMask.GetMask("Ground"));
        if (hit)
        {
            if (dir == Vector2.left)
            {
                foreach (TargetStep target in rightTargetSteps)
                {
                    target.transform.position = new Vector3(target.transform.position.x + stepShift, target.transform.position.y, target.transform.position.y);
                }
                foreach (TargetStep target in leftTargetSteps)
                {
                    target.transform.position = new Vector3(target.transform.position.x + stepShift, target.transform.position.y, target.transform.position.y);
                }
                dir = Vector2.right;
            }
            else
            {
                foreach (TargetStep target in rightTargetSteps)
                {
                    target.transform.position = new Vector3(target.transform.position.x - stepShift, target.transform.position.y, target.transform.position.y);
                }
                foreach (TargetStep target in leftTargetSteps)
                {
                    target.transform.position = new Vector3(target.transform.position.x - stepShift, target.transform.position.y, target.transform.position.y);
                }
                dir = Vector2.left;
            }
        }
    }
    private void Move()
    {
        if (dir == Vector2.left)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, dir * distanceToChangeDistance);
    }
}
