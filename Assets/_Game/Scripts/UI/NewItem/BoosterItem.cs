using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Paint3D;
using UnityEngine.UI;

public class BoosterItem : Item
{
    public string boosterName;
    public Image addBooster;
    public Image boosterQuantity;
    public TextMeshProUGUI quantityText;
    public BoosterType BoosterType;
    private int quantity;

    public void OnEnable()
    {
        CheckBoosterQuantity(0);
    }

    public void SetQuantity(int quantity)
    {
        this.quantity = quantity;
    }
    public virtual bool CheckQuantity => quantity > 0;

    public override void OnClick()
    {
        base.OnClick();
        if (CurrentItemState == ItemState.TurnOff)
        {
            ChangeItemState(ItemState.TurnOn);
        }
        else 
        {
            ChangeItemState(ItemState.TurnOff);
        }
    }
    public override void TurnOn()
    {
        base.TurnOn();

        BoosterManager.Ins.SelectedBoosterType = BoosterType;
        ItemManager.Ins.TurnOffBoosterItemExceptEnum(BoosterType);
        

        MoveUp();

    }
    public override void TurnOff()
    {
        base.TurnOff();
        BoosterManager.Ins.SelectedBoosterType = BoosterType.None;
        MoveDown();

    }
    public void SetQuantityText(int quantity)
    {
        quantityText.text = quantity.ToString();
    }

    public virtual void CheckBoosterQuantity(int quantity)
    {
        
    }



}
