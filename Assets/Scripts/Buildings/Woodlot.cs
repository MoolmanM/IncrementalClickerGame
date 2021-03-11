using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodlot : Building
{
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
    }

    private void Start()
    {
        _buildings.Add(Type, _building);
        SetInitialValues();
        _building.MainBuildingPanel.SetActive(false);
        
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