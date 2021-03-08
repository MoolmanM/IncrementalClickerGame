using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigSite : Building
{
    private Building _building;
    private GameObject objectColor1;

    private void Awake()
    {
        SelfCount = 5;
        _building = GetComponent<Building>();
    }

    private void Start()
    {
        _buildings.Add(type, _building);
        GetIncrementAmount();
        //DisplayConsole();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<BuildingType, Building> kvp in _buildings)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    private void Update()
    {
        UpdateBuildingElements();
    }
}
