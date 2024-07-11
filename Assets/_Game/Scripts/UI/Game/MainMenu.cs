using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private HomeLevelUI homeLevelUI;
  
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        ReLoadData();
        base.Open();
    }
    public void ReLoadData()
    {
        homeLevelUI.ReLoad();
    }
    public void BtnSpin()
    {
        
    }
}
