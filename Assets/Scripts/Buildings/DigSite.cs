using System.Collections.Generic;
using UnityEngine;

public class DigSite : Building
{
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        _resourceMultiplier = 0.08f;
        _costMultiplier = 1.10f;
        _resourceTypeToModify = ResourceType.Stones;
        SetInitialValues();
    }
    private void Start()
    {
        CheckIfUnlocked();     
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

    private void Update()
    {
        UpdateResourceCosts();
    }
}
