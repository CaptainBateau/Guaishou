using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingNeck : MonoBehaviour
{
    [SerializeField] MonsterDetectionEvent detectionEvent = null;
    [SerializeField] Transform neckBase = null;
    [SerializeField] Transform targetPos = null;
    [SerializeField] AnimationCurve neckDownCurve = null;
    [SerializeField] float neckDownDuration = 0.2f;
    [SerializeField] AnimationCurve neckRecoverCurve = null;
    [SerializeField] float neckRecoverDuration = 0.2f;


    [SerializeField] Transform originalNeckPos = null;
    Vector2 _dir = Vector2.left;
    bool inMovement = false;

    void Start()
    {
        detectionEvent.OnShiftDirection += OnShiftDirection;
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;
        detectionEvent.OnPlayerNotDetectedAnymore += OnPlayerNotDetectedAnymoreHandler;
    }

    private void OnShiftDirection(object sender, MonsterDetectionEvent.ShiftDirectionEventArgs e)
    {
        _dir = e.newDir;
        if(_dir == Vector2.left)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(_dir == Vector2.right)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {
        StartCoroutine(NeckDown());
    }

    private void OnPlayerNotDetectedAnymoreHandler(object sender, MonsterDetectionEvent.PlayerNotDetectedAnymoreEventArgs e)
    {
        StartCoroutine(NeckRecover());
    }


    IEnumerator NeckDown()
    {
        while (inMovement)
        {
            yield return null;
        }
        inMovement = true;
        StartCoroutine(Movements.Move(neckBase, targetPos, neckDownCurve, neckDownDuration, 0, 0, true));
        yield return new WaitForSeconds(neckDownDuration);
        inMovement = false;
    }
    IEnumerator NeckRecover()
    {
        while (inMovement)
        {
            yield return null;
        }
        inMovement = true;
        StartCoroutine(Movements.Move(neckBase, originalNeckPos, neckRecoverCurve, neckRecoverDuration, 0, 0, true));
        yield return new WaitForSeconds(neckRecoverDuration);
        inMovement = false;
    }
}
