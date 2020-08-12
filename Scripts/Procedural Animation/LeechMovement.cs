using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechMovement : MonoBehaviour
{
    public float speed;
    public float distanceToGround = 0;
    public float heightPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckHeightPosition();
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
        transform.position = new Vector3(transform.position.x, heightPosition, transform.position.y);
    }

    private void CheckHeightPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(transform.position.x, transform.position.y + 5),
        Vector2.down, 50f, LayerMask.GetMask("Ground"));
        //float projectedHeight = -1;
        if (hit)
        {
            heightPosition = hit.point.y + distanceToGround;
        }
    }
}
