using System.Collections;
using System.Collections.Generic;
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
    public override void Setup()
    {
        base.Setup();
    }
    public override void Open()
    {
        base.Open();
    }
    public void SetData(int id)
    {
        this.id = id;
        textCostBooster.text = BoosterManager.Ins.costBooster.ToString();
        imgBG1.gameObject.SetActive(id == 1);
        imgBG2.gameObject.SetActive(id == 2);
        //imgBG3.gameObject.SetActive(id == 3);
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
                    DataManager.Ins.playerData.boosterQuantity += 3;
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
    }
}
