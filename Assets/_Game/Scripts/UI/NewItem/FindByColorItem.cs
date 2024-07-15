using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindByColorItem : BoosterItem
{
    public void Start()
    {
        DataManager.Ins.OnBoosterFindByColorQuantityChanged += SetQuantityText;
    }
    public override void OnClick()
    {

        if (DataManager.Ins.playerData.boosterFindByColorQuantity <= 0)
        {
            UIManager.Ins.OpenUI<UIBuyBooster>().SetData(BoosterType);
            GameManager.Ins.ChangeState(GameState.Pause);
        }
        else { 
            base.OnClick();
            if(LevelManager.Ins.currentColor!=0)
                BoosterManager.Ins.FindNextCubeByColor(LevelManager.Ins.currentColor);
        }
    }
  
}


