using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Resource
{
    public ResourceType type;
    public float storedAmount;
    private float amount;

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

public abstract class Building : MonoBehaviour
{
    private readonly Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();

    protected Resource GivenResource;
    protected uint SelfCount;
    protected float cost;
    protected float CostMultiplier;

    public virtual void Build(ResourceType type)
    {
        if (!_resources.TryGetValue(type, out Resource storedResource) || storedResource.storedAmount < cost)
        {
            return;
        }

        storedResource.storedAmount -= cost;
        cost *= Mathf.Pow(CostMultiplier, SelfCount);

        _resources[type] = storedResource;
    }

    public void RegisterResource(ResourceType type, float amount)
    {
        _resources.Add(type, new Resource(amount));
    }
}







