
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelPrefabData : ScriptableObject
{
    public float playTime = 60;
    public List<PrefabData> prefabDatas;

    public void SaveLevelData(List<PrefabData> prefabDatas)
    {
        this.prefabDatas = new List<PrefabData>(prefabDatas);
    }

    public void LoadLevelData(Transform parent)
    {
        parent.position = Vector3.zero;
        parent.rotation = Quaternion.identity;
        parent.localScale = Vector3.one;

        for (int i = 0; i < prefabDatas.Count; i++)
        {
            for (int j = 0; j < prefabDatas[i].poisitionDatas.Count; j++)
            {
                Transform obj = Instantiate(prefabDatas[i].prefab, prefabDatas[i].poisitionDatas[j].position, prefabDatas[i].poisitionDatas[j].rotation, parent).transform;
                obj.localScale = prefabDatas[i].poisitionDatas[j].scale;
            }
        }

        Debug.Log("Init");
    }

#if UNITY_EDITOR


    public void LoadLevel()
    {
        Transform parent = new GameObject(name).transform;
        parent.position = Vector3.zero;
        parent.rotation = Quaternion.identity;
        parent.localScale = Vector3.one;

        for (int i = 0; i < prefabDatas.Count; i++)
        {
            for (int j = 0; j < prefabDatas[i].poisitionDatas.Count; j++)
            {
                Transform obj = UnityEditor.PrefabUtility.InstantiatePrefab(prefabDatas[i].prefab).GetComponent<Transform>();
                obj.SetParent(parent);
                obj.localPosition = prefabDatas[i].poisitionDatas[j].position;
                obj.localRotation = prefabDatas[i].poisitionDatas[j].rotation;
                obj.localScale = prefabDatas[i].poisitionDatas[j].scale;
            }
        }

        Debug.Log("Init  " + name);
    }

 
    public void Clear()
    {
        GameObject gameObject = GameObject.Find(name);
        DestroyImmediate(gameObject);
    }

    public void FixAll()
    {
        for (int i = 0; i < prefabDatas.Count; i++)
        {
            for (int j = 0; j < prefabDatas[i].poisitionDatas.Count; j++)
            {
                prefabDatas[i].poisitionDatas[j].position += Vector3.up * 0.25f;
            }
        }

        UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Save level data");
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }



#endif

}

[System.Serializable]
public class PrefabData
{
    public GameObject prefab;
    public List<PoisitionData> poisitionDatas = new List<PoisitionData>();
}

[System.Serializable]
public class PoisitionData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}