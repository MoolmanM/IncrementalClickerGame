﻿using System.Collections.Generic;
using UnityEngine;

public class Woodlot : Building
{
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        _resourceMultiplier = 0.12f;
        _costMultiplier = 1.07f;
        _resourceTypeToModify = ResourceType.Sticks;
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