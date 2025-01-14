using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronAxe : CraftableModifier
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
