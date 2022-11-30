using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneAxe : CraftMultiplier
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
        // I think it's to early to unlock another building related to woodcutting.
        // Maybe make it so that for every woodcutter they can get 1 log per.... minute? 30 seconds? 1 second? need some more thinking on this.
        SetDescriptionText("Multiplies Lumber Mill's production by " + buildingMultiplierIncreasing.multiplier);

        foreach (var resourceCost in Building.Buildings[buildingMultiplierIncreasing.buildingType].resourceCost)
        {
            newAmount = resourceCost.baseCostAmount * Mathf.Pow(Building.Buildings[buildingMultiplierIncreasing.buildingType].costMultiplier, 25);
            //newAmount += newAmount;
        }

        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].costAmount = newAmount;
        }
        Debug.Log(newAmount);
    }
}
