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
    private static string systemPath;

    private void Awake()
    {
        systemPath = Application.persistentDataPath + PLAYER_DATA_PATH;
    }

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
            .Select(ld => new LevelDataModel(ld.levelID, false, ld.level.unlockType)).ToList();
        SaveData();
    }
#if UNITY_EDITOR
    [MenuItem("UserDataManager/ResetData")]
    public static void ResetData()
    {
        DataUtilities.UpdateData(new PlayerData());
        Debug.Log("Reset thành công Data người chơi ");
    }

    [MenuItem("UserDataManager/DelData")]
    public static void Delete()
    {
        DataUtilities.DeleteData(systemPath);
        Debug.Log("Reset thành công Data người chơi ");
    }
#endif

    public void UnlockGold(LevelItem _levelItem)
    {
        LevelDataModel levelDataModel = playerData.GetDataWithID(_levelItem.GetID());

        if (levelDataModel.unlockType == UnlockType.gold)
        {
            if (LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costGold > playerData.gold)
            {

            }
            else
            {
                playerData.gold -= LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costGold;
                levelDataModel.unlockType = UnlockType.free;
                SaveData();
                StartCoroutine(IE_LoadData());
            }
        }
    }

    public void UnlockDiamond(LevelItem _levelItem)
    {
        LevelDataModel levelDataModel = playerData.GetDataWithID(_levelItem.GetID());
        if (levelDataModel.unlockType == UnlockType.diamond)
        {
            if (LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costDiamond > playerData.diamond)
            {

            }
            else
            {
                playerData.diamond -= LevelManager.Ins.levelDatas.GetLevelWithID(_levelItem.GetID()).level.costDiamond;
                levelDataModel.unlockType = UnlockType.free;
                SaveData();
                StartCoroutine(IE_LoadData());
            }
        }
    }

    IEnumerator IE_LoadData()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<MainMenu>();
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
    [Header("--------- Level Data ---------")]
    public List<LevelDataModel> levelDataModels;
    public PlayerData()
    {
        currentlevelID = 1;
        gold = 10000;
        diamond = 10000;


        boosterQuantity = 1000;
        boosterFillByColorQuantity = 1000;
        isPassedTutorialBooster1 = false;
        isPassedTutorialBooster2 = false;
        isPassedTutorialBooster3 = false;
        isPassedTutorialClick = false;
        isPassedTutorialRotate = false;
        isPassedTutorialZoom = false;
    }
    public LevelDataModel GetDataWithID(int _id)
    {
        return levelDataModels.Find(id => id.levelID == _id);
    }
}
[System.Serializable]
public class LevelDataModel
{
    public int levelID;
    public bool isColored; // 0 la fales, 1 true.
    public UnlockType unlockType;
    public LevelDataModel(int levelID, bool isColored, UnlockType unlockType)
    {
        this.levelID = levelID;
        this.isColored = isColored;
        this.unlockType = unlockType;
    }
}
