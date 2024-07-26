using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilllAllColorItem : BoosterItem
{
    public void Start()
    {
        DataManager.Ins.OnBoosterFillAllColorQuantityChanged += SetQuantityText;
        DataManager.Ins.OnBoosterFillAllColorQuantityChanged += CheckBoosterQuantity;
    }
    public override void OnClick()
    {
        if (DataManager.Ins.playerData.boosterFillAllColorQuantity <= 0)
        {
            UIManager.Ins.OpenUI<UIBuyBooster>().SetData(BoosterType, boosterName);
            GameManager.Ins.ChangeState(GameState.Pause);
        }
        else
            base.OnClick();
    }
    public override void TurnOff()
    {
        base.TurnOff();
        BoosterManager.Ins.IsCanUseZoomBooster = false;
        BoosterManager.Ins.IsCanUseFillAllNumberBooster = false;

    }

    public override void TurnOn()
    {
        base.TurnOn();
        ItemManager.Ins.TurnOffAllColorItem();

        BoosterManager.Ins.IsCanUseZoomBooster = true;
        BoosterManager.Ins.ZoomBoosterByColor();

        BoosterManager.Ins.IsCanUseFillAllNumberBooster = true;
        BoosterManager.Ins.IsCanUseFillByNumberBooster = false;


        if (LevelManager.Ins.currentColor != 0)
        {
            ItemManager.Ins.TurnOffAllColorItem();
        }

        LevelManager.Ins.ReleaseFocusCube();
    }
    public override void CheckBoosterQuantity(int quantity)
    {
        if (DataManager.Ins.playerData.boosterFillAllColorQuantity <= 0)
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
