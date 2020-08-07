using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDetectionsEvent : MonoBehaviour
{
    Vector2 dir;
    [SerializeField] float wallDetectionDistance;
    [SerializeField] float playerNextToDistance;

    private void Start()
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

    private void CheckPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(transform.position.x, transform.position.y),
        dir, playerNextToDistance, LayerMask.GetMask("Ground"));
        if (hit)
        {
            PlayerIsNextBy(new PlayerIsNextByEventArgs {});
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, dir * wallDetectionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), dir * playerNextToDistance);
    }

    public class ShiftDirectionEventArgs : EventArgs
    {
        public Vector2 newDir;
    }
    public event EventHandler<ShiftDirectionEventArgs> OnShiftDirection;
    public void ShiftDirection(ShiftDirectionEventArgs e) => OnShiftDirection?.Invoke(this, e);

    public class PlayerIsNextByEventArgs : EventArgs
    {
        
    }
    public event EventHandler<PlayerIsNextByEventArgs> OnPlayerIsNextBy;
    void PlayerIsNextBy(PlayerIsNextByEventArgs e) => OnPlayerIsNextBy?.Invoke(this, e);

    public class WallIsNextByEventArgs : EventArgs
    {

    }
    public event EventHandler<WallIsNextByEventArgs> OnWallIsNextBy;
    void WallIsNextBy(WallIsNextByEventArgs e) => OnWallIsNextBy?.Invoke(this, e);
}
