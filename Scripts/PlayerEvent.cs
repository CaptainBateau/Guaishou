using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerEvent : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        // layer 11 is monster
        if (collision.collider.gameObject.layer == 11)
        {
            PlayerGotHit(new PlayerGotHitEventArgs { });
            Debug.Log("You got hit");
        }
    }

    public class PlayerGotHitEventArgs : EventArgs
    {

    }
    public event EventHandler<PlayerGotHitEventArgs> OnPlayerGotHit;
    void PlayerGotHit(PlayerGotHitEventArgs e) => OnPlayerGotHit?.Invoke(this, e);
}
