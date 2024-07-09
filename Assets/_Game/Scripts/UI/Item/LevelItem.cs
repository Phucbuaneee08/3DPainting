using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] private Button button;
    [SerializeField] private Material grayScaleMaterial;

    public void SetData(int levelID, Sprite avatar, bool isPassed, bool showImgUnlock, bool showImgUnlock2, bool isUnlock)
    {
        this.levelID = levelID;
        imageSource.sprite = avatar;
        imgLevelPassed.gameObject.SetActive(isPassed);
        imgUnlock.gameObject.SetActive(isUnlock);
        imgUnleckAdsAndGold.gameObject.SetActive(showImgUnlock);
        imgUnleckDiamond.gameObject.SetActive(showImgUnlock2);
    }

    public void SelectLevel()
    {
        LevelDataModel lvDataModel = DataManager.Ins.playerData.GetDataWithID(levelID);
        if (!lvDataModel.isColored)
        {
            if (lvDataModel.unlockType == UnlockType.free)
            {
                LevelManager.Ins.OnLoadLevel(levelID);
            }
            else
            {
                bool isGoldOrAds = lvDataModel.unlockType == UnlockType.gold || lvDataModel.unlockType == UnlockType.ads;
                UIManager.Ins.OpenUI<UIUnlockLevel>().SetData(this, isGoldOrAds, isGoldOrAds, lvDataModel.unlockType == UnlockType.diamond);
            }
        }
        else
        {
            UIManager.Ins.OpenUI<UIPassedLevel>();
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
}
