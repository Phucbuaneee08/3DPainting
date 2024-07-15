using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilllAllColorItem : BoosterItem
{
    public override void OnInit()
    {
    }
    public override void OnClick()
    {
        if (DataManager.Ins.playerData.boosterFillAllColorQuantity <= 0)
        {
            UIManager.Ins.OpenUI<UIBuyBooster>().SetData(BoosterType);
            GameManager.Ins.ChangeState(GameState.Pause);
        }
        else
            base.OnClick();
    }
    public override void TurnOff()
    {
        base.TurnOff();
        BoosterManager.Ins.IsCanUseZoomBooster = false;
        BoosterManager.Ins.IsCanUseFillAllNumberBooster = false;

    }

    public override void TurnOn()
    {
        base.TurnOn();
        ItemManager.Ins.TurnOffAllColorItem();

        BoosterManager.Ins.IsCanUseZoomBooster = true;
        BoosterManager.Ins.ZoomBoosterByColor();

        BoosterManager.Ins.IsCanUseFillAllNumberBooster = true;
        BoosterManager.Ins.IsCanUseFillByNumberBooster = false;


        if (LevelManager.Ins.currentColor != 0)
        {
            ItemManager.Ins.TurnOffAllColorItem();
        }

        LevelManager.Ins.ReleaseFocusCube();
    }
   
    
}
