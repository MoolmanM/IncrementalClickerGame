using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodlot : Building
{
    private Building _building;
    private BuildingUI _ui;
    private Collector collector;
    private float amountPerSecond, resourceAmount, incrementTest;
    private float _timer = 1f;
    private float maxValue = 0.1f;
    public override void HandleCollector(ref Collector collector)
    {
        collector.buildingMultiplier = (float)0.10;
        SelfCount = 15;
        base.HandleCollector(ref collector);
        amountPerSecond = collector.amountPerSecond;
        Debug.Log("Amount per second from this building " + _building + " " + amountPerSecond);
    }

    private void Awake()
    {
        _building = GetComponent<Building>();
        _ui = GetComponent<BuildingUI>();
    }
    public void Start()
    {
        HandleCollector(ref collector);
        Resource[] resources = _ui.FillResourcesInDictionary();

        BuildingManager.Register(_building, resources);
        
        for (int i = 0; i < resources.Length; i++)
        {
            _building.RegisterResource(resources[i].type, resources[i].amount);
            resourceAmount = resources[i].amount;
        }
        
    }
    private void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;
            resourceAmount += amountPerSecond;
            Debug.Log(resourceAmount);
        }
    }


}