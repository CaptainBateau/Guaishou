using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceShiftTowardDirection : MonoBehaviour
{
    [SerializeField] MonsterDetectionEvent detectionEvent;
    [SerializeField] Transform leftDirPos;
    [SerializeField] Transform rightDirPos;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] float animDuration;


    // Start is called before the first frame update
    void Start()
    {
        detectionEvent.OnShiftDirection += OnShiftDirectionHandler;
    }

    private void OnShiftDirectionHandler(object sender, MonsterDetectionEvent.ShiftDirectionEventArgs e)
    {
        if (e.newDir == Vector2.left)
            StartCoroutine(Movements.Move(transform, leftDirPos, animCurve, animDuration));
        if(e.newDir == Vector2.right)
            StartCoroutine(Movements.Move(transform, rightDirPos, animCurve, animDuration));
    }
}
