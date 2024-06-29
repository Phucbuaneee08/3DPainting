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
    public const string PLAYER_DATA = "PLAYER_DATA";

    private void OnApplicationPause(bool pause) { SaveData(); }
    private void OnApplicationQuit() { SaveData(); }

    public void LoadData()
    {   
        Debug.Log("START LOAD DATA");
        string d = PlayerPrefs.GetString(PLAYER_DATA, "");
        if (d != "")
        {
            playerData = JsonUtility.FromJson<PlayerData>(d);
        }
        else
        {
            playerData = new PlayerData();
            FirstLoad();
        }
        isLoaded = true;
    }
    public void SaveData()
    {
        if (!isLoaded) return;
        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PLAYER_DATA, json);
        Debug.Log("SAVE DATA");
    }
    void FirstLoad()
    {

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
    public int boosterQuantity;
    public int boosterFillByColorQuantity;
    public PlayerData()
    {
        currentlevelID = 0;
        gold = 0;
        boosterQuantity = 100;
        boosterFillByColorQuantity = 100;

        isPassedTutorialBooster1 = false;
        isPassedTutorialBooster2 = false;
        isPassedTutorialBooster3 = false;
        isPassedTutorialClick = false;
        isPassedTutorialRotate = false;
        isPassedTutorialZoom = false;
    }
}
