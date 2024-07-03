using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;

[Serializable]
public class DataManager : Singleton<DataManager>
{
    public bool isLoaded = false;
    public PlayerData playerData;
    private const string PLAYER_DATA_PATH = "/GameData/PlayerData.json";
    private static string systemPath = Application.dataPath + PLAYER_DATA_PATH;

    private void OnApplicationPause(bool pause) { SaveData(); }
    private void OnApplicationQuit() { SaveData(); }

    public void LoadData()
    {
        Debug.Log("START LOAD DATA");
        playerData = DataUtilities.LoadData<PlayerData>(systemPath);

        if (playerData == null)
        {
            playerData = new PlayerData();
            FirstLoad();
        }
        isLoaded = true;
    }

    public void SaveData()
    {
        if (!isLoaded) return;
        DataUtilities.SaveData(playerData, systemPath);
        Debug.Log("SAVE DATA");
    }

    void FirstLoad()
    {
        SaveLevelDataModels();
    }

    public void SaveLevelDataModels()
    {
        playerData.levelDataModels = LevelManager.Ins.levelDatas.level3D
            .Select(ld => new LevelDataModel(ld.levelID, 0,UnlockType.free,100,1)).ToList();
        SaveData();
    }

    public void UnlockGold(LevelItem _levelItem)
    {
        LevelDataModel levelDataModel = playerData.GetDataWithID(_levelItem.GetID());
        if (levelDataModel.unlockType == UnlockType.gold)
        {
            if(levelDataModel.goldUnlock > playerData.gold)
            {

            }
            else
            {
                playerData.gold -= levelDataModel.goldUnlock;
                levelDataModel.unlockType = UnlockType.free;
                SaveData();
            }
        }
    }
    public void UnlockDiamond(LevelItem _levelItem)
    {
        LevelDataModel levelDataModel = playerData.GetDataWithID(_levelItem.GetID());
        if (levelDataModel.unlockType == UnlockType.diamond)
        {
            if (levelDataModel.diamondUnlock > playerData.diamond)
            {

            }
            else
            {
                playerData.diamond -= levelDataModel.diamondUnlock;
                levelDataModel.unlockType = UnlockType.free;
                SaveData();
            }
        }
    }
}
[System.Serializable]
public class PlayerData
{
    [Header("--------- Game Params ---------")]
    public bool isPassedTutorialClick;
    public bool isPassedTutorialRotate;
    public bool isPassedTutorialZoom;
    public bool isPassedTutorialBooster1;
    public bool isPassedTutorialBooster2;
    public bool isPassedTutorialBooster3;

    public int currentlevelID;
    public int gold;
    public int diamond;
    public int boosterQuantity;
    public int boosterFillByColorQuantity;


    public List<LevelDataModel> levelDataModels;
    public PlayerData()
    {
        currentlevelID = 1;
        gold = 100;
        diamond = 100;
        boosterQuantity = 100;
        boosterFillByColorQuantity = 100;
        isPassedTutorialBooster1 = false;
        isPassedTutorialBooster2 = false;
        isPassedTutorialBooster3 = false;
        isPassedTutorialClick = false;
        isPassedTutorialRotate = false;
        isPassedTutorialZoom = false;
    }
    public LevelDataModel GetDataWithID(int _id)
    {
        return levelDataModels.Find(id => id.levelID ==  _id);
    }
}
[System.Serializable]
public class LevelDataModel
{
    public int levelID;
    public int isColored; // 0 la fales, 1 true.
    public UnlockType unlockType;
    public int goldUnlock;
    public int diamondUnlock;
    public LevelDataModel(int levelID, int isColored, UnlockType unlockType, int goldUnlock, int diamondUnlock)
    {
        this.levelID = levelID;
        this.isColored = isColored;
        this.unlockType = unlockType;
        this.goldUnlock = goldUnlock;
        this.diamondUnlock = diamondUnlock;
    }
}