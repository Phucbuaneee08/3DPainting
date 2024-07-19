using AssetKits.ParticleImage;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class UISpin : UICanvas
{
    [SerializeField] private List<int> probabilities = new List<int>();
    [SerializeField] private List<SpinReward> rewardList = new List<SpinReward>();
    [SerializeField] private List<SpinPieceUI> pieceList = new List<SpinPieceUI>();
    [SerializeField] private Transform circleTf;
    [SerializeField] private Transform circleCenter;
    [SerializeField] private ParticleImage goldPi;
    [SerializeField] private GameObject btnPlaySpin;
    [SerializeField] private GameObject btnPlayIap;
    [SerializeField] private TMP_Text textPlay;
    [SerializeField] private RectTransform tfGold;
    private int rewardIndex;
    public bool isRotate = false;
    public override void Setup()
    {
        base.Setup();
    }

    public override void Open()
    {
        base.Open();
        goldPi.Stop();
        isRotate = false;
        if (rewardList == null || rewardList.Count == 0)
        {
            return;
        }

        if (pieceList == null || pieceList.Count != rewardList.Count)
        {
            return;
        }

        for (int i = 0; i < rewardList.Count; i++)
        {
            pieceList[i].Init(rewardList[i]);
        }
        goldPi.attractorTarget = tfGold.transform;
        goldPi.duration = 0.25f;
        goldPi.lifetime = 1.25f;
        UpdateBtn();
    }
    public void UpdateBtn()
    {
        if (DataManager.Ins.playerData.isSpinReward != 0)
        {
            textPlay.text = $"Spin";
        }
        else
        {
            textPlay.text = $"Pass: {DataManager.Ins.playerData.countProgresses}/10";
        }
    }
    private void Update()
    {
        if (circleCenter != null)
        {
            circleCenter.rotation = Quaternion.identity;
        }
    }

    private void StartSpinning()
    {
        if (DataManager.Ins.playerData.isSpinReward == 0) return;
        isRotate = true;
        if (circleTf == null)
        {
            return;
        }
        rewardIndex = GetRandomIndex();
        DataManager.Ins.playerData.isSpinReward = 0;
        DataManager.Ins.playerData.countProgresses = 0;
        DataManager.Ins.SaveData();
        float pieceRotZ = 360f / pieceList.Count;
        float extraSpin = Random.Range(0f, pieceRotZ);
        float rotationZ = 12 * 360f + rewardIndex * pieceRotZ /*+ extraSpin*/;
        Vector3 rotation = new Vector3(0f, 0f, rotationZ);
        Tween tween = circleTf.DOLocalRotate(rotation, 3.5f, RotateMode.FastBeyond360);
        tween.SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            //SpinCompleted();
            UIManager.Ins.OpenUI<PopupClaim>().OnintSpin(rewardList[rewardIndex]);
        });
    }
    public void SpinCompleted()
    {
        SpinReward reward = rewardList[rewardIndex];
        switch (reward.rewardType)
        {
            case SpinRewardType.gold:
                CollectGoldReward(reward.amount);
                Debug.Log("Gold " + reward.amount);
                break;
            case SpinRewardType.magnifier:
                CollectMagnifierReward(reward.amount);
                Debug.Log("magnifier " + reward.amount);
                break;
            case SpinRewardType.bucket:
                CollectBucketReward(reward.amount);
                Debug.Log("bucket " + reward.amount);
                break;
            case SpinRewardType.brush:
                CollectBrushReward(reward.amount);
                Debug.Log("brush " + reward.amount);
                break;
            default:
                Debug.LogError("Invalid spin reward type !!?");
                break;
        }

    }

    private int GetRandomIndex()
    {
        if (probabilities == null || probabilities.Count == 0)
        {
            return 0;
        }

        int totalWeight = 0;
        for (int i = 0; i < probabilities.Count; i++)
        {
            totalWeight += probabilities[i];
        }

        if (totalWeight == 0)
        {
            return 0;
        }

        int randomValue = Random.Range(0, totalWeight);

        int cumulativeWeight = 0;
        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulativeWeight += probabilities[i];
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }
        return Random.Range(0, pieceList.Count);
    }

    private void CollectGoldReward(int amount)
    {
        float duration = 0.75f;
        goldPi.rateOverTime = 30 / duration;
        goldPi.Play();
        goldPi.onLastParticleFinish.AddListener(() =>
        {
            goldPi.onLastParticleFinish.RemoveAllListeners();
        });
        Ultilities.DelayThenDoTask(this, 2f, () =>
        {
            DataManager.Ins.ChangeGold(amount);
            isRotate = false;
        });
    }
    private void CollectMagnifierReward(int amount)
    {
        Ultilities.DelayThenDoTask(this, 2f, () =>
        {
            DataManager.Ins.ChangeBoosterFindByColor(amount);
            isRotate = false;
        });
    }
    private void CollectBucketReward(int amount)
    {
        Ultilities.DelayThenDoTask(this, 2f, () =>
        {
            DataManager.Ins.ChangeBoosterFillAllColor(amount);
            isRotate = false;
        });
    }
    private void CollectBrushReward(int amount)
    {
        Ultilities.DelayThenDoTask(this, 2f, () =>
        {
            DataManager.Ins.ChangeBoosterFillByColor(amount);
            isRotate = false;
        });
    }
    public void ButtonSpin()
    {
        if (isRotate == true) return;
        StartSpinning();
    }
    public void BtnExit()
    {
        if (isRotate == true) return;
        UIManager.Ins.CloseUI<UISpin>();
        UIManager.Ins.GetUI<MainMenu>().UIBtnSpin();
    }
}

[Serializable]
public class SpinReward
{
    public int amount;
    public SpinRewardType rewardType;
}