using Sirenix.OdinInspector;
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
    Library,
    None


}

[System.Serializable]
public struct BuildingResourcesToModify
{
    public ResourceType resourceTypeToModify;
    public float currentResourceMultiplier, baseResourceMultiplier;
    public float contributionAmount;
}

public abstract class Building : GameEntity
{
    public static Dictionary<BuildingType, Building> Buildings = new Dictionary<BuildingType, Building>();
    public static bool isBuildingUnlockedEvent;

    public List<BuildingResourcesToModify> resourcesToIncrement = new List<BuildingResourcesToModify>();
    public List<BuildingResourcesToModify> resourcesToDecrement = new List<BuildingResourcesToModify>();


    public BuildingType Type;
    public float costMultiplier;

    public uint _selfCount;
    protected string _selfCountString, _IsUnlockedString, _strInitialSelfCount;
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

    public uint trackedBuiltAmount;

    public void ModifySelfCount()
    {
        _selfCount = 0;
        _selfCount += (permCountAddition + prestigeCountAddition);
        prestigeCountAddition = 0;

        // Maybe need a permCountAddition and a prestigeCountAddition, prestigeCount needs to be set to zero after prestiging. permanent one needs to be saved throughout.
        // I think at least, needs some more brainstorming.
        // Both needs to be saved on exit and assigned values on start. 

        Debug.Log(string.Format("Changed building {0}'s self count from {1} to {2}", ActualName, "Hopefully 0", _selfCount));
    }
    public void InitialUnlock()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            //Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
            ResourceCost[i].CostAmount *= Mathf.Pow(costMultiplier, _selfCount);
            ResourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[ResourceCost[i].AssociatedType].amount), NumberToLetter.FormatNumber(ResourceCost[i].CostAmount));
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
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            ResourceCost[i].CostAmount = ResourceCost[i].BaseCostAmount;
            float subtractionAmount = ResourceCost[i].BaseCostAmount * (prestigeCostSubtraction + permCostSubtraction);
            prestigeCostSubtraction = 0;
            ResourceCost[i].CostAmount -= subtractionAmount;
            Debug.Log(string.Format("Changed building {0}'s cost from {1} to {2}", ActualName, ResourceCost[i].BaseCostAmount, ResourceCost[i].CostAmount));
            ResourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[ResourceCost[i].AssociatedType].amount, ResourceCost[i].CostAmount);
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
            Debug.Log(string.Format("Changed building {0}'s resource multi from {1} to {2}", ActualName, buildingResourcesToModify.baseResourceMultiplier, buildingResourcesToModify.currentResourceMultiplier));
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

        IsUnlocked = false;
        Canvas.enabled = false;
        ObjMainPanel.SetActive(false);
        GraphicRaycaster.enabled = false;
        UnlockAmount = 0;
        _selfCount = 0;
        HasSeen = true;
        IsUnlockedByResource = false;
        ModifySelfCount();
        ModifyMultiplier();
        ModifyCost();
        TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);
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

        TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);

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

        IsUnlocked = PlayerPrefs.GetInt(_IsUnlockedString) == 1 ? true : false;
        _selfCount = (uint)PlayerPrefs.GetInt(_selfCountString, (int)_selfCount);
        initialSelfCount = (uint)PlayerPrefs.GetInt(_strInitialSelfCount, (int)initialSelfCount);

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            ResourceCost[i].CostAmount = PlayerPrefs.GetFloat(_costString[i], ResourceCost[i].CostAmount);
        }

        if (IsUnlocked)
        {
            ObjMainPanel.SetActive(true);
            Canvas.enabled = true;
            GraphicRaycaster.enabled = true;
        }

        else
        {
            ObjMainPanel.SetActive(false);
            Canvas.enabled = false;
            GraphicRaycaster.enabled = false;
        }

        TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);
    }
    public virtual void OnBuild()
    {
        bool canPurchase = true;

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (ResourceCost[i].CurrentAmount < ResourceCost[i].CostAmount)
            {
                canPurchase = false;
                break;
            }
        }

        if (canPurchase)
        {
            _selfCount++;
            trackedBuiltAmount++;
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
                //ResourceCost[i].BaseCostAmount = 0;
                ResourceCost[i].CostAmount = ResourceCost[i].BaseCostAmount * Mathf.Pow(costMultiplier, _selfCount);
                ResourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[ResourceCost[i].AssociatedType].amount), NumberToLetter.FormatNumber(ResourceCost[i].CostAmount));
            }
            ModifyAmountPerSecond();

            // I moved this to amount per second
            //UpdateResourceInfo();
        }

        TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);
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

        TxtDescription.text = strDescription;
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

        ObjBtnMain.GetComponent<Button>().onClick.AddListener(OnBuild);

        _strInitialSelfCount = (Type.ToString() + "_initialSelfCount");
        _selfCountString = (Type.ToString() + "_selfCount");
        _IsUnlockedString = (Type.ToString() + "IsUnlocked");
        _costString = new string[ResourceCost.Length];

        AssignPrestigeStrings();

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            _costString[i] = Type.ToString() + ResourceCost[i].AssociatedType.ToString();
            PlayerPrefs.GetFloat(_costString[i], ResourceCost[i].CostAmount);
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
        if (IsUnlocked)
        {
            InitialUnlock();
            ObjMainPanel.SetActive(true);
            if (UIManager.isBuildingVisible)
            {
                Canvas.enabled = true;
                GraphicRaycaster.enabled = true;
                HasSeen = true;
            }
            else if (HasSeen)
            {
                isBuildingUnlockedEvent = true;
                HasSeen = false;
                //PointerNotification.leftAmount++;
            }
        }
    }
    private void CheckIfUnlocked()
    {
        if (!IsUnlocked)
        {
            if (GetCurrentFill() >= 0.8f & !IsUnlockedByResource && IsUnlockableByResource)
            {
                IsUnlockedByResource = true;
                UnlockAmount++;

                if (UnlockAmount == UnlocksRequired)
                {
                    IsUnlocked = true;

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
        if ((timer -= Time.deltaTime) <= 0)
        {
            timer = maxValue;
            CheckIfPurchaseable();
            UpdateResourceCostTexts();
            CheckIfUnlocked();
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_IsUnlockedString, IsUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(_selfCountString, (int)_selfCount);
        PlayerPrefs.SetInt(_strInitialSelfCount, (int)initialSelfCount);

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            PlayerPrefs.SetFloat(_costString[i], ResourceCost[i].CostAmount);
        }

        SavePrestigeValues();
    }
    public void MultiplyIncrementAmount(float mulitplierAmount)
    {
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            BuildingResourcesToModify buildingResourcesToModify = resourcesToIncrement[i];
            buildingResourcesToModify.currentResourceMultiplier *= mulitplierAmount;
            resourcesToIncrement[i] = buildingResourcesToModify;
            ModifyDescriptionText();

            if (CalculateAdBoost.isAdBoostActivated)
            {
                //Debug.Log("Multiplier: " + resourcesToIncrement[i].currentResourceMultiplier + " Self Count: " + _selfCount);
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond *= mulitplierAmount;
            }
            else
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond *= mulitplierAmount;
            }
            StaticMethods.ModifyAPSText(Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        }

    }
}