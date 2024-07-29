using AssetKits.ParticleImage;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class UISpin : MonoBehaviour
{
    [SerializeField] private List<int> probabilities = new List<int>();
    [SerializeField] private List<SpinReward> rewardList = new List<SpinReward>();
    [SerializeField] private List<RewardTypeUI> pieceList = new List<RewardTypeUI>();
    [SerializeField] private Transform circleTf;
    [SerializeField] private Transform circleCenter;
    [SerializeField] private GameObject btnPlaySpin;
    [SerializeField] private GameObject btnPlayIap;
    [SerializeField] private TMP_Text textPlay;
    private RewardTypeUI rewardTypeUISpin;
    private int rewardIndex;
    public bool isRotate = false;

    public void Open()
    {
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
            pieceList[i].InitSpin(rewardList[i]);
        }
        UpdateBtn();
        Debug.LogError("Spin");

    }
    public void UpdateBtn()
    {

        if (DataManager.Ins.playerData.isSpinReward != 0)
        {
            textPlay.text = $"Spin";
        }
        else
        {
            textPlay.text = $"{DataManager.Ins.playerData.countProgresses}/10";
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
        /*DataManager.Ins.playerData.isSpinReward = 0;
        DataManager.Ins.playerData.countProgresses = 0;
        DataManager.Ins.SaveData();*/
        float pieceRotZ = 360f / pieceList.Count;
        float extraSpin = Random.Range(0f, pieceRotZ);
        float rotationZ = 12 * 360f + rewardIndex * pieceRotZ /*+ extraSpin*/;
        Vector3 rotation = new Vector3(0f, 0f, rotationZ);
        Tween tween = circleTf.DOLocalRotate(rotation, 3.5f, RotateMode.FastBeyond360);
        tween.SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            // UIManager.Ins.OpenUI<PopupClaim>().OnintSpin(rewardList[rewardIndex]);
            SpinCompleted();
        });
    }

    private void SpinCompleted()
    {
        RewardTypeUI rewardTypeUISpin = pieceList[rewardIndex];
        rewardTypeUISpin.OnCollectSpin(1, () =>
        {
            isRotate = false;
            UpdateBtn();
            UIManager.Ins.GetUI<MainMenu>().UIBtnSpin();
        });
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

    public void ButtonSpin()
    {
        if (isRotate == true) return;
        StartSpinning();
    }
}

[Serializable]
public class SpinReward
{
    public int amount;
    public SpinRewardType rewardType;
}