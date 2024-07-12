using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindByColorItem : BoosterItem
{
    public override void OnClick()
    {
        base.OnClick();
        if(LevelManager.Ins.currentColor!=0)
            BoosterManager.Ins.FindNextCubeByColor(LevelManager.Ins.currentColor);
    }
}


