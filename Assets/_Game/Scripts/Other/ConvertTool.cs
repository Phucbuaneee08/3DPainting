using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
public class ConvertTool : EditorWindow
{
    private GameObject model3D;
    private Level level;

    private float app;
    [MenuItem("Tools/ConvertObjectToLevel")]
    public static void ShowWindow()
    {
        GetWindow<ConvertTool>("Convert Object");
    }

    private void OnGUI()
    {
        GUILayout.Label("Convert Object Settings", EditorStyles.boldLabel);

        model3D = (GameObject)EditorGUILayout.ObjectField("3DModel", model3D, typeof(GameObject), true);
       
        level = (Level)EditorGUILayout.ObjectField("Level", level, typeof(Level), true);

    
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
                                CubeData cube = new CubeData(child.transform.position, i, 1);
                                level.cubes.Add(cube);
                            }
                        }
                    }
                    else
                    {
                        listColor.Add(mat.color);
                        uniqueColors.Add(mat);
                        CubeData cube = new CubeData(child.transform.position, listColor.Count-1, 1);
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
             

               
            }
            else
            {
                Debug.LogError("Please assign both LevelPrefabData and Level.");
            }
        }
    }
}

#endif