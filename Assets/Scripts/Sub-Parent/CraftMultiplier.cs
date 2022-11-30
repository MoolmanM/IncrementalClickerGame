using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuildingMultiplierIncreasing
{
    public BuildingType buildingType;
    public float multiplier;
}
public class CraftMultiplier : Craftable
{
    public BuildingMultiplierIncreasing buildingMultiplierIncreasing;
    protected override void OnCraft()
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
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            }
            isCrafted = true;
            Crafted();
            Building.Buildings[buildingMultiplierIncreasing.buildingType].MultiplyMultiplier(buildingMultiplierIncreasing.multiplier);                      
        }
    }
}
