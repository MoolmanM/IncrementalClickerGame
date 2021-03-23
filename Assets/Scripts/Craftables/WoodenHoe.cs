using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenHoe : Craftable
{
    private Craftable _craftable;

    private void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        BuildingTypesToModify = new BuildingType[1];
        BuildingTypesToModify[0] = BuildingType.PotatoField;
    }
    private void Start()
    {
        SetInitialValues();
        CheckIfUnlocked();
        SetDescriptionText("Enables building of the Potato Field to automatically gather potatoes.");
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftables)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}
