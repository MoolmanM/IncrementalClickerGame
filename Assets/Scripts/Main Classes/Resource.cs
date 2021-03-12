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
    [System.NonSerialized] public GameObject MainResourcePanel;
    public float StorageAmount;
    public ResourceType Type;

    protected UiForResource uiForResource;
    protected Transform amountTransform, amountPerSecondTransform, transformStorageAmount;

    protected float _timer = 0.1f;
    protected readonly float maxValue = 0.1f;

    public void SetInitialValues()
    {
        amountTransform = transform.Find("Header_Panel/Amount");
        uiForResource.amount = amountTransform.GetComponent<TMP_Text>();
        amountPerSecondTransform = transform.Find("Header_Panel/Amount");
        uiForResource.amountPerSecond = amountTransform.GetComponent<TMP_Text>();
        transformStorageAmount = transform.Find("Header_Panel/Amount");
        uiForResource.storageAmount = amountTransform.GetComponent<TMP_Text>();
        MainResourcePanel = this.gameObject;
    }

    public virtual void UpdateResources()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            if (Amount < (StorageAmount - AmountPerSecond))
            {
                _resources[Type].Amount += _resources[Type].AmountPerSecond;
                _resources[Type].uiForResource.amountPerSecond.text = string.Format("{0}/sec", _resources[Type].AmountPerSecond);
                _resources[Type].uiForResource.amount.text = string.Format("{0:0.00}", _resources[Type].Amount);
            }
            else
            {
                Amount = StorageAmount;
            }
            
        }
    }
}
