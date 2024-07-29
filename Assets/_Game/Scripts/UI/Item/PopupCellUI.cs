using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopupCellUI : MonoBehaviour
{
    public int idSelect = 0;

    public void SetData(bool _isAtive)
    {
        this.gameObject.SetActive(_isAtive);
        Debug.LogError("Load");
    }
}
