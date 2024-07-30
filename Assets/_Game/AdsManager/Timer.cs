using System;
using System.Collections;
using UnityEngine;

public class Timer2
{
    public static void Schedule(MonoBehaviour mb, float delay, Action onComplete)
    {
        mb.StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return new WaitForSeconds(delay);
            onComplete?.Invoke();
        }
    }
}
