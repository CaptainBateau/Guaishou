using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class PlayerEvent : MonoBehaviour
{
    private CheckPointSystem _checkpoint;
    [SerializeField] float wallDetectionDistance = 0.65f;
    WeaponController _weaponController;
    Light2D _light2D;
    bool hit;
    int initialPelletNbr;
    private void Start()
    {
        _weaponController = GetComponentInChildren<WeaponController>();
        _light2D = GetComponentInChildren<Light2D>();
        initialPelletNbr = _weaponController._pelletNumber;
        _checkpoint = FindObjectOfType<CheckPointSystem>();
        OnGameOver += OnGameOverHandler;
        if(_checkpoint.checkPointDefined)
            transform.position = _checkpoint.lastCheckPointPos;
    }
    private void Update()
    {
        CheckWall();
    }
    private void OnGameOverHandler(object sender, GameOverEventArgs e)
    {
        transform.position = _checkpoint.lastCheckPointPos;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // je gère déjà la collision avec les monstres ici
    void OnCollisionEnter2D(Collision2D collision)
    {
        // layer 11 is monster
        if (collision.collider.gameObject.layer == 11 && !hit)
        {
            hit = true;
            PlayerGotHit(new PlayerGotHitEventArgs { collisionPosition = collision.contacts[0].point });          
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // layer 11 is monster
        if (collision.gameObject.layer == 11 && !hit)
        {
            hit = true;
            PlayerGotHit(new PlayerGotHitEventArgs { collisionPosition = collision.bounds.ClosestPoint(transform.position) });
        }
    }

    private void CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.2f),
        new Vector2(transform.right.x, transform.right.y) * transform.localScale, wallDetectionDistance, LayerMask.GetMask("Ground"));
        if (hit)
        {
            _weaponController._pelletNumber = 0;
            _light2D.enabled = false;
            PlayerInFrontOfWall(new PlayerInFrontOfWallEventArgs { });
        }
        else
        {
            _weaponController._pelletNumber = initialPelletNbr;
            _light2D.enabled = true;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y + 1.2f), new Vector2(transform.right.x, transform.right.y) * transform.localScale * wallDetectionDistance);
    }

    public class PlayerInFrontOfWallEventArgs : EventArgs { }
    public event EventHandler<PlayerInFrontOfWallEventArgs> OnPlayerInFrontOfWall;
    public void PlayerInFrontOfWall(PlayerInFrontOfWallEventArgs e) => OnPlayerInFrontOfWall?.Invoke(this, e);

    public class PlayerStepEventArgs : EventArgs { }
    public event EventHandler<PlayerStepEventArgs> OnPlayerStep;
    public void PlayerStep(PlayerStepEventArgs e) => OnPlayerStep?.Invoke(this, e);

    public class PlayerStepInsideEventArgs : EventArgs { }
    public event EventHandler<PlayerStepInsideEventArgs> OnPlayerStepInside;
    public void PlayerStepInside(PlayerStepInsideEventArgs e) => OnPlayerStepInside?.Invoke(this, e);


    public class PlayerStepMetalEventArgs : EventArgs { }
    public event EventHandler<PlayerStepMetalEventArgs> OnPlayerStepMetal;
    public void PlayerStepMetal(PlayerStepMetalEventArgs e) => OnPlayerStepMetal?.Invoke(this, e);
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

    public class PlayerGotHitEventArgs : EventArgs {
        public Vector2 collisionPosition;
    }
    public event EventHandler<PlayerGotHitEventArgs> OnPlayerGotHit;
    public void PlayerGotHit(PlayerGotHitEventArgs e) => OnPlayerGotHit?.Invoke(this, e);

    public class GameOverEventArgs : EventArgs { }
    public event EventHandler<GameOverEventArgs> OnGameOver;
    public void GameOver(GameOverEventArgs e) => OnGameOver?.Invoke(this, e);
}
