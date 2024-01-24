using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppDataSystem
{
    //This system will have generic methods to seralize and deserialize almost any kind of data we want ,except...
    //MonoBehaviours,GameObjects,prefabs,etc...
    //However,almost everything is ok: built in types ,your own POCO.

    // Needs 2 generic methods: 
    // Save: AppDataSystem.Save(obj, fileName);
    // 1. Check if a DIRECTORY exists and if not.. create it automatically.
    // 2. Check if a FILE exists, and if not.. create it automatically.
    // 3. Save the file with the serialized object

    // Load: AppDataSystem.Load<T>(fileName);
    // 1. Needs to Load and return the requested object, if the file exists...
    // 2. Needs to Load and return a default object, if the file doesn't exist (perhaps it should call save if not..) 

    //Finish Save and Load for AppDataSystem
    //Make 10 different levels
    //LevelGenerator should pick a random level from the 10
    //Bonus Points => Create a LoadAll method in the AppDataSystem
    //When the game is run,won,or lost ,user should see a UI message telling them what happened
    //And have a button to click to play another level.

    //Can't comment JSON File.

    //Save Method
    public static void Save<T>(T data, string fileName)
    {

        var directoryPath = $"{Application.dataPath}/StreamingAssets/" + typeof(T).Name;
        var filePath = directoryPath + "/" + fileName + ".json";

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (!File.Exists(filePath))
        {
            var fileStream = File.Create(filePath);
            fileStream.Close();
        }

        var serializedData = JsonConvert.SerializeObject(data);
        File.WriteAllText(filePath, serializedData);
    }
    //Load Method
    public static T Load<T>(string fileName)
    {
        var filePath = $"{Application.dataPath}/StreamingAssets/{typeof(T).Name}/{fileName}.json";

        if (!File.Exists(filePath))
        {
            T defaultObject = default;
            Save(defaultObject, fileName);
        }
        var serializedData = File.ReadAllText(filePath);
        var data = JsonConvert.DeserializeObject<T>(serializedData);
        return data;
    }

    public static List<T> LoadAll<T>()
    {
        var directoryPath = $"{Application.dataPath}/StreamingAssets/{typeof(T).Name}";

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            return new List<T>();
        }
        var filePaths = Directory.GetFiles(directoryPath, "*.json");

        var fileDataList = new List<T>();

        foreach (var filePath in filePaths)
        {
            var serializedData = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<T>(serializedData);

            if (!fileDataList.Contains(data))
            {
                fileDataList.Add(data);
            }
        }
        return fileDataList;
    }
}