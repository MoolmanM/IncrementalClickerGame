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

    [System.NonSerialized] public float Amount;
    [System.NonSerialized] public float AmountPerSecond;
    public float StorageAmount;

    public ResourceType type;
    public UiForResource uiForResource;

    protected float _timer = 0.1f, maxValue = 0.1f;

    public virtual void UpdateResources()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            if (Amount < (StorageAmount - AmountPerSecond))
            {
                _resources[type].Amount += _resources[type].AmountPerSecond;
                _resources[type].uiForResource.amountPerSecond.text = string.Format("{0}/sec", _resources[type].AmountPerSecond);
                _resources[type].uiForResource.amount.text = string.Format("{0:0.00}", _resources[type].Amount);
            }
            else
            {
                Amount = StorageAmount;
            }
            
        }
    }
}
