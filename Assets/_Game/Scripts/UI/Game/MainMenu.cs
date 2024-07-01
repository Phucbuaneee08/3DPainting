using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] LevelDatas levelDatas;
    [SerializeField] Transform content;
    [SerializeField] LevelItem levelItemPrefab;
    MiniPool<LevelItem> miniPool = new MiniPool<LevelItem>();
    private List<LevelItem> levelItems;
    private bool canUpdateTrans = false;
    private Vector3 correctPos;
    int count = 0;
    private void Awake()
    {
        miniPool.OnInit(levelItemPrefab, 10, content);
        levelItems = new List<LevelItem>();
    }
    public void Start()
    {
        SpawnUILevel();
    }
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        base.Open();
    }
    private void Update()
    {
        if (canUpdateTrans)
        {
            content.localPosition = Vector3.Lerp(content.localPosition, correctPos, 5 * Time.deltaTime);
        }
    }
    public void OnDragContent()
    {
        canUpdateTrans = false;
    }
    public void OnDropContent()
    {
        canUpdateTrans = true;
    }

    private void SpawnUILevel()
    {
        Canvas.ForceUpdateCanvases();
        levelItems = new List<LevelItem>();
        for (int i = Mathf.Max(1, DataManager.Ins.playerData.currentlevelID - 10); i < DataManager.Ins.playerData.currentlevelID + 14; i++)
        {
            if (i < levelDatas.level3D.Count)
            {
                LevelItem levelItem = miniPool.Spawn();
                LevelData levelData = levelDatas.GetLevelWithID(i);
                levelItems.Add(levelItem);
                levelItem.SetData(levelData.levelID, levelData.level, levelData.imageSource, false, false, false);
                count++;
                canUpdateTrans = true;
                if (i == DataManager.Ins.playerData.currentlevelID)
                {
                    levelItem.transform.DOScale(1.1f, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
                }
            }
        }
        content.localPosition -= new Vector3(0, count * 300f, 0);
        correctPos = content.localPosition;
    }
}
