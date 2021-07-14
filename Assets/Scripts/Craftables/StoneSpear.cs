using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpear : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();

        _craftingTypesToModify = new CraftingType[1];
        _craftingTypesToModify[0] = CraftingType.FireHardenedSpear;       
    }

    void Start()
    {
        SetDescriptionText("Increases your hunting efficiency.");
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
    protected override void UnlockBuilding()
    {
        // Do nothing.
    }
    protected override void UnlockWorkerJob()
    {
        // do nothing
    }
    void Update()
    {
        UpdateResourceCosts();
    }
}
