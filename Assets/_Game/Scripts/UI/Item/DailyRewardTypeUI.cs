using AssetKits.ParticleImage;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DailyRewardTypeUI : MonoBehaviour
{
    [SerializeField] private GameObject coinSmall;
    [SerializeField] private GameObject magnifier;
    [SerializeField] private GameObject bucket;
    [SerializeField] private GameObject brush;
    [SerializeField] private ParticleImage coinPi;
    [SerializeField] private TMP_Text amountTmp;

    private DailyReward reward;
    public void Init(DailyReward reward)
    {
        this.reward = reward;

        switch (reward.rewardType)
        {
            case DailyRewardType.gold:
                coinSmall.SetActive(true);
                amountTmp.text = "+" + reward.amount.ToString();
                break;
            case DailyRewardType.magnifier:
                magnifier.SetActive(true);
                amountTmp.text = "+" + reward.amount.ToString();
                break;
            case DailyRewardType.bucket:
                bucket.SetActive(true);
                amountTmp.text = "+" + reward.amount.ToString();
                break;
            case DailyRewardType.brush:
                brush.SetActive(true);
                amountTmp.text = "+" + reward.amount.ToString();
                break;
            default:
                break;
        }
    }

    public void OnCollect(int multiplier, UnityAction OnComplete = null)
    {
        switch (reward.rewardType)
        {
            case DailyRewardType.gold:
                coinPi.Play();
                coinPi.onFirstParticleFinish.AddListener(() =>
                {
                    coinPi.onFirstParticleFinish.RemoveAllListeners();
                });
                coinPi.onLastParticleFinish.AddListener(() =>
                {
                    OnComplete?.Invoke();
                    coinPi.onLastParticleFinish.RemoveAllListeners();
                });
                break;
            case DailyRewardType.magnifier:

                break;
            case DailyRewardType.bucket:

                break;
            case DailyRewardType.brush:

                break;
            default:
                break;
        }
    }
}
