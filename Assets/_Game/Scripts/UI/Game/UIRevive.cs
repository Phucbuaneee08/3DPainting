using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRevive : UICanvas
{
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Revive);
    }
    public void ReviveButton()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        LevelManager.Ins.Revive();
        Close(0);
    }
}
