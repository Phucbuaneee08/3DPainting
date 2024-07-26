using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class ColorItem : Item
{
    [SerializeField] private Image bg;
    [SerializeField] private int colorID;
    [SerializeField] private TextMeshProUGUI text;

 
    public override void OnClick()
    {
        base.OnClick();
        if (LevelManager.Ins.currentColor == colorID) return;
        BoosterManager.Ins.ZoomBoosterByColor();

        ChangeItemState(ItemState.TurnOn);

        ItemManager.Ins.TurnOffColorItemExceptID(LevelManager.Ins.currentColor);
        ItemManager.Ins.TurnOffBoosterItemExceptEnum(BoosterType.FillByColor);
    }
   
    public void SetData(int colorID,Color color) 
    { 
        this.colorID = colorID;
        bg.color = color;
        text.text = colorID.ToString();
        CurrentItemState = ItemState.TurnOff;
    }
    public int GetColorID()
    {
        return colorID;
    }
    public override void TurnOff()
    {
        base.TurnOff();
        MoveDown();
    }
    public override void TurnOn() 
    { 
        base.TurnOn();
        if (BoosterManager.Ins.IsCanUseFillByNumberBooster)
        {
            LevelManager.Ins.OnChangeColor(bg.color);
        }
        LevelManager.Ins.FocusByColorId(colorID);
        MoveUp();
    }
    public void SetFillAmount(float amount)
    {

        bg.fillAmount = amount;
    }
    public Color GetItemColor()
    {
        return bg.color;
    }
   
}
