// Thanks Eric Daily

using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad
{
    public static Manager savedGameManager;
    public static string path = "/savedGameManager.week02";

    public static void Save(Manager a_Manager)
    {
        if (a_Manager != null)
        {
            savedGameManager = a_Manager;
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + path);

        binaryFormatter.Serialize(fileStream, savedGameManager);
        fileStream.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + path, FileMode.Open);

            savedGameManager = (Manager)binaryFormatter.Deserialize(fileStream);

            fileStream.Close();
        }
    }
}