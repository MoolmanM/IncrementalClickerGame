using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct UiForResource
{
    public TMP_Text storageAmount;
    public TMP_Text amount;
    public TMP_Text amountPerSecond;
}

public enum ResourceType
{
    Food,
    Sticks,
    Stones
}

public class Resource : MonoBehaviour
{
    public static Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();

    [System.NonSerialized] public float amount;
    [System.NonSerialized] public float amountPerSecond;
    public float storageAmount;

    public ResourceType type;
    public UiForResource uiForResource;

    private float _timer = 0.1f, maxValue = 0.1f;

    public virtual void UpdateResources()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            //Actually, maybe have an update method inside the resource script linked to each resource, and not one inside each building.
            //Also tag all ui elements. 
            //If summer make the background sky blue with clouds and a shining sun that moves.

            _resources[type].amount += _resources[type].amountPerSecond;
            _resources[type].uiForResource.amountPerSecond.text = string.Format("{0}/sec", _resources[type].amountPerSecond);
            _resources[type].uiForResource.amount.text = string.Format("{0:0.00}", _resources[type].amount);
        }

    }
}
