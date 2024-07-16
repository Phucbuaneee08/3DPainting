using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    public Image imageSource;
    [SerializeField] private int levelID;
    public Image imgUnleckAdsAndGold;
    public Image imgUnleckDiamond;
    public Image imgUnlock;
    public Image imgLevelPassed;
    public PoolType  poolType;
    [SerializeField] private Button button;

    public void SetData(int levelID, Sprite avatar, bool isPassed, bool showImgUnlock, bool showImgUnlock2, bool isUnlock,PoolType poolType)
    {
        this.levelID = levelID;
        imageSource.sprite = avatar;
        imgLevelPassed.gameObject.SetActive(isPassed);
        imgUnlock.gameObject.SetActive(isUnlock);
        imgUnleckAdsAndGold.gameObject.SetActive(showImgUnlock);
        imgUnleckDiamond.gameObject.SetActive(showImgUnlock2);
        this.poolType = poolType;
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
            UIManager.Ins.OpenUI<UIPassedLevel>().CreateAnimation(poolType);
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
