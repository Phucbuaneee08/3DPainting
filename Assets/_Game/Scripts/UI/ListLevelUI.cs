using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListLevelUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textLevelType;
    MiniPool<LevelItem> miniPool = new MiniPool<LevelItem>();
    LevelData levelData;
    LevelDatas levelDatas;
    List<LevelData> levelDatasList;

    public LevelItem levelItemPrefab;
    public List<LevelItem> levelItems = new List<LevelItem>();
    public RectTransform tfContent;
    public LevelType levelType;

    private void Awake()
    {
        miniPool.OnInit(levelItemPrefab, 10, tfContent);

    }

    private void Start()
    {
        StartCoroutine(IE_LoadData());
    }

    IEnumerator IE_LoadData()
    {
        yield return new WaitForEndOfFrame();
        LevelType levelType = levelData.level.levelType;
        string capitalizedLevelType = char.ToUpper(levelType.ToString()[0]) + levelType.ToString().Substring(1);
        var levelsOfType = levelDatas.level3D.Where(type => type.level.levelType == levelData.level.levelType).ToList();
        int totalLevels = levelsOfType.Count;
        int uncoloredLevels = levelsOfType.Count(type => DataManager.Ins.playerData.GetDataWithID(type.levelID).isColored);
        textLevelType.text = $"{capitalizedLevelType} {uncoloredLevels}/{totalLevels}";
        var sortedLevelDatasList = levelDatasList.OrderByDescending(lvData =>
        {
            var model = levelDatas.GetLevelWithID(lvData.levelID);
            return model != null && model.level.unlockType == UnlockType.free;
        }).ToList();
        foreach (var lvData in sortedLevelDatasList)
        {
            LevelItem levelItem = miniPool.Spawn();
            levelItems.Add(levelItem);
            LevelDataModel lvDataModel = DataManager.Ins.playerData.GetDataWithID(lvData.levelID);
            bool isColored = lvDataModel.isColored;
            bool isGoldOrAds = lvDataModel.unlockType == UnlockType.gold || lvDataModel.unlockType == UnlockType.ads;
            bool isDiamond = lvDataModel.unlockType == UnlockType.diamond;
            bool isUnlock = lvDataModel.unlockType != UnlockType.free;
            levelItem.SetData(lvData.levelID, lvData.level.imageSource, lvData.level.imageSourcePassed, isColored, isUnlock, lvData.level.poolType, lvData.level.zoomInfo);
        }


    }


    public void SetData(LevelData _levelData, LevelDatas _levelDatas, List<LevelData> _leveldataList, LevelType _levelType)
    {
        this.levelType = _levelType;
        levelData = _levelData;
        levelDatas = _levelDatas;
        this.levelDatasList = _leveldataList;
    }


}
