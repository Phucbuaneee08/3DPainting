using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text amountTMP;
    [SerializeField] private Canvas canvas;

    private int oldValue;
    private Transform cachedTransform;
    private Tweener valueTweener;
    private Tweener scaleTweener;
    private Tweener colorTweener;

    private void Awake()
    {
        cachedTransform = transform;
    }

    public void SetActiveOverrideSorting(bool isActive)
    {
        canvas.overrideSorting = isActive;
    }

    public void ChangeValue(int newValue, float duration = 1f, UnityAction OnComplete = null)
    {
        CompleteAllTween();

        if (Mathf.Approximately(duration, 0f))
        {
            oldValue = newValue;
            amountTMP.text = newValue.ToString();
        }
        else
        {
            valueTweener = DOVirtual.Int(oldValue, newValue, duration, value => amountTMP.text = value.ToString());
            valueTweener.OnComplete(() =>
            {
                oldValue = newValue;
                OnComplete?.Invoke();
            });
        }
    }

    public void ChangeValueImmediately(int value)
    {
        CompleteAllTween();

        oldValue = value;
        amountTMP.text = value.ToString();
    }

    protected virtual void CompleteAllTween()
    {
        CompleteTween(valueTweener);
        CompleteTween(scaleTweener);
        CompleteTween(colorTweener);
    }

    protected void CompleteTween(Tweener tweener)
    {
        if (tweener.IsActive())
        {
            tweener.Complete();
        }
    }
}