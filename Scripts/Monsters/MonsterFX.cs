using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterDetectionEvent))]
public class MonsterFX : MonoBehaviour
{
    MonsterDetectionEvent detectionEvent;
    [SerializeField] float dissolveDuration = 1;
    private void Start()
    {
        detectionEvent = GetComponent<MonsterDetectionEvent>();
        detectionEvent.OnMonsterDie += OnMonsterDie;
    }

    private void OnMonsterDie(object sender, MonsterDetectionEvent.MonsterDieEventArgs e)
    {
        Debug.Log("shader anim");
        SpriteRenderer[] spriteRends = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in spriteRends)
        {
            StartCoroutine(DissolveAnim(rend));
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
