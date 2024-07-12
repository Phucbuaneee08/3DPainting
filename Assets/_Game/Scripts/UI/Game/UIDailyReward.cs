using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UIDailyReward : UICanvas
{
    [SerializeField] private GameObject claimObj;
    [SerializeField] private GameObject claimX2Obj;
    [SerializeField] private GameObject waitObj;
    [SerializeField] private TMP_Text timeTmp;
    [SerializeField] private DailyRewardUI[] dayArray;
    [SerializeField] private WeeklyRewardAssetData weeklyRewardList;
    private int todayIndex;
    private int weekIndex;
    private DateTime nextDay;
    private WeeklyRewardData weeklyData;

    public override void Open()
    {
        base.Open();

        DateTime now = DateTime.Now;
        int daysNow = (int)now.Subtract(new DateTime(1970, 1, 1)).TotalDays;
        bool isNewDay = daysNow > DataManager.Ins.playerData.daysLastOpen;

        if (isNewDay && DataManager.Ins.playerData.isTodayCollected == 1)
        {
            DataManager.Ins.playerData.isTodayCollected = 0;
            DataManager.Ins.playerData.daysCollected++;
        }

        DataManager.Ins.playerData.daysLastOpen = daysNow;
        DataManager.Ins.SaveData();

        todayIndex = DataManager.Ins.playerData.daysCollected % 7;
        weekIndex = DataManager.Ins.playerData.daysCollected / 7;
        weekIndex %= weeklyRewardList.GetWeekAmount();

        claimObj.SetActive(DataManager.Ins.playerData.isTodayCollected == 0 || DataManager.Ins.playerData.isTodayCollectFree == 1);
        claimX2Obj.SetActive(DataManager.Ins.playerData.isTodayCollected == 0 || DataManager.Ins.playerData.isTodayCollectFree == 1);
        waitObj.SetActive(DataManager.Ins.playerData.isTodayCollected == 1 && DataManager.Ins.playerData.isTodayCollectFree == 1);

        weeklyData = weeklyRewardList.weeklyList[weekIndex];
        DailyRewardData dailyData;
        for (int i = 0; i < dayArray.Length; i++)
        {
            dailyData = weeklyData.dayArray[i];
            dayArray[i].Init(dailyData, todayIndex);
        }
        dayArray[todayIndex].Tf.SetAsLastSibling();
        UpdateTimeRemaining(now);
    }

    private void UpdateTimeRemaining(DateTime now)
    {
        nextDay = now.Date.AddDays(1);
        // TimeSpan timeRemaining = nextDay - now;
        // timeTmp.text = $"Next reward in: {timeRemaining.Hours:D2}:{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";
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

    public void BtnExit()
    {
        UIManager.Ins.CloseUI<UIDailyReward>();
    }

    public void ButtonClaim()
    {
        if (DataManager.Ins.playerData.isTodayCollectFree == 1) return;
        DataManager.Ins.playerData.isTodayCollected = 1;
        DataManager.Ins.playerData.isTodayCollectFree = 1;
        DataManager.Ins.SaveData();
        claimObj.SetActive(true);
        claimX2Obj.SetActive(true);
        waitObj.SetActive(false);
        dayArray[todayIndex].OnCollect(1, () =>
        {
        });
    }

    public void ButtonClaimX2()
    {
        if (DataManager.Ins.playerData.isTodayCollectFree == 1)
        {
            dayArray[todayIndex].OnCollect(1, () =>
            {
            });
        }
        else
        {
            dayArray[todayIndex].OnCollect(2, () =>
            {
            });
        }
        claimObj.SetActive(false);
        claimX2Obj.SetActive(false);
        waitObj.SetActive(true);
        DataManager.Ins.playerData.isTodayCollectFree = 1;
        DataManager.Ins.SaveData();
    }
}
