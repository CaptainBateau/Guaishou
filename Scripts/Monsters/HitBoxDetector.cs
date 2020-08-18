using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxDetector : MonoBehaviour
{
    MonsterDetectionEvent _eventDetection;
    MonsterDetectionEvent.HitBox _hitbox;

    public void Initialize(MonsterDetectionEvent eventDetection, MonsterDetectionEvent.HitBox hitbox)
    {
        _eventDetection = eventDetection;
        _hitbox = hitbox;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Projectile")
        {
            _eventDetection.MonsterHit(new MonsterDetectionEvent.MonsterHitEventArgs { hitbox = _hitbox });
            Destroy(collision.collider.gameObject);
        }
            
    }
}
