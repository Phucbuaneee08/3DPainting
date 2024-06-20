using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public static class LoadPrefabLevelToScriptableObject 
{
    private const string PATH = "Assets/_Game/Level";

    [MenuItem("Auto/RePrefabInLevelToScriptableObject &s", false, -101)]
    public static void RePrefabInLevelToScriptableObjects()
    {
        var selections = Selection.gameObjects;
        for (int i = 0; i < selections.Length; i++)
        {
            PrefabToScriptableObject(selections[i]);
        }

    }

    private static void PrefabToScriptableObject(GameObject gameObject)
    {
        //list prefab root va danh sach vi tri
        List<PrefabData> prefabDatas = new List<PrefabData>();

        // EditorUtility.CollectDeepHierarchy does not include inactive children
        var deeperSelection = Selection.gameObjects.SelectMany(go => gameObject.GetComponentsInChildren<Transform>(true))
            .Select(t => t.gameObject);

        GameObject parent = null;

        HashSet<GameObject> hash = new HashSet<GameObject>();

        foreach (var deeper in deeperSelection)
        {
            //thang dau tien chinh la cha
            if (parent == null)
            {
                parent = deeper;
            }

            //list gameobject
            GameObject prefab = GetPrefabRoot(deeper);
            if (prefab != null && prefab.activeSelf && !hash.Contains(prefab))
            {
                hash.Add(prefab);
                //Debug.Log($"{hash.Count} - {prefab.name}");
            }
        }

        //add new scriptableobject
        foreach (var item in hash)
        {
            GameObject root = GetPrefabSource(item);

            PrefabData prefabData = GetPrefabData(ref prefabDatas, root);

            prefabData.poisitionDatas.Add(new PoisitionData() { position = item.transform.position, rotation = item.transform.rotation, scale = item.transform.lossyScale });
        }

        SaveToScriptableObject(PATH +"/"+ parent.name, prefabDatas);
    }

    //tim thang prefabdata trong list
    private static PrefabData GetPrefabData(ref List<PrefabData> prefabDatas, GameObject gameObject)
    {
        PrefabData prefabData = null;

        for (int i = 0; i < prefabDatas.Count; i++)
        {
            if (prefabDatas[i].prefab == gameObject)
            {
                prefabData = prefabDatas[i];
            }
        }
        if (prefabData == null)
        {
            prefabData = new PrefabData() { prefab = gameObject};
            prefabDatas.Add(prefabData);
        }

        return prefabData;
    }

    //tim thang prefab cha
    private static GameObject GetPrefabRoot(GameObject obj)
    {
        return PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
    }

    //tim thang prefab goc trong game
    private static GameObject GetPrefabSource(GameObject obj)
    {
        return PrefabUtility.GetCorrespondingObjectFromSource(obj);
    }

    public static void SaveToScriptableObject(string path, List<PrefabData> prefabDatas)
    {
        //kiem tra xem da co data level nay chua
        LevelPrefabData scriptable = AssetDatabase.LoadAssetAtPath<LevelPrefabData>(path + ".asset");

        //chua co thi tao moi
        if (scriptable == null)
        {
            scriptable = ScriptableObject.CreateInstance<LevelPrefabData>();
            // path has to start at "Assets"
            AssetDatabase.CreateAsset(scriptable, path + ".asset");
        }

        scriptable.SaveLevelData(prefabDatas);

        Undo.RegisterCompleteObjectUndo(scriptable, "Save level data");
        EditorUtility.SetDirty(scriptable);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"!!! SAVE {path} COMPLETE !!! ");
    }

}

#endif
