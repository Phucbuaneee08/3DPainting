using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Image imgBackG;
    [SerializeField] private int levelID;
    [SerializeField] private Button button;
    public Image imgUnlock;
    public Image imageSource;
    public PoolType poolType;
    public ZoomInfo zoomInfo;
    public Sprite imageSourcePass;

    public void SetData(int levelID, Sprite avatar, Sprite avatarPass, bool isPassed, bool isUnlock, PoolType poolType, ZoomInfo zoomInfo, LevelType levelType)
    {
        this.levelID = levelID;
        LevelData test = LevelManager.Ins.levelDatas.GetLevelWithID(levelID);
        Debug.Log( test.level.name+" "+ test.level.unlockType);
        if (isPassed)
        {
            imageSource.sprite = avatarPass;
            imgBackG.color = MaterialManager.Ins.ChangeRandomColor();
        }
        else
        {
            imageSource.sprite = avatar;
        }
        if (avatarPass != null)
        {
            this.imageSourcePass = avatarPass;
        }

        imgUnlock.gameObject.SetActive(isUnlock);
        if (isUnlock)
        {
            imgUnlock.sprite = MaterialManager.Ins.ChangeImgLock(levelType);
        }
        this.poolType = poolType;
        this.zoomInfo = zoomInfo;
    }
    public void SelectLevel()
    {
        if (GameManager.Ins.gameState != GameState.MainMenu) return;
        LevelDataModel lvDataModel = DataManager.Ins.playerData.GetDataWithID(levelID);
        if (!lvDataModel.isColored)
        {
            if (lvDataModel.unlockType == UnlockType.free)
            {
                LevelManager.Ins.OnLoadLevel(levelID);
                Debug.Log(GameManager.Ins.gameState);
            }
            else
            {
                UIManager.Ins.OpenUI<UIUnlockLevel>().SetData(this, lvDataModel.unlockType == UnlockType.diamond);
            }
        }
        else
        {
            AnimationController.Ins.LoadPrefabOfType(poolType.ToString());
            UIManager.Ins.OpenUI<UIPassedLevel>();
            CameraManager.Ins.SetOrthoSize(zoomInfo.checkPointZoom);
        }
    }

    public int GetID()
    {
        return levelID;
    }
    public void SetColorImg()
    {
        Color originalColor = imageSource.color;
        Color grayscaleColor = Ultilities.ConvertToGrayscale(originalColor);
        imageSource.color = grayscaleColor;
    }
    public void UnlockUI()
    {
        imgUnlock.gameObject.SetActive(false);
    }
}
