using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Loading : UICanvas
{
    public Image imgFill; 
    public float duration = 2f;

    public override void Open()
    {
        base.Open();
        imgFill.fillAmount = 0;
        RunSlider();
    }

    public void RunSlider()
    {
        DOVirtual.Float(0, 1, duration, (value) =>
        {
            imgFill.fillAmount = value;
        }).SetEase(Ease.InCubic)
        .OnComplete(() =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManager.Ins.CloseUI<Loading>();
                UIManager.Ins.OpenUI<MainMenu>();
            });
        });
    }

    public void CacheSomeUIs()
    {

        UIManager.Ins.OpenUI<MainMenu>();
        UIManager.Ins.CloseUI<MainMenu>();
    }
}
