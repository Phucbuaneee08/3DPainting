using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopupUnlockBtnSpin : PopupUnlockButton
{
    public override void Awake()
    {
        base.Awake();
        tfStart = UIManager.Ins.GetUI<MainMenu>().tfBtnSpin;
    }
}
