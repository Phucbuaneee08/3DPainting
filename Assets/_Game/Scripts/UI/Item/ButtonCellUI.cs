using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCellUI : Item
{
    public bool isActive = false;
    public int idSelect = 0;
    public override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        CurrentItemState = ItemState.TurnOff;
    }
    public void SetData(int _id)
    {
        idSelect = _id;
        this.gameObject.SetActive(isActive);
        initialPosition = rectTransform.anchoredPosition;
    }
    public void SetInitialPosition()
    {
        initialPosition = rectTransform.anchoredPosition;
    }
    public override void TurnOn()
    { 
        base.TurnOn();
        MoveUp();
    }
    public override void OnClick()
    {
        base.OnClick();
        if (CurrentItemState == ItemState.TurnOff)
        {
            ChangeItemState(ItemState.TurnOn);
        }
        else
        {
            ChangeItemState(ItemState.TurnOff);
        }
    }
    public override void TurnOff()
    {
        base.TurnOff();
        MoveDown();
    }
}
