using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Food : Resource
{
    private Resource _resource;

    private void Awake()
    {
        _resource = GetComponent<Resource>();
        _resources.Add(Type, _resource);
        SetInitialValues();
        //DisplayConsole();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<ResourceType, Resource> kvp in _resources)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    private void Update()
    {
        UpdateResources();
    }
}
