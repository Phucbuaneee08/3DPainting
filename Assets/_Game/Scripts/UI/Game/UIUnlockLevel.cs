using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIUnlockLevel : UICanvas
{
    public Image imgLevel;
    public GameObject objBtnUnlockGol;
    public GameObject objBtnUnlockAds;
    public GameObject objBtnUnlockDiamond;
    public TextMeshProUGUI textGoldUnlock;
    public TextMeshProUGUI textDiamondUnlock;
    public LevelItem levelItem;
    public int id;
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        base.Open();
    }
    public void SetData(LevelItem _levelItem, bool _isShowBtnUnlockGold, bool _isShowBtnUnlockAds, bool _isShowBtnUnlockDiamond)
    {
        this.id = _levelItem.GetID();
        this.levelItem = _levelItem;
        this.imgLevel.sprite = _levelItem.imageSource.sprite;
        this.objBtnUnlockGol.SetActive(_isShowBtnUnlockGold);
        this.objBtnUnlockAds.SetActive(_isShowBtnUnlockAds);
        this.objBtnUnlockDiamond.SetActive(_isShowBtnUnlockDiamond);
        var levelData = LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level;
        this.textGoldUnlock.text = levelData.costGold.ToString();
        this.textGoldUnlock.color = levelData.costGold > DataManager.Ins.playerData.Gold ? Color.red : Color.black;
        this.textDiamondUnlock.text = levelData.costDiamond.ToString();
        this.textDiamondUnlock.color = levelData.costDiamond > DataManager.Ins.playerData.Diamond ? Color.red : Color.black;
    }

    public void BtnUnlockGold()
    {
        DataManager.Ins.UnlockGold(levelItem);

    }
    public void BtnUnlockAds()
    {
       
    }
    public void BtnUnlockDiamond()
    {
        DataManager.Ins.UnlockDiamond(levelItem);
       
    }
    public void BtnExit()
    {
        UIManager.Ins.CloseUI<UIUnlockLevel>();
    }
}
