using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoosterItem : Item
{
  
    public BoosterType BoosterType;
    public int quantity;
  
    public virtual bool CheckQuantity => quantity > 0;

    public override void OnClick()
    {
        if (DataManager.Ins.playerData.boosterQuantity <= 0)
        {
            UIManager.Ins.OpenUI<UIBuyBooster>().SetData(_id);
            GameManager.Ins.ChangeState(GameState.Pause);
        }
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
        ItemManager.Ins.TurnOffAllColorItem();

        MoveUp();

    }
    public override void TurnOff()
    {
        base.TurnOff();
        BoosterManager.Ins.SelectedBoosterType = BoosterType.None;
        MoveDown();

    }



}
