using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MonsterDetectionEvent))]
public class MonsterStatus : MonoBehaviour
{
    [SerializeField] int health = 500;
    MonsterDetectionEvent detectionEvent;
    public int Health { get => health; set { health = value; } }

    private void Start()
    {
        detectionEvent = GetComponent<MonsterDetectionEvent>();
        detectionEvent.OnMonsterHit += OnMonsterHitHandler;
    }

    private void OnMonsterHitHandler(object sender, MonsterDetectionEvent.MonsterHitEventArgs e)
    {
        Health -= e.hitbox.healthLost;

        //HurtColorFeedback(e.hitbox);


        if (Health < 0)
        {
            detectionEvent.MonsterDie(new MonsterDetectionEvent.MonsterDieEventArgs { });
            Collider2D[] cols = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D col in cols)
            {
                col.enabled = false;
            }
            detectionEvent.enabled = false;
            Destroy(gameObject, 1.5f);
        }
    }

    //private void HurtColorFeedback(MonsterDetectionEvent.HitBox hitbox)
    //{
    //    SpriteRenderer spriteRend = GetComponent<SpriteRenderer>();
    //    if (spriteRend != null && spriteRend.color != Color.red)
    //    {
    //        Color originalColor = spriteRend.color;
    //        spriteRend.color = Color.red;
    //        StartCoroutine(ChangeColor(spriteRend, originalColor, 0.05f));          
    //    }
    //}

    //IEnumerator ChangeColor(SpriteRenderer spriteRend, Color color, float delay)
    //{

    //    yield return new WaitForSeconds(delay);
    //    spriteRend.color = color;
    //    yield return null;
    //}
}
