using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillByColorItem : BoosterItem
{
    public void Start()
    {
        DataManager.Ins.OnBoosterFillByColorQuantityChanged += SetQuantityText;
    }
    public override void OnInit()
    {
       
    }
    public override void OnClick()
    {
        if (DataManager.Ins.playerData.boosterFillByColorQuantity <= 0)
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
        BoosterManager.Ins.IsCanUseFillByNumberBooster = false;

       
    }
    
    public override void TurnOn()
    {
        base.TurnOn();
        ItemManager.Ins.TurnOffAllColorItem();
        BoosterManager.Ins.IsCanUseFillByNumberBooster = true;
        BoosterManager.Ins.IsCanUseFillAllNumberBooster = false;
        BoosterManager.Ins.IsCanUseZoomBooster = false;

        LevelManager.Ins.ReleaseFocusCube();
    }
  

}
