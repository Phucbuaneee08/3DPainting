using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
public class DailyRewardUI : MonoBehaviour
{
    [SerializeField] private GameObject type1Obj;
    [SerializeField] private GameObject type2Obj;
    [SerializeField] private GameObject type4Obj;
    [SerializeField] private GameObject collectedObj;
    [SerializeField] private GameObject collectingObj;
    [SerializeField] private TMP_Text dayTmp;
    [SerializeField] private Image bgImg;
    [SerializeField] private GameObject giftBox2Obj;
    [SerializeField] private GameObject giftBox4Obj;
    private Transform tf;
    public List<RewardTypeUI> typeList = new List<RewardTypeUI>();
    public Sprite spriteColecting;
    private DailyRewardData data;
    public Transform Tf => tf;

    private void Awake()
    {
        tf = transform;
    }

    public void Init(DailyRewardData data, int todayIndex)
    {
        this.data = data;
        dayTmp.text = "Day " + (data.dayIndex + 1).ToString();
        bool isToday = data.dayIndex == todayIndex;
        bool todayCollected = DataManager.Ins.playerData.isTodayCollected == 1 && isToday;
        bool isCollected = data.dayIndex < todayIndex || todayCollected;

        collectedObj.SetActive(isCollected);
        collectingObj.SetActive(isCollected);
        bool isShowGiftBox = DataManager.Ins.playerData.isTodayCollected == 1;
        bgImg.sprite = isToday ? spriteColecting : bgImg.sprite;
        switch (data.dailyList.Count)
        {
            case 1:
                type1Obj.SetActive(true);
                typeList = GetTypeList(type1Obj.transform);
                break;
            case 2:
                type2Obj.SetActive(isCollected);
                giftBox2Obj.SetActive(!isCollected);
                typeList = GetTypeList(type2Obj.transform);
                break;
            case 4:
                type4Obj.SetActive(isCollected);
                giftBox4Obj.SetActive(!isCollected);
                typeList = GetTypeList(type4Obj.transform);
                break;
            default:
                break;
        }
        for (int i = 0; i < typeList.Count; i++)
        {
            typeList[i].InitDailyReward(data.dailyList[i]);
        }
    }
    public void UpdateUI()
    {
        switch (this.data.dailyList.Count)
        {
            case 2:
                giftBox2Obj.SetActive(false);
                type2Obj.SetActive(true);
                break;
            case 4:
                giftBox4Obj.SetActive(false);
                type4Obj.SetActive(true);
                Debug.Log("Type 4 Obj");
                break;
            default:
                break;
        }
        for (int i = 0; i < typeList.Count; i++)
        {
            typeList[i].InitDailyReward(this.data.dailyList[i]);
        }
    }
    private List<RewardTypeUI> GetTypeList(Transform parent)
    {
        RewardTypeUI type;
        List<RewardTypeUI> typeList = new List<RewardTypeUI>();
        foreach (Transform child in parent)
        {
            if (child.TryGetComponent(out type))
            {
                typeList.Add(type);
            }
        }
        return typeList;
    }

    public void OnCollect(int multiplier, UnityAction OnComplete = null)
    {
        UpdateUI();
        for (int i = 0; i < typeList.Count; i++)
        {
            typeList[i].OnCollectDailyReward(multiplier, () =>
            {
                OnComplete?.Invoke();
                collectedObj.SetActive(true);
                collectingObj.SetActive(true);
            });
        }
    }
}
