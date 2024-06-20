using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
public class CubeGeneratorWindow : EditorWindow
{
    private int cubeSize = 5;
    private GameObject cubePrefab;

    [MenuItem("Tools/Generate Cube")]
    public static void ShowWindow()
    {
        GetWindow<CubeGeneratorWindow>("Generate Cube");
    }

    private void OnGUI()
    {
        GUILayout.Label("Cube Generator", EditorStyles.boldLabel);

        cubeSize = EditorGUILayout.IntField("Cube Size", cubeSize);
        cubePrefab = (GameObject)EditorGUILayout.ObjectField("Cube Prefab", cubePrefab, typeof(GameObject), false);

        if (GUILayout.Button("Generate"))
        {
            GenerateCube();
        }
    }

    private void GenerateCube()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Cube prefab is not assigned.");
            return;
        }

        GameObject parent = new GameObject("GeneratedCube");
        Vector3 startPos = parent.transform.position - new Vector3(cubeSize / 2f - 0.5f, cubeSize / 2f - 0.5f, cubeSize / 2f - 0.5f);

        for (int x = 0; x < cubeSize; x++)
        {
            for (int y = 0; y < cubeSize; y++)
            {
                for (int z = 0; z < cubeSize; z++)
                {
                    if (x == 0 || x == cubeSize - 1 || y == 0 || y == cubeSize - 1 || z == 0 || z == cubeSize - 1)
                    {
                        Vector3 pos = startPos + new Vector3(x, y, z);
                        GameObject cube = (GameObject)PrefabUtility.InstantiatePrefab(cubePrefab, parent.transform);
                        cube.transform.position = pos;
                    }
                }
            }
        }
    }
}
#endif