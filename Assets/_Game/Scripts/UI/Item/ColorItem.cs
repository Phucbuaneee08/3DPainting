using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ColorItemState { Default = 0, IsSelected =1 }

public class ColorItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private int colorID;
    [SerializeField] private Text text;
    [SerializeField] private RectTransform element;
    [SerializeField] private ColorItemState colorItemState;
    private float moveUpDistance = 50f;
    private float duration = 0.5f;
    private UIGameplay uIGameplay;


    public void FocusCubeByColorID()
    {
        if (LevelManager.Ins.currentColor == colorID) return;
        SetMovePosition();
        LevelManager.Ins.FocusByColorID(colorID);
    }
    public void SetData(int colorID,Color color) 
    { 
        this.colorID = colorID;
        bg.color = color;
        text.text = colorID.ToString();
        colorItemState = ColorItemState.Default;
    }
    public int GetColorID()
    {
        return colorID;
    }
    public void SetMovePosition()
    {
        switch (colorItemState)
        {
            case ColorItemState.Default:
                if (UIManager.Ins.GetUI<UIGameplay>().fillBoosterItem.IsState(FillBoosterState.TurnOn))
                    UIManager.Ins.GetUI<UIGameplay>().fillBoosterItem.ChangeBoosterItemState();
                colorItemState = ColorItemState.IsSelected;
                element.DOMoveY(element.position.y + moveUpDistance, duration);
         
                break;
            case ColorItemState.IsSelected:
                colorItemState= ColorItemState.Default;
                element.DOMoveY(element.position.y - moveUpDistance, duration);
                Debug.Log(element.position.y);
                break;
        }
    }
    public void SetFillAmount(float amount)
    {

        bg.fillAmount = amount;
    }

}
