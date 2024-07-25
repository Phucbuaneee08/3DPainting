using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Loading : UICanvas
{
    public RectTransform tfImg;
    public float targetWidth = 400f;
    private float initialWidth;

    public override void Open()
    {
        base.Open();
        tfImg.anchorMin = new Vector2(0.5f, tfImg.anchorMin.y);
        tfImg.anchorMax = new Vector2(0.5f, tfImg.anchorMax.y);
        tfImg.pivot = new Vector2(0, tfImg.pivot.y);
        RunSlider();
    }

    public void RunSlider()
    {
        initialWidth = tfImg.sizeDelta.x;
        float duration = 2f;
        DOVirtual.Float(0, 1, duration, (value) =>
        {
            float newWidth = Mathf.Lerp(initialWidth, targetWidth, value);
            tfImg.sizeDelta = new Vector2(newWidth, tfImg.sizeDelta.y);
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
