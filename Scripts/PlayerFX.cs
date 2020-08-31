using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerEvent))]
public class PlayerFX : MonoBehaviour
{
    PlayerEvent playerEvent;
    [SerializeField] ParticleSystem playerHitParticles;


    void Start()
    {
        playerEvent = GetComponent<PlayerEvent>();
        playerEvent.OnPlayerGotHit += OnPlayerGotHitHandler;
    }

    private void OnPlayerGotHitHandler(object sender, PlayerEvent.PlayerGotHitEventArgs e)
    {

        ParticleSystem fx = Instantiate(playerHitParticles, e.collisionPosition, Quaternion.identity, transform);
        Destroy(fx, playerHitParticles.main.duration);
    }
}
