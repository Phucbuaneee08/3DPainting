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

    public Image imgUnleckAdsAndGold;
    public Image imgUnleckDiamond;
    public Image imgUnlock;
    public Image imgLevelPassed;
    public Image imageSource;
    public PoolType poolType;
    public ZoomInfo zoomInfo;
    public Sprite imageSourcePass;
    private Color[] colors = new Color[]
   {
        new Color(0.878f, 0.631f, 0.706f),
        new Color(0.93f, 0.84f, 0.69f)/*,
        new Color(0.235f, 0.784f, 0.949f),
        new Color(0.918f, 0.318f, 0.529f)*/
   };
    public void SetData(int levelID, Sprite avatar, Sprite avatarPass, bool isPassed, bool showImgUnlock, bool showImgUnlock2, bool isUnlock, PoolType poolType, ZoomInfo zoomInfo)
    {
        this.levelID = levelID;

        if (isPassed)
        {
            imageSource.sprite = avatarPass;
            imgBackG.color = ChangeColor(0);
        }
        else
        {
            if (isUnlock)
            {
                imgBackG.color = ChangeColor(1);
            }
            imageSource.sprite = avatar;
        }
        if (avatarPass != null)
        {
            this.imageSourcePass = avatarPass;
        }
        imgLevelPassed.gameObject.SetActive(isPassed);
        imgUnlock.gameObject.SetActive(isUnlock);
       
        imgUnleckAdsAndGold.gameObject.SetActive(showImgUnlock);
        imgUnleckDiamond.gameObject.SetActive(showImgUnlock2);
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
    public Color ChangeColor(int index)
    {
        return colors[index];
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
