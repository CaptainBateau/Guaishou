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
        anim.Play();
        Invoke("EndGame", anim.clip.averageDuration);
    }

    void EndGame()
    {
        playerEvent.GameOver(new PlayerEvent.GameOverEventArgs { });
    }
}
