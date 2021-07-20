using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterWheel : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        
        _workerTypesToModify = new WorkerType[1];
        _workerTypesToModify[0] = WorkerType.EnergyProducers;

        _resourceTypesToModify = new ResourceType[1];
        _resourceTypesToModify[0] = ResourceType.Energy;

        SetInitialValues();
    }

    void Start()
    {
        SetDescriptionText("Enables very inefficient production of energy via workers running on a hamster wheel.");
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftables)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    protected override void UnlockBuilding()
    {
        // Do nothing.
    }
    void Update()
    {
        UpdateResourceCosts();
    }
}
