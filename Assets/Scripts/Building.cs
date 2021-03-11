using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct UiForBuilding
{
    public TMP_Text costNameText;
    public TMP_Text costAmountText;
}

[System.Serializable]
public struct ResourceCost
{
    public ResourceType associatedType;
    [System.NonSerialized] public float currentAmount;
    public float costAmount;
    public UiForBuilding uiForBuilding;

    public ResourceCost(float currentAmount, float costAmount, UiForBuilding uiForBuilding) : this()
    {
        CurrentAmount = currentAmount;
        CostAmount = costAmount;

    }

    public float CurrentAmount { get; }
    public float CostAmount { get; }
}

public enum BuildingType
{
    PotatoField,
    Woodlot,
    DigSite
}

public abstract class Building : MonoBehaviour
{
    public static Dictionary<BuildingType, Building> _buildings = new Dictionary<BuildingType, Building>();

    public uint SelfCount;
    public BuildingType type;
    public float ResourceMultiplier, CostMultiplier;
    public ResourceType resourceTypeToModify;
    public ResourceCost[] resourceCost;
    protected float incrementAmount;
    public TMP_Text descriptionText, headerText;
    public GameObject mainBuildingPanel;

    private string headerString;

    private float _timer = 0.1f, maxValue = 0.1f;

    public void SetInitialValues()
    {
        incrementAmount = SelfCount * ResourceMultiplier;
        Resource._resources[resourceTypeToModify].amountPerSecond += incrementAmount;
        mainBuildingPanel = this.gameObject;
        headerString = _buildings[type].headerText.text;
    }

    public virtual void UpdateBuildingElements()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            for (int i = 0; i < resourceCost.Length; i++)
            {
                _buildings[type].resourceCost[i].currentAmount = Resource._resources[_buildings[type].resourceCost[i].associatedType].amount;
                _buildings[type].resourceCost[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", _buildings[type].resourceCost[i].currentAmount, _buildings[type].resourceCost[i].costAmount);
                _buildings[type].resourceCost[i].uiForBuilding.costNameText.text = string.Format("{0}", _buildings[type].resourceCost[i].associatedType.ToString());
                _buildings[type].descriptionText.text = string.Format("Increases {0} yield by: {1:0.00}", _buildings[type].resourceCost[i].associatedType.ToString(), _buildings[type].incrementAmount);
            }
        }
    }
    public virtual void Build()
    {
        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (!_buildings.TryGetValue(type, out Building associatedResource) || associatedResource.resourceCost[i].currentAmount < associatedResource.resourceCost[i].costAmount)
            {
                return;
            }

            Resource._resources[_buildings[type].resourceCost[i].associatedType].amount -= associatedResource.resourceCost[i].costAmount;
            //Debug.Log(string.Format("Cost Amount: {0}, CostMultiplier: {1}, SelfCount: {2}", associatedResource.resourceCost[i].costAmount, CostMultiplier, SelfCount));
            associatedResource.resourceCost[i].costAmount *= Mathf.Pow(CostMultiplier, SelfCount);
            associatedResource.resourceCost[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[_buildings[type].resourceCost[i].associatedType].amount, associatedResource.resourceCost[i].costAmount);
            SelfCount++;
            
            //This seems to work but not sure for how long
            incrementAmount += ResourceMultiplier;
            Resource._resources[resourceTypeToModify].amountPerSecond += ResourceMultiplier;
            _buildings[type] = associatedResource;
        }
        _buildings[type].headerText.text = string.Format("{0} ({1})", headerString, SelfCount);

        
    }
}








