using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BoosterItem : Item
{
    public TextMeshProUGUI quantityText;
    public BoosterType BoosterType;
    private int quantity;
    
    public void SetQuantity(int quantity)
    {
        this.quantity = quantity;
    }
    public virtual bool CheckQuantity => quantity > 0;

    public override void OnClick()
    {
      
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




}
