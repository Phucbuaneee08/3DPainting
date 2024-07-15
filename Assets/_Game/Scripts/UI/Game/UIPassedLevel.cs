using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPassedLevel : UICanvas
{
    private AnimationGameUnit passedLevel;
    public Vector3 quaternion;
    public void Btn_Home()
    {
        Close(0);
        UIManager.Ins.OpenUI<MainMenu>();
        if(passedLevel!=null)
        Destroy(passedLevel.gameObject);

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
            passedLevel = SimplePool.Spawn<AnimationGameUnit>(poolType);
            passedLevel.transform.DORotate(quaternion, 2f);
        }
    }
}
