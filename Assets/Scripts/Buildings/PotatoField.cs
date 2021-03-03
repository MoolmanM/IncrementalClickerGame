using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoField : Building
{
    private Building _building;
    private BuildingUI _ui;
    private Collector collector;

    public override void HandleCollector(ref Collector collector)
    {
        SelfCount = 4;
        collector.multiplier = (float)0.10;
        collector.type = ResourceType.Food;

        base.HandleCollector(ref collector);
    }

    private void Awake()
    {
        _building = GetComponent<Building>();
        _ui = GetComponent<BuildingUI>();
    }
    public void Start()
    {
        ResourceCost[] resourceCosts = _ui.FillResourceCacheArray();
        BuildingManager.Register(_building, resourceCosts);

        for (int i = 0; i < resourceCosts.Length; i++)
        {
            _building.RegisterResourceCosts(resourceCosts[i].type, resourceCosts[i].costAmount, resourceCosts[i].currentAmount, resourceCosts[i].uiForBuilding);

            collector.resourceCostArray = new ResourceCost[resourceCosts.Length];

            collector.resourceCostArray[i].uiForBuilding = resourceCosts[i].uiForBuilding;
            collector.resourceCostArray[i].type = resourceCosts[i].type;
            collector.resourceCostArray[i].costAmount = resourceCosts[i].costAmount;
            collector.resourceCostArray[i].currentAmount = resourceCosts[i].currentAmount;

            collector.resourceCostDictionary = new Dictionary<ResourceType, ResourceCost>
            {
                { collector.resourceCostArray[i].type, collector.resourceCostArray[i] }
            };
            //Debug.Log(string.Format("Display for the start: {0}, {1}, {2}", collector.dicResourceCosts[collector.resourceCostArray[i].type].type, collector.dicResourceCosts[collector.resourceCostArray[i].type].costAmount, collector.dicResourceCosts[collector.resourceCostArray[i].type].currentAmount));
        }
    }

    private void Update()
    {
        HandleCollector(ref collector);
    }
}
