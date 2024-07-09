using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListLevelUI : MonoBehaviour
{
    public LevelItem levelItemPrefab;
    public List<LevelItem> levelItems = new List<LevelItem>();
    MiniPool<LevelItem> miniPool = new MiniPool<LevelItem>();
    public RectTransform tfContent;

    [SerializeField] TextMeshProUGUI textLevelType;
    LevelData levelData;
    LevelDatas levelDatas;
    List<LevelData> levelDatasList;
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
        textLevelType.text = $"{capitalizedLevelType} {levelDatas.level3D.Count(type => type.level.levelType == levelData.level.levelType)}";

        foreach (var lvData in levelDatasList)
        {
            LevelItem levelItem = miniPool.Spawn();
            levelItems.Add(levelItem);

            LevelDataModel lvDataModel = DataManager.Ins.playerData.GetDataWithID(lvData.levelID);
            bool isColored = lvDataModel.isColored;
            bool isGoldOrAds = lvDataModel.unlockType == UnlockType.gold || lvDataModel.unlockType == UnlockType.ads;
            bool isDiamond = lvDataModel.unlockType == UnlockType.diamond;
            bool isUnlock = lvDataModel.unlockType != UnlockType.free;
            levelItem.SetData(lvData.levelID, lvData.level.imageSource, isColored, !isColored && isGoldOrAds, !isColored && isDiamond, isUnlock);
        }

        StartCoroutine(IE_SetSizeDetal());
    }

    public void SetData(LevelData _levelData, LevelDatas _levelDatas, List<LevelData> _leveldataList, LevelType _levelType)
    {
        this.levelType = _levelType;
        levelData = _levelData;
        levelDatas = _levelDatas;
        this.levelDatasList = _leveldataList;
    }

    IEnumerator IE_SetSizeDetal()
    {
        yield return new WaitForEndOfFrame();
        if (tfContent != null && levelItems.Count > 0)
        {
            RectTransform buttonRectTransform = levelItems[0].GetComponent<RectTransform>();
            float buttonHeight = buttonRectTransform.rect.width + 50;
            float totalHeight = buttonHeight * (levelItems.Count + 1);
            tfContent.sizeDelta = new Vector2(totalHeight / 2, tfContent.sizeDelta.y);
            ScrollRect scrollRect = tfContent.GetComponentInParent<ScrollRect>();
            scrollRect.horizontalNormalizedPosition = 0;
        }
    }
}
