using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceData
{
    public Dictionary<ResourceType, Resource> _resourcesData;

    public ResourceData(Dictionary<ResourceType, Resource> resources)
    {
        resources = Resource._resources;
    }

    //private void OnApplicationQuit()
    //{
    //    SaveSystem.SaveResource(_resources[Type]);
    //}

    //private void Start()
    //{
    //    ResourceData data = SaveSystem.LoadResource();

    //    _resources[Type].Amount = data.amount;
    //}
}