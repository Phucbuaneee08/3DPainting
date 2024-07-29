using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillByColorItem : BoosterItem
{
    [SerializeField] private Image colorBrush;

    public void Start()
    {
        DataManager.Ins.OnBoosterFillByColorQuantityChanged += SetQuantityText;
        DataManager.Ins.OnBoosterFillByColorQuantityChanged += CheckBoosterQuantity;
        LevelManager.Ins.OnChangeColor += ChangeColorBrush;
    }
    public override void OnInit()
    {
       
    }
    public override void OnClick()
    {
        if (DataManager.Ins.playerData.boosterFillByColorQuantity <= 0)
        {
            
            UIManager.Ins.OpenUI<UIBuyBooster>().SetData(BoosterType, boosterName,_isCanBuyBoosterByAds);
            GameManager.Ins.ChangeState(GameState.Pause);

        }
        else
            base.OnClick();
    }
    public override void TurnOff()
    {
        base.TurnOff();
        BoosterManager.Ins.IsCanUseFillByNumberBooster = false;
        ChangeColorBrush(Color.white);
       
    }
    
    public override void TurnOn()
    {
        base.TurnOn();

        BoosterManager.Ins.IsCanUseZoomBooster = true;
        BoosterManager.Ins.ZoomBoosterByColor();

        //ItemManager.Ins.TurnOffAllColorItem();
        BoosterManager.Ins.IsCanUseFillByNumberBooster = true;
        BoosterManager.Ins.IsCanUseFillAllNumberBooster = false;
        BoosterManager.Ins.IsCanUseZoomBooster = false;

        if (LevelManager.Ins.currentColor != 0)
        {
            ChangeColorBrush(ItemManager.Ins.GetColorItembyColorID(LevelManager.Ins.currentColor).GetItemColor());
        }
        //LevelManager.Ins.ReleaseFocusCube();
    }
    public void ChangeColorBrush(Color color)
    {
        colorBrush.color = color;
    }
    public override void CheckBoosterQuantity(int quantity)
    {
        if (DataManager.Ins.playerData.boosterFillByColorQuantity <= 0)
        {
            boosterQuantity.gameObject.SetActive(false);
            addBooster.gameObject.SetActive(true);
        }
        else
        {
            boosterQuantity.gameObject.SetActive(true);
            addBooster.gameObject.SetActive(false);
        }
    }




}
