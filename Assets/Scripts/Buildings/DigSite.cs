using System.Collections.Generic;
using UnityEngine;

public class DigSite : Building
{
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
        _buildings.Add(Type, _building);

    }

    private void Start()
    {
        SetInitialValues();
        SetDescriptionText();
        //DisplayConsole();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<BuildingType, Building> kvp in _buildings)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    public override void Build()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (!_buildings.TryGetValue(Type, out Building associatedResource) || associatedResource.ResourceCost[i].currentAmount < associatedResource.ResourceCost[i].costAmount)
            {
                return;
            }

            SelfCount++;
            Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount -= associatedResource.ResourceCost[i].costAmount;
            associatedResource.ResourceCost[i].costAmount *= Mathf.Pow(CostMultiplier, SelfCount);
            associatedResource.ResourceCost[i].UiForResourceCost.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount, associatedResource.ResourceCost[i].costAmount);
            _buildings[Type] = associatedResource;

            //This seems to work but not sure for how long
            IncrementAmount += ResourceMultiplier;
            Resource._resources[ResourceTypeToModify].AmountPerSecond += ResourceMultiplier;
            _buildings[BuildingType.MakeshiftBed].MainBuildingPanel.SetActive(true);
            _buildings[BuildingType.MakeshiftBed].IsUnlocked = 1;
        }
       HeaderText.text = string.Format("{0} ({1})", HeaderString, SelfCount);
    }


    private void Update()
    {
        UpdateResourceCosts();
    }
}
