using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCellUI : Item
{
    public bool isActive = false;
    public int idSelect = 0;

    public void SetData(int _id)
    {
        idSelect = _id;
        this.gameObject.SetActive(isActive);
        initialPosition = rectTransform.anchoredPosition;
    }
}
