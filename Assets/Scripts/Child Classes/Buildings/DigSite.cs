using System.Collections.Generic;
using UnityEngine;

public class DigSite : Building
{
    private Building _building;

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        _resourceMultiplier = 0.08f;
        _costMultiplier = 1.10f;
        resourceTypeToModify = ResourceType.Stones;
        SetInitialValues();
    }
    void Start()
    {    
        SetDescriptionText();
        //DisplayConsole();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<BuildingType, Building> kvp in Buildings)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    void Update()
    {
        UpdateResourceCosts();
    }
}
