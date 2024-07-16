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
    [SerializeField] private GameObject lockBtnSpin;
    [SerializeField] private GameObject lockBtnDailyReward;
    [SerializeField] private TMP_Text textNumberPass;

    public RectTransform tfBtnSpin;
    public RectTransform tfBtnDailyReward;

    public override void Setup() => base.Setup();

    public override void Open()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        AudioManager.Ins.OnPlayHomeMusic();
        ReLoadData();
        base.Open();
        UpdateNotfi();
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
            }
            else
            {
                lockBtnDailyReward.SetActive(false);
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
                lockBtnSpin.SetActive(false);
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
        Ultilities.DelayThenDoTask(this, 6f, () =>
        {
            UIManager.Ins.CloseUI<PopupUnlockBtnSpin>();
            UIManager.Ins.OpenUI<UISpin>();
            lockBtnSpin.SetActive(false);
            notiSpin.SetActive(DataManager.Ins.playerData.isSpinReward != 0);
            GameManager.Ins.ChangeState(GameState.MainMenu);
        });
    }

    IEnumerator IE_AutoShowDailyReward()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<PopupUnlockBtnDaily>().UnlockButton();
        Ultilities.DelayThenDoTask(this, 6f, () =>
        {
            UIManager.Ins.CloseUI<PopupUnlockBtnDaily>();
            UIManager.Ins.OpenUI<UIDailyReward>();
            lockBtnDailyReward.SetActive(false);
            notiDailyReward.SetActive(DataManager.Ins.playerData.isCollectFullInDay != 1);
            GameManager.Ins.ChangeState(GameState.MainMenu);
        });
    }

    public void ReLoadData() => homeLevelUI.ReLoad();

    public void BtnSpin()
    {
        if (DataManager.Ins.playerData.CountLevelPassed >= 5)
            UIManager.Ins.OpenUI<UISpin>();
    }

    public void BtnOpenDailtReward()
    {
        if (DataManager.Ins.playerData.CountLevelPassed >= 3)
            UIManager.Ins.OpenUI<UIDailyReward>();
    }

    public void UpdateNotfi()
    {
        UIBtnSpin();
        UIBtnDailyReward();
    }

    public void BtnTut() => UIManager.Ins.OpenUI<PopupUnlockBtnSpin>();

    public void BtnShop() => UIManager.Ins.OpenUI<PopupUnlockBtnDaily>();
}
