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
        SetInitialValues();
        SetDescriptionText("Enables building of the Dig Site.");
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
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (!_craftables.TryGetValue(Type, out Craftable associatedResource) || associatedResource.ResourceCost[i].currentAmount < associatedResource.ResourceCost[i].costAmount)
            {
                return;
            }

            Resource._resources[_craftables[Type].ResourceCost[i].associatedType].Amount -= associatedResource.ResourceCost[i].costAmount;

            _craftables[Type] = associatedResource;
            HeaderText.text = string.Format("{0} (Crafted)", HeaderText.text);
            Destroy(ButtonMain);
            Building._buildings[BuildingTypeToActivate].MainBuildingPanel.SetActive(true);
            Resource._resources[ResourceType.Stones].MainResourcePanel.SetActive(true);
        }
    }

    private void Update()
    {
        UpdateResourceCosts();
    }
}
