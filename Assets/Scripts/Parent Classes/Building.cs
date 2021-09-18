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
    //StorageTent, // For food, and more delicate stuff.
    //StorageHouse
    MineShaft,
    WoodGenerator


}

[System.Serializable]
public struct ResourceTypesToModify
{
    public ResourceType resourceTypeToModify;
    public float resourceMulitplier;
}

public abstract class Building : SuperClass
{
    public static Dictionary<BuildingType, Building> Buildings = new Dictionary<BuildingType, Building>();
    public static bool isBuildingUnlockedEvent;

    public float buildingContributionAPS;
    public List<ResourceTypesToModify> resourcesToIncrement = new List<ResourceTypesToModify>();
    public List<ResourceTypesToModify> resourcesToDecrement = new List<ResourceTypesToModify>();


    public BuildingType Type;
    public float costMultiplier;

    private string _selfCountString, _isUnlockedString;
    private string[] _costString;

    protected string _stringOriginalHeader;
    protected uint _selfCount;

    private void DeprecatedUpdateResourceInfo()
    {
        foreach (var resource in Resource.Resources)
        {
            // Why am I checking types to unlock here
            // Shouldn't it be types to modify.
            if (resource.Key == typesToUnlock.resourceTypesToUnlock[0] && _selfCount == 1)
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
            else if (resource.Key == typesToUnlock.resourceTypesToUnlock[0] && _selfCount != 1)
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
    private void UpdateResourceInfo()
    {
        foreach (var resource in Resource.Resources)
        {
            // Why am I checking types to unlock here
            // Shouldn't it be types to modify.
            foreach (var resourceToIncrement in resourcesToIncrement)
            {
                buildingContributionAPS = 0;
                buildingContributionAPS = _selfCount * resourceToIncrement.resourceMulitplier;
                if (resource.Key == resourceToIncrement.resourceTypeToModify && _selfCount == 1)
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
                else if (resource.Key == resourceToIncrement.resourceTypeToModify && _selfCount != 1)
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
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[resourceCost[i].associatedType].amount, resourceCost[i].costAmount);
            }
            ModifyAmountPerSecond();
            //buildingContributionAPS = 0;
            //buildingContributionAPS = _selfCount * _resourceMultiplier;
            UpdateResourceInfo();
        }

        _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    protected virtual void SetDescriptionText()
    {
        // Need to do a for loop here and most likely instantiate new gameobjects, depending on how many there are just the way I do it in fthe resourceinfo, with resources and also how I do it with ResourcePanels.
        //_txtDescription.text = string.Format("Increases {0} yield by: {1:0.00}", Resource.Resources[resourceTypeToModify].Type.ToString(), _resourceMultiplier);
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








