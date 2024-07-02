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
    private LevelType lvType = LevelType.none;
    public RectTransform tfContent;
    public void Start()
    {
        LoadData();
    }
    public void ReLoad()
    {
        LoadData();
    }
    IEnumerator IE_LoadData()
    {
        yield return new WaitForEndOfFrame();
        List<LevelType> uniqueLevelTypes = levelDatas.level3D
          .Select(level => level.levelType)
          .Distinct()
          .ToList();
        foreach (LevelType lvType in uniqueLevelTypes)
        {
            ListLevelUI listLevelUI = Instantiate(ListLevelUIPrefabs, tfContent);
            listLevelUIs.Add(listLevelUI);
            LevelData levelOfType = levelDatas.GetLevelWithType(lvType);
            List<LevelData> levelsOfType = levelDatas.GetLevelsWithType(lvType);
            listLevelUI.SetData(levelOfType, levelDatas, levelsOfType);
        }
        Debug.LogError("Load");
        //StartCoroutine(IE_SetSizeDetal());
    }

    public void LoadData()
    {
        if (listLevelUIs.Count > 0)
        {
            foreach (ListLevelUI levelUI in listLevelUIs)
            {
                Destroy(levelUI.gameObject);
            }
        }
        listLevelUIs.Clear();
        List<LevelType> uniqueLevelTypes = levelDatas.level3D
          .Select(level => level.levelType)
          .Distinct()
          .ToList();
        foreach (LevelType lvType in uniqueLevelTypes)
        {
            ListLevelUI listLevelUI = Instantiate(ListLevelUIPrefabs, tfContent);
            listLevelUIs.Add(listLevelUI);
            LevelData levelOfType = levelDatas.GetLevelWithType(lvType);
            List<LevelData> levelsOfType = levelDatas.GetLevelsWithType(lvType);
            listLevelUI.SetData(levelOfType, levelDatas, levelsOfType);
        }
        Debug.LogError("Load");
        SetSizeDetal();
    }

    public void SetSizeDetal()
    {
        if (tfContent != null && listLevelUIs.Count > 0)
        {
            RectTransform buttonRectTransform = listLevelUIs[0].GetComponent<RectTransform>();
            float buttonHeight = buttonRectTransform.rect.height + 20;
            float totalHeight = buttonHeight * listLevelUIs.Count;
            tfContent.sizeDelta = new Vector2(tfContent.sizeDelta.x, totalHeight);
        }
    }
}