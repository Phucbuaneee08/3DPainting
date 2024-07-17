using AssetKits.ParticleImage;
using DG.Tweening;
using Paint3D;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UICanvas
{
    [SerializeField] private HomeLevelUI homeLevelUI;
    public GameObject textGold;
    [SerializeField] private GameObject notiSpin;
    [SerializeField] private GameObject notiDailyReward;
    [SerializeField] private GameObject notiShop;
    [SerializeField] private GameObject notiTut;
    [SerializeField] private TMP_Text textNumberPass;
    [SerializeField] private ButtonUI buttonUI;
    public RectTransform tfBtnSpin;
    public RectTransform tfBtnDailyReward;

    public override void Setup() => base.Setup();

    public override void Open()
    {
        base.Open();
       
        GameManager.Ins.ChangeState(GameState.MainMenu);
        AudioManager.Ins.OnPlayHomeMusic();
        ReLoadData();
        LoadUIButtonHome();
        UpdateNotfi();
    }

    private void LoadUIButtonHome()
    {
      
            buttonUI.LoadUIButtonItem();
            //buttonUI.SetRecTF(2);
      
    }

    public void UIBtnDailyReward()
    {
       

        int levelsPassed = DataManager.Ins.playerData.CountLevelPassed;
        bool isFirstDailyReward = DataManager.Ins.playerData.isShowDailyRewardFirst;
        bool isFullCollect = DataManager.Ins.playerData.isCollectFullInDay == 1;

        if (levelsPassed >= 3)
        {
            if (!isFirstDailyReward && levelsPassed == 3)
            {
                GameManager.Ins.ChangeState(GameState.Pause);
                StartCoroutine(IE_AutoShowDailyReward());
                DataManager.Ins.playerData.isShowDailyRewardFirst = true;
                DataManager.Ins.SaveData();
                buttonUI.LoadUIButtonItem();
            }
            else
            {
                notiDailyReward.SetActive(!isFullCollect);
            }
        }
    }

    public void UIBtnSpin()
    {
     

        int levelsPassed = DataManager.Ins.playerData.CountLevelPassed;
        bool isFirstSpinReward = DataManager.Ins.playerData.isShowSpinRewardFirst;
        bool isSpinReward = DataManager.Ins.playerData.isSpinReward != 0;

        if (levelsPassed >= 5)
        {
            if (!isFirstSpinReward && levelsPassed == 5)
            {
                GameManager.Ins.ChangeState(GameState.Pause);
                StartCoroutine(IE_AutoShowSpin());
                DataManager.Ins.playerData.isShowSpinRewardFirst = true;
                DataManager.Ins.SaveData();
               
            }
            else
            {
                notiSpin.SetActive(isSpinReward);
                textNumberPass.gameObject.SetActive(!isSpinReward);
                if (!isSpinReward)
                {
                    textNumberPass.text = $"{levelsPassed % 10}/10";
                }
            }
        }
    }

    IEnumerator IE_AutoShowSpin()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<PopupUnlockBtnSpin>().UnlockButton();
        Ultilities.DelayThenDoTask(this, 4f, () =>
        {
            UIManager.Ins.CloseUI<PopupUnlockBtnSpin>();
            UIManager.Ins.OpenUI<UISpin>();
            notiSpin.SetActive(DataManager.Ins.playerData.isSpinReward != 0);
            GameManager.Ins.ChangeState(GameState.MainMenu);
        });
    }

    IEnumerator IE_AutoShowDailyReward()
    {
        yield return new WaitForEndOfFrame();
      
        UIManager.Ins.OpenUI<PopupUnlockBtnDaily>().UnlockButton();
        Ultilities.DelayThenDoTask(this, 4f, () =>
        {
            UIManager.Ins.CloseUI<PopupUnlockBtnDaily>();
            UIManager.Ins.OpenUI<UIDailyReward>();
            notiDailyReward.SetActive(DataManager.Ins.playerData.isCollectFullInDay != 1);
            buttonUI.SetCellActive(1, true);
            GameManager.Ins.ChangeState(GameState.MainMenu);
        });
    }

    public void ReLoadData() => homeLevelUI.ReLoad();

    public void BtnSpin()
    {
        if ( DataManager.Ins.playerData.CountLevelPassed >= 5)
        {
            //buttonUI.SetRecTF(3);
            UIManager.Ins.OpenUI<UISpin>();
        }
    }

    public void BtnOpenDailyReward()
    {
        if (DataManager.Ins.playerData.CountLevelPassed >= 3)
        {
            //buttonUI.SetRecTF(1);
            UIManager.Ins.OpenUI<UIDailyReward>();
        }
    }
    public void BtnHome()
    {
        //buttonUI.SetRecTF(2);
    }
    public void UpdateNotfi()
    {
        UIBtnSpin();
        UIBtnDailyReward();
    }

    public void BtnTut()
    {
        //buttonUI.SetRecTF(4);
        //UIManager.Ins.OpenUI<PopupUnlockBtnSpin>();

    }

    public void BtnShop()
    {
        //buttonUI.SetRecTF(0);
        //UIManager.Ins.OpenUI<PopupUnlockBtnDaily>();
    }
}
