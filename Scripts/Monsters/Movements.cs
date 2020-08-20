using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Movements
{
    public static IEnumerator Move(Transform initialPos, Transform targetPos, AnimationCurve animCurve, float animDuration, float yOffset = 0, float randomOffset = 0f)
    {
        float timer = 0;
        Vector2 target;
        if (randomOffset != 0 && yOffset != 0)
            target = new Vector2(targetPos.position.x, targetPos.position.y + Random.Range(yOffset - randomOffset / 2, yOffset + randomOffset / 2));
        else
            target = targetPos.position;
        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            if (randomOffset == 0 && yOffset == 0)
                target = targetPos.position;

            initialPos.position = Vector2.Lerp(initialPos.position, target, animCurve.Evaluate(timer / animDuration));
            yield return null;
        }
    }
}
