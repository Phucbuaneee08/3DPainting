using AssetKits.ParticleImage;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RewardTypeUI : MonoBehaviour
{
    [SerializeField] private GameObject coinSmall;
    [SerializeField] private GameObject magnifier;
    [SerializeField] private GameObject bucket;
    [SerializeField] private GameObject brush;
    [SerializeField] private ParticleImage coinPi;
    [SerializeField] private TMP_Text amountTmp;
    [SerializeField] RectTransform tfEnd;
    private DailyReward dailyReward;
    private SpinReward spinReward;
    private void Awake()
    {
        coinPi.gameObject.SetActive(false);
    }
    #region DailyReward
    public void InitDailyReward(DailyReward reward)
    {
        this.dailyReward = reward;

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
    public void OnCollectDailyReward(int multiplier, UnityAction OnComplete = null)
    {
        switch (dailyReward.rewardType)
        {
            case DailyRewardType.gold:
                CollectGold(multiplier, dailyReward.amount, OnComplete);
                break;
            case DailyRewardType.magnifier:
                CollectMagnifier(multiplier, dailyReward.amount, OnComplete);
                break;
            case DailyRewardType.bucket:
                CollectBucket(multiplier, dailyReward.amount, OnComplete);
                break;
            case DailyRewardType.brush:
                CollectBrush(multiplier, dailyReward.amount, OnComplete);
                break;
            default:
                break;
        }
    }
    #endregion
    #region Spin
    public void InitSpin(SpinReward reward)
    {
        this.spinReward = reward;

        switch (reward.rewardType)
        {
            case SpinRewardType.gold:
                coinSmall.SetActive(true);
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
    public void OnCollectSpin(int multiplier, UnityAction OnComplete = null)
    {
        switch (spinReward.rewardType)
        {
            case SpinRewardType.gold:
                CollectGold(multiplier, spinReward.amount, OnComplete);
                break;
            case SpinRewardType.magnifier:
                CollectMagnifier(multiplier, spinReward.amount, OnComplete);
                break;
            case SpinRewardType.bucket:
                CollectBucket(multiplier, spinReward.amount, OnComplete);
                break;
            case SpinRewardType.brush:
                CollectBrush(multiplier, spinReward.amount, OnComplete);
                break;
            default:
                break;
        }
    }
    #endregion
    private void CollectGold(int multiplier, int amount, UnityAction OnComplete = null)
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
            DataManager.Ins.ChangeGold(amount * multiplier);
            coinPi.onFirstParticleFinish.RemoveAllListeners();
        });
        coinPi.onLastParticleFinish.AddListener(() =>
        {
            OnComplete?.Invoke();
            coinPi.onLastParticleFinish.RemoveAllListeners();
            coinSmall.SetActive(false);
        });
    }

    private void CollectMagnifier(int multiplier, int amount, UnityAction OnComplete = null)
    {
        MoveImgItem(magnifier, () =>
        {
            DataManager.Ins.ChangeBoosterFindByColor(amount * multiplier);
            OnComplete?.Invoke();
            magnifier.SetActive(false);
        });
    }

    private void CollectBucket(int multiplier, int amount, UnityAction OnComplete = null)
    {
        MoveImgItem(bucket, () =>
        {
            DataManager.Ins.ChangeBoosterFillAllColor(amount * multiplier);
            OnComplete?.Invoke();
            bucket.SetActive(false);
        });
    }

    private void CollectBrush(int multiplier, int amount, UnityAction OnComplete = null)
    {
        MoveImgItem(brush, () =>
        {
            DataManager.Ins.ChangeBoosterFillByColor(amount * multiplier);
            OnComplete?.Invoke();
            brush.SetActive(false);
        });
    }

    private void MoveImgItem(GameObject gameObject, UnityAction onMoveComplete = null)
    {
        GameObject objIns = Instantiate(gameObject,gameObject.transform);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(objIns.transform.DOMove(tfEnd.position, 1.5f).SetEase(Ease.InOutQuad))
                .Join(objIns.transform.DOScale(Vector3.one / 2, 1.5f).SetEase(Ease.InOutQuad))
                .OnComplete(() =>
                {
                    onMoveComplete?.Invoke();
                   Destroy(objIns);
                });
    }

}
