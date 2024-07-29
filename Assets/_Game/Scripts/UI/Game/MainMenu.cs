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
    public TypePopup currentType;
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
        LoadUI();
        Debug.Log("2");
    }

    public void ReLoadData() => homeLevelUI.ReLoad();

    public void SetActiveButton(int index, bool isActive) => buttonUI.SetActiveCell(index, isActive);

    private void LoadUI()
    {
        ReLoadData();
        buttonUI.LoadUIButtonItem();
        LoadStart();
        UpdateNotfi();
    }

    private void LoadPopupUI(int id)
    {
        popupCells.ForEach(cell =>
        {
            bool isActive = cell.idSelect == id;
            cell.SetData(isActive);
            buttonUI.SetSpritesBtn(popupCells.IndexOf(cell), id);
        });
    }

    private void LoadStart()
    {
        buttonUI.SetUIStart((int)TypePopup.home);
        popupCells[0].SetData(true);
        currentType = TypePopup.home;
    }

    #region Notifications
    public void UIBtnDailyReward()
    {
        int levelsPassed = DataManager.Ins.playerData.CountLevelPassed;
        if (levelsPassed >= 3)
        {
            if (!DataManager.Ins.playerData.isShowDailyRewardFirst && levelsPassed == 3)
            {
                GameManager.Ins.ChangeState(GameState.Pause);
                DataManager.Ins.playerData.isShowDailyRewardFirst = true;
                DataManager.Ins.SaveData();
                StartCoroutine(IE_AutoShowDailyReward());
                Debug.LogError("auto reward");
            }
            else
            {
                notiDailyReward.SetActive(DataManager.Ins.playerData.isCollectFullInDay != 1);
            }
        }
    }

    public void UIBtnSpin()
    {
        int levelsPassed = DataManager.Ins.playerData.CountLevelPassed;
        if (levelsPassed >= 5)
        {
            if (!DataManager.Ins.playerData.isShowSpinRewardFirst && levelsPassed == 5)
            {
                GameManager.Ins.ChangeState(GameState.Pause);
                DataManager.Ins.playerData.isShowSpinRewardFirst = true;
                DataManager.Ins.SaveData();
                StartCoroutine(IE_AutoShowSpin());
            }
            else
            {
                bool isSpinReward = DataManager.Ins.playerData.isSpinReward != 0;
                notiSpin.SetActive(isSpinReward);
                objBar.SetActive(!isSpinReward);
                if (!isSpinReward)
                {
                    int countProgress = DataManager.Ins.playerData.countProgresses;
                    textNumberPass.text = $"{countProgress}/10";
                    SetProgressSpin((float)countProgress / 10);
                }
            }
        }
    }

    public void SetProgressSpin(float amount) => imgProgress.fillAmount = amount;

    private IEnumerator IE_AutoShowSpin()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<PopupUnlockBtnSpin>().UnlockButton(3);
        objBar.SetActive(false);
        notiSpin.SetActive(true);
        yield return new WaitForSeconds(4f);
        UIManager.Ins.CloseUI<PopupUnlockBtnSpin>();
        SetActiveSelect((int)TypePopup.spin);
        uiSpin.Open();
        currentType = TypePopup.spin;
        notiSpin.SetActive(DataManager.Ins.playerData.isSpinReward != 0);
        GameManager.Ins.ChangeState(GameState.MainMenu);
    }

    private IEnumerator IE_AutoShowDailyReward()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<PopupUnlockBtnDaily>().UnlockButton(1);
        yield return new WaitForSeconds(4f);
        UIManager.Ins.CloseUI<PopupUnlockBtnDaily>();
        uIDailyReward.Open();
        Debug.Log(11);
        SetActiveSelect((int)TypePopup.dailyRewards);
        currentType = TypePopup.dailyRewards;
        notiDailyReward.SetActive(DataManager.Ins.playerData.isCollectFullInDay != 1);
        GameManager.Ins.ChangeState(GameState.MainMenu);
    }
    #endregion

    public void BtnSpin()
    {
        OpenPopup(TypePopup.spin);
        uiSpin.Open();
    }
    public void BtnOpenDailyReward()
    {
        OpenPopup(TypePopup.dailyRewards);
        uIDailyReward.Open();
    }
    private bool IsClick(int index) => buttonUI.listCell[index].typePopup == currentType;

    public void BtnHome() => OpenPopup(TypePopup.home);

    public void UpdateNotfi()
    {
        UIBtnSpin();
        UIBtnDailyReward();
    }

    public void BtnTut()
    {
        OpenPopup(TypePopup.tut);
        UIManager.Ins.OpenUI<PopupRate>();
    }

    public void BtnShop() => OpenPopup(TypePopup.shop);

    public void BtnSetting() => UIManager.Ins.OpenUI<UISettings>();

    private void OpenPopup(TypePopup type)
    {
        int index = (int)type;
        if (IsClick(index)) return;
        UIManager.Ins.OpenUI<UIShortLoading>().With(() =>
        {
            SetActiveSelect(index);
            currentType = type;
        });
    }
    public void SetActiveSelect(int id) => LoadPopupUI(id);
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