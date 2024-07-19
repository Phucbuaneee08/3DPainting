using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AssetKits.ParticleImage;
using UnityEngine.Events;
using System;
using Paint3D;

public class UIVictory : UICanvas
{
    [SerializeField] Button btnClaimX2;
    [SerializeField] TextMeshProUGUI textGoldBonus;
    [SerializeField] private int goldBonus;
    [SerializeField] private RectTransform tfCointBonus;

    [SerializeField] private ParticleImage coinPi;
    [SerializeField] private ParticleImage confesti;
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
        confesti.Play();
        GameManager.Ins.ChangeState(GameState.Finish);
        ReLoadUI();
        LoopButton();
    }
    private void ReLoadUI()
    {
        textGoldBonus.text = $"{"+"} {goldBonus}";
        coinPi.transform.position = tfCointBonus.position;
    }
    private void LoopButton()
    {
        btnClaimX2.transform.DOScale(1.1f, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
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
            DOVirtual.DelayedCall(3f, () =>
            {
                LevelManager.Ins.Home();
            });
            isClickBtn= true;
        }
    }
    private void CollectCoin(int amount)
    {

        coinPi.rateOverTime = amount;
        coinPi.Play();
       
        Ultilities.DelayThenDoTask(this, 1.8f, () =>
        {
            DataManager.Ins.ChangeGold(amount);
            AudioManager.Ins.OnGetMultiCoins(amount);
        });
       
      
    }
}
