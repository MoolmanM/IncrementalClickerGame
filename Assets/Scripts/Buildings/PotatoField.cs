using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoField : Building
{
    private Building _building;
    private BuildingUI _ui;
    private Collector collector;
    private float amountPerSecond; 
    public override void HandleCollector(ref Collector collector)
    {
        SelfCount = 15;
        collector.buildingMultiplier = (float)0.16;
        base.HandleCollector(ref collector);
        //amountPerSecond = collector.amountPerSecond;
        Debug.Log("Amount per second from this building " + _building + " " + collector.amountPerSecond);
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
            //resources[i].amount += collector.amountPerSecond;
            //Debug.Log(resources[i].type + "  " + resources[i].amount);
        }

        
    }

    private void Update()
    {

        //_incrementResource.UpdateResourceAmount(resources[i].amount);
    }
}
