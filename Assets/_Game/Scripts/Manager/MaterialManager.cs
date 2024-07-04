
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MaterialManager : Singleton<MaterialManager>
{
    [SerializeField] private Material shaderMaterial;
    [SerializeField] private List<MaterialData>  matData;
    [SerializeField] private List<Texture2D> texture2Ds;

    [SerializeField] private List<Material> shaderMaterials;
    [SerializeField] private List<Material> currentShaderMaterials;

    [SerializeField] private List<Material> numberMats;
    [SerializeField] private List<Material> defaultMats;

    [SerializeField] private List<Material> focusNumberMats;
    [SerializeField] private Material unHighLightMaterial;
    [SerializeField] private Color showTextColor;
    [SerializeField] private Color hightLightColor;
    
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
        SetCurrentShaderMaterial(md.Count);
    }
    public void SetUnHightLightColor(Cube cube)
    {
        cube.colorRender.material = unHighLightMaterial;        
    }
    public void SetHighLightColor(Cube cube)
    {
        cube.colorRender.material = focusNumberMats[cube.GetColorID()-1];
    }
    public void SetShowTextColor(Cube cube) 
    {
        numberMats[cube.GetColorID() - 1].color = defaultMats[cube.GetColorID()-1].color;
        cube.colorRender.material = numberMats[cube.GetColorID() - 1];
    }

    #region Change color by shader
    public void SetCurrentShaderMaterial(int colorQuantity)
    {
        for(int i = 0; i < colorQuantity; i++)
        {
            StartCoroutine(_SetFloatMaterial(shaderMaterials[i], 1));
            shaderMaterials[i].SetColor("_DefaultColor", defaultMats[i].color);
            shaderMaterials[i].SetColor("_NearColor", defaultMats[i].color);
            currentShaderMaterials.Add(shaderMaterials[i]);
        }
    }
    public void ChangeColorStateByFOV(float FOV)
    {
        foreach(Material mat in currentShaderMaterials)
        {
           StartCoroutine(_SetFloatMaterial(mat, FOV));
        }
    }
    private IEnumerator _SetFloatMaterial(Material mat,float FOV)
    {
        yield return null;
        mat.SetFloat("_Float", FOV);
    }
    public void SetDefaultShaderColor(Cube cube, int colorID)
    {
        cube.colorRender.sharedMaterial = currentShaderMaterials[colorID % (currentShaderMaterials.Count)];

    }
    public void ResetFloatShaderColor()
    {
        foreach (Material mat in currentShaderMaterials)
        {
            StartCoroutine(_SetFloatMaterial(mat, 1));
        }
    }
    public void SetHightLigtShaderColor(int colorID) 
    {
        currentShaderMaterials[colorID - 1].SetColor("_NearColor", hightLightColor);
    }
    public void SetShowTextShaderColor(int colorID)
    {
        currentShaderMaterials[colorID - 1].SetColor("_NearColor", defaultMats[colorID - 1].color);
    }
   
    #endregion
    public void ConvertFromRealColorToDefaultColor()
    {
        defaultMats.Clear();
        foreach(MaterialData mat in matData) 
        {
            Material newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
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
