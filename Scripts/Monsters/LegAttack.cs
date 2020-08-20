using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.U2D.IK;

public class LegAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MonsterDetectionEvent detectionEvent;
    [SerializeField] LimbSolver2D _target;
    [SerializeField] Transform anticipationPosition;
    [SerializeField] Transform basePosition;

    [Header("Parameters")]
    [SerializeField] float _prepareDuration = 1;
    [SerializeField] AnimationCurve _preparingAttackCurve;
    [SerializeField] float _attackingDuration = 1;
    [SerializeField] float _playerYOffset = 1;
    [SerializeField] float _playerRandomHitRange = 0.5f;
    [SerializeField] AnimationCurve _attackingCurve;
    [SerializeField] float _recoveringDuration = 1;
    [SerializeField] AnimationCurve _recoveringCurve;
    [SerializeField] AnimationCurve _breathCurve;

    
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
    private void OnPlayerIsNextByHandler(object sender, MonsterDetectionEvent.PlayerIsNextByEventArgs e)
    {        
        if((e.direction == Vector2.left && LeftLeg && !attacking) || (e.direction == Vector2.right && !LeftLeg && !attacking))
        {
            attacking = true;
            StartCoroutine(PrepareAttack(e.player.transform));
        }
            
    }

    IEnumerator PrepareAttack(Transform targetToHit)
    {     
        StartCoroutine(Movements.Move(_target.transform, anticipationPosition.transform, _preparingAttackCurve, _prepareDuration));
        yield return new WaitForSeconds(_prepareDuration);
        StartCoroutine(Attack(targetToHit));
    }

    IEnumerator Attack(Transform targetToHit)
    {
        detectionEvent.MonsterAttack(new MonsterDetectionEvent.MonsterAttackEventArgs { });
        StartCoroutine(Movements.Move(_target.transform, targetToHit, _attackingCurve, _attackingDuration, _playerYOffset, _playerRandomHitRange));
        yield return new WaitForSeconds(_attackingDuration);
        StartCoroutine(Recover());
    }

    IEnumerator Recover()
    {
        StartCoroutine(Movements.Move(_target.transform, basePosition, _recoveringCurve, _recoveringDuration));
        yield return new WaitForSeconds(_recoveringDuration);
        attacking = false;
    }
    
}


