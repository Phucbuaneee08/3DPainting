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
}
