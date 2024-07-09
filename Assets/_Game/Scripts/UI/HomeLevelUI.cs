using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HomeLevelUI : MonoBehaviour
{
    [SerializeField] LevelDatas levelDatas;
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
        HashSet<LevelType> uniqueLevelTypes = new HashSet<LevelType>();

        foreach (var levelData in levelDatas.level3D)
        {
            uniqueLevelTypes.Add(levelData.level.levelType);
        }

        foreach (LevelType lvType in uniqueLevelTypes)
        {
            ListLevelUI listLevelUI = miniPool.Spawn();
            listLevelUIs.Add(listLevelUI);

            LevelData levelOfType = levelDatas.GetLevelWithType(lvType);
            List<LevelData> levelsOfType = levelDatas.GetLevelsWithType(lvType);
            listLevelUI.SetData(levelOfType, levelDatas, levelsOfType, lvType);

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
