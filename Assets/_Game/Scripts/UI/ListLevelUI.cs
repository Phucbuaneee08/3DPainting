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
    private void Awake()
    {
        miniPool.OnInit(levelItemPrefab, 10, tfContent);
        levelItems = new List<LevelItem>();
    }
    private void Start()
    {
        StartCoroutine(IE_LoadData());
    }
    IEnumerator IE_LoadData()
    {
        yield return new WaitForEndOfFrame();
        textLevelType.text = levelData.levelType.ToString() + " " + levelDatas.level3D.Count(type => type.levelType == levelData.levelType).ToString();
        for (int i = 0; i < levelDatasList.Count; i++)
        {
            LevelItem levelItem = miniPool.Spawn();
            levelItems.Add(levelItem);
            LevelData lvData = levelDatasList[i];
            if (DataManager.Ins.playerData.GetDataWithID(lvData.levelID).isColored == 1)
            {
                levelItem.SetData(lvData.levelID, lvData.imageSource, true, true, true);
            }
            else
            {
                levelItem.SetData(lvData.levelID, lvData.imageSource, false, true, true);
            }
        }
        StartCoroutine(IE_SetSizeDetal());
    }
    public void SetData(LevelData _levelData, LevelDatas _levelDatas, List<LevelData> _leveldataList)
    {
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
            float buttonHeight = buttonRectTransform.rect.width + 20;
            float totalHeight = buttonHeight * (levelItems.Count + 1);
            tfContent.sizeDelta = new Vector2(totalHeight / 2, tfContent.sizeDelta.y);
            ScrollRect scrollRect = tfContent.GetComponentInParent<ScrollRect>();
            scrollRect.horizontalNormalizedPosition = 0;
        }
    }
}
