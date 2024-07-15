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
    private void Awake()
    {
        coinPi.gameObject.SetActive(false);
    }
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
                CollectGold(multiplier, OnComplete); 
                break;
            case DailyRewardType.magnifier:
                CollectMagnifier(multiplier, OnComplete);
                break;
            case DailyRewardType.bucket:
                Collectbucket(multiplier, OnComplete);
                break;
            case DailyRewardType.brush:
                CollectBrush(multiplier, OnComplete);
                break;
            default:
                break;
        }
    }
    private void CollectGold(int multiplier, UnityAction OnComplete = null)
    {
        coinPi.gameObject.SetActive(true);
        coinPi.attractorTarget = UIManager.Ins.GetUI<MainMenu>().textGold.transform;
        coinPi.Play();
        coinPi.onFirstParticleFinish.AddListener(() =>
        {
            coinPi.onFirstParticleFinish.RemoveAllListeners();
        });
        coinPi.onFirstParticleFinish.AddListener(() =>
        {
            DataManager.Ins.ChangeGold(reward.amount * multiplier);
            coinPi.onFirstParticleFinish.RemoveAllListeners();
        });
        coinPi.onLastParticleFinish.AddListener(() =>
        {
            OnComplete?.Invoke();
            coinPi.onLastParticleFinish.RemoveAllListeners();
        });
        Debug.Log("collect Gold");
    }
    private void CollectMagnifier(int multiplier, UnityAction OnComplete = null)
    {
        DataManager.Ins.ChangeMagnifier(reward.amount * multiplier);
        OnComplete?.Invoke();
        Debug.Log("collect Magnfier");
    }
    private void Collectbucket(int multiplier, UnityAction OnComplete = null)
    {
        DataManager.Ins.ChangeBucket(reward.amount * multiplier);
        OnComplete?.Invoke();
        Debug.Log("collect Bucket");
    }
    private void CollectBrush(int multiplier, UnityAction OnComplete = null)
    {
        DataManager.Ins.ChangeBrush(reward.amount * multiplier);
        OnComplete?.Invoke();
        Debug.Log("collect brush");
    }
}
