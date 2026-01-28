using UnityEngine;
using System.IO; // Required for File handling

public static class SaveSystem
{
    // Finds a safe folder on any computer/phone
    private static string path = Application.persistentDataPath + "/savefile.json";

    public static void SaveGame(GameData data)
    {
        // 1. Convert the C# object to a JSON string
        string json = JsonUtility.ToJson(data);

        // 2. Write that string to a file
        File.WriteAllText(path, json);
        Debug.Log("Game Saved to: " + path);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(path))
        {
            // 1. Read the text from the file
            string json = File.ReadAllText(path);

            // 2. Convert JSON back into C# variables
            GameData data = JsonUtility.FromJson<GameData>(json);
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found.");
            return null;
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}