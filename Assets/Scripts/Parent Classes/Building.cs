using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BuildingType
{
    PotatoField,
    LumberMill,
    DigSite,
    Hut,
    Smelter,
    StoragePile, // For Stone, Lumber, maybe other materials
    //StorageTent, For food, and more delicate stuff.
    // Storage Tent maybe after storage pile, because it needs to be made of pelts i.e leather.
    //StorageHouse
    //Storage Facility
    //Transformer maybe transformer should be something that you craft or research, for a once of boost to energy
    MineShaft,
    WoodGenerator,
    None


}

[System.Serializable]
public struct BuildingResourcesToModify
{
    public ResourceType resourceTypeToModify;
    public float currentResourceMultiplier, baseResourceMultiplier;
    public float contributionAmount;
}

public abstract class Building : SuperClass
{
    public static Dictionary<BuildingType, Building> Buildings = new Dictionary<BuildingType, Building>();
    public static bool isBuildingUnlockedEvent;

    public List<BuildingResourcesToModify> resourcesToIncrement = new List<BuildingResourcesToModify>();
    public List<BuildingResourcesToModify> resourcesToDecrement = new List<BuildingResourcesToModify>();


    public BuildingType Type;
    public float costMultiplier;

    public uint _selfCount;
    protected string _selfCountString, _isUnlockedString, _strInitialSelfCount;
    protected string[] _costString;

    public uint initialSelfCount;

    private string strDescription;

    // Reset variables

    public uint permCountAddition;
    public float permAllMultiplierAddition, permMultiplierAddition, permCostSubtraction;

    public uint prestigeCountAddition;
    public float prestigeAllMultiplierAddition, prestigeMultiplierAddition, prestigeCostSubtraction;

    private string _strPermCountAddition, _strPermAllMultiplierAddition, _strPermCostSubtraction, _strPermMultiplierAddition;
    private string _strPrestigeCountAddition, _strPrestigeAllMultiplierAddition, _strPrestigeCostSubtraction, _strPrestigeMultiplierAddition;

    public void ModifySelfCount()
    {
        _selfCount = 0;
        _selfCount += (permCountAddition + prestigeCountAddition);
        prestigeCountAddition = 0;

        // Maybe need a permCountAddition and a prestigeCountAddition, prestigeCount needs to be set to zero after prestiging. permanent one needs to be saved throughout.
        // I think at least, needs some more brainstorming.
        // Both needs to be saved on exit and assigned values on start. 

        Debug.Log(string.Format("Changed building {0}'s self count from {1} to {2}", actualName, "Hopefully 0", _selfCount));
    }
    public void InitialUnlock()
    {
        for (int i = 0; i < resourceCost.Length; i++)
        {
            //Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            resourceCost[i].costAmount *= Mathf.Pow(costMultiplier, _selfCount);
            resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[resourceCost[i].associatedType].amount), NumberToLetter.FormatNumber(resourceCost[i].costAmount));
        }

        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            if (CalculateAdBoost.isAdBoostActivated)
            {
                Debug.Log("Multiplier: " + resourcesToIncrement[i].currentResourceMultiplier + " Self Count: " + _selfCount);
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier * _selfCount * CalculateAdBoost.adBoostMultiplier;
            }
            else
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier * _selfCount;
            }
            StaticMethods.ModifyAPSText(Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        }

        UpdateResourceInfo();
    }
    public void ModifyCost()
    {
        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].costAmount = resourceCost[i].baseCostAmount;
            float subtractionAmount = resourceCost[i].baseCostAmount * (prestigeCostSubtraction + permCostSubtraction);
            prestigeCostSubtraction = 0;
            resourceCost[i].costAmount -= subtractionAmount;
            Debug.Log(string.Format("Changed building {0}'s cost from {1} to {2}", actualName, resourceCost[i].baseCostAmount, resourceCost[i].costAmount));
            resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[resourceCost[i].associatedType].amount, resourceCost[i].costAmount);
        }
    }
    public void ModifyMultiplier()
    {
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            BuildingResourcesToModify buildingResourcesToModify = resourcesToIncrement[i];
            buildingResourcesToModify.currentResourceMultiplier = buildingResourcesToModify.baseResourceMultiplier;
            float additionAmount = buildingResourcesToModify.baseResourceMultiplier * ((prestigeAllMultiplierAddition + permAllMultiplierAddition) + (permMultiplierAddition + prestigeMultiplierAddition));
            prestigeAllMultiplierAddition = 0;
            prestigeMultiplierAddition = 0;
            buildingResourcesToModify.currentResourceMultiplier += additionAmount;
            Debug.Log(string.Format("Changed building {0}'s resource multi from {1} to {2}", actualName, buildingResourcesToModify.baseResourceMultiplier, buildingResourcesToModify.currentResourceMultiplier));
            resourcesToIncrement[i] = buildingResourcesToModify;
        }
        //ModifyDescriptionText();
    }
    public void UpdateDescription()
    {
        UpdateResourceInfo();
        ModifyDescriptionText();
    }
    public virtual void ResetBuilding()
    {
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            if (CalculateAdBoost.isAdBoostActivated)
            {
                // Why not just set this to zero?
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= (resourcesToIncrement[i].currentResourceMultiplier * CalculateAdBoost.adBoostMultiplier) * _selfCount;
            }
            else
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= resourcesToIncrement[i].currentResourceMultiplier * _selfCount;
            }
            StaticMethods.ModifyAPSText(Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        }

        isUnlocked = false;
        canvas.enabled = false;
        objMainPanel.SetActive(false);
        graphicRaycaster.enabled = false;
        unlockAmount = 0;
        _selfCount = 0;
        hasSeen = true;
        isUnlockedByResource = false;
        ModifySelfCount();
        ModifyMultiplier();
        ModifyCost();
        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
        ModifyDescriptionText();
    }
    public void SetInitialAmountPerSecond()
    {
        // So this is going to loop through every building inside the prestige 
        // Building list for buildings to calculate
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            float amountToIncreaseBy = resourcesToIncrement[i].currentResourceMultiplier * _selfCount;
            Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += amountToIncreaseBy;
        }

        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);

        // This seems to do everything that I want.
    }
    protected virtual void UpdateResourceInfo()
    {
        foreach (var resourceToIncrement in resourcesToIncrement)
        {
            float buildingAmountPerSecond = _selfCount * resourceToIncrement.currentResourceMultiplier;
            Resource.Resources[resourceToIncrement.resourceTypeToModify].UpdateResourceInfo(gameObject, buildingAmountPerSecond, resourceToIncrement.resourceTypeToModify);
        }

        foreach (var resourceToDecrement in resourcesToDecrement)
        {
            float buildingAmountPerSecond = _selfCount * resourceToDecrement.currentResourceMultiplier;
            Resource.Resources[resourceToDecrement.resourceTypeToModify].UpdateResourceInfo(gameObject, -buildingAmountPerSecond, resourceToDecrement.resourceTypeToModify);
        }
    }
    protected void SetInitialValues()
    {
        InitializeObjects();

        FetchPrestigeValues();

        isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;
        _selfCount = (uint)PlayerPrefs.GetInt(_selfCountString, (int)_selfCount);
        initialSelfCount = (uint)PlayerPrefs.GetInt(_strInitialSelfCount, (int)initialSelfCount);

        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].costAmount = PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
        }

        if (isUnlocked)
        {
            objMainPanel.SetActive(true);
            canvas.enabled = true;
            graphicRaycaster.enabled = true;
        }

        else
        {
            objMainPanel.SetActive(false);
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
        }

        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
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

            // I moved this to amount per second
            //UpdateResourceInfo();
        }

        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
    }
    protected virtual void ModifyDescriptionText()
    {
        strDescription = "";

        foreach (var resourcePlus in resourcesToIncrement)
        {
            if (resourcesToIncrement.Count > 1)
            {
                strDescription += string.Format("Increase <color=#F3FF0A>{0:0.00}</color> amount per second by <color=#FF0AF3>{1:0.00}</color>\n", resourcePlus.resourceTypeToModify.ToString(), resourcePlus.currentResourceMultiplier);
            }
            else
            {
                strDescription += string.Format("Increase <color=#F3FF0A>{0:0.00}</color> amount per second by <color=#FF0AF3>{1:0.00}</color>", resourcePlus.resourceTypeToModify.ToString(), resourcePlus.currentResourceMultiplier);
            }
        }
        if (resourcesToDecrement != null)
        {
            foreach (var resourceMinus in resourcesToDecrement)
            {

                strDescription += string.Format("Decrease <color=#F3FF0A>{0:0.00}</color> amount per second by <color=#FF0AF3>{1:0.00}</color>\n", resourceMinus.resourceTypeToModify.ToString(), resourceMinus.currentResourceMultiplier);
            }
        }

        _txtDescription.text = strDescription;
    }
    protected virtual void ModifyAmountPerSecond()
    {
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            if (CalculateAdBoost.isAdBoostActivated)
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier * CalculateAdBoost.adBoostMultiplier;
            }
            else
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier;
            }
            StaticMethods.ModifyAPSText(Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        }

        UpdateResourceInfo();
    }
    protected override void InitializeObjects()
    {
        base.InitializeObjects();

        _objBtnMain.GetComponent<Button>().onClick.AddListener(OnBuild);

        _strInitialSelfCount = (Type.ToString() + "_initialSelfCount");
        _selfCountString = (Type.ToString() + "_selfCount");
        _isUnlockedString = (Type.ToString() + "isUnlocked");
        _costString = new string[resourceCost.Length];

        AssignPrestigeStrings();

        for (int i = 0; i < resourceCost.Length; i++)
        {
            _costString[i] = Type.ToString() + resourceCost[i].associatedType.ToString();
            PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
        }

        ModifyDescriptionText();
    }
    private void AssignPrestigeStrings()
    {
        _strPermCountAddition = Type.ToString() + "permCountAddition";
        _strPermAllMultiplierAddition = Type.ToString() + "permAllMultiplierAddition";
        _strPermMultiplierAddition = Type.ToString() + "permMultiplierAddition";
        _strPermCostSubtraction = Type.ToString() + "permCostSubtraction";

        _strPrestigeCountAddition = Type.ToString() + "PrestigeCountAddition";
        _strPrestigeAllMultiplierAddition = Type.ToString() + "PrestigeAllMultiplierAddition";
        _strPrestigeMultiplierAddition = Type.ToString() + "PrestigeMultiplierAddition";
        _strPrestigeCostSubtraction = Type.ToString() + "PrestigeCostSubtraction";
    }
    private void SavePrestigeValues()
    {
        PlayerPrefs.SetInt(_strPermCountAddition, (int)permCountAddition);
        PlayerPrefs.SetFloat(_strPermAllMultiplierAddition, permAllMultiplierAddition);
        PlayerPrefs.SetFloat(_strPermMultiplierAddition, permMultiplierAddition);
        PlayerPrefs.SetFloat(_strPermCostSubtraction, permCostSubtraction);

        PlayerPrefs.SetInt(_strPrestigeCountAddition, (int)prestigeCountAddition);
        PlayerPrefs.SetFloat(_strPrestigeAllMultiplierAddition, prestigeAllMultiplierAddition);
        PlayerPrefs.SetFloat(_strPrestigeMultiplierAddition, prestigeMultiplierAddition);
        PlayerPrefs.SetFloat(_strPrestigeCostSubtraction, prestigeCostSubtraction);
    }
    private void FetchPrestigeValues()
    {
        permCountAddition = (uint)PlayerPrefs.GetInt(_strPermCountAddition, (int)permCountAddition);
        permAllMultiplierAddition = PlayerPrefs.GetFloat(_strPermAllMultiplierAddition, permAllMultiplierAddition);
        permMultiplierAddition = PlayerPrefs.GetFloat(_strPermMultiplierAddition, permMultiplierAddition);
        permCostSubtraction = PlayerPrefs.GetFloat(_strPermCostSubtraction, permCostSubtraction);

        prestigeCountAddition = (uint)PlayerPrefs.GetInt(_strPrestigeCountAddition, (int)prestigeCountAddition);
        prestigeAllMultiplierAddition = PlayerPrefs.GetFloat(_strPrestigeAllMultiplierAddition, prestigeAllMultiplierAddition);
        prestigeMultiplierAddition = PlayerPrefs.GetFloat(_strPrestigeMultiplierAddition, prestigeMultiplierAddition);
        prestigeCostSubtraction = PlayerPrefs.GetFloat(_strPrestigeCostSubtraction, prestigeCostSubtraction);
    }
    private void UnlockedViaResources()
    {
        if (isUnlocked)
        {
            InitialUnlock();
            objMainPanel.SetActive(true);
            if (UIManager.isBuildingVisible)
            {
                canvas.enabled = true;
                graphicRaycaster.enabled = true;
                hasSeen = true;
            }
            else if (hasSeen)
            {
                isBuildingUnlockedEvent = true;
                hasSeen = false;
                PointerNotification.leftAmount++;
            }
        }
    }
    private void CheckIfUnlocked()
    {
        if (!isUnlocked)
        {
            if (GetCurrentFill() >= 0.8f & !isUnlockedByResource && isUnlockableByResource)
            {
                isUnlockedByResource = true;
                unlockAmount++;

                if (unlockAmount == unlocksRequired)
                {
                    isUnlocked = true;

                    //if (type)
                    UnlockedViaResources();

                    PointerNotification.HandleRightAnim();
                    PointerNotification.HandleLeftAnim();
                }
            }
        }
    }
    protected virtual void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;
            CheckIfPurchaseable();
            UpdateResourceCosts();
            CheckIfUnlocked();
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(_selfCountString, (int)_selfCount);
        PlayerPrefs.SetInt(_strInitialSelfCount, (int)initialSelfCount);

        for (int i = 0; i < resourceCost.Length; i++)
        {
            PlayerPrefs.SetFloat(_costString[i], resourceCost[i].costAmount);
        }

        SavePrestigeValues();
    }
}