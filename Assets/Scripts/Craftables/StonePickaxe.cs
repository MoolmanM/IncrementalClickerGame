using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePickaxe : Craftable
{
    private Craftable _craftable;

    private void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
    }

    private void Start()
    {
        SetInitialValues();
        CheckIfUnlocked();      
        // Maybe enables the mining of ore.
        // Can start digging bronze ore at the dig site?
        SetDescriptionText("Increases stone production speed and worker speed perhaps?.");
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
