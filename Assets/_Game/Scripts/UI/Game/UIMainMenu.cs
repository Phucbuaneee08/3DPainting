using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UICanvas
{
    [SerializeField] LevelDatas levelDatas;
    [SerializeField] Transform content;
    [SerializeField] LevelItem levelItemPrefab;
    MiniPool<LevelItem> miniPool = new MiniPool<LevelItem>();
    private List<LevelItem> levelItems;
    private ColorItem colorItemSelected;

    private void Awake()
    {
        miniPool.OnInit(levelItemPrefab, 10, content);
        levelItems = new List<LevelItem>();
    }
    public override void Setup()
    {

        base.Setup();

    }
    public override void Open()
    {
        ResetLevelItem();
        InitLevelItem();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        base.Open();

    }
    public void InitLevelItem()
    {
        foreach(LevelData l3d in levelDatas.level3D) {
            LevelItem levelItem = miniPool.Spawn();
            levelItem.SetData(l3d.levelID,l3d.level);
            levelItems.Add(levelItem);
        }
    }
    public void ResetLevelItem()
    {
        if(levelItems.Count > 0) { 
            foreach (LevelItem li in levelItems)
            {
                miniPool.Despawn(li);
            }
            levelItems.Clear();
        }
    }


}
