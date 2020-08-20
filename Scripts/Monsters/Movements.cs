using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Movements
{
    public static IEnumerator Move(Transform initialPos, Transform targetPos, AnimationCurve animCurve, float animDuration)
    {
        float timer = 0;
        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            initialPos.position = Vector2.Lerp(initialPos.position, targetPos.position, animCurve.Evaluate(timer / animDuration));
            yield return null;
        }
    }
}
