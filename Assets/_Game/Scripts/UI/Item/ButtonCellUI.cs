using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCellUI : MonoBehaviour
{
    public bool isActive = false;
    public void SetData()
    {
        this.gameObject.SetActive(isActive);
    }
}
