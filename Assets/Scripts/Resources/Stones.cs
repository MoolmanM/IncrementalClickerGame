using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stones : Resource
{
    private Resource _resource;

    private void Awake()
    {
        _resource = GetComponent<Resource>();
        _resources.Add(Type, _resource);
        SetInitialValues();
        MainResourcePanel.SetActive(false);
        
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
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            if (Amount > 0)
            {
                Building._buildings[BuildingType.MakeshiftBed].MainBuildingPanel.SetActive(true);
            }
        }
    }
}
