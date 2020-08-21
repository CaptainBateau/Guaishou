using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechAudio : AudioManager
{
    MonsterDetectionEvent detectionEvent;

    [SerializeField] Sound squishStep = null;
    [SerializeField] Sound aggro = null;
    [SerializeField] Sound takeDamage = null;
    [SerializeField] Sound death = null;

    public virtual void Start()
    {
        detectionEvent = GetComponent<MonsterDetectionEvent>();
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;
        detectionEvent.OnMonsterHit += OnMonsterHitHandler;
        detectionEvent.OnMonsterDie += OnMonsterDie;
        detectionEvent.OnTakeStep += OnTakeStep;
    }

    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {
        Play(aggro);
    }

    private void OnMonsterHitHandler(object sender, MonsterDetectionEvent.MonsterHitEventArgs e)
    {
        Play(takeDamage);
    }

    private void OnMonsterDie(object sender, MonsterDetectionEvent.MonsterDieEventArgs e)
    {
        Play(death);
    }

    private void OnTakeStep(object sender, MonsterDetectionEvent.TakeStepEventArgs e)
    {
        Play(squishStep);
    }
}
