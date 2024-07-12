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

    private Transform tf;
    public List<DailyRewardTypeUI> typeList = new List<DailyRewardTypeUI>();

    public Transform Tf => tf;

    private void Awake()
    {
        tf = transform;
    }

    public void Init(DailyRewardData data, int todayIndex)
    {
        dayTmp.text = "DAY " + (data.dayIndex + 1).ToString();
        bool isToday = data.dayIndex == todayIndex;
        bool todayCollected = DataManager.Ins.playerData.isTodayCollected == 1 && isToday;
        bool isCollected = data.dayIndex < todayIndex || todayCollected;
        collectedObj.SetActive(isCollected);
        collectingObj.SetActive(isCollected);
        bgImg.color = isToday ? new Color(0, 255, 0) : bgImg.color;
        switch (data.dailyList.Count)
        {
            case 1:
                type1Obj.SetActive(true);
                typeList = GetTypeList(type1Obj.transform);
                break;
            case 2:
                type2Obj.SetActive(true);
                typeList = GetTypeList(type2Obj.transform);
                break;
            case 4:
                type4Obj.SetActive(true);
                typeList = GetTypeList(type4Obj.transform);
                break;
            default:
                Debug.Log("Null");
                break;
        }

        for (int i = 0; i < typeList.Count; i++)
        {
            typeList[i].Init(data.dailyList[i]);
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
            if (i == 0)
            {
                typeList[i].OnCollect(multiplier, () =>
                {
                    OnComplete?.Invoke();
                    collectedObj.SetActive(true);
                    collectingObj.SetActive(true);
                });
            }
            else
            {
                typeList[i].OnCollect(multiplier);
            }
        }
    }
}
