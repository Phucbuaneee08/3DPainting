using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private int colorID;
    [SerializeField] private Text text;
    private UIGameplay uIGameplay;
    private void Start()
    {
      
    }

    public void FocusCubeByColorID()
    {
        LevelManager.Ins.FocusByColorID(colorID);
    }
    public void SetData(int colorID,Color color) 
    { 
        this.colorID = colorID;
        bg.color = color;
        text.text = colorID.ToString();
    }
    public int GetColorID()
    {
        return colorID;
    }

}
