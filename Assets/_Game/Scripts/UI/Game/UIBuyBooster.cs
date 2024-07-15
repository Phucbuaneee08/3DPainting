using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyBooster : UICanvas
{
    public TextMeshProUGUI textCostBooster;
    public TextMeshProUGUI numberBooster;
    public Image imgBG1;
    public Image imgBG2;
    public Image imgBG3;
    private int id;
    
    [SerializeField] TextMeshProUGUI textGold;
    [SerializeField] TextMeshProUGUI textDiamoind;
 
    void UpdateGoldText(int gold)
    {
        textGold.text = gold.ToString();
    }
    void UpdateDiamondText(int diamond)
    {
        textDiamoind.text = diamond.ToString();
    }
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        base.Open();
    }
    public void SetData(BoosterType boosterType)
    {
        textCostBooster.text = BoosterManager.Ins.GetBoosterPrice(boosterType).ToString();
        imgBG1.gameObject.SetActive(boosterType == BoosterType.FillAllColor);
        imgBG2.gameObject.SetActive(boosterType == BoosterType.FillByColor);
        imgBG3.gameObject.SetActive(boosterType == BoosterType.FindByColor);
    }
    public void BtnBuyBoosterGold()
    {
        if (UIManager.Ins.IsOpened<UIGameplay>())
        {
            if (DataManager.Ins.playerData.gold >= BoosterManager.Ins.costBooster)
            {
                DataManager.Ins.playerData.gold -= BoosterManager.Ins.costBooster;
                if (id == 1)
                {
                    DataManager.Ins.playerData.boosterFillAllColorQuantity += 3;
                }
                if(id == 2)
                {
                    DataManager.Ins.playerData.boosterFillByColorQuantity += 3;
                }
                DataManager.Ins.SaveData();
            }
        }
    }
    public void BtnBuyBoosterByAds()
    {

    }
    public void BtnExit()
    {
        UIManager.Ins.CloseUI<UIBuyBooster>();
        GameManager.Ins.ChangeState(GameState.GamePlay);
    }
}
