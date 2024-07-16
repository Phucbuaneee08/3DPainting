using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    public List<Material> materials;
    public MeshRenderer meshRenderer;
    public void ChangeBackground()
    {
        meshRenderer.material = materials[Random.Range(1, materials.Count - 1)];
    }
    public void ChangeDefaultBackground()
    {
        meshRenderer.sharedMaterial = materials[0];
    }

}
