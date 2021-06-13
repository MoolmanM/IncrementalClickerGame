    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sticks : Resource
{
    private Resource _resource;

    private void Awake()
    {
        _resource = GetComponent<Resource>();
        Resources.Add(Type, _resource);
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked);
        isUnlocked = 1;
        SetInitialValues();
        amount = 500;
        // DisplayConsole();

    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<ResourceType, Resource> kvp in Resources)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    private void Update()
    {
        UpdateResources();
    }
}
