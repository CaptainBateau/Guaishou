using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofSpiderAudio : SpiderAudio
{
    [SerializeField] Sound neckMovement;

    public override void Start()
    {
        base.Start();
        detectionEvent.OnPlayerDetected += OnPlayerDetectedHandler;
    }

    private void OnPlayerDetectedHandler(object sender, MonsterDetectionEvent.PlayerDetectedEventArgs e)
    {
        Play(neckMovement);
    }
}
