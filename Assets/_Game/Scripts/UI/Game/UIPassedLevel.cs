using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPassedLevel : UICanvas
{
    public void Btn_Home()
    {
        UIManager.Ins.CloseUI<UIPassedLevel>();
    }
}
