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
        isUnlockableByResource = true;
        _buildingTypesToModify = new BuildingType[1];
        _buildingTypesToModify[0] = BuildingType.Woodlot;

        _workerTypesToModify = new WorkerType[1];
        _workerTypesToModify[0] = WorkerType.Woodcutters;
        SetInitialValues();
    }
    void Start()
    {
        SetDescriptionText("Enables building of the Wood-lot to automatically gather sticks.");
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

    void Update()
    {
        UpdateResourceCosts();
    }
}
