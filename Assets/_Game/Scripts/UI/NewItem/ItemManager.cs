using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemManager : Singleton<ItemManager>
{
    public List<ColorItem> colorItems;
    public List<BoosterItem> boosterItems;

    public void TurnOffBoosterItemExceptEnum(BoosterType boosterType)
    {
        foreach (var item in boosterItems)
        {
            if (item.BoosterType != boosterType)
            {
                item.ChangeItemState(ItemState.TurnOff);
            }
        }
    }
    public void TurnOffBoosterItemByEnum(BoosterType boosterType) 
    {
        foreach (var item in boosterItems)
        {
            if (item.BoosterType == boosterType)
            {
                item.ChangeItemState(ItemState.TurnOff);
            }
        }
    }
    public void ResetBuyBoosterByAds()
    {
        foreach (var item in boosterItems)
        {
            item.SetBuyBoosterByAds(true);
        }
    }
    public BoosterItem GetBoosterItemByEnum(BoosterType boosterType)
    {
        foreach (var item in boosterItems)
        {
            if (item.BoosterType == boosterType)
            {
                return item;
            }
        }
        return null;
    }

    public void TurnOnColorItemByColorID(int colorID)
    {
        foreach (var item in colorItems)
        {
            if (item.GetColorID() == colorID)
            {
                item.ChangeItemState(ItemState.TurnOn);
            }
        }
    }
    public ColorItem GetColorItembyColorID(int colorID)
    {
        foreach (var item in colorItems)
        {
            if (item.GetColorID() == colorID)
            {
                return item;
            }
        }
        return null;
        
    }

    // tắt item ngoại trừ item có ID là id 
    public void TurnOffColorItemExceptID(int colorID)
    {
        
        Debug.Log(colorID);
        foreach(var item in colorItems)
        {
            if(item.GetColorID() != colorID)
            {
                item.ChangeItemState(ItemState.TurnOff);
            }
        }
    }
    public void TurnOffAllColorItem()
    {
        foreach (var item in colorItems)
        {
            
            item.ChangeItemState(ItemState.TurnOff);
        }
    }
    public void TurnOffBoosterItemExceptID(int boosterID)
    {
        foreach(var item in boosterItems)
        {
            if(item.ID!=boosterID)
            {
                item.ChangeItemState(ItemState.TurnOff);
            }
        }
    }
    public void TurnOffAllBoosterItem()
    {
        BoosterManager.Ins.SelectedBoosterType = BoosterType.None;
        foreach (var item in boosterItems)
        {          
            item.ChangeItemState(ItemState.TurnOff);
        }
    }
    public void TurnOffAllItems()
    {
        TurnOffAllColorItem();
        TurnOffBoosterItemExceptEnum(BoosterType.None);
    }
    

}
