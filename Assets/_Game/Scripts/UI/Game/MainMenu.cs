using AssetKits.ParticleImage;
using DG.Tweening;
using Paint3D;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UICanvas
{
    [SerializeField] private HomeLevelUI homeLevelUI;
    [SerializeField] private List<PopupCellUI> popupCells;
    [SerializeField] private GameObject notiSpin;
    [SerializeField] private GameObject notiDailyReward;
    [SerializeField] private GameObject notiShop;
    [SerializeField] private GameObject notiTut;
    [SerializeField] private GameObject objBar;
    [SerializeField] private TMP_Text textNumberPass;
    [SerializeField] private ButtonUI buttonUI;
    [SerializeField] private Image imgProgress;

    public UISpin uiSpin;
    public UIDailyReward uIDailyReward;
    public GameObject textGold;
    public RectTransform tfBtnSpin;
    public RectTransform tfBtnDailyReward;
    public override void Setup() => base.Setup();

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        AudioManager.Ins.OnPlayHomeMusic();
        LoadUIButtonHome();
        ReLoadData();
        UpdateNotfi();
    }
    public void ReLoadData()
    {
        homeLevelUI.ReLoad();
    }

    public void SetActiveButton(int index, bool isActive)
    {
        buttonUI.SetActiveCell(index, isActive);
    }

    private void LoadUIButtonHome()
    {
        SetActiveSelect((int)TypePopup.home);
        buttonUI.LoadUIButtonItem();
    }
    private void LoadPopupUI(int id)
    {
        StartCoroutine(IE_LoadPopupUI(id));
    }
    IEnumerator IE_LoadPopupUI(int id)
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < popupCells.Count; i++)
        {
            if (popupCells[i].idSelect == id)
            {
                popupCells[i].SetData(true);
            }
            else
            {
                popupCells[i].SetData(false);
            }
        }
    }
    #region Notf
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
                DataManager.Ins.playerData.isShowDailyRewardFirst = true;
                DataManager.Ins.SaveData();
                StartCoroutine(IE_AutoShowDailyReward());
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
        int countProgress = DataManager.Ins.playerData.countProgresses;
        bool isFirstSpinReward = DataManager.Ins.playerData.isShowSpinRewardFirst;
        bool isSpinReward = DataManager.Ins.playerData.isSpinReward != 0;

        if (levelsPassed >= 5)
        {
            if (!isFirstSpinReward && levelsPassed == 5)
            {
                GameManager.Ins.ChangeState(GameState.Pause);
                DataManager.Ins.playerData.isShowSpinRewardFirst = true;
                DataManager.Ins.SaveData();
                StartCoroutine(IE_AutoShowSpin());
            }
            else
            {
                notiSpin.SetActive(isSpinReward);
                objBar.gameObject.SetActive(!isSpinReward);
                if (!isSpinReward)
                {
                    textNumberPass.text = $"{countProgress}/10";
                    SetProgressSpin((float)countProgress / (float)10);
                }
            }
        }
    }
    public void SetProgressSpin(float amount)
    {
        imgProgress.fillAmount = amount;
    }
    IEnumerator IE_AutoShowSpin()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<PopupUnlockBtnSpin>().UnlockButton(3);
        objBar.gameObject.SetActive(false);
        notiSpin.SetActive(true);
        Ultilities.DelayThenDoTask(this, 4f, () =>
        {
            UIManager.Ins.CloseUI<PopupUnlockBtnSpin>();
            SetActiveSelect((int)TypePopup.spin);
            uiSpin.Open();
            notiSpin.SetActive(DataManager.Ins.playerData.isSpinReward != 0);
            GameManager.Ins.ChangeState(GameState.MainMenu);
        });
    }

    IEnumerator IE_AutoShowDailyReward()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<PopupUnlockBtnDaily>().UnlockButton(1);
        Ultilities.DelayThenDoTask(this, 4f, () =>
        {
            UIManager.Ins.CloseUI<PopupUnlockBtnDaily>();
            uIDailyReward.Open();
            SetActiveSelect((int)TypePopup.dailyRewards);
            notiDailyReward.SetActive(DataManager.Ins.playerData.isCollectFullInDay != 1);
            GameManager.Ins.ChangeState(GameState.MainMenu);
        });
    }
    #endregion
    public void BtnSpin()
    {
        uiSpin.Open();
        SetActiveSelect((int)TypePopup.spin);
    }

    public void BtnOpenDailyReward()
    {
        uIDailyReward.Open();
        SetActiveSelect((int)TypePopup.dailyRewards);

    }
    public void BtnHome()
    {
        SetActiveSelect((int)TypePopup.home);
    }
    public void UpdateNotfi()
    {
        UIBtnSpin();
        UIBtnDailyReward();
    }
    public void BtnTut()
    {
        SetActiveSelect((int)TypePopup.tut);
    }
    public void BtnShop()
    {
        SetActiveSelect((int)TypePopup.shop);
    }
    public void BtnSetting()
    {
        UIManager.Ins.OpenUI<UISettings>();
    }
    public void SetActiveSelect(int id)
    {
        LoadPopupUI(id);
       // buttonUI.SetRecTF(id); set button up
    }
}
[Serializable]
public enum TypePopup
{
    shop = 0,
    dailyRewards = 1,
    home = 2,
    spin = 3,
    tut = 4
}