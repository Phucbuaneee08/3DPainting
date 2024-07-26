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
    public List<DailyRewardTypeUI> typeList = new List<DailyRewardTypeUI>();
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
                if (isShowGiftBox && isToday)
                {
                    type2Obj.SetActive(true);
                }
                else
                {
                    giftBox2Obj.SetActive(true);
                }
                typeList = GetTypeList(type2Obj.transform);
                break;
            case 4:
                if (isShowGiftBox && isToday)
                {
                    type4Obj.SetActive(true);
                }
                else
                {
                    giftBox4Obj.SetActive(true);

                }
                typeList = GetTypeList(type4Obj.transform);
                break;
            default:
                break;
        }
        for (int i = 0; i < typeList.Count; i++)
        {
            typeList[i].Init(data.dailyList[i]);
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
            typeList[i].Init(this.data.dailyList[i]);
        }
    }
    private List<DailyRewardTypeUI> GetTypeList(Transform parent)
    {
        DailyRewardTypeUI type;
        List<DailyRewardTypeUI> typeList = new List<DailyRewardTypeUI>();
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
        for (int i = 0; i < typeList.Count; i++)
        {
            typeList[i].OnCollect(multiplier, () =>
            {
                OnComplete?.Invoke();
                collectedObj.SetActive(true);
                collectingObj.SetActive(true);
                UpdateUI();
            });
        }
    }
}
