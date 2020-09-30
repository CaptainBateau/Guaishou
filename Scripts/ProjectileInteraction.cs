using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileInteraction : MonoBehaviour
{
    bool _activable = true;
    public int _healthPoint = 1;
    public bool _oneTimeEffect = false;
    public bool _destroyProjectileOnCollision = true;
    public UnityEvent _activation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckActivation(collision.gameObject);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckActivation(collision.collider.gameObject);
    }

    private void CheckActivation(GameObject projectile)
    {
        if (projectile.tag == "Projectile")
        {
            if (_activable)
            {
                _healthPoint--;
                if (_healthPoint <= 0)
                {
                    _activation.Invoke();
                    if (_oneTimeEffect)
                        _activable = false;
                }
                if (_destroyProjectileOnCollision)
                    Destroy(projectile);
            }
        }       
    }
}


