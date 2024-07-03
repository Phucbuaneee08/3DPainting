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
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        base.Open();
    }
    public void SetData(LevelItem _levelItem, bool _isShowBtnUnlockGol, bool _isShowBtnUnlockAds, bool _isShowBtnUnlockDiamond)
    {

        this.levelItem = _levelItem;
        this.imgLevel.sprite = _levelItem.imageSource.sprite;
        this.objBtnUnlockGol.SetActive(_isShowBtnUnlockGol);
        this.objBtnUnlockAds.SetActive(_isShowBtnUnlockAds);
        this.objBtnUnlockDiamond.SetActive(_isShowBtnUnlockDiamond);
        this.textGoldUnlock.text = LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costGold.ToString();
        this.textDiamondUnlock.text = LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costDiamond.ToString();

    }
    public void BtnUnlockGold()
    {
        DataManager.Ins.UnlockGold(levelItem);
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<MainMenu>();
    }
    public void BtnUnlockAds()
    {
       
    }
    public void BtnUnlockDiamond()
    {
        DataManager.Ins.UnlockDiamond(levelItem);
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<MainMenu>();
    }
    public void BtnExit()
    {
        UIManager.Ins.CloseUI<UIUnlockLevel>();
    }
}
