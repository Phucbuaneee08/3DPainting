/*using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class LevelEditorTool : EditorWindow
{
    private Level level;
    private string cubeIDsInput = string.Empty;

    [MenuItem("Tools/Level Editor Tool")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorTool>("Level Editor Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor Tool", EditorStyles.boldLabel);

        level = (Level)EditorGUILayout.ObjectField("Level ScriptableObject", level, typeof(Level), false);

        GUILayout.Label("Cube IDs to Delete (comma separated)");
        cubeIDsInput = EditorGUILayout.TextField(cubeIDsInput);

        if (GUILayout.Button("Delete Cubes with Specified IDs"))
        {
            DeleteCubesWithSpecifiedIDs();
        }
    }

    private void DeleteCubesWithSpecifiedIDs()
    {
        if (level != null && !string.IsNullOrEmpty(cubeIDsInput))
        {
            // Tách chuỗi ID thành một danh sách các số nguyên
            List<int> idsToDelete = cubeIDsInput.Split(',')
                .Select(id => int.TryParse(id.Trim(), out int parsedID) ? parsedID : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            if (idsToDelete.Count > 0)
            {
                // Lọc danh sách cubes và xóa cubes có ID tương ứng
                level.cubes.RemoveAll(cube => idsToDelete.Contains(cube.ID));

                // Lưu lại ScriptableObject sau khi đã xóa các cubes
                EditorUtility.SetDirty(level);
                AssetDatabase.SaveAssets();

                Debug.Log("Deleted cubes with IDs " + cubeIDsInput + " from level " + level.name);

                // Reset input field after deletion
                cubeIDsInput = string.Empty;
            }
        }
    }
}
*/