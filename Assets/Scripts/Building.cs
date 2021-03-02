using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Resource
{
    public ResourceType type;
    public float amount;
    public Resource(float amount) : this()
    {
        this.amount = amount;
    }
}

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
    public ResourceType type;
}

public abstract class Building : MonoBehaviour
{
    //private readonly Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();
    public Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();

    protected Resource GivenResource;
    protected uint SelfCount;
    protected float Cost;
    protected float CostMultiplier;
    public virtual void HandleCollector(ref Collector collector)
    {
        collector.buildingIncrementPerSecondAmount = SelfCount * collector.buildingMultiplier;
        //Debug.Log(collector.type + " " + collector.buildingIncrementPerSecondAmount);
    }

    public virtual void Build(ResourceType type)
    {
        if (!_resources.TryGetValue(type, out Resource storedResource) || storedResource.amount < Cost)
        {
            return;
        }
        SelfCount++;
        storedResource.amount -= Cost;
        Cost *= Mathf.Pow(CostMultiplier, SelfCount);
        _resources[type] = storedResource;
    }

    public void RegisterResource(ResourceType type, float amount)
    {
        _resources.Add(type, new Resource(amount));
    }
}








