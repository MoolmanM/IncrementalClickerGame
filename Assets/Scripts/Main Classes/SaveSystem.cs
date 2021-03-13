using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public static class SaveSystem
{
    public static void SaveResource(Dictionary<ResourceType, Resource> resources)
    {
        string path = Application.persistentDataPath + "/resource.save";
        ResourceData data = new ResourceData(resources);
        Debug.Log(data._resourcesData);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string jsonTest = JsonConvert.SerializeObject(Resource._resources[ResourceType.Sticks], Formatting.Indented);
        Debug.Log(jsonTest);
        Debug.Log(json);

        using StreamWriter sw = new StreamWriter(path);
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(sw, json);
        sw.Close();
    }

    public static void LoadResource()
    {
        string path = Application.persistentDataPath + "/resource.save";

        if (File.Exists(path))
        {
            using StreamReader sr = new StreamReader(path);
            JsonSerializer serializer = new JsonSerializer();
            Dictionary<ResourceType, Resource> resourcesData = (Dictionary<ResourceType, Resource>)serializer.Deserialize(sr, typeof(Dictionary<ResourceType, Resource>));
            sr.Close();
        }
        else
        {
            Debug.LogError("Save File not found in " + path);
        }
    }
}
