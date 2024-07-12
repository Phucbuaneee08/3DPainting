using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilllAllColorItem : BoosterItem
{
    public override void OnInit()
    {
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
