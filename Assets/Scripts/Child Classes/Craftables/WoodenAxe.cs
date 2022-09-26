using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenAxe : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();
        SetDescriptionText("Enables building of the Wood-lot to automatically gather sticks.");
    }
}
