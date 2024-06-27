using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : UICanvas
{
    public Slider slider;

    public override void Open()
    {
        base.Open();
        RunSlider();
    }
    public void RunSlider()
    {
        float duration = 2f;
        DOVirtual.Float(0, 1, duration, (value) =>
        {
            slider.value = value;
        }).SetEase(Ease.InCubic)
        .OnComplete(() =>
        {
            slider.value = 1f;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                UIManager.Ins.CloseUI<Loading>();
                UIManager.Ins.OpenUI<UIMainMenu>();
            });
        });
    }
}
