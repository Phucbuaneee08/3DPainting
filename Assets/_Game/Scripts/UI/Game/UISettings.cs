using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings : UICanvas
{
    public override void Open()
    {
        base.Open();
    }
    public void BtnExit()
    {
        UIManager.Ins.CloseUI<UISettings>();
    }
}
