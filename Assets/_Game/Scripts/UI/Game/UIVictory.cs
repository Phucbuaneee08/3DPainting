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
    [SerializeField] private ParticleImage coinPi;
    private void Start()
    {
        coinPi.Stop();
    }
   
    public void Home()
    {
        CollectCoin(DataManager.Ins.playerData.gold + goldBonus);
        DataManager.Ins.SaveData();
        DOVirtual.DelayedCall(3f, () =>
        {
            LevelManager.Ins.Home();
        });
    }
    public override void Open()
    {
        base.Open();
        textGoldBonus.text = $"{"+"} {goldBonus}";
        GameManager.Ins.ChangeState(GameState.Finish);
    }
    public void NextLevel()
    {
        LevelManager.Ins.NextLevel();
    }
    public void ButtonClaimX2()
    {
        CollectCoin(DataManager.Ins.playerData.gold + (goldBonus * 2));
        DataManager.Ins.SaveData();
        DOVirtual.DelayedCall(3.2f, () =>
        {
            LevelManager.Ins.Home();
        });
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
