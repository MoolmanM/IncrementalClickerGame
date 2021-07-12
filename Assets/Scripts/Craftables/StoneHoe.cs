using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneHoe : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();
    }

    void Start()
    {
        SetDescriptionText("Increases food production and worked speed perhaps.");
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
        // base.UnlockBuilding();
    }
    void Update()
    {
        UpdateResourceCosts();
    }
}
