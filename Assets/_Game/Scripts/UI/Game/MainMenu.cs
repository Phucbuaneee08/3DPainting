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
    private PlayerData playerData;
    public override void Setup() => base.Setup();

    public override void Open()
    {
        base.Open();
        playerData = DataManager.Ins.playerData;
        GameManager.Ins.ChangeState(GameState.MainMenu);
        AudioManager.Ins.OnPlayHomeMusic();
        ReLoadData();
        LoadUIButtonHome();
        UpdateNotfi();
    }

    private void LoadUIButtonHome()
    {
        if (playerData != null)
        {
            buttonUI.LoadUIButtonItem(playerData);
        }
    }

    public void UIBtnDailyReward()
    {
        if (playerData == null) return;

        int levelsPassed = playerData.CountLevelPassed;
        bool isFirstDailyReward = playerData.isShowDailyRewardFirst;
        bool isFullCollect = playerData.isCollectFullInDay == 1;

        if (levelsPassed >= 3)
        {
            if (!isFirstDailyReward && levelsPassed == 3)
            {
                GameManager.Ins.ChangeState(GameState.Pause);
                StartCoroutine(IE_AutoShowDailyReward());
                playerData.isShowDailyRewardFirst = true;
                DataManager.Ins.SaveData();
            }
            else
            {
                notiDailyReward.SetActive(!isFullCollect);
            }
        }
    }

    public void UIBtnSpin()
    {
        if (playerData == null) return;

        int levelsPassed = playerData.CountLevelPassed;
        bool isFirstSpinReward = playerData.isShowSpinRewardFirst;
        bool isSpinReward = playerData.isSpinReward != 0;

        if (levelsPassed >= 5)
        {
            if (!isFirstSpinReward && levelsPassed == 5)
            {
                GameManager.Ins.ChangeState(GameState.Pause);
                StartCoroutine(IE_AutoShowSpin());
                playerData.isShowSpinRewardFirst = true;
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
            notiSpin.SetActive(playerData.isSpinReward != 0);
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
            notiDailyReward.SetActive(playerData.isCollectFullInDay != 1);
            GameManager.Ins.ChangeState(GameState.MainMenu);
        });
    }

    public void ReLoadData() => homeLevelUI.ReLoad();

    public void BtnSpin()
    {
        if (playerData != null && playerData.CountLevelPassed >= 5)
        {
            UIManager.Ins.OpenUI<UISpin>();
        }
    }

    public void BtnOpenDailyReward()
    {
        if (playerData != null && playerData.CountLevelPassed >= 3)
        {
            UIManager.Ins.OpenUI<UIDailyReward>();
        }
    }

    public void UpdateNotfi()
    {
        UIBtnSpin();
        UIBtnDailyReward();
    }

    public void BtnTut() => UIManager.Ins.OpenUI<PopupUnlockBtnSpin>();

    public void BtnShop() => UIManager.Ins.OpenUI<PopupUnlockBtnDaily>();
}
