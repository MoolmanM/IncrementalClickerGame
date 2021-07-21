using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stones : Resource
{
    private Resource _resource;

    void Awake()
    {
        _resource = GetComponent<Resource>();
        Resources.Add(Type, _resource);
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked);
        SetInitialValues();
        //MainResourcePanel.SetActive(false);
        
        //DisplayConsole();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<ResourceType, Resource> kvp in Resources)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    void Update()
    {
        UpdateResources();
    }

    //public void SaveResource()
    //{
    //    SaveSystem.SaveResource(_resource);
    //}

    //public void LoadResource()
    //{
    //    ResourceData data = SaveSystem.LoadResource();

    //    amount = data.amount;
    //}


}
