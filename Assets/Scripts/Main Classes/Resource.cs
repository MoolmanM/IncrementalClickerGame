using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct UiForResource
{
    public TMP_Text TxtStorageAmount;
    public TMP_Text TxtAmount;
    public TMP_Text TxtAmountPerSecond;
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

    [System.NonSerialized] public float Amount, AmountPerSecond, AmountGainedWhileAfk;    
    [System.NonSerialized] public int IsUnlocked;
    [System.NonSerialized] public UiForResource _UiForResource;
    [System.NonSerialized] public GameObject objMainPanel;

    public float StorageAmount;
    public ResourceType _Type;
    public TMP_Text TxtEarned;
    public GameObject ObjSpacerBelow;

    private string _perSecondString, _amountString, _storageAmountString, _isUnlockedString;

    protected Transform TformTxtAmount, TformTxtAmountPerSecond, TformTxtStorage;  
    protected float _timer = 0.1f;
    protected readonly float maxValue = 0.1f;

    public virtual void SetInitialValues()
    {
        TformTxtAmount = transform.Find("Header_Panel/Text_Amount");
        TformTxtAmountPerSecond = transform.Find("Header_Panel/Text_AmountPerSecond");
        TformTxtStorage = transform.Find("Header_Panel/Text_Storage");

        _UiForResource.TxtAmount = TformTxtAmount.GetComponent<TMP_Text>();
        _UiForResource.TxtAmountPerSecond = TformTxtAmountPerSecond.GetComponent<TMP_Text>();
        _UiForResource.TxtStorageAmount = TformTxtStorage.GetComponent<TMP_Text>();

        objMainPanel = gameObject;
        objMainPanel.SetActive(false);
        _perSecondString = _Type.ToString() + "PS";
        _amountString = _Type.ToString() + "A";
        _storageAmountString = _Type.ToString() + "Storage";
        _isUnlockedString = _Type.ToString() + "Unlocked";

        if (TimeManager.hasPlayedBefore)
        {
            Amount = PlayerPrefs.GetFloat(_amountString, Amount);
            AmountPerSecond = PlayerPrefs.GetFloat(_perSecondString, AmountPerSecond);
            StorageAmount = PlayerPrefs.GetFloat(_storageAmountString, StorageAmount);
        }

        if (IsUnlocked == 1)
        {
            objMainPanel.SetActive(true);
            ObjSpacerBelow.SetActive(true);
            TimeManager.GetAFKResource(_Type);
            _UiForResource.TxtStorageAmount.text = string.Format("{0}", StorageAmount);

            if (AmountPerSecond >= 0)
            {
                _resources[_Type]._UiForResource.TxtAmountPerSecond.text = string.Format("+{0}/sec", _resources[_Type].AmountPerSecond);                
            }
            else
            {
                _resources[_Type]._UiForResource.TxtAmountPerSecond.text = string.Format("-{0}/sec", _resources[_Type].AmountPerSecond);
            }
            _resources[_Type]._UiForResource.TxtAmount.text = string.Format("{0:0.00}", _resources[_Type].Amount);
        }
        else
        {
            objMainPanel.SetActive(false);
            ObjSpacerBelow.SetActive(false);
        }
        
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(_amountString, Amount);
        PlayerPrefs.SetFloat(_perSecondString, AmountPerSecond);
        PlayerPrefs.SetFloat(_storageAmountString, StorageAmount);
        PlayerPrefs.SetInt(_isUnlockedString, IsUnlocked);
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
                _resources[_Type].Amount += _resources[_Type].AmountPerSecond;
            }
            
            if (AmountPerSecond < 0)
            {
                _resources[_Type]._UiForResource.TxtAmountPerSecond.text = string.Format("-{0}/sec", _resources[_Type].AmountPerSecond);
            }
            else
            {
                _resources[_Type]._UiForResource.TxtAmountPerSecond.text = string.Format("+{0}/sec", _resources[_Type].AmountPerSecond);
            }
            _resources[_Type]._UiForResource.TxtAmount.text = string.Format("{0:0.00}", _resources[_Type].Amount);

        }
    }
}
