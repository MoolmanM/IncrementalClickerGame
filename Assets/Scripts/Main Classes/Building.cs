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
    public BuildingType Type;
    public float ResourceMultiplier, CostMultiplier;
    public ResourceType ResourceTypeToModify;
    public ResourceCost[] ResourceCost;
    protected float IncrementAmount;
    public TMP_Text DescriptionText, HeaderText;
    public GameObject MainBuildingPanel;

    private string HeaderString;

    private float _timer = 0.1f, maxValue = 0.1f;

    public void SetInitialValues()
    {
        IncrementAmount = SelfCount * ResourceMultiplier;
        Resource._resources[ResourceTypeToModify].AmountPerSecond += IncrementAmount;
        MainBuildingPanel = this.gameObject;
        HeaderString = _buildings[Type].HeaderText.text;
    }

    public virtual void UpdateBuildingElements()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            for (int i = 0; i < ResourceCost.Length; i++)
            {
                _buildings[Type].ResourceCost[i].currentAmount = Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount;
                _buildings[Type].ResourceCost[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", _buildings[Type].ResourceCost[i].currentAmount, _buildings[Type].ResourceCost[i].costAmount);
                _buildings[Type].ResourceCost[i].uiForBuilding.costNameText.text = string.Format("{0}", _buildings[Type].ResourceCost[i].associatedType.ToString());
                _buildings[Type].DescriptionText.text = string.Format("Increases {0} yield by: {1:0.00}", _buildings[Type].ResourceCost[i].associatedType.ToString(), _buildings[Type].IncrementAmount);
            }
        }
    }
    public virtual void Build()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (!_buildings.TryGetValue(Type, out Building associatedResource) || associatedResource.ResourceCost[i].currentAmount < associatedResource.ResourceCost[i].costAmount)
            {
                return;
            }

            Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount -= associatedResource.ResourceCost[i].costAmount;
            //Debug.Log(string.Format("Cost Amount: {0}, CostMultiplier: {1}, SelfCount: {2}", associatedResource.resourceCost[i].costAmount, CostMultiplier, SelfCount));
            associatedResource.ResourceCost[i].costAmount *= Mathf.Pow(CostMultiplier, SelfCount);
            associatedResource.ResourceCost[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount, associatedResource.ResourceCost[i].costAmount);
            SelfCount++;

            //This seems to work but not sure for how long
            IncrementAmount += ResourceMultiplier;
            Resource._resources[ResourceTypeToModify].AmountPerSecond += ResourceMultiplier;
            _buildings[Type] = associatedResource;
        }
        _buildings[Type].HeaderText.text = string.Format("{0} ({1})", HeaderString, SelfCount);

        
    }
}








