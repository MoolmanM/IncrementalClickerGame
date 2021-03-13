using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeReference]
    public static Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();

    [System.NonSerialized] public float Amount;
    [System.NonSerialized] public float AmountPerSecond;
    [System.NonSerialized] public GameObject MainResourcePanel;
    public float StorageAmount;
    public ResourceType Type;

    protected UiForResource uiForResource;
    protected Transform amountTransform, amountPerSecondTransform, transformStorageAmount;
    private string _perSecondString, _amountString, _storageAmountString;

    protected float _timer = 0.1f;
    protected readonly float maxValue = 0.1f;

    public void SetInitialValues()
    {
        amountTransform = transform.Find("Header_Panel/Amount");
        uiForResource.amount = amountTransform.GetComponent<TMP_Text>();
        amountPerSecondTransform = transform.Find("Header_Panel/Amount_Per_Second");
        uiForResource.amountPerSecond = amountPerSecondTransform.GetComponent<TMP_Text>();
        transformStorageAmount = transform.Find("Header_Panel/Storage_Amount");
        uiForResource.storageAmount = transformStorageAmount.GetComponent<TMP_Text>();
        MainResourcePanel = this.gameObject;

        _perSecondString = Type.ToString() + "PS";
        _amountString = Type.ToString() + "A";
        _storageAmountString = Type.ToString() + "Storage";

        Amount = PlayerPrefs.GetFloat(_amountString, Amount);
        AmountPerSecond = PlayerPrefs.GetFloat(_perSecondString, AmountPerSecond);
        StorageAmount = PlayerPrefs.GetFloat(_storageAmountString, StorageAmount);
        uiForResource.storageAmount.text = string.Format("{0}", StorageAmount);

        if (AmountPerSecond < 0)
        {
            _resources[Type].uiForResource.amountPerSecond.text = string.Format("-{0}/sec", _resources[Type].AmountPerSecond);
        }
        else
        {
            _resources[Type].uiForResource.amountPerSecond.text = string.Format("+{0}/sec", _resources[Type].AmountPerSecond);
        }
        _resources[Type].uiForResource.amount.text = string.Format("{0:0.00}", _resources[Type].Amount);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(_amountString, Amount);
        PlayerPrefs.SetFloat(_perSecondString, AmountPerSecond);
        PlayerPrefs.SetFloat(_storageAmountString, StorageAmount);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            focus = true;
        }
    }

    public virtual void UpdateResources()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            if (Amount >= (StorageAmount - AmountPerSecond))
            {
                Amount = StorageAmount;
            }
            else
            {
                _resources[Type].Amount += _resources[Type].AmountPerSecond;
            }
            
            if (AmountPerSecond < 0)
            {
                _resources[Type].uiForResource.amountPerSecond.text = string.Format("-{0}/sec", _resources[Type].AmountPerSecond);
            }
            else
            {
                _resources[Type].uiForResource.amountPerSecond.text = string.Format("+{0}/sec", _resources[Type].AmountPerSecond);
            }
            _resources[Type].uiForResource.amount.text = string.Format("{0:0.00}", _resources[Type].Amount);

        }
    }
}
