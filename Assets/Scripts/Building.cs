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
public struct Resource
{
    public ResourceType type;
    public float amount;
}

public struct Collector
{
    public float multiplier;
    public float incrementPerSecond;
    public float resourceToIncrement;
    public ResourceType type;
    public Dictionary<ResourceType, ResourceCost> resourceCostDictionary;
    public ResourceCost[] resourceCostArray;
    public UiForBuilding uiForBuilding;
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
    public Dictionary<ResourceType, float> _resourceDictionary = new Dictionary<ResourceType, float>();

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
        collector.resourceCostDictionary = new Dictionary<ResourceType, ResourceCost>();

        var costArray = collector.resourceCostArray;
        var costDictionary = collector.resourceCostDictionary;
        var mainResources = _resourceDictionary;
        var incrementPerSecond = collector.incrementPerSecond;
        var type = collector.type;

        incrementPerSecond = SelfCount * collector.multiplier;

        if (!mainResources.ContainsKey(type))
        {
            mainResources.Add(type, collector.resourceToIncrement);
        }       

        if (mainResources.TryGetValue(type, out float value))
        {
            if ((_timer -= Time.deltaTime) <= 0)
            {
                _timer = _maxValue;

                value += incrementPerSecond;
                mainResources[type] = value;

                for (int i = 0; i < costArray.Length; i++)
                {
                    costDictionary = new Dictionary<ResourceType, ResourceCost>
                    {
                        { costArray[i].type, costArray[i] }
                    };

                    if (type == costDictionary[costArray[i].type].type)
                    {
                        costArray[i].currentAmount = value;
                        
                    }
                    costArray[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", costArray[i].currentAmount, costArray[i].costAmount);

                    
                    //This is for description text, this works nicely because I can have a default value here, but then in cases like the makeshift bed, I can edit the value inside the override method.
                    costArray[i].uiForBuilding.descriptionText.text = string.Format("Increases {0} yield by: {1}/sec", costArray[i].type.ToString(), incrementPerSecond);
                }
            }           
        }

        collector.incrementPerSecond = 0;
        collector.multiplier = 0;
        collector.resourceToIncrement = 0;
        mainResources = null;
        costDictionary = null;
        costArray = null;
        type = 0;
    }

    public void RegisterResourceCosts(ResourceType type, float costAmount, float currentAmount, UiForBuilding uiForBuilding)
    {
        _resourceCosts.Add(type, new ResourceCost(costAmount, currentAmount, uiForBuilding));
    } 
}
 








