using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUnlockBtnDaily : PopupUnlockButton
{
    public override void Awake()
    {
        base.Awake();
        tfStart = UIManager.Ins.GetUI<MainMenu>().tfBtnDailyReward;
    }
}
