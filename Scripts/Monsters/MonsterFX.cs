using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterDetectionEvent))]
public class MonsterFX : MonoBehaviour
{
    MonsterDetectionEvent detectionEvent;
    [SerializeField] float dissolveDuration = 1;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] float scale = 1;
    [SerializeField] Transform parent;
    private void Start()
    {
        detectionEvent = GetComponent<MonsterDetectionEvent>();
        detectionEvent.OnMonsterDie += OnMonsterDie;
        detectionEvent.OnMonsterHit += OnMonsterHit;
    }

    private void OnMonsterHit(object sender, MonsterDetectionEvent.MonsterHitEventArgs e)
    {
        // spawn FX at location, fx depend on life lost
        if(hitParticles != null)
        {
            ParticleSystem fx = null;
            if(parent != null)
                fx = Instantiate(hitParticles, e.collisionPosition, Quaternion.identity, parent);
            else
                fx = Instantiate(hitParticles, e.collisionPosition, Quaternion.identity, transform);

            //float newScale = Mathf.Lerp(1, scaleFor20healthLost, (float)e.hitbox.healthLost / (float)20);
            fx.transform.localScale = new Vector3(scale, scale, scale);
            Destroy(fx, hitParticles.main.duration + 5f);
        }
        
    }

    private void OnMonsterDie(object sender, MonsterDetectionEvent.MonsterDieEventArgs e)
    {
        SpriteRenderer[] spriteRends = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in spriteRends)
        {
            StartCoroutine(DissolveAnim(rend));
        }
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            Destroy(particle, 0.5f);
        }
    }

    private IEnumerator DissolveAnim(SpriteRenderer rend)
    {
        float timer = 0;
        while(timer < dissolveDuration)
        {
            timer += Time.deltaTime;
            rend.material.SetFloat("_Fade", Mathf.Lerp(1, -1, timer / dissolveDuration));
            yield return null;
        }       
    }
}
