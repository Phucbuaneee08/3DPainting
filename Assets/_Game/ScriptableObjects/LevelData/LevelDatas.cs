using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    public LevelData GetLevelWithID(int _id)
    {
        return level3D.Find(id => id.levelID == _id);
    }
    public LevelData GetLevelWithType(LevelType _levelType)
    {
        return level3D.Find(type =>type.levelType == _levelType);
    }
    public List<LevelData> GetLevelsWithType(LevelType _levelType)
    {
        return level3D.FindAll(level => level.levelType == _levelType);
    }

   
}
[System.Serializable]
public class LevelData
{
    public int levelID;
    public LevelType levelType;
    public Level level;
    public Sprite imageSource;
}

