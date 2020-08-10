using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class LegAttack : MonoBehaviour
{
    [SerializeField] SpiderDetectionsEvent detectionEvent;
    [SerializeField] LimbSolver2D _target;

    [SerializeField] AnimationCurve _preparingAttackCurve;
    [SerializeField] AnimationCurve _attackingCurve;   
    [SerializeField] AnimationCurve _recoveringCurve;
    [SerializeField] AnimationCurve _breathCurve;

    [SerializeField] Transform anticipationPosition;
    [SerializeField] Transform basePosition;
    [SerializeField] bool LeftLeg;
    bool attacking;
    float breathTimer;

    // Start is called before the first frame update
    void Start()
    {       
        detectionEvent.OnPlayerIsNextBy += OnPlayerIsNextByHandler;
    }
    private void Update()
    {
        if (!attacking)
        {
            breathTimer += Time.deltaTime;
            if (breathTimer > _breathCurve.keys[_breathCurve.length - 1].time)
            {
                breathTimer = 0;
            }
            _target.transform.position = new Vector3(_target.transform.position.x, basePosition.position.y + _breathCurve.Evaluate(breathTimer));
        }
    }
    private void OnPlayerIsNextByHandler(object sender, SpiderDetectionsEvent.PlayerIsNextByEventArgs e)
    {
        if((e.direction == Vector2.left && LeftLeg && !attacking) || (e.direction == Vector2.right && !LeftLeg && !attacking))
            StartCoroutine(PrepareAttack(e.player.transform));
    }
    IEnumerator PrepareAttack(Transform targetToHit)
    {
        attacking = true;
        float timer = 0;
        Vector2 startPos = _target.transform.position;
        float animDuration = _preparingAttackCurve.keys[_preparingAttackCurve.length - 1].time;
        while (timer < animDuration)
        {
            timer += Time.deltaTime;          
            _target.transform.position = Vector2.Lerp(startPos, anticipationPosition.transform.position, _preparingAttackCurve.Evaluate(timer));
            yield return null;
        }
        StartCoroutine(Attack(targetToHit));
    }

    IEnumerator Attack(Transform targetToHit)
    {
        float timer = 0;
        Vector2 startPos = _target.transform.position;
        float animDuration = _attackingCurve.keys[_attackingCurve.length - 1].time;
        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            _target.transform.position = Vector2.Lerp(startPos, targetToHit.position, _preparingAttackCurve.Evaluate(timer));
            yield return null;
        }
        StartCoroutine(Recover());
    }
    IEnumerator Recover()
    {
        float timer = 0;
        Vector2 startPos = _target.transform.position;
        float animDuration = _attackingCurve.keys[_attackingCurve.length - 1].time;
        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            _target.transform.position = Vector2.Lerp(startPos, basePosition.position, _recoveringCurve.Evaluate(timer));
            yield return null;
        }
        attacking = false;
    }
}


