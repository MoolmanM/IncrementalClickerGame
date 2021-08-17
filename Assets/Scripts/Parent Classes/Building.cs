using System;
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
    Furnace
}

public abstract class Building : SuperClass
{
    public static Dictionary<BuildingType, Building> Buildings = new Dictionary<BuildingType, Building>();
    public float buildingContributionAPS;

    public BuildingType Type;

    private string _selfCountString, _isUnlockedString;
    private string[] _costString;

    protected float _resourceMultiplier, _costMultiplier;
    protected ResourceType resourceTypeToModify;

    protected string _stringOriginalHeader;
    protected uint _selfCount;

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
                Resource.Resources[Buildings[Type].resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
                resourceCost[i].costAmount *= Mathf.Pow(_costMultiplier, _selfCount);
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[Buildings[Type].resourceCost[i].associatedType].amount, resourceCost[i].costAmount);
            }
            ModifyAmountPerSecond();
            buildingContributionAPS = 0;
            buildingContributionAPS = _selfCount * _resourceMultiplier;
        }

        _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    protected virtual void SetDescriptionText()
    {
        Buildings[Type]._txtDescription.text = string.Format("Increases {0} yield by: {1:0.00}", Resource.Resources[resourceTypeToModify].Type.ToString(), _resourceMultiplier);
    }
    protected virtual void ModifyAmountPerSecond()
    {
        Resource.Resources[resourceTypeToModify].amountPerSecond += _resourceMultiplier;
    }
    protected override void InitializeObjects()
    {
        base.InitializeObjects();


        _stringOriginalHeader = _objTxtHeader.GetComponent<TMP_Text>().text;

        _selfCountString = (Type.ToString() + "_selfCount");
        _isUnlockedString = (Type.ToString() + "isUnlocked");
        _costString = new string[resourceCost.Length];

        for (int i = 0; i < resourceCost.Length; i++)
        {
            _costString[i] = Type.ToString() + resourceCost[i].associatedType.ToString();
            PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
        }
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








