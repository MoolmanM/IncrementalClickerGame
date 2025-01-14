using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CopperAxe : CraftableModifier
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
        InitializeDescriptionText();
    }
}
