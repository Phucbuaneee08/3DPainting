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
            DOVirtual.DelayedCall(0.5f, () =>
            {
                UIManager.Ins.CloseUI<Loading>();
                UIManager.Ins.GetUI<MainMenu>().Open();
            });
        });
    }

    public void CacheSomeUIs()
    {
        Debug.Log(1);
        UIManager.Ins.OpenUI<MainMenu>();
        UIManager.Ins.CloseUI<MainMenu>();
    }
}
