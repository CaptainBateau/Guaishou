﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DestroyableProps : MonoBehaviour
{
    [Header("DON'T FORGET THE COLLIDER 2D")]
    [SerializeField] bool destroyableByProjectile;
    [SerializeField] int healthPoint;
    [SerializeField] float dissolveDuration;

    SpriteRenderer rend;
    bool isDissolving;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (destroyableByProjectile && collision.collider.tag == "Projectile")
        {
            Destroy(collision.collider.gameObject);
            healthPoint--;
            if(healthPoint <= 0 && !isDissolving)
            {
                StartCoroutine(DissolveAnim(rend));
                isDissolving = true;                
                Destroy(collision.otherCollider);
                Destroy(gameObject, dissolveDuration);
            }            
        }
    }

    public void Dissolve()
    {
        StartCoroutine(DissolveAnim(rend));
        isDissolving = true;
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, dissolveDuration);
    }


    private IEnumerator DissolveAnim(SpriteRenderer rend)
    {
        float timer = 0;
        while (timer < dissolveDuration)
        {
            timer += Time.deltaTime;
            rend.material.SetFloat("_Fade", Mathf.Lerp(1, -1, timer / dissolveDuration));
            yield return null;
        }
    }
}
