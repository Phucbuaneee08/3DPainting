
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : Singleton<MaterialManager>
{
    [SerializeField] private List<MaterialData>  matData;
    [SerializeField] private List<Material> defaultMats;
    [SerializeField] private List<Material> numberMats;
    [SerializeField] private Color highLightColor;
    [SerializeField] private Color showTextColor;
    public void SetColor(Cube cube,int colorID)
    {
        foreach(MaterialData md in matData)
        {
            if(md.colorID == colorID) cube.colorRender.material = md.material;
        }
       
    }
    public void SetDefaultColor(Cube cube,int colorID)
    {
        cube.colorRender.material = defaultMats[colorID%(defaultMats.Count)];
    }
    public void SetMatData(List<MaterialData> md)
    {
        this.matData = md;
    }
    public void SetHighLightColor(Cube cube)
    {
        Material mat = numberMats[cube.GetColorID()-1];
        mat.color = highLightColor;
        cube.colorRender.material = mat;
    }
    public void SetShowTextColor(Cube cube) 
    {
        Material mat = numberMats[cube.GetColorID() - 1];
        mat.color = showTextColor;
        cube.colorRender.material = mat;
    }
   

}
