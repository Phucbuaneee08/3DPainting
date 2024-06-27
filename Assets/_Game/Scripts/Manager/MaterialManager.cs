
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : Singleton<MaterialManager>
{
    [SerializeField] private List<MaterialData>  matData;
    [SerializeField] private List<Material> numberMats;
    [SerializeField] private List<Material> defaultMats;
    [SerializeField] private Color highLightColor;
    [SerializeField] private Color showTextColor;
    
    public void SetColor(Cube cube,int colorID)
    {
        //foreach(MaterialData md in matData)
        //{
        //    if(md.colorID == colorID) cube.colorRender.material = md.material;
        //}
        cube.colorRender.material = matData[colorID-1].material;
       
    }
    public void SetDefaultColor(Cube cube,int colorID)
    {
        cube.colorRender.material = defaultMats[colorID%(defaultMats.Count)];
       
    }
    public void SetMatData(List<MaterialData> md)
    {
        this.matData = md;
        ConvertFromRealColorToDefaultColor();
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
    public void ConvertFromRealColorToDefaultColor()
    {
        defaultMats.Clear();
        foreach(MaterialData mat in matData) 
        {
            Material newMaterial = new Material(Shader.Find("Standard"));
            Color color = mat.material.color;
            newMaterial.color = Ultilities.ConvertToGrayscale(color);
            defaultMats.Add(newMaterial);
        }
    }
    public void OnResetDefaultColor()
    {
        if (defaultMats.Count > 0) {
         
            foreach(Material mat in defaultMats)
            {
                Destroy(mat);
            }  
            defaultMats.Clear();  
        }
      
    }  

}
