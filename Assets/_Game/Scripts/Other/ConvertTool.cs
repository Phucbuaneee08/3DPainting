using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

#if UNITY_EDITOR
public class ConvertTool : EditorWindow
{
    private GameObject model3D;
    //private Level level;

    private float app;
    [MenuItem("Tools/ConvertObjectToLevel")]
    public static void ShowWindow()
    {
        GetWindow<ConvertTool>("Convert Object");
    }

    private void OnGUI()
    {
        Level level = new Level();
        GUILayout.Label("Convert Object Settings", EditorStyles.boldLabel);

        model3D = (GameObject)EditorGUILayout.ObjectField("3DModel", model3D, typeof(GameObject), true);
       
       

    
        if (GUILayout.Button("Generate Level"))
        {
          
            if (model3D != null && level != null)
            {
                //ConverObject.SetParameters(lpd, level);
                //ConverObject.Convert();
                List<Material> uniqueColors = new List<Material>();
                List<Color> listColor = new List<Color>();
      
                foreach (Transform child in model3D.GetComponentsInChildren<Transform>())
                {
                   
                    //CubeData cubeData = new CubeData(child.transform.position, 2, 2);
                    Material mat = child.GetComponent<MeshRenderer>().sharedMaterial;
                  
                 
                    if (Ultilities.CheckColorsInList(listColor,mat.color))
                    {
                        
                        for (int i = 0;i< listColor.Count; i++)
                        {
                            if(Ultilities.AreColorsApproximatelyEqual(mat.color, listColor[i], app))
                            {
                                CubeData cube = new CubeData(level.cubes.Count,child.transform.position, i, 1);
                                level.cubes.Add(cube);
                            }
                        }
                    }
                    else
                    {
                        listColor.Add(mat.color);
                        uniqueColors.Add(mat);
                        Debug.Log(level);
                        CubeData cube = new CubeData(level.cubes.Count,child.transform.position, listColor.Count-1, 1);
                        level.cubes.Add(cube);
                    }
                   
                    
                }
          
                for(int i=0;i < uniqueColors.Count; i++)
                {
                 
                    MaterialData matd = new MaterialData(uniqueColors[i], i);
                    level.materials.Add(matd);
                }
                level.materials.RemoveAt(0);
                level.cubes.RemoveAt(0);




                string folderPath = "Assets/_Game/ScriptableObjects/Level";
                if (!AssetDatabase.IsValidFolder(folderPath))
                {
                    AssetDatabase.CreateFolder("Assets", "MyScriptableObjects");
                    AssetDatabase.Refresh();
                }

                // Tạo ScriptableObject mới
       

                // Lưu ScriptableObject vào thư mục
                string assetPath = Path.Combine(folderPath, "MyData.asset");
                AssetDatabase.CreateAsset(level, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("ScriptableObject saved at: " + assetPath);

            }
            else
            {
                Debug.LogError("Please assign both LevelPrefabData and Level.");
            }
        }
    }
}

#endif