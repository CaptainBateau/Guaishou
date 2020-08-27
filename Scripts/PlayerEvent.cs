﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class PlayerEvent : MonoBehaviour
{
    private void Start()
    {
        OnGameOver += OnGameOverHandler;
    }

    private void OnGameOverHandler(object sender, GameOverEventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // je gère déjà la collision avec les monstres ici
    void OnCollisionEnter2D(Collision2D collision)
    {
        // layer 11 is monster
        if (collision.collider.gameObject.layer == 11)
        {
            PlayerGotHit(new PlayerGotHitEventArgs { });
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // layer 11 is monster
        if (collision.gameObject.layer == 11)
        {
            PlayerGotHit(new PlayerGotHitEventArgs { });
        }
    }

    public class PlayerStepEventArgs : EventArgs { }
    public event EventHandler<PlayerStepEventArgs> OnPlayerStep;
    public void PlayerStep(PlayerStepEventArgs e) => OnPlayerStep?.Invoke(this, e);

    public class PlayerStepInsideEventArgs : EventArgs { }
    public event EventHandler<PlayerStepInsideEventArgs> OnPlayerStepInside;
    public void PlayerStepInside(PlayerStepInsideEventArgs e) => OnPlayerStepInside?.Invoke(this, e);

    public class PlayerShootEventArgs : EventArgs { }
    public event EventHandler<PlayerShootEventArgs> OnPlayerShoot;
    public void PlayerShoot(PlayerShootEventArgs e) => OnPlayerShoot?.Invoke(this, e);

    public class PlayerReloadEventArgs : EventArgs { }
    public event EventHandler<PlayerReloadEventArgs> OnPlayerReload;
    public void PlayerReload(PlayerReloadEventArgs e) => OnPlayerReload?.Invoke(this, e);

    public class LightItensityChangeEventArgs : EventArgs {
        public bool lowerIntensity;
        // if false it will assume it higher intensity
    }
    public event EventHandler<LightItensityChangeEventArgs> OnLightItensityChange;
    public void LightItensityChange(LightItensityChangeEventArgs e) => OnLightItensityChange?.Invoke(this, e);

    public class PlayerTouchVegetationEventArgs : EventArgs { }
    public event EventHandler<PlayerTouchVegetationEventArgs> OnPlayerTouchVegetation;
    public void PlayerTouchVegetation(PlayerTouchVegetationEventArgs e) => OnPlayerTouchVegetation?.Invoke(this, e);

    public class OpenDoorEventArgs : EventArgs
    {
        public bool destroyDoor;
    }
    public event EventHandler<OpenDoorEventArgs> OnOpenDoor;
    public void OpenDoor(OpenDoorEventArgs e) => OnOpenDoor?.Invoke(this, e);

    public class PlayerGotHitEventArgs : EventArgs { }
    public event EventHandler<PlayerGotHitEventArgs> OnPlayerGotHit;
    public void PlayerGotHit(PlayerGotHitEventArgs e) => OnPlayerGotHit?.Invoke(this, e);

    public class GameOverEventArgs : EventArgs { }
    public event EventHandler<GameOverEventArgs> OnGameOver;
    public void GameOver(GameOverEventArgs e) => OnGameOver?.Invoke(this, e);
}
