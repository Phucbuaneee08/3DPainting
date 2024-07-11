using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpinPieceUI : MonoBehaviour
{
    [SerializeField] private GameObject goldSmall;
    [SerializeField] private GameObject goldMedium;
    [SerializeField] private GameObject goldLarge;
    [SerializeField] private GameObject goldExtraLarge;
    [SerializeField] private GameObject goldHuge;
    [SerializeField] private GameObject magnifier;
    [SerializeField] private GameObject bucket;
    [SerializeField] private GameObject brush;
    [SerializeField] private TMP_Text amountTmp;

  
    public void Init(SpinReward reward)
    {
        switch (reward.rewardType)
        {
            case SpinRewardType.gold:
                if (reward.amount <= 5)
                {
                    goldSmall.SetActive(true);
                }
                else if (reward.amount > 5 && reward.amount <= 10)
                {
                    goldMedium.SetActive(true);
                }
                else if (reward.amount > 10 && reward.amount <= 15)
                {
                    goldMedium.SetActive(true);
                }
                else if (reward.amount > 15 && reward.amount <= 20)
                {
                    goldMedium.SetActive(true);
                }
                else if (reward.amount > 20 && reward.amount <= 30)
                {
                    goldMedium.SetActive(true);
                }
                amountTmp.text = "+" + reward.amount.ToString();
                break;
            case SpinRewardType.magnifier:
                magnifier.SetActive(true);
                amountTmp.text = "+" + reward.amount.ToString();
                break;
            case SpinRewardType.bucket:
                bucket.SetActive(true);
                amountTmp.text = "+" + reward.amount.ToString();
                break;
            case SpinRewardType.brush:
                brush.SetActive(true);
                amountTmp.text = "+" + reward.amount.ToString();
                break;
            default:
                break;
        }
    }
}
