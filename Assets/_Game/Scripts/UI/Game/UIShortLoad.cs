using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShortLoad : UICanvas
{
    public CanvasGroup canvasGroup;
    public Transform layoutTF;
    public Transform loadingSpinner;
    public float animDuration;

    Action action;
    Action closeCallback;

    Tween fadeInTween;
    Tween fadeOutTween;
    Tween scaleInTween;
    Tween scaleOutTween;
    Tween rotationTween;
    float delay;
    bool autoClose = true;
    Func<bool> predicate;

    void Awake()
    {
        fadeInTween = canvasGroup.DOFade(1, animDuration)
            .From(0)
            .SetEase(Ease.InOutSine)
            .SetAutoKill(false)
            .OnComplete(() => scaleInTween.Restart())
            .Pause();
        scaleOutTween = layoutTF.DOScale(0, animDuration)
            .From(1)
            .SetEase(Ease.InBack)
            .SetAutoKill(false)
            .OnComplete(Close)
            .Pause();
        scaleInTween = layoutTF.DOScale(1, animDuration)
            .From(0)
            .SetEase(Ease.OutBack)
            .SetAutoKill(false)
            .OnComplete(Load)
            .Pause();
        fadeOutTween = canvasGroup.DOFade(0, animDuration)
            .From(1)
            .SetEase(Ease.InOutSine)
            .SetAutoKill(false)
            .OnComplete(base.CloseDirectly)
            .Pause();
        rotationTween = loadingSpinner.DORotate(new Vector3(0, 0, -360), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .Pause();
    }

    public override void Open()
    {
        base.Open();
        layoutTF.localScale = Vector3.zero;
        fadeInTween.Restart();
        rotationTween.Restart();
    }

    public override void CloseDirectly()
    {
        scaleOutTween.Restart();
    }

    void Load()
    {
        if (predicate != null)
        {
            IEnumerator Wait()
            {
                action?.Invoke();
                yield return new WaitUntil(predicate);
                predicate = null;
                CloseDirectly();
            }
            StartCoroutine(Wait());
        }
        else
        {
            Invoke(nameof(DoAction), delay);
        }
    }

    void DoAction()
    {
        if (action != null)
        {
            action.Invoke();
            if (autoClose)
            {
                CloseDirectly();
            }
        }
    }
    void Close()
    {
        closeCallback?.Invoke();
        closeCallback = null;
        fadeOutTween.Restart();
        rotationTween.Pause();
    }
    public UIShortLoad With(
        Action action = null,
        float delay = 0,
        bool autoClose = true,
        Action closeCallback = null
       )
    {
        this.action = action;
        this.autoClose = autoClose;
        this.delay = delay;
        this.closeCallback = closeCallback;
        return this;
    }
}
