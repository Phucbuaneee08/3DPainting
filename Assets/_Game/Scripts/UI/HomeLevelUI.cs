using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HomeLevelUI : MonoBehaviour
{
    public ListLevelUI ListLevelUIPrefabs;
    public List<ListLevelUI> listLevelUIs = new List<ListLevelUI>();
    public RectTransform tfContent;
    public ScrollRect scrollRect;
    MiniPool<ListLevelUI> miniPool = new MiniPool<ListLevelUI>();

    private void Awake()
    {
        miniPool.OnInit(ListLevelUIPrefabs, 10, tfContent);
    }

    public void ReLoad()
    {
        LoadData();
    }

    public void LoadData()
    {
        miniPool.Release();
        listLevelUIs.Clear();
        Dictionary<LevelType, LevelData> firstLevelByType = new Dictionary<LevelType, LevelData>();
        Dictionary<LevelType, List<LevelData>> levelsByType = new Dictionary<LevelType, List<LevelData>>();
        foreach (LevelData levelData in LevelManager.Ins.levelDatas.level3D)
        {
            LevelType levelType = levelData.level.levelType;
            if (!levelsByType.ContainsKey(levelType))
            {
                levelsByType[levelType] = new List<LevelData>();
                firstLevelByType[levelType] = levelData;
            }
            levelsByType[levelType].Add(levelData);
        }
        foreach (var kvp in levelsByType)
        {
            ListLevelUI listLevelUI = miniPool.Spawn();
            listLevelUIs.Add(listLevelUI);
            LevelType lvType = kvp.Key;
            List<LevelData> levelsOfType = kvp.Value;
            LevelData firstLevelOfType = firstLevelByType[lvType];
            listLevelUI.SetData(firstLevelOfType, LevelManager.Ins.levelDatas, levelsOfType, lvType);
            NestedScrollRect nestedScrollHandler = listLevelUI.GetComponentInChildren<NestedScrollRect>();
            nestedScrollHandler.parentScrollRect = scrollRect;
        }
        SetSizeDetal();
    }
    public void SetSizeDetal()
    {
        if (tfContent != null && listLevelUIs.Count > 0)
        {
            RectTransform buttonRectTransform = listLevelUIs[0].GetComponent<RectTransform>();
            float buttonHeight = buttonRectTransform.rect.height + 50;
            float totalHeight = buttonHeight * listLevelUIs.Count;
            tfContent.sizeDelta = new Vector2(tfContent.sizeDelta.x, totalHeight);
            scrollRect.verticalNormalizedPosition = 1;
        }
    }
}
