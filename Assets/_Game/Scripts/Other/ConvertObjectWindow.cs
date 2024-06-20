using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ConvertObjectWindow : EditorWindow
{
    private LevelPrefabData lpd;
    private Level level;

    [MenuItem("Converter/Convert Object Window")]
    public static void ShowWindow()
    {
        GetWindow<ConvertObjectWindow>("Convert Object Window");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert Object Settings", EditorStyles.boldLabel);

        lpd = (LevelPrefabData)EditorGUILayout.ObjectField("Level Prefab Data", lpd, typeof(LevelPrefabData), true);
        level = (Level)EditorGUILayout.ObjectField("Level", level, typeof(Level), true);

        if (GUILayout.Button("Generate New Level"))
        {
            if (lpd != null && level != null)
            {
                ConverObject.SetParameters(lpd, level);
                ConverObject.Convert();
            }
            else
            {
                Debug.LogError("Please assign both LevelPrefabData and Level.");
            }
        }
    }
}
