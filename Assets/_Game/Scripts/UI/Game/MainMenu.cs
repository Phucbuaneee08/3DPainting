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
    [SerializeField] private Image imgProgress;
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
    public void SetActiveButton(int index, bool isActive)
    {
        buttonUI.SetActiveCell(index, isActive);
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
                textNumberPass.gameObject.SetActive(!isSpinReward);
                if (!isSpinReward)
                {
                    int amount = (levelsPassed + 5) % 10;
                    textNumberPass.text = $"{amount}/10";
                    SetProgressSpin(amount / 10);
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
        UIManager.Ins.OpenUI<PopupUnlockBtnDaily>().UnlockButton(1);
        Ultilities.DelayThenDoTask(this, 4f, () =>
        {
            UIManager.Ins.CloseUI<PopupUnlockBtnDaily>();
            UIManager.Ins.OpenUI<UIDailyReward>();
            notiDailyReward.SetActive(DataManager.Ins.playerData.isCollectFullInDay != 1);
            GameManager.Ins.ChangeState(GameState.MainMenu);
        });
    }

    public void ReLoadData() => homeLevelUI.ReLoad();

    public void BtnSpin()
    {
        if (DataManager.Ins.playerData.CountLevelPassed >= 5)
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
