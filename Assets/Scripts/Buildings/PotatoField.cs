using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoField : Building
{
    private Building _building;
    private BuildingUI _ui;

    private void Awake()
    {
        _building = GetComponent<Building>();
        _ui = GetComponent<BuildingUI>();
    }
    public void Start()
    {
        Resource[] resources = _ui.FillResourcesInDictionary();

        BuildingManager.Register(_building, resources);

        for (int i = 0; i < resources.Length; i++)
        {
            _building.RegisterResource(resources[i].type, resources[i].storedAmount);
            Debug.Log(resources[i].type + " " + resources[i].storedAmount);
        }
    }
}
