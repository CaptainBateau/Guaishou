using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class PlayerEvent : MonoBehaviour
{

    // je gère déjà la collision avec les monstres ici
    void OnCollisionEnter2D(Collision2D collision)
    {
        // layer 11 is monster
        if (collision.collider.gameObject.layer == 11)
        {
            PlayerGotHit(new PlayerGotHitEventArgs { });
            Debug.Log("You got hit");
            // temporary restart
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
    }

    #region HOW TO CALL EVENT
    PlayerEvent playerEvent;
    private void Start()
    {
        // il faut une référence au player event, par l'inspecteur ou via getComponent comme tu veux

        // playerEvent.GetComponent<PlayerEvent>();
    }

    private void LightIntensityLow()
    {
        // appeler l'event à l'endroit où il doit être appelé, avec ses paramètre si nécessaire
        playerEvent.LightItensityChange(new LightItensityChangeEventArgs { lowerIntensity = true });
    }
    #endregion

    public class PlayerStepEventArgs : EventArgs
    {
        public bool indoor;
    }
    public event EventHandler<PlayerStepEventArgs> OnPlayerStep;
    public void PlayerStep(PlayerStepEventArgs e) => OnPlayerStep?.Invoke(this, e);

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
}
