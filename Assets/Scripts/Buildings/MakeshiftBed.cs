using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeshiftBed : Building
{
    private Building _building;
    public TMP_Text availableWorkerText;

    private void Awake()
    {
        _building = GetComponent<Building>();
        _buildings.Add(Type, _building);
        
    }

    private void Start()
    {
        SetInitialValues();        
        SetDescriptionText();
        MainBuildingPanel.SetActive(false);
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

            Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount -= associatedResource.ResourceCost[i].costAmount;
            associatedResource.ResourceCost[i].costAmount *= Mathf.Pow(CostMultiplier, SelfCount);
            associatedResource.ResourceCost[i].UiForResourceCost.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount, associatedResource.ResourceCost[i].costAmount);
            SelfCount++;
            Worker.AvailableWorkerCount++;
            availableWorkerText.text = string.Format("Available Workers: [{0}]", Worker.AvailableWorkerCount);
            _buildings[Type] = associatedResource;
        }
        HeaderText.text = string.Format("{0} ({1})", HeaderString, SelfCount);


    }
    public override void SetDescriptionText()
    {
        availableWorkerText.text = string.Format("Available Workers: [{0}]", Worker.AvailableWorkerCount);
    }

    private void Update()
    {
        UpdateResourceCosts();
    }
}
