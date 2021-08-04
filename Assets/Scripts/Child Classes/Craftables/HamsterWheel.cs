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
}
