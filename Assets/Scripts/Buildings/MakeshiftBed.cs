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
        Buildings.Add(Type, _building);
        SetInitialValues();
    }
    private void Start()
    {       
        CheckIfUnlocked();
        SetDescriptionText();
        // DisplayConsole();
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<BuildingType, Building> kvp in Buildings)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    public override void Build()
    {
        bool canPurchase = true;

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (ResourceCost[i].CurrentAmount < ResourceCost[i].CostAmount)
            {
                canPurchase = false;
                break;
            }          
        }

        if (canPurchase)
        {
            SelfCount++;
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource._resources[Buildings[Type].ResourceCost[i]._AssociatedType].Amount -= ResourceCost[i].CostAmount;
                ResourceCost[i].CostAmount *= Mathf.Pow(CostMultiplier, SelfCount);                
                ResourceCost[i]._UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[Buildings[Type].ResourceCost[i]._AssociatedType].Amount, ResourceCost[i].CostAmount);              
            }
            Worker.AvailableWorkerCount++;
            availableWorkerText.text = string.Format("Available Workers: [{0}]", Worker.AvailableWorkerCount);
            
        }

        TxtHeader.text = string.Format("{0} ({1})", OriginalHeaderString, SelfCount);
    }
    public override void SetDescriptionText()
    {
        TxtDescription.text = string.Format("Increases population by 1");
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}
