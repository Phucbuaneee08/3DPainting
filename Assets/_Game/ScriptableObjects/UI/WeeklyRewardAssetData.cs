using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/Weekly Reward List")]
public class WeeklyRewardAssetData : ScriptableObject
{
    public List<WeeklyRewardData> weeklyList = new List<WeeklyRewardData>();

    public int GetWeekAmount()
    {
        return weeklyList.Count;
    }
}
[System.Serializable]
public class WeeklyRewardData
{
    public DailyRewardData[] dayArray = new DailyRewardData[7];

    public WeeklyRewardData()
    {
        for (int i = 0; i < dayArray.Length; i++)
        {
            dayArray[i] = new DailyRewardData();
            dayArray[i].dayIndex = i;
        }
    }
}

[System.Serializable]
public class DailyRewardData
{
    public int dayIndex;
    public List<DailyReward> dailyList;
}

[System.Serializable]
public class DailyReward
{
    public int amount;
    public DailyRewardType rewardType;
}