using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataUtilities : MonoBehaviour
{
    private const string DEFAULT_PATH = "/GameData";
    private const string DEFAULT_FILE_NAME = "PlayerData.json";
    private static string systemPath = Application.dataPath + DEFAULT_PATH + "/" + DEFAULT_FILE_NAME;

    public static void SaveData<T>(T data, string filePath = null)
    {
        if (filePath == null)
        {
            filePath = systemPath;
        }

        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Đã lưu thành công Data tại: " + filePath);
    }

    public static T LoadData<T>(string filePath = null)
    {
        if (filePath == null)
        {
            filePath = systemPath;
        }

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            Debug.Log("Đã tải thành công Data tại: " + filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            Debug.LogError("Không tìm thấy đường dẫn: " + filePath);
            return default(T);
        }
    }

    public static void UpdateData<T>(T newData, string filePath = null)
    {
        if (filePath == null)
        {
            filePath = systemPath;
        }

        if (File.Exists(filePath))
        {
            string jsonData = JsonUtility.ToJson(newData);
            File.WriteAllText(filePath, jsonData);
        }
        else
        {
            Debug.LogError("Không tìm thấy đường dẫn: " + filePath);
        }
    }

    public static void DeleteData(string filePath = null)
    {
        if (filePath == null)
        {
            filePath = systemPath;
        }

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            Debug.LogError("Không tìm thấy đường dẫn: " + filePath);
        }
    }
}
