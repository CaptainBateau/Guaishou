using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerEvent))]
public class PlayerAudio : AudioManager
{
    PlayerEvent playerEvent;

    [SerializeField] Sound lightIntensityUp = null;
    [SerializeField] Sound lightIntensityDown = null;
    [SerializeField] Sound openDoor = null;
    [SerializeField] Sound breakDoor = null;
    [SerializeField] Sound playerHit = null;
    [SerializeField] Sound playerReload = null;
    [SerializeField] Sound playerShoot = null;
    [SerializeField] Sound playerStep = null;
    [SerializeField] Sound playerStepInside = null;
    [SerializeField] Sound playerTouchVegetation = null;
    



    private void Start()
    {
        playerEvent = GetComponent<PlayerEvent>();
        playerEvent.OnLightItensityChange += OnLightIntensityChange;
        playerEvent.OnOpenDoor += OnOpenDoor;
        playerEvent.OnPlayerGotHit += OnPlayerGotHit;
        playerEvent.OnPlayerReload += OnPlayerReload;
        playerEvent.OnPlayerShoot += OnpLayerShoot;
        playerEvent.OnPlayerStep += OnPlayerStep;
        playerEvent.OnPlayerStepInside += OnPlayerStepInside;
        playerEvent.OnPlayerTouchVegetation += OnPlayerTouchVegetaion;
    }

    private void OnLightIntensityChange(object sender, PlayerEvent.LightItensityChangeEventArgs e)
    {
        if (e.lowerIntensity)
            Play(lightIntensityDown);
        else
            Play(lightIntensityUp);
    }

    private void OnOpenDoor(object sender, PlayerEvent.OpenDoorEventArgs e)
    {
        if (e.destroyDoor)
            Play(breakDoor);
        else
            Play(openDoor);
    }

    private void OnPlayerGotHit(object sender, PlayerEvent.PlayerGotHitEventArgs e)
    {
        Play(playerHit);
    }

    private void OnPlayerReload(object sender, PlayerEvent.PlayerReloadEventArgs e)
    {
        Play(playerReload);
    }

    private void OnpLayerShoot(object sender, PlayerEvent.PlayerShootEventArgs e)
    {
        Play(playerShoot);
    }

    private void OnPlayerStep(object sender, PlayerEvent.PlayerStepEventArgs e)
    {
        Play(playerStep);
    }
    private void OnPlayerStepInside(object sender, PlayerEvent.PlayerStepInsideEventArgs e)
    {
        Play(playerStepInside);
    }

    private void OnPlayerTouchVegetaion(object sender, PlayerEvent.PlayerTouchVegetationEventArgs e)
    {
        Play(playerTouchVegetation);
    }
}
