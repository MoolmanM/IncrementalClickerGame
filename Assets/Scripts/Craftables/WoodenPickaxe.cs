using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPickaxe : Craftable
{
    private Craftable _craftable;

    private void Awake()
    {
        _craftable = GetComponent<Craftable>();
        _craftables.Add(Type, _craftable);
        //DisplayConsole();
    }

    private void Start()
    {
        SetDescriptionText("Enables building of the Dig-Site.");
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<CraftingType, Craftable> kvp in _craftables)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    public override void Craft()
    {
        base.Craft();
    }

    private void Update()
    {
        UpdateCraftingElements();
    }
}
