﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterDetectionEvent))]
public class SpiderAudio : AudioManager
{
    protected MonsterDetectionEvent detectionEvent;

    [SerializeField] Sound step;
    [SerializeField] Sound aggro;
    // randomly played with a delay, need more var
    // Sound Idle;
    [SerializeField] Sound takeDamage;
    [SerializeField] Sound death;
    [SerializeField] Sound attack;
    public virtual void Start()
    {
        detectionEvent = GetComponent<MonsterDetectionEvent>();
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
