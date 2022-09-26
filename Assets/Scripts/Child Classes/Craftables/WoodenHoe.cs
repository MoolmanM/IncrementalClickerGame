using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenHoe : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();
        SetDescriptionText("Enables building of the Potato Field to automatically gather potatoes.");
    }
}
