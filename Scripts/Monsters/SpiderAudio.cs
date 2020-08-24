using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAudio : AudioManager
{
    [SerializeField] protected MonsterDetectionEvent detectionEvent;

    [SerializeField] Sound step = null;
    [SerializeField] Sound aggro = null;
    // randomly played with a delay, need more var
    // Sound Idle;
    [SerializeField] Sound takeDamage = null;
    [SerializeField] Sound death = null;
    [SerializeField] Sound attack = null;
    public virtual void Start()
    {
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;
        detectionEvent.OnMonsterHit += OnMonsterHitHandler;
        detectionEvent.OnMonsterAttack += OnMonsterAttack;
        detectionEvent.OnMonsterDie += OnMonsterDie;
        detectionEvent.OnTakeStep += OnTakeStep;
    }

    private void OnTakeStep(object sender, MonsterDetectionEvent.TakeStepEventArgs e)
    {
        Play(step);
    }

    private void OnMonsterDie(object sender, MonsterDetectionEvent.MonsterDieEventArgs e)
    {
        Play(death);
    }

    private void OnMonsterAttack(object sender, MonsterDetectionEvent.MonsterAttackEventArgs e)
    {
        Play(attack);
    }

    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {
        Play(aggro);
    }


    private void OnMonsterHitHandler(object sender, MonsterDetectionEvent.MonsterHitEventArgs e)
    {
        Play(takeDamage);
    }
}
