using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPassedLevel : UICanvas
{
    private AnimationGameUnit passedLevel;
    public Vector3 quaternion;
    public Player player;
    public override void Setup()
    {
        base.Setup();
        player = FindObjectOfType<Player>();
        GameManager.Ins.ChangeState(GameState.ShowPassedLevel);
        BackgroundManager.Ins.ChangeBackground();
    }
    public void Btn_Home()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<MainMenu>();
        if(passedLevel!=null)
        SimplePool.Collect(passedLevel);
        BackgroundManager.Ins.ChangeDefaultBackground();
        player.OnReset();
    }
    public override void Open()
    {
        UIManager.Ins.CloseAll();
        base.Open();
    }
    public void CreateAnimation(PoolType poolType)
    {
        if (SimplePool.FindPrefabByType(poolType))
        {
            passedLevel = SimplePool.Spawn<AnimationGameUnit>(poolType,player.transform);
            player.TF.DORotate(LevelManager.Ins.rotateOffset, 0f);
        }
    }
}
