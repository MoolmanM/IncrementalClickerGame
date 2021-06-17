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
        SetInitialValues();
    }

    private void Start()
    {
        // I think it's to early to unlock another building related to woodcutting.
        // Maybe make it so that for every woodcutter they can get 1 log per.... minute? 30 seconds? 1 second? need some more thinking on this.
        SetDescriptionText("Enables collection of logs after woodcutting, maybe even unlocks a new woodcutting related building.");
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
    private void Update()
    {
        UpdateResourceCosts();
    }
}
