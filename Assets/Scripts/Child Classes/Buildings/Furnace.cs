using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : Building
{
    private Building _building;

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        //ResourceMultiplier = 0.10f;
        //_costMultiplier = 1.15f;
        //resourceTypeToModify = ResourceType.Food;
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
