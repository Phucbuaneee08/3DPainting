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
    public List<CubeData> cubes = new List<CubeData>();
    public List<MaterialData> materials = new List<MaterialData>();
    public ZoomInfo zoomInfo = new ZoomInfo();
    public Sprite imageSource;
    public PoolType poolType;
    public LevelType levelType;
    public int costGold = 100;
    public int costDiamond = 10;
    public UnlockType unlockType = UnlockType.free;
}

[System.Serializable]
public class CubeData
{
    public Vector3 position;
    public int ID;
    public int realColorID;
    public int defaultColorID;
    public CubeData(int ID,Vector3 position, int realColorID, int defaultColorID)
    {
        this.ID = ID;
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

