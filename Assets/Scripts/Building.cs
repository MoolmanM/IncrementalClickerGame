using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ResourceType
{
    Food,
    Sticks,
    Stones
}

public struct Collector
{
    public float buildingMultiplier;
    public float buildingIncrementPerSecondAmount;
    public Resource resourceToModify;
    public ResourceType type;
    public Dictionary<ResourceType, ResourceCost> dicResourceCosts;
    public ResourceCost[] resourceCostArray;
    public UiForBuilding ui;
}

[System.Serializable]
public struct ResourceCost
{
    public ResourceType type;
    public float currentAmount;
    public float costAmount;
    public UiForBuilding uiForBuilding;

    public ResourceCost(float currentAmount, float costAmount, UiForBuilding uiForBuilding) : this()
    {
        CurrentAmount = currentAmount;
        CostAmount = costAmount;
    }

    public float CurrentAmount { get; }
    public float CostAmount { get; }
}

public abstract class Building : MonoBehaviour
{
    [SerializeField]
    public Resource.ResourcesDictionary _resourceDictionary;

    private readonly Dictionary<ResourceType, ResourceCost> _resourceCosts;

    protected uint SelfCount;
    protected float Cost;
    protected float CostMultiplier;
    
    private float _timer = 0.1f, _maxValue = 5;

    protected Building()
    {

        _resourceCosts = new Dictionary<ResourceType, ResourceCost>();
    }

    public virtual void Build(ResourceType type)
    {
        if (!_resourceCosts.TryGetValue(type, out ResourceCost associatedResource) || associatedResource.currentAmount < associatedResource.costAmount)
        {
            return;
        }

        associatedResource.currentAmount -= associatedResource.costAmount;
        associatedResource.costAmount *= Mathf.Pow(CostMultiplier, SelfCount);

        _resourceCosts[type] = associatedResource;
    }

    public virtual void HandleCollector(ref Collector collector)
    {
        collector.dicResourceCosts = new Dictionary<ResourceType, ResourceCost>();

        var cCostArray = collector.resourceCostArray;
        var cResourceCosts = collector.dicResourceCosts;
        var resources = _resourceDictionary;
        var incrementPerSecond = collector.buildingIncrementPerSecondAmount;
        var type = collector.type;

        incrementPerSecond = SelfCount * collector.buildingMultiplier;

        //I just realized this contains key just check to see if there is a key? not a specific key, same with the try get value. but it should be like that though. I actually this this works?
        if (!resources.ContainsKey(type))
        {
            resources.Add(type, collector.resourceToModify);
        }
        Debug.Log(type + " " + resources[type]);
        if (resources.TryGetValue(type, out Resource resource))
        {
            if ((_timer -= Time.deltaTime) <= 0)
            {
                _timer = _maxValue;

                resource.amount += incrementPerSecond;
                resources[type] = resource;
                Debug.Log(type + " " + resources[type]);
                //Maybe do some code here that says if type == typeof(one of the resource classes?) 
                //I think I have to createa script maybe that says ResourceUI? 
                //idk need to give this some more though.
                for (int i = 0; i < cCostArray.Length; i++)
                {
                    cResourceCosts = new Dictionary<ResourceType, ResourceCost>
                    {
                        { cCostArray[i].type, cCostArray[i] }
                    };

                    if (type == cResourceCosts[cCostArray[i].type].type)
                    {
                        cCostArray[i].currentAmount = resource.amount;
                        //Debug.Log(string.Format("Display for the update: {0}, {1}, {2}", cResourceCosts[cCostArray[i].type].type, cResourceCosts[cCostArray[i].type].costAmount, cResourceCosts[cCostArray[i].type].currentAmount));
                    }
                    cCostArray[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", cCostArray[i].currentAmount, cCostArray[i].costAmount);

                    //This is for description text, this works nicely because I can have a default value here, but then in cases like the makeshift bed, I can edit the value inside the override method.
                    cCostArray[i].uiForBuilding.descriptionText.text = string.Format("Increases {0} yield by: {1}/sec", cCostArray[i].type.ToString(), incrementPerSecond);
                }
            }

        }

        collector.buildingIncrementPerSecondAmount = 0;
        collector.buildingMultiplier = 0;
        collector.resourceToModify = null;
        cResourceCosts = null;
        cCostArray = null;
        collector.type = 0;
    }

    public void RegisterResourceCosts(ResourceType type, float costAmount, float currentAmount, UiForBuilding uiForBuilding)
    {
        _resourceCosts.Add(type, new ResourceCost(costAmount, currentAmount, uiForBuilding));
    }
} 








