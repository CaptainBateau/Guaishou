using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingNeck : MonoBehaviour
{
    [SerializeField] MonsterDetectionEvent detectionEvent;
    [SerializeField] Transform neckBase = null;
    [SerializeField] Transform targetPos = null;
    [SerializeField] AnimationCurve neckDownCurve = null;
    [SerializeField] float neckDownDuration = 0.2f;
    [SerializeField] AnimationCurve neckRecoverCurve = null;
    [SerializeField] float neckRecoverDuration = 0.2f;


    [SerializeField] Transform originalNeckPos = null;

    void Start()
    {
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;
        detectionEvent.OnPlayerNotDetectedAnymore += OnPlayerNotDetectedAnymoreHandler;
    }

    
    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {
        Debug.Log("player detected");
        StartCoroutine(NeckDown());
    }

    private void OnPlayerNotDetectedAnymoreHandler(object sender, MonsterDetectionEvent.PlayerNotDetectedAnymoreEventArgs e)
    {
        Debug.Log("player not detected");
        StartCoroutine(NeckRecover());
    }


    IEnumerator NeckDown()
    {
        StartCoroutine(Movements.Move(neckBase, targetPos, neckDownCurve, neckDownDuration));
        yield return new WaitForSeconds(neckDownDuration);
    }
    IEnumerator NeckRecover()
    {
        StartCoroutine(Movements.Move(neckBase, originalNeckPos, neckRecoverCurve, neckRecoverDuration));
        yield return new WaitForSeconds(neckRecoverDuration);
    }
}
