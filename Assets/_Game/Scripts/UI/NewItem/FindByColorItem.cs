using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindByColorItem : BoosterItem
{
    public void Start()
    {
        DataManager.Ins.OnBoosterFindByColorQuantityChanged += SetQuantityText;
        DataManager.Ins.OnBoosterFindByColorQuantityChanged += CheckBoosterQuantity;
    }
    public override void OnClick()
    {

        if (DataManager.Ins.playerData.boosterFindByColorQuantity <= 0)
        {
            UIManager.Ins.OpenUI<UIBuyBooster>().SetData(BoosterType, boosterName);
            GameManager.Ins.ChangeState(GameState.Pause);
        }
        else {

            if (LevelManager.Ins.currentColor != 0)
            {
                base.OnClick();
                BoosterManager.Ins.FindNextCubeByColor(LevelManager.Ins.currentColor);
        }
            }
                
    }
    public override void CheckBoosterQuantity(int quantity)
    {
        if (DataManager.Ins.playerData.boosterFindByColorQuantity <= 0)
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


