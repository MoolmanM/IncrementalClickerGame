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
        SetInitialValues();
        _buildingTypesToModify = new BuildingType[1];
        _buildingTypesToModify[0] = BuildingType.PotatoField;
    }
    private void Start()
    {
        SetDescriptionText("Enables building of the Potato Field to automatically gather potatoes.");
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftables)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    protected override void UnlockResource()
    {
        // Do nothing.
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}
