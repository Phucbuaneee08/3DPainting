using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AssetKits.ParticleImage;
using UnityEngine.Events;
using System;


public class UIVictory : UICanvas
{
    [SerializeField] Button btnClaimX2;
    [SerializeField] TextMeshProUGUI textGoldBonus;
    [SerializeField] private int goldBonus;
    [SerializeField] private RectTransform tfCointBonus;

  [SerializeField] private ParticleImage coinPi;
    private bool isClickBtn = false;
 
    private void Start()
    {
        coinPi.Stop();
    }

    public void Home()
    {
        if (isClickBtn == false)
        {
            CollectCoin(goldBonus);
            DataManager.Ins.SaveData();
            DOVirtual.DelayedCall(3f, () =>
            {
                LevelManager.Ins.Home();
            });
            isClickBtn = true;
        }
    }
    public override void Open()
    {
        base.Open(); 
        isClickBtn = false;
        GameManager.Ins.ChangeState(GameState.Finish);
        ReLoadUI();
        LoopScaleButton();
    }
    private void ReLoadUI()
    {
        textGoldBonus.text = $"{"+"} {goldBonus}";
        coinPi.transform.position = tfCointBonus.position;
    }
    private void LoopScaleButton()
    {
        btnClaimX2.transform.localScale = new Vector3(1, 1, 1);
        btnClaimX2.transform.DOScale(1.2f, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void NextLevel()
    {
        LevelManager.Ins.NextLevel();
    }
    public void ButtonClaimX2()
    {
        if (isClickBtn == false)
        {
            CollectCoin(goldBonus * 2);
            DataManager.Ins.SaveData();
            DOVirtual.DelayedCall(3.2f, () =>
            {
                LevelManager.Ins.Home();
            });
            isClickBtn= true;
        }
    }
    private void CollectCoin(int amount)
    {
        float duration = 0.75f;
        coinPi.rateOverTime = 30 / duration;
        coinPi.Play();
        Ultilities.DelayThenDoTask(this, 2f, () =>
        {
            DataManager.Ins.ChangeGold(amount);
        });
    }
}
