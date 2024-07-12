using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillByColorItem : BoosterItem
{
    public override void OnInit()
    {
       
    }

    public override void TurnOff()
    {
        base.TurnOff();
        BoosterManager.Ins.IsCanUseFillByNumberBooster = false;

       
    }
    
    public override void TurnOn()
    {
        base.TurnOn();

        BoosterManager.Ins.IsCanUseFillByNumberBooster = true;
        BoosterManager.Ins.IsCanUseFillAllNumberBooster = false;
        BoosterManager.Ins.IsCanUseZoomBooster = false;

        LevelManager.Ins.ReleaseFocusCube();
    }
  

}
