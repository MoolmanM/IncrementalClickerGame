using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneAxe : Craftable
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
        SetDescriptionText("Enables collection of logs after woodcutting, maybe even unlocks a new woodcutting related building.");
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
