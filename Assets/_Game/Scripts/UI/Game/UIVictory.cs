using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVictory : UICanvas
{
   public void Home()
    {
        LevelManager.Ins.Home();
    }
    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Finish);
    }
    public void NextLevel()
    {
        LevelManager.Ins.NextLevel();
    }

}