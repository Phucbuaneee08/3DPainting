using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
public class ConverObject : MonoBehaviour
{
    public static LevelPrefabData lpd;
    public static Level level;

    public static void SetParameters(LevelPrefabData lpdParam, Level levelParam)
    {
        lpd = lpdParam;
        level = levelParam;
    }

    [MenuItem("Converter/GenerateNewLevel")]
    public static void Convert()
    {
        if (lpd == null || level == null)
        {
            Debug.LogError("lpd or level is not assigned.");
            return;
        }

        List<PrefabData> list = lpd.prefabDatas;
        PrefabData pd = list[0];
        foreach (PoisitionData data in pd.poisitionDatas)
        {
            CubeData cb = new CubeData(data.position, 2, 1);
            level.cubes.Add(cb);
        }
    }
}
#endif
