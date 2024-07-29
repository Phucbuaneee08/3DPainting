using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class UIDailyReward : MonoBehaviour
{
    [SerializeField] private GameObject claimObj;
    [SerializeField] private GameObject claimX2Obj;
    [SerializeField] private GameObject waitObj;
    [SerializeField] private TMP_Text timeTmp;
    [SerializeField] private TMP_Text textClaimX2;
    [SerializeField] private DailyRewardUI[] dayArray;
    [SerializeField] private WeeklyRewardAssetData weeklyRewardList;
    private int todayIndex;
    private int weekIndex;
    private DateTime nextDay;
    private WeeklyRewardData weeklyData;

    public void Open()
    {
        todayIndex = DataManager.Ins.playerData.daysCollected % 7;
        weekIndex = DataManager.Ins.playerData.daysCollected / 7;
        weekIndex %= weeklyRewardList.GetWeekAmount();
        bool isCollectInDay = DataManager.Ins.playerData.isCollectFullInDay == 1;
        claimObj.SetActive(!isCollectInDay);
        claimX2Obj.SetActive(!isCollectInDay);
        if (DataManager.Ins.playerData.isTodayCollectFree == 1)
        {
            textClaimX2.text = "Claim More";
        }
        else
        {
            textClaimX2.text = "Claim X2";
        }
        waitObj.SetActive(isCollectInDay);

        weeklyData = weeklyRewardList.weeklyList[weekIndex];
        DailyRewardData dailyData;
        for (int i = 0; i < dayArray.Length; i++)
        {
            dailyData = weeklyData.dayArray[i];
            dayArray[i].Init(dailyData, todayIndex);
        }
        //dayArray[todayIndex].Tf.SetAsLastSibling();
        UpdateTimeRemaining(DateTime.Now);
        Debug.Log("Load Daily Reward");
    }
    private void UpdateTimeRemaining(DateTime now)
    {
        nextDay = now.Date.AddDays(1);
    }

    private void Update()
    {
        if (nextDay != default)
        {
            if (DataManager.Ins.playerData.isTodayCollectFree == 1)
            {
                TimeSpan timeRemaining = nextDay - DateTime.Now;
                timeTmp.text = $"{timeRemaining.Hours:D2}:{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";
            }
        }
    }

    public void ButtonClaim()
    {
        if (DataManager.Ins.playerData.isTodayCollectFree == 1) return;
        DataManager.Ins.playerData.isTodayCollected = 1;
        DataManager.Ins.playerData.isTodayCollectFree = 1;
        DataManager.Ins.SaveData();
        claimObj.SetActive(true);
        claimX2Obj.SetActive(true);
        textClaimX2.text = "Claim More";
        waitObj.SetActive(false);
        dayArray[todayIndex].OnCollect(1, () =>
        {
        });
    }
    public void ButtonClaimX2()
    {
        if (DataManager.Ins.playerData.isCollectFullInDay == 1) return;
        DataManager.Ins.playerData.isTodayCollected = 1;
        if (DataManager.Ins.playerData.isTodayCollectFree == 1)
        {
            dayArray[todayIndex].OnCollect(1, () =>
            {
                UIManager.Ins.GetUI<MainMenu>().UIBtnDailyReward();
            });
        }
        else
        {
            dayArray[todayIndex].OnCollect(2, () =>
            {
                UIManager.Ins.GetUI<MainMenu>().UIBtnDailyReward();
            });
        }
        DataManager.Ins.playerData.isTodayCollectFree = 1;
        DataManager.Ins.playerData.isCollectFullInDay = 1;
        claimObj.SetActive(false);
        claimX2Obj.SetActive(false);
        waitObj.SetActive(true);
        DataManager.Ins.SaveData();
    }
}
