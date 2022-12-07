using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHoe : CraftMultiplier
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();
    }

    void Start()
    {
        InitializeCostAmount();
        SetDescriptionText("Multiplies Potato Field's production by " + buildingMultiplierIncreasing.multiplier);
    }
}
