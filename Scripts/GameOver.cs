using UnityEngine;

[RequireComponent(typeof(Animation))]
public class GameOver : MonoBehaviour
{
    PlayerEvent playerEvent;
    Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        playerEvent = FindObjectOfType<PlayerEvent>();
        playerEvent.OnPlayerGotHit += OnPlayerHit;
    }

    private void OnPlayerHit(object sender, PlayerEvent.PlayerGotHitEventArgs e)
    {
        Invoke("EndGame", anim.clip.length - 0.2f);
        anim.Play();       
    }

    void EndGame()
    {
        playerEvent.GameOver(new PlayerEvent.GameOverEventArgs { });
    }
}
