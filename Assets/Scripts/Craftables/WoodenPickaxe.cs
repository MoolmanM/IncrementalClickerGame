using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPickaxe : Craftable
{
    private Craftable _craftable;

    private void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        isUnlockableByResource = true;
        SetInitialValues();

        _buildingTypesToModify = new BuildingType[1];
        _buildingTypesToModify[0] = BuildingType.DigSite;

        _resourceTypesToModify = new ResourceType[1];
        _resourceTypesToModify[0] = ResourceType.Stones;

        _workerTypesToModify = new WorkerType[1];
        _workerTypesToModify[0] = WorkerType.Miners;
    }
    private void Start()
    {
        SetDescriptionText("Enables building of the Dig Site to start gathering stones.");
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
