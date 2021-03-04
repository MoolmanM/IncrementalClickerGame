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

    protected uint SelfCount;
    public BuildingType type;  
    public float CostMultiplier;
    public ResourceType resourceTypeToModify;
    public ResourceCost[] resourceCost;
    protected float incrementAmount;
    public TMP_Text descriptionText;

    private float _timer = 0.1f, maxValue = 1f;
    private bool gotTotalIncrement;
    private float totalIncrementAmount;
    private float totalAmount;

    public virtual void HandleBuilding()
    {
        incrementAmount = SelfCount * CostMultiplier;

        //Debug.Log(type + " " + resourceTypeToModify + " " + incrementAmount);
        
    }

    public virtual void UpdateResources()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;


            Resource._resources[resourceTypeToModify].amountPerSecond = incrementAmount;
            Resource._resources[resourceTypeToModify].amount += Resource._resources[resourceTypeToModify].amountPerSecond;
            Resource._resources[resourceTypeToModify].uiForResource.amount.text = string.Format("{0:0.00}", Resource._resources[resourceTypeToModify].amount);
            if (gotTotalIncrement == false)
            {
                totalIncrementAmount = Resource._resources[resourceTypeToModify].amount;
                gotTotalIncrement = true;
            }
            Resource._resources[resourceTypeToModify].uiForResource.amountPerSecond.text = string.Format("{0:0.00}/sec", totalIncrementAmount);

            for (int i = 0; i < resourceCost.Length; i++)
            {
                _buildings[type].resourceCost[i].currentAmount = Resource._resources[_buildings[type].resourceCost[i].associatedType].amount;
                Debug.Log("Current resource amount: " + _buildings[type].resourceCost[i].currentAmount);
                _buildings[type].resourceCost[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", _buildings[type].resourceCost[i].currentAmount, _buildings[type].resourceCost[i].costAmount);
                _buildings[type].resourceCost[i].uiForBuilding.costNameText.text = string.Format("{0}", _buildings[type].resourceCost[i].associatedType.ToString());
                _buildings[type].descriptionText.text = string.Format("Increases {0} yield by: {1:0.00}", _buildings[type].resourceCost[i].associatedType.ToString(), _buildings[type].incrementAmount);
            }
        }

    }

    //I will get to this later. Need to loop through the array and check if all resorucecosts meet the requirements to build the building.

    //public virtual void Build(BuildingType type)
    //{
    //    if (!_buildings.TryGetValue(type, out Building associatedResource) || associatedResource.resourceCost.currentAmount < associatedResource.resourceCost.costAmount)
    //    {
    //        return;
    //    }

    //    associatedResource.resourceCost.currentAmount -= associatedResource.resourceCost.costAmount;
    //    associatedResource.resourceCost.costAmount *= Mathf.Pow(CostMultiplier, SelfCount);

    //    _buildings[type] = associatedResource;
    //}
}
 








