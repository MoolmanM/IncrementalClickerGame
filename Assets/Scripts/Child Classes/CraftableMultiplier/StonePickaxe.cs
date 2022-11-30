using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePickaxe : CraftMultiplier
{
    private Craftable _craftable;
    private float newAmount;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();
    }

    void Start()
    {
        // Maybe enables the mining of ore.
        // Can start digging bronze ore at the dig site?
        // So should either enable dig site to mine some ores, 
        // Or unlock a new building responsible with mining ores.
        // Lets go with unlocking a new building for now.
        SetDescriptionText("Multiplies Lumber Mill's production by " + buildingMultiplierIncreasing.multiplier);

        foreach (var resourceCost in Building.Buildings[buildingMultiplierIncreasing.buildingType].resourceCost)
        {
            Debug.Log(resourceCost.associatedType + " " +  resourceCost.baseCostAmount * Mathf.Pow(Building.Buildings[buildingMultiplierIncreasing.buildingType].costMultiplier, 25));
            newAmount += resourceCost.baseCostAmount * Mathf.Pow(Building.Buildings[buildingMultiplierIncreasing.buildingType].costMultiplier, 25);
            //newAmount += newAmount;
        }

        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].costAmount = newAmount;
        }
        Debug.Log("STONE PICKAXE: " + newAmount);
    }
}
