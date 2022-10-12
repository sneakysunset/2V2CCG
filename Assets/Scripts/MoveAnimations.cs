using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class MoveAnimations
{
    public static IEnumerator LerpToAnchor(Vector3 startPos, Vector3 endPos, AnimationCurve animCurve, Transform ItemToMove, float timeToComplete, UnityEvent endOfAnimEvent = default)
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime * (1 / timeToComplete);
            ItemToMove.position = Vector3.Lerp(startPos, endPos, animCurve.Evaluate(i));
            yield return null;
        }
        ItemToMove.position = endPos;
        endOfAnimEvent?.Invoke();
        yield return null;
    }


    public static IEnumerator LerpToScaling(Vector3 startScale, Vector3 endScale, AnimationCurve animCurve, Transform ItemToMove, float timeToComplete, UnityEvent endOfAnimEvent = default)
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime * (1 / timeToComplete);
            ItemToMove.localScale = Vector3.Lerp(startScale, endScale, animCurve.Evaluate(i));
            yield return null;
        }
        ItemToMove.localScale = endScale;
        endOfAnimEvent?.Invoke();
        yield return null;
    }
} 
