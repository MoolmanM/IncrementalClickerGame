using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPickaxe : Craftable
{
    private Craftable _craftable;

    private void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        //DisplayConsole();
    }
    private void Start()
    {
        SetInitialValues();
        SetDescriptionText("Enables building of the Dig Site.");
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftables)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    protected override void Craft()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (!Craftables.TryGetValue(Type, out Craftable associatedResource) || associatedResource.ResourceCost[i].CurrentAmount < associatedResource.ResourceCost[i].CostAmount)
            {
                return;
            }

            Resource._resources[Craftables[Type].ResourceCost[i]._AssociatedType].Amount -= associatedResource.ResourceCost[i].CostAmount;

            Craftables[Type] = associatedResource;

            IsCrafted = 1;
            FinishedCrafting();
            Building.Buildings[BuildingType.MakeshiftBed].IsUnlocked = 1;

        }
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}
