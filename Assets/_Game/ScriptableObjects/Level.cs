using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Data/Level", order = 1)]
public class Level : ScriptableObject
{
    public int levelID;
    public List<CubeData> cubes;
    public List<MaterialData> materials;
    public ZoomInfo zoomInfo = new ZoomInfo(5,60,50);
    public Sprite imageSource;
    public PoolType poolType;
}

[System.Serializable]
public class CubeData
{
    public Vector3 position;
    public int realColorID;
    public int defaultColorID;
    public CubeData(Vector3 position, int realColorID, int defaultColorID)
    {
        this.position = position;
        this.realColorID = realColorID;
        this.defaultColorID = defaultColorID;
    }
}
[System.Serializable]
public class MaterialData
{
    public Material material;
    public int colorID;
    public MaterialData(Material material,int colorId)
    {
        this.material = material;
        this.colorID = colorId;
    }
}

