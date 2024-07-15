using AssetKits.ParticleImage;
using DG.Tweening;
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
    [SerializeField]
    private TMP_Text textNumberPass;

    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        GameManager.Ins.ChangeState(GameState.MainMenu);
        ReLoadData();
        base.Open();
        UpdateNotfi();
    }
    public void UIBtnDailyReward()
    {
        if (DataManager.Ins.playerData.CountLevelPassed >= 3)
        {

            lockBtnDailyReward.SetActive(false);
            if (DataManager.Ins.playerData.CountLevelPassed == 3)
            {
                if (DataManager.Ins.playerData.isShowDailyRewardFirst == false)
                {
                    StartCoroutine(IE_AutoShowDailyReward());
                    DataManager.Ins.playerData.isShowDailyRewardFirst = true;
                    DataManager.Ins.SaveData();
                }
            }
            notiDailyReward.SetActive(DataManager.Ins.playerData.isCollectFullInDay != 1);
        }
    }
    public void UIBtnSpin()
    {
        if (DataManager.Ins.playerData.CountLevelPassed >= 5)
        {
            lockBtnSpin.SetActive(false);
            if (DataManager.Ins.playerData.CountLevelPassed == 5)
            {
                if (DataManager.Ins.playerData.isShowSpinRewardFirst == false)
                {
                    StartCoroutine(IE_AutoShowSpin());
                    DataManager.Ins.playerData.isShowSpinRewardFirst = true;
                    DataManager.Ins.SaveData();
                }
            }
            if (DataManager.Ins.playerData.isSpinReward != 0)
            {
                notiSpin.SetActive(DataManager.Ins.playerData.isSpinReward != 0);
                Debug.Log("Show Notf");
            }
            else
            {
                notiSpin.SetActive(DataManager.Ins.playerData.isSpinReward != 0);
                textNumberPass.gameObject.SetActive(true);
                int numberPassed = DataManager.Ins.playerData.CountLevelPassed % 10;
                textNumberPass.text = numberPassed.ToString() + "/10";
            }
        }
    }
    IEnumerator IE_AutoShowSpin()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<UISpin>();
    }
    IEnumerator IE_AutoShowDailyReward()
    {
        yield return new WaitForEndOfFrame();
        UIManager.Ins.OpenUI<UIDailyReward>();
    }
    public void ReLoadData()
    {
        homeLevelUI.ReLoad();
    }
    public void BtnSpin()
    {
        if (DataManager.Ins.playerData.CountLevelPassed < 5) return;
        UIManager.Ins.OpenUI<UISpin>();
    }
    public void BtnOpenDailtReward()
    {
        if (DataManager.Ins.playerData.CountLevelPassed < 3) return;
        UIManager.Ins.OpenUI<UIDailyReward>();
    }
    public void UpdateNotfi()
    {
        UIBtnSpin();
        UIBtnDailyReward();
    }
}
