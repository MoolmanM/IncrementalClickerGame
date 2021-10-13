using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum BuildingType
{
    PotatoField,
    Woodlot,
    DigSite,
    MakeshiftBed,
    Smelter,
    StoragePile, // For Stone, Lumber, maybe other materials
    //StorageTent, For food, and more delicate stuff.
    //StorageHouse
    //Storage Facility
    //Transformer maybe transformer should be something that you craft or research, for a once of boost to energy
    MineShaft,
    WoodGenerator


}

[System.Serializable]
public struct ResourceTypesToModify
{
    public ResourceType resourceTypeToModify;
    public float resourceMulitplier;
    public float contributionAmount;
}

public abstract class Building : SuperClass
{
    public static Dictionary<BuildingType, Building> Buildings = new Dictionary<BuildingType, Building>();
    public static bool isBuildingUnlockedEvent;

    public List<ResourceTypesToModify> resourcesToIncrement = new List<ResourceTypesToModify>();
    public List<ResourceTypesToModify> resourcesToDecrement = new List<ResourceTypesToModify>();


    public BuildingType Type;
    public float costMultiplier;

    protected uint _selfCount;
    protected string _stringOriginalHeader;
    private string _selfCountString, _isUnlockedString;
    private string[] _costString;

    //private TMP_Text 

    public void ResetBuilding()
    {
        isUnlocked = false;
        objMainPanel.SetActive(false);
        objSpacerBelow.SetActive(false);
        unlockAmount = 0;
        _selfCount = 0;
        hasSeen = true;
        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].costAmount = resourceCost[i].initialCostAmount;
            resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[resourceCost[i].associatedType].amount, resourceCost[i].costAmount);
        }
        _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    public void SetSelfCount(uint selfCountAmount)
    {
        _selfCount += selfCountAmount;
    }
    public void SetInitialAmountPerSecond()
    {
        // So this is going to loop through every building inside the prestige 
        // Building list for buildings to calculate
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            float amountToIncreaseBy = resourcesToIncrement[i].resourceMulitplier * _selfCount;
            Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += amountToIncreaseBy;
        }

        _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);

        // This seems to do everything that I want.
    }
    protected void UpdateResourceInfo()
    {
        foreach (var resourceToIncrement in resourcesToIncrement)
        {
            Debug.Log("Type: " + Type);
            Resource.Resources[resourceToIncrement.resourceTypeToModify].UpdateResourceInfo(Type, _selfCount, resourceToIncrement.resourceMulitplier, resourceToIncrement.resourceTypeToModify);
        }
    }
    protected void SetInitialValues()
    {
        InitializeObjects();

        if (TimeManager.hasPlayedBefore)
        {
            isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;
            _selfCount = (uint)PlayerPrefs.GetInt(_selfCountString, (int)_selfCount);

            for (int i = 0; i < resourceCost.Length; i++)
            {
                resourceCost[i].costAmount = PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
            }
        }
        _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    public virtual void OnBuild()
    {
        bool canPurchase = true;

        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resourceCost[i].currentAmount < resourceCost[i].costAmount)
            {
                canPurchase = false;
                break;
            }
        }

        if (canPurchase)
        {
            _selfCount++;
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
                resourceCost[i].costAmount *= Mathf.Pow(costMultiplier, _selfCount);
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[resourceCost[i].associatedType].amount), NumberToLetter.FormatNumber(resourceCost[i].costAmount));
            }
            ModifyAmountPerSecond();
            UpdateResourceInfo();
        }

        _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    protected virtual void ModifyDescriptionText()
    {
        string oldString;
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            if (i > 0)
            {
                oldString = _txtDescription.text;
                _txtDescription.text = string.Format("{0} \nIncrease <color=#F3FF0A>{1}</color> amount per second by <color=#FF0AF3>{2}</color>", oldString, resourcesToIncrement[i].resourceTypeToModify.ToString(), resourcesToIncrement[i].resourceMulitplier);
            }
            else
            {
                _txtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> amount per second by <color=#FF0AF3>{1}</color>.", resourcesToIncrement[i].resourceTypeToModify.ToString(), resourcesToIncrement[i].resourceMulitplier);
            }
        }
        for (int i = 0; i < resourcesToDecrement.Count; i++)
        {
            if (i > 0)
            {
                oldString = _txtDescription.text;
                _txtDescription.text = string.Format("{0} \nDecrease <color=#F3FF0A>{1}</color> amount per second by <color=#FF0AF3>{2}</color>", oldString, resourcesToDecrement[i].resourceTypeToModify.ToString(), resourcesToDecrement[i].resourceMulitplier);
            }
            else
            {
                _txtDescription.text = string.Format("Decrease <color=#F3FF0A>{0}</color> amount per second by <color=#FF0AF3>{1}</color>.", resourcesToDecrement[i].resourceTypeToModify.ToString(), resourcesToDecrement[i].resourceMulitplier);
            }
        }
    }
    protected virtual void ModifyAmountPerSecond()
    {
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].resourceMulitplier;
        }
    }
    protected override void InitializeObjects()
    {
        base.InitializeObjects();

        _objBtnMain.GetComponent<Button>().onClick.AddListener(OnBuild);

        _stringOriginalHeader = _objTxtHeader.GetComponent<TMP_Text>().text;

        _selfCountString = (Type.ToString() + "_selfCount");
        _isUnlockedString = (Type.ToString() + "isUnlocked");
        _costString = new string[resourceCost.Length];

        for (int i = 0; i < resourceCost.Length; i++)
        {
            _costString[i] = Type.ToString() + resourceCost[i].associatedType.ToString();
            PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
        }

        ModifyDescriptionText();
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(_selfCountString, (int)_selfCount);

        for (int i = 0; i < resourceCost.Length; i++)
        {
            PlayerPrefs.SetFloat(_costString[i], resourceCost[i].costAmount);
        }
    }
}








