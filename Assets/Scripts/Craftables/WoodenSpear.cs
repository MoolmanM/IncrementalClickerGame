using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpear : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();

        _workerTypesToModify = new WorkerType[1];
        _workerTypesToModify[0] = WorkerType.Hunters;

        _craftingTypesToModify = new CraftingType[1];
        _craftingTypesToModify[0] = CraftingType.StoneSpear;
    }
    void Start()
    {
        SetDescriptionText("Enables you to assign your workers to go hunting.");
    }
    protected override void UnlockResource()
    {
        // Do nothing.
    }
    protected override void UnlockBuilding()
    {
        // Do nothing.
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftables)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    void Update()
    {
        UpdateResourceCosts();
    }
}
