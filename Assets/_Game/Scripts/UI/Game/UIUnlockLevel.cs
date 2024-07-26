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
    public void SetData(LevelItem _levelItem, bool _isUnlockTypeDiamond)
    {
        this.id = _levelItem.GetID();
        this.levelItem = _levelItem;
        this.imgLevel.sprite = _levelItem.imageSourcePass;
        if (_isUnlockTypeDiamond)
        {
            this.objBtnUnlockDiamond.SetActive(_isUnlockTypeDiamond);
            this.objBtnUnlockGol.SetActive(!_isUnlockTypeDiamond);
            this.objBtnUnlockAds.SetActive(!_isUnlockTypeDiamond);
        }
        else
        {
            if (DataManager.Ins.playerData.unlockAds < 3)
            {
                this.objBtnUnlockGol.SetActive(!_isUnlockTypeDiamond);
                this.objBtnUnlockAds.SetActive(!_isUnlockTypeDiamond);
                this.objBtnUnlockDiamond.SetActive(_isUnlockTypeDiamond);
            }
            else
            {
                this.objBtnUnlockGol.SetActive(!_isUnlockTypeDiamond);
                this.objBtnUnlockAds.SetActive(_isUnlockTypeDiamond);
                this.objBtnUnlockDiamond.SetActive(_isUnlockTypeDiamond);
            }
        }
        var levelData = LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level;
        this.textGoldUnlock.text = levelData.costGold.ToString();
        this.textGoldUnlock.color = levelData.costGold > DataManager.Ins.playerData.gold ? Color.red : Color.white;
        this.textDiamondUnlock.text = levelData.costDiamond.ToString();
        this.textDiamondUnlock.color = levelData.costDiamond > DataManager.Ins.playerData.diamond ? Color.red : Color.white;
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
            if (LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costGold <= DataManager.Ins.playerData.gold)
            {
                DataManager.Ins.ChangeGold( - LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costGold);
                UnLock(_levelItem, levelDataModel);
            }
        }
    }
    public void UnLock(LevelItem _levelItem, LevelDataModel levelDataModel)
    {
        levelDataModel.unlockType = UnlockType.free;
        DataManager.Ins.SaveData();
        StartCoroutine(IE_LoadData());
        _levelItem.UnlockUI();
    }
    public void UnlockDiamond(LevelItem _levelItem)
    {
        LevelDataModel levelDataModel = DataManager.Ins.playerData.GetDataWithID(_levelItem.GetID());
        if (levelDataModel.unlockType == UnlockType.diamond)
        {
            if (LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costDiamond <= DataManager.Ins.playerData.diamond)
            {
                DataManager.Ins.ChangeDiamond( - LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costDiamond);
                UnLock(_levelItem, levelDataModel);
                Debug.Log("Diamond");
            }
        }
    }
    public void BtnUnlockAds()
    {
        if (DataManager.Ins.playerData.unlockAds < 3)
        {
            LevelDataModel levelDataModel = DataManager.Ins.playerData.GetDataWithID(levelItem.GetID());
            levelDataModel.unlockType = UnlockType.free;
            DataManager.Ins.playerData.unlockAds += 1;
            DataManager.Ins.SaveData();
            StartCoroutine(IE_LoadData());
            levelItem.UnlockUI();
        }
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
