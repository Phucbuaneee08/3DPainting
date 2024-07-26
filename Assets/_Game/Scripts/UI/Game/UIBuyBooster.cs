using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyBooster : UICanvas
{
    public TextMeshProUGUI boosterName;
    private int _cost;
    private BoosterType _boosterType;

    public GameObject adsButton;

    public TextMeshProUGUI textCostBooster;
    public TextMeshProUGUI numberBooster;
    public Image imgFillAllColor;
    public Image imgFillByColor;
    public Image imgFindByColor;
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

   
    public void SetData(BoosterType boosterType,string name)
    {
        if (LevelManager.Ins.IsCanUseAds) 
        { 
            adsButton.SetActive(true); 
            LevelManager.Ins.IsCanUseAds = false;
        }
        else adsButton.SetActive(false);
        boosterName.text = name;
        _cost = BoosterManager.Ins.GetBoosterPrice(boosterType);
        _boosterType = boosterType;

      
        textCostBooster.text = _cost.ToString();
        if(DataManager.Ins.playerData.gold < _cost)
        {
            textCostBooster.color = Color.red;
        }

   

        imgFillAllColor.gameObject.SetActive(boosterType == BoosterType.FillAllColor);
        imgFillByColor.gameObject.SetActive(boosterType == BoosterType.FillByColor);
        imgFindByColor.gameObject.SetActive(boosterType == BoosterType.FindByColor);
    }



    public void BtnBuyBoosterGold()
    {
        if (UIManager.Ins.IsOpened<UIGameplay>())
        {
            if (DataManager.Ins.playerData.gold >= _cost)
            {
                DataManager.Ins.ChangeGold(-_cost);
                if (_boosterType == BoosterType.FillAllColor)
                {
                 
                    DataManager.Ins.ChangeBoosterFillAllColor(3);
                }
                if(_boosterType == BoosterType.FillByColor)
                {
                    DataManager.Ins.ChangeBoosterFillByColor(3);
                }
                if (_boosterType == BoosterType.FindByColor)
                {
                    DataManager.Ins.ChangeBoosterFindByColor(3);
                }

                if (DataManager.Ins.playerData.gold < _cost)
                {
                    textCostBooster.color = Color.red;
                }
                DataManager.Ins.SaveData();
                BtnExit();
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
