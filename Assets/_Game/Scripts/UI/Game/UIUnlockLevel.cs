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
        UnlockGold(levelItem);
    }
    public void UnlockGold(LevelItem _levelItem)
    {
        LevelDataModel levelDataModel = DataManager.Ins.playerData.GetDataWithID(_levelItem.GetID());

        if (levelDataModel.unlockType == UnlockType.gold)
        {
            if (LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costGold > DataManager.Ins.playerData.Gold)
            {

            }
            else
            {
                DataManager.Ins.playerData.Gold -= LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costGold;
                levelDataModel.unlockType = UnlockType.free;
                DataManager.Ins.SaveData();
                StartCoroutine(IE_LoadData());
                _levelItem.UnlockUI();
            }
        }
    }

    public void UnlockDiamond(LevelItem _levelItem)
    {
        LevelDataModel levelDataModel = DataManager.Ins.playerData.GetDataWithID(_levelItem.GetID());
        if (levelDataModel.unlockType == UnlockType.diamond)
        {
            if (LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costDiamond > DataManager.Ins.playerData.Diamond)
            {

            }
            else
            {
                DataManager.Ins.playerData.Diamond -= LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costDiamond;
                levelDataModel.unlockType = UnlockType.free;
                DataManager.Ins.SaveData();
                StartCoroutine(IE_LoadData());
                _levelItem.UnlockUI();
            }
        }
    }
    public void BtnUnlockAds()
    {
       
    }
    public void BtnUnlockDiamond()
    {
        UnlockDiamond(levelItem);
    }
    public void BtnExit()
    {
        UIManager.Ins.CloseUI<UIUnlockLevel>();
    }
    IEnumerator IE_LoadData()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.GetUI<MainMenu>();
        UIManager.Ins.CloseUI<UIUnlockLevel>();
    }
}
