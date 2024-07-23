using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    public int id;
    public GameObject objStar;

    public void ActiveObj(bool isActive)
    {
        objStar.SetActive(isActive);
    }
}
