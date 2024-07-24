using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;
using UnityEngine.Events;

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
    private void OnDestroy()
    {
        OnGoldChanged = null;
        OnDiamondChanged = null;
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
        else
        {
            UpdateDataJson();
            CheckDailyReward();
        }
        isLoaded = true;
    }
    public void UpdateDataJson()
    {
        int countNew = LevelManager.Ins.levelDatas.level3D.Count;
        int countOld = playerData.levelDataModels.Count;

        if (countNew != countOld)
        {
            var existingLevelDict = playerData.levelDataModels.ToDictionary(ld => ld.levelID);
            var newLevels = LevelManager.Ins.levelDatas.level3D;
            var newLevelIDs = newLevels.Select(l => l.levelID).ToList();

            if (countNew > countOld)
            {
                AddNewLevels(newLevels, existingLevelDict);
            }

            if (countNew < countOld)
            {
                RemoveOldLevels(newLevelIDs);
            }
            Debug.Log("Update");
            SaveData();
        }
    }

    private void AddNewLevels(List<LevelData> newLevels, Dictionary<int, LevelDataModel> existingLevelDict)
    {
        foreach (var newLevel in newLevels)
        {
            if (!existingLevelDict.ContainsKey(newLevel.levelID))
            {
                var newLevelDataModel = new LevelDataModel(newLevel.levelID, false, newLevel.level.unlockType);
                playerData.levelDataModels.Add(newLevelDataModel);
            }
        }
    }

    private void RemoveOldLevels(List<int> newLevelIDs)
    {
        playerData.levelDataModels.RemoveAll(ld => !newLevelIDs.Contains(ld.levelID));
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

    public void ChangeGold(int newGold)
    {
        playerData.gold += newGold;
        if (playerData.gold < 0)
        {
            playerData.gold = 0;
        }
        SaveData();
        OnGoldChanged?.Invoke((int)playerData.gold);
    }

    public void ChangeDiamond(int newDiamond)
    {
        playerData.diamond += newDiamond;
        if (playerData.diamond < 0)
        {
            playerData.diamond = 0;
        }
        SaveData();
        OnDiamondChanged?.Invoke((int)playerData.diamond);
    }

    public void ChangeCoin(int amount)
    {
        playerData.gold += amount;
        if (playerData.gold < 0)
        {
            playerData.gold = 0;
        }
        SaveData();
        OnCoinChanged?.Invoke((int)playerData.gold);
    }

    //booster fill nhiều màu 
    public void ChangeBoosterFillByColor(int amount)
    {
        playerData.boosterFillByColorQuantity += amount;
        if (playerData.boosterFillByColorQuantity < 0)
        {
            playerData.boosterFillByColorQuantity = 0;
        }
        SaveData();
        OnBoosterFillByColorQuantityChanged?.Invoke((int)playerData.boosterFillByColorQuantity);
    }
    //booster fill theo màu 
    public void ChangeBoosterFillAllColor(int amount)
    {
        playerData.boosterFillAllColorQuantity += amount;
        if (playerData.boosterFillAllColorQuantity < 0)
        {
            playerData.boosterFillAllColorQuantity = 0;
        }
        SaveData();
        OnBoosterFillAllColorQuantityChanged?.Invoke((int)playerData.boosterFillAllColorQuantity);
    }
    // booster tìm cube theo màu 
    public void ChangeBoosterFindByColor(int amount)
    {
        playerData.boosterFindByColorQuantity += amount;
        if (playerData.boosterFindByColorQuantity < 0)
        {
            playerData.boosterFindByColorQuantity = 0;
        }
        SaveData();
        OnBoosterFindByColorQuantityChanged?.Invoke((int)playerData.boosterFindByColorQuantity);
    }

    public event Action<int> OnGoldChanged;
    public event Action<int> OnDiamondChanged;
    public event Action<int> OnBoosterFillByColorQuantityChanged;
    public event Action<int> OnBoosterFillAllColorQuantityChanged;
    public event Action<int> OnBoosterFindByColorQuantityChanged;
    public UnityAction<int> OnCoinChanged;

    private void CheckDailyReward()
    {
        DateTime now = DateTime.Now;
        int daysNow = (int)now.Subtract(new DateTime(1970, 1, 1)).TotalDays;
        bool isNewDay = daysNow > playerData.daysLastOpen;

        if (isNewDay && playerData.isTodayCollected == 1)
        {
            playerData.isTodayCollected = 0;
            playerData.daysCollected++;
        }

        playerData.daysLastOpen = daysNow;
    }
}

[System.Serializable]
public class PlayerData
{
    [Header("------Chỉ số Game--------")]
    public double timeLastOpen;//days
    public int daysPlayed;
    public int CountLevelPassed;
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

    public int boosterFillAllColorQuantity;
    public int boosterFillByColorQuantity;
    public int boosterFindByColorQuantity;

    public int unlockAds;

    public bool music;
    public bool sound;
    public bool vibrationEnabled;

    [Header("--------- Daily Reward ---------")]
    public int daysCollected;
    public double daysLastOpen;
    public int isTodayCollected; // 0: not collected, 1: collected
    public int isTodayCollectFree;
    public int isCollectFullInDay;
    public bool isShowDailyRewardFirst;
    [Header("--------- Spin  ---------")]
    public int isSpinReward;
    public bool isShowSpinRewardFirst;
    public int countProgresses;

    [Header("--------- Level Data ---------")]
    public List<LevelDataModel> levelDataModels;

    public PlayerData()
    {
        timeLastOpen = DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalDays;
        daysPlayed = 0;
        currentlevelID = 1;
        CountLevelPassed = 0;
        gold = 10000;
        diamond = 10000;

        boosterFillAllColorQuantity = 1000;
        boosterFillByColorQuantity = 1000;
        boosterFindByColorQuantity = 1000;

        unlockAds = 0;
        isPassedTutorialBooster1 = false;
        isPassedTutorialBooster2 = false;
        isPassedTutorialBooster3 = false;
        isPassedTutorialClick = false;
        isPassedTutorialRotate = false;
        isPassedTutorialZoom = false;

        daysCollected = 0;
        daysLastOpen = (int)timeLastOpen;
        isTodayCollected = 0;
        isTodayCollectFree = 0;

        isCollectFullInDay = 0;

        isSpinReward = 1;
        countProgresses = 0;

        isShowDailyRewardFirst = false;
        isShowSpinRewardFirst = false;

        music = true;
        sound = true;
        vibrationEnabled = true;
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
