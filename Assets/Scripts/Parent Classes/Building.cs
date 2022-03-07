using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BuildingType
{
    PotatoField,
    LumberMill,
    DigSite,
    Hut, // Change this to "Hut"?
    Smelter,
    StoragePile, // For Stone, Lumber, maybe other materials
    //StorageTent, For food, and more delicate stuff.
    // Storage Tent maybe after storage pile, because it needs to be made of pelts i.e leather.
    //StorageHouse
    //Storage Facility
    //Transformer maybe transformer should be something that you craft or research, for a once of boost to energy
    MineShaft,
    WoodGenerator


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

    protected uint _selfCount;
    protected string _selfCountString, _isUnlockedString;
    protected string[] _costString;

    //private TMP_Text 
    private uint typesToModifyCount;

    public uint initialSelfCount;

    public void ReModifyDescription()
    {
        UpdateResourceInfo();
        ModifyDescriptionText();
    }
    public void ResetBuilding()
    {
        isUnlocked = false;
        objMainPanel.SetActive(false);
        unlockAmount = 0;
        _selfCount = 0;
        hasSeen = true;
        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].costAmount = resourceCost[i].initialCostAmount;
            resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[resourceCost[i].associatedType].amount, resourceCost[i].costAmount);
        }
        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
    }
    public uint ReturnSelfCount()
    {
        return _selfCount;
    }
    public void SetSelfCount(uint selfCountAmount)
    {
        initialSelfCount += selfCountAmount;
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

        //if (TimeManager.hasPlayedBefore)
        //{
            isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;
            _selfCount = (uint)PlayerPrefs.GetInt(_selfCountString, (int)_selfCount);

            for (int i = 0; i < resourceCost.Length; i++)
            {
                resourceCost[i].costAmount = PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
            }
        //}

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
        string oldString;

        // Bug here, so lets say it has 2 decrements, and one increment, well it's going to not refer to oldString here because it only has 1 increment.
        //for (int i = 0; i < resourcesToIncrement.Count; i++)
        //{
        //    if (i > 0)
        //    {
        //        oldString = _txtDescription.text;
        //        _txtDescription.text = string.Format("{0} \nIncrease <color=#F3FF0A>{1}</color> amount per second by <color=#FF0AF3>{2}</color>", oldString, resourcesToIncrement[i].resourceTypeToModify.ToString(), resourcesToIncrement[i].currentResourceMultiplier);
        //    }
        //    else
        //    {
        //        _txtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> amount per second by <color=#FF0AF3>{1}</color>.", resourcesToIncrement[i].resourceTypeToModify.ToString(), resourcesToIncrement[i].currentResourceMultiplier);
        //    }
        //    Debug.Log(oldString);
        //}
        //for (int i = 0; i < resourcesToDecrement.Count; i++)
        //{
        //    if (i > 0)
        //    {
        //        oldString = _txtDescription.text;
        //        _txtDescription.text = string.Format("{0} \nDecrease <color=#F3FF0A>{1}</color> amount per second by <color=#FF0AF3>{2}</color>", oldString, resourcesToDecrement[i].resourceTypeToModify.ToString(), resourcesToDecrement[i].currentResourceMultiplier);
        //    }
        //    else
        //    {
        //        _txtDescription.text = string.Format("Decrease <color=#F3FF0A>{0}</color> amount per second by <color=#FF0AF3>{1}</color>.", resourcesToDecrement[i].resourceTypeToModify.ToString(), resourcesToDecrement[i].currentResourceMultiplier);
        //    }
        //}
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            typesToModifyCount++;
        }
        for (int i = 0; i < resourcesToDecrement.Count; i++)
        {
            typesToModifyCount++;
        }


        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            if (typesToModifyCount > 1)
            {
                oldString = _txtDescription.text;

                if (!string.IsNullOrEmpty(oldString))
                {
                    _txtDescription.text = string.Format("{0} \nIncrease <color=#F3FF0A>{1}</color> amount per second by <color=#FF0AF3>{2:0.00}</color>", oldString, resourcesToIncrement[i].resourceTypeToModify.ToString(), resourcesToIncrement[i].currentResourceMultiplier);
                }
                else
                {
                    _txtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> amount per second by <color=#FF0AF3>{1:0.00}</color>.", resourcesToIncrement[i].resourceTypeToModify.ToString(), resourcesToIncrement[i].currentResourceMultiplier);
                }
            }
            else
            {
                _txtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> amount per second by <color=#FF0AF3>{1:0.00}</color>.", resourcesToIncrement[i].resourceTypeToModify.ToString(), resourcesToIncrement[i].currentResourceMultiplier);
            }
        }

        for (int i = 0; i < resourcesToDecrement.Count; i++)
        {
            if (typesToModifyCount > 1)
            {
                oldString = _txtDescription.text;

                if (!string.IsNullOrEmpty(oldString))
                {
                    _txtDescription.text = string.Format("{0} \nDecrease <color=#F3FF0A>{1}</color> amount per second by <color=#FF0AF3>{2}</color>", oldString, resourcesToDecrement[i].resourceTypeToModify.ToString(), resourcesToDecrement[i].currentResourceMultiplier);
                }
                else
                {
                    _txtDescription.text = string.Format("Decrease <color=#F3FF0A>{0}</color> amount per second by <color=#FF0AF3>{1}</color>.", resourcesToDecrement[i].resourceTypeToModify.ToString(), resourcesToDecrement[i].currentResourceMultiplier);
                }
            }
            else
            {
                _txtDescription.text = string.Format("Decrease <color=#F3FF0A>{0}</color> amount per second by <color=#FF0AF3>{1}</color>.", resourcesToDecrement[i].resourceTypeToModify.ToString(), resourcesToDecrement[i].currentResourceMultiplier);
            }
        }
        // So I need to check if increment and decrement combined is more than 1
    }
    protected virtual void ModifyAmountPerSecond()
    {
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            if (WatchAdPopup.isAdBoostActivated)
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier * WatchAdPopup.adBoostMultiplier;
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
    protected void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;
            CheckIfPurchaseable();
            UpdateResourceCosts();
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








