using System.Collections.Generic;
using TMPro;
using UnityEngine;


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

    private void UpdateResourceInfo()
    {
        foreach (var resource in Resource.Resources)
        {
            if (resource.Key == typesToModify.resourceTypesToModify[0] && _selfCount == 1)
            {
                resource.Value.resourceInfoList = new List<ResourceInfo>()
                {
                    new ResourceInfo(){ name = Type.ToString(), amountPerSecond=buildingContributionAPS }
                };

                for (int i = 0; i < resource.Value.resourceInfoList.Count; i++)
                {
                    ResourceInfo resourceInfo = resource.Value.resourceInfoList[i];

                    GameObject newObj = Instantiate(resource.Value.prefabResourceInfoPanel, resource.Value.tformResourceTooltip);

                    Transform _tformNewObj = newObj.transform;
                    Transform _tformInfoName = _tformNewObj.Find("Text_Name");
                    Transform _tformInfoAmountPerSecond = _tformNewObj.Find("Text_AmountPerSecond");

                    resourceInfo.uiForResourceInfo.textInfoName = _tformInfoName.GetComponent<TMP_Text>();
                    resourceInfo.uiForResourceInfo.textInfoAmountPerSecond = _tformInfoAmountPerSecond.GetComponent<TMP_Text>();

                    resourceInfo.uiForResourceInfo.textInfoName.text = Type.ToString();
                    resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", buildingContributionAPS);

                    resource.Value.resourceInfoList[i] = resourceInfo;
                }
            }
            else if (resource.Key == typesToModify.resourceTypesToModify[0] && _selfCount != 1)
            {
                for (int i = 0; i < resource.Value.resourceInfoList.Count; i++)
                {
                    ResourceInfo resourceInfo = resource.Value.resourceInfoList[i];
                    resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", buildingContributionAPS);
                    resource.Value.resourceInfoList[i] = resourceInfo;
                }
            }
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
                resourceCost[i].costAmount *= Mathf.Pow(_costMultiplier, _selfCount);
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[resourceCost[i].associatedType].amount, resourceCost[i].costAmount);
            }
            ModifyAmountPerSecond();
            buildingContributionAPS = 0;
            buildingContributionAPS = _selfCount * _resourceMultiplier;
            UpdateResourceInfo();
        }

        _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    protected virtual void SetDescriptionText()
    {
        _txtDescription.text = string.Format("Increases {0} yield by: {1:0.00}", Resource.Resources[resourceTypeToModify].Type.ToString(), _resourceMultiplier);
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








