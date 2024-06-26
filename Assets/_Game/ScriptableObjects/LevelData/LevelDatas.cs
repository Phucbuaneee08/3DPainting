using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.XR;
public enum LevelCategory 
{ 
    ThreeDimention = 0,
    TrueDimention = 1,
    Popular = 2,
    Special = 3,
}
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Data/LevelData", order = 1)]
public class LevelDatas : ScriptableObject
{
    public List<LevelData> level3D;
  
}
[System.Serializable]
public class LevelData
{
    public int levelID;
    public Level level;
    public Sprite imageSource;

}




