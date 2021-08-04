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
    public BuildingType Type;

    private string _selfCountString;
    private string[] _costString;

    protected float _resourceMultiplier, _costMultiplier;
    protected ResourceType resourceTypeToModify;
    protected string _stringOriginalHeader;
    protected uint _selfCount;
    protected TMP_Text _txtHeader;

    void OnValidate()
    {
        if (typesToModify.buildingTypesToModify.Length != 0)
        {
            typesToModify.isModifyingBuilding = true;
        }
        else
        {
            typesToModify.isModifyingBuilding = false;
        }

        if (typesToModify.craftingTypesToModify.Length != 0)
        {
            typesToModify.isModifyingCrafting = true;
        }
        else
        {
            typesToModify.isModifyingCrafting = false;
        }

        if (typesToModify.researchTypesToModify.Length != 0)
        {
            typesToModify.isModifyingResearch = true;
        }
        else
        {
            typesToModify.isModifyingResearch = false;
        }

        if (typesToModify.workerTypesToModify.Length != 0)
        {
            typesToModify.isModifyingWorker = true;
        }
        else
        {
            typesToModify.isModifyingWorker = false;
        }

        if (typesToModify.resourceTypesToModify.Length != 0)
        {
            typesToModify.isModifyingResource = true;
        }
        else
        {
            typesToModify.isModifyingResource = false;
        }
    }
    public void SetInitialValues()
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
        _txtHeader.text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    protected override void InitializeObjects()
    {
        _tformBody = transform.Find("Panel_Main/Body");

        #region Prefab Initializion

        _prefabResourceCost = Resources.Load<GameObject>("ResourceCost_Prefab/ResourceCost_Panel");
        _prefabBodySpacer = Resources.Load<GameObject>("ResourceCost_Prefab/Body_Spacer");

        for (int i = 0; i < resourceCost.Length; i++)
        {
            GameObject newObj = Instantiate(_prefabResourceCost, _tformBody);

            // This for loop just makes sure that there is a never a body spacer underneath the last element(the last resource cost panel)
            for (int spacerI = i + 1; spacerI < resourceCost.Length; spacerI++)
            {
                Instantiate(_prefabBodySpacer, _tformBody);
            }

            Transform _tformNewObj = newObj.transform;
            Transform _tformCostName = _tformNewObj.Find("Cost_Name_Panel/Text_CostName");
            Transform _tformCostAmount = _tformNewObj.Find("Cost_Amount_Panel/Text_CostAmount");

            resourceCost[i].uiForResourceCost.textCostName = _tformCostName.GetComponent<TMP_Text>();
            resourceCost[i].uiForResourceCost.textCostAmount = _tformCostAmount.GetComponent<TMP_Text>();
        }

        #endregion

        _tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        _tformDescription = transform.Find("Panel_Main/Body/Description_Panel/Text_Description");
        _tformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        _tformObjProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        _tformObjMain = transform.Find("Panel_Main");
        _tformBtnCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformBtnExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");

        objMainPanel = _tformObjMain.gameObject;
        _txtHeader = _tformTxtHeader.GetComponent<TMP_Text>();
        _txtDescription = _tformDescription.GetComponent<TMP_Text>();
        _imgProgressbar = _tformObjProgressCircle.GetComponent<Image>();
        _objBtnMain = _tformBtnMain.gameObject;
        _objBtnExpand = _tformBtnExpand.gameObject;
        _objBtnCollapse = _tformBtnCollapse.gameObject;
        _objBody = _tformBody.gameObject;

        _objBtnExpand.GetComponent<Button>().onClick.AddListener(OnExpandCloseAll);

        _stringOriginalHeader = _txtHeader.text;

        _selfCountString = (Type.ToString() + "_selfCount");
        _isUnlockedString = (Type.ToString() + "isUnlocked");
        _costString = new string[resourceCost.Length];

        for (int i = 0; i < resourceCost.Length; i++)
        {
            _costString[i] = Type.ToString() + resourceCost[i].associatedType.ToString();
            PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
        }
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
        }

        _txtHeader.text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    public virtual void SetDescriptionText()
    {
        Buildings[Type]._txtDescription.text = string.Format("Increases {0} yield by: {1:0.00}", Resource.Resources[resourceTypeToModify].Type.ToString(), _resourceMultiplier);
    }
    protected virtual void ModifyAmountPerSecond()
    {
        Resource.Resources[resourceTypeToModify].amountPerSecond += _resourceMultiplier;
    }
    protected void SetDescriptionText(string description)
    {
        Buildings[Type]._txtDescription.text = string.Format("{0}", description);
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








