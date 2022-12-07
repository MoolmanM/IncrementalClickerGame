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
        InitializeCostAmount();
        SetDescriptionText("Multiplies Dig Site's production by " + buildingMultiplierIncreasing.multiplier);
    }
}
