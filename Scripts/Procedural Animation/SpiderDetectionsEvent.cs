using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDetectionsEvent : MonoBehaviour
{
    Vector2 dir;
    [SerializeField] float wallDetectionDistance;
    [SerializeField] float playerNextToDistance;
    [SerializeField] float playerDetectionDistance;

    private void Awake()
    {
        OnShiftDirection += OnShiftDirectionHandler;
    }

    private void OnShiftDirectionHandler(object sender, ShiftDirectionEventArgs e)
    {
        dir = e.newDir;
    }

    void Update()
    {
        CheckWall();
        CheckPlayerNext();
        CheckPlayerDetection();
    }

    private void CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(transform.position.x, transform.position.y),
        dir, wallDetectionDistance, LayerMask.GetMask("Ground"));
        if (hit)
        {
            WallIsNextBy(new WallIsNextByEventArgs {});
        }
    }

    private void CheckPlayerNext()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(transform.position.x, transform.position.y),
        dir, playerNextToDistance, LayerMask.GetMask("Default"));
        if (hit && hit.transform.tag == "Player")
        {
            PlayerIsNextBy(new PlayerIsNextByEventArgs { player = hit.collider, direction = dir});
        }
    }
    private void CheckPlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(transform.position.x - playerDetectionDistance, transform.position.y),
        Vector2.right, playerDetectionDistance * 2, LayerMask.GetMask("Default"));
        if (hit && hit.transform.tag == "Player")
        {
            PlayerDetected(new PlayerDetectedEventArgs { player = hit.collider, direction = dir});
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, dir * wallDetectionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), dir * playerNextToDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(new Vector3(transform.position.x - playerDetectionDistance, transform.position.y - 0.2f, transform.position.z), Vector2.right * playerDetectionDistance * 2);
    }

    public class ShiftDirectionEventArgs : EventArgs
    {
        public Vector2 newDir;
    }
    public event EventHandler<ShiftDirectionEventArgs> OnShiftDirection;
    public void ShiftDirection(ShiftDirectionEventArgs e) => OnShiftDirection?.Invoke(this, e);

    public class PlayerIsNextByEventArgs : EventArgs
    {
        public Collider2D player;
        public Vector2 direction;
    }
    public event EventHandler<PlayerIsNextByEventArgs> OnPlayerIsNextBy;
    void PlayerIsNextBy(PlayerIsNextByEventArgs e) => OnPlayerIsNextBy?.Invoke(this, e);
    
    public class PlayerDetectedEventArgs : EventArgs
    {
        public Collider2D player;
        public Vector2 direction;
    }
    public event EventHandler<PlayerDetectedEventArgs> OnPlayerDetected;
    void PlayerDetected(PlayerDetectedEventArgs e) => OnPlayerDetected?.Invoke(this, e);

    public class WallIsNextByEventArgs : EventArgs
    {

    }
    public event EventHandler<WallIsNextByEventArgs> OnWallIsNextBy;
    void WallIsNextBy(WallIsNextByEventArgs e) => OnWallIsNextBy?.Invoke(this, e);
}
