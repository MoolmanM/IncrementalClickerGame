using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHardenedSpear : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        unlocksRequired = 2;
        SetInitialValues();

        
    }
    void Start()
    {
        SetDescriptionText("Increases efficiency of hunting.");
    }
    protected override void UnlockResource()
    {
        // Do nothing.
    }
    protected override void UnlockBuilding()
    {
        // Do nothing.
    }
    protected override void UnlockWorkerJob()
    {
        // Do nothing
    }
    protected override void UnlockCrafting()
    {
        // do nothing
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
