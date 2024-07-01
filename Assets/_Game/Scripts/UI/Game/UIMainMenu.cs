using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UICanvas
{
    [SerializeField] LevelDatas levelDatas;
    [SerializeField] Transform content;
    [SerializeField] LevelItem levelItemPrefab;
    MiniPool<LevelItem> miniPool = new MiniPool<LevelItem>();
    private List<LevelItem> levelItems;
    private ColorItem colorItemSelected;
    public RectTransform tfContent;
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
        StartCoroutine(IE_SetSizeDetal(DataManager.Ins.playerData.currentlevelID));
    }
    public void InitLevelItem()
    {
        foreach (LevelData l3d in levelDatas.level3D)
        {
            LevelItem levelItem = miniPool.Spawn();
            if (DataManager.Ins.playerData.currentlevelID - 1 >= l3d.levelID)
            {
                levelItem.SetData(l3d.levelID, l3d.level, l3d.imageSource, true, false, true);
            }
            else if (DataManager.Ins.playerData.currentlevelID  == l3d.levelID)
            {
                levelItem.SetData(l3d.levelID, l3d.level, l3d.imageSource, false, true, true);
                levelItem.transform.DOScale(1.1f, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                levelItem.SetData(l3d.levelID, l3d.level, l3d.imageSource, false, false, true);
            }
            levelItems.Add(levelItem);
        }
    }
    public void ResetLevelItem()
    {
        if (levelItems.Count > 0)
        {
            foreach (LevelItem li in levelItems)
            {
                miniPool.Despawn(li);
            }
            levelItems.Clear();
        }
    }
    IEnumerator IE_SetSizeDetal(int targetButtonIndex)
    {
        yield return new WaitForEndOfFrame();
        if (tfContent != null && levelItems.Count > 0)
        {
            RectTransform buttonRectTransform = levelItems[0].GetComponent<RectTransform>();
            float buttonHeight = buttonRectTransform.rect.height + 200;
            float totalHeight = buttonHeight * levelItems.Count;
            tfContent.sizeDelta = new Vector2(tfContent.sizeDelta.x, totalHeight);
            ScrollRect scrollRect = tfContent.GetComponentInParent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 0;
            yield return new WaitForEndOfFrame();
            if (targetButtonIndex >= 0 && targetButtonIndex < levelItems.Count)
            {
                RectTransform targetButtonRectTransform = levelItems.Find(id => id.GetID() == targetButtonIndex).GetComponent<RectTransform>();
                float targetButtonYPos = Mathf.Abs(targetButtonRectTransform.anchoredPosition.y + 210);
                float scrollPosition = (targetButtonYPos + buttonHeight / 2) / totalHeight;
                scrollPosition = 1 - scrollPosition;
                if (targetButtonIndex >= levelItems.Count - 2)
                {
                    scrollRect.verticalNormalizedPosition = 1;
                }
                else if (targetButtonIndex == 1)
                {
                    scrollRect.verticalNormalizedPosition = 0;
                }
                else
                {
                    scrollRect.verticalNormalizedPosition = scrollPosition;
                }
            }
        }
    }

}
