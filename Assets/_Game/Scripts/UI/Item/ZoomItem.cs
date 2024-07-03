using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomItem : MonoBehaviour
{
   
    public List<GameObject> imageButton;
    public void ChangeButtonState(CameraState state)
    {
        for (int i = 0; i < imageButton.Count; i++)
        {
            imageButton[i].SetActive(false);
        }
        imageButton[(int)state].SetActive(true);

    }
}
