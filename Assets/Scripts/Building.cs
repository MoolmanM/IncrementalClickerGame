using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Resource
{
    public ResourceType type;
    public float storedAmount;
    // I personally don't like this at all. Cost should probably be calculated elsewhere
    public float cost;
}

public struct ResourceType
{
    public string name;
}

public abstract class Building
{
    private readonly Dictionary<ResourceType, Resource> _resources;

    protected Resource GivenResource;
    protected uint SelfCount;
    protected float CostMultiplier;
    protected Building()
    {
        _resources = new Dictionary<ResourceType, Resource>();
    }

    public virtual void Build(ResourceType type)
    {
        if (!_resources.TryGetValue(type, out Resource storedResource) || storedResource.storedAmount < storedResource.cost)
        {
            return;
        }

        storedResource.storedAmount -= storedResource.cost;
        storedResource.cost *= Mathf.Pow(CostMultiplier, SelfCount);

        _resources[type] = storedResource;
    }

    public void RegisterResource(ResourceType type, float amount, float baseCost)
    {
        _resources.Add(type, new Resource(amount, baseCost));
    }
}

