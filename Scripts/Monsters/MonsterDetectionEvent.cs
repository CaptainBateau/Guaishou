﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDetectionEvent : MonoBehaviour
{
    
    [Header("Parameters")]
    [SerializeField] bool showDebug;
    [SerializeField] bool _centerBetween2element;
    [SerializeField] Transform _firstElement;
    [SerializeField] Transform _secondElement;
    [SerializeField] Vector2 _offset;
    [SerializeField] Vector2 _playerDetectionOffset;
    [SerializeField] float wallDetectionDistance;
    [SerializeField] float playerNextToDistance;
    [SerializeField] float playerDetectionDistance;
    [SerializeField] List<HitBox> _hitBoxes = new List<HitBox>();

    [System.Serializable]
    public struct HitBox
    {
        public Collider2D collider;
        public int healthLost;
    }

    bool playerDetected;
    Vector2 dir = Vector2.left;
    public Vector3 _center;

    #region Unity Functions
    private void Awake()
    {
        OnShiftDirection += OnShiftDirectionHandler;
    }

    private void Start()
    {
        foreach (HitBox hitbox in _hitBoxes)
        {
            HitBoxDetector hitboxDetector = hitbox.collider.gameObject.AddComponent<HitBoxDetector>();
            hitboxDetector.Initialize(this, hitbox);
        }
    }

    void Update()
    {
        if (_centerBetween2element)
            _center = Vector3.Lerp(_firstElement.position, _secondElement.position, 0.5f);
        else
            _center = transform.position;

        CheckWall();
        CheckPlayerNext();
        CheckPlayerDetection();
    }

    #endregion

    #region Check Functions
    private void CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(_center.x, _center.y) + _offset,
        dir, wallDetectionDistance, LayerMask.GetMask("Ground"));
        if (hit)
        {
            WallIsNextBy(new WallIsNextByEventArgs {});
        }
    }
    private void CheckPlayerNext()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(_center.x, _center.y) + _offset + _playerDetectionOffset,
        dir, playerNextToDistance, LayerMask.GetMask("Player"));
        if (hit)
        {
            PlayerIsNextBy(new PlayerIsNextByEventArgs { player = hit.collider, direction = dir});
        }
    }
    private void CheckPlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(new
        Vector2(_center.x - playerDetectionDistance, _center.y) + _offset + _playerDetectionOffset,
        Vector2.right, playerDetectionDistance * 2, LayerMask.GetMask("Player"));
        if (hit && !playerDetected)
        {
            playerDetected = true;
            PlayerDetected(new PlayerDetectedEventArgs { player = hit.collider, direction = dir});
        }
        if (!hit && playerDetected)
        {
            playerDetected = false;
            PlayerNotDetectedAnymore(new PlayerNotDetectedAnymoreEventArgs {});
        }
    }

    #endregion
    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            if (_centerBetween2element)
                _center = Vector3.Lerp(_firstElement.position, _secondElement.position, 0.5f);
            else
                _center = transform.position;

            Gizmos.DrawRay(new Vector2(_center.x, _center.y) + _offset, dir * wallDetectionDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector2(_center.x, _center.y + 0.03f) + _offset + _playerDetectionOffset, dir * playerNextToDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(new Vector2(_center.x - playerDetectionDistance, _center.y - 0.03f) + _offset + _playerDetectionOffset, Vector2.right * playerDetectionDistance * 2);
        }       
    }

    private void OnShiftDirectionHandler(object sender, ShiftDirectionEventArgs e)
    {
        dir = e.newDir;
    }

    #region Events
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
    public class PlayerNotDetectedAnymoreEventArgs : EventArgs
    {
    }
    public event EventHandler<PlayerNotDetectedAnymoreEventArgs> OnPlayerNotDetectedAnymore;
    void PlayerNotDetectedAnymore(PlayerNotDetectedAnymoreEventArgs e) => OnPlayerNotDetectedAnymore?.Invoke(this, e);

    public class WallIsNextByEventArgs : EventArgs
    {

    }
    public event EventHandler<WallIsNextByEventArgs> OnWallIsNextBy;
    void WallIsNextBy(WallIsNextByEventArgs e) => OnWallIsNextBy?.Invoke(this, e);

    public class MonsterHitEventArgs : EventArgs
    {
        public HitBox hitbox;
    }
    public event EventHandler<MonsterHitEventArgs> OnMonsterHit;
    public void MonsterHit(MonsterHitEventArgs e) => OnMonsterHit?.Invoke(this, e);

    #endregion
}
