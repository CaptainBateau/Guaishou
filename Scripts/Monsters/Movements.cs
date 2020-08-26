using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Movements
{
    public static IEnumerator Move(Transform initialPos, Transform targetPos, AnimationCurve animCurve, float animDuration, float yOffset = 0, float randomOffset = 0f, bool updateStartPos = false)
    {
        float timer = 0;
        Vector2 target;
        Vector3 startPos = initialPos.position;
        if (randomOffset != 0 || yOffset != 0)
            target = new Vector2(targetPos.position.x, targetPos.position.y + Random.Range(yOffset - randomOffset / 2, yOffset + randomOffset / 2));
        else
            target = targetPos.position;
        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            if (randomOffset == 0 && yOffset == 0)
                target = targetPos.position;
            if(updateStartPos)
                initialPos.position = Vector2.Lerp(initialPos.position, target, animCurve.Evaluate(timer / animDuration));
            else
                initialPos.position = Vector2.Lerp(startPos, target, animCurve.Evaluate(timer / animDuration));
            yield return null;
        }
    }
}
