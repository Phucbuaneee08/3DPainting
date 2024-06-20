using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : UICanvas
{
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.MainMenu);
    }
    public void Home()
    {
        LevelManager.Ins.Home();
    }
}
