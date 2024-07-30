using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/UIModel")]
public class UITypeAssetData : ScriptableObject
{
    public List<UITypeModel> models = new List<UITypeModel>();
    public UITypeModel GetDataByType(LevelType type)
    {
        return models.Find(t => t.type == type);
    }
}
[System.Serializable]
public class UITypeModel
{
   public LevelType type;
   public Color color;
   public Sprite image;
}
