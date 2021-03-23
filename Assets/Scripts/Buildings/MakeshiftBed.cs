using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeshiftBed : Building
{
    private Building _building; 
    public static new uint SelfCount;

    private void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
        SelfCount = 10;
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
    public override void OnBuild()
    {
        bool canPurchase = true;

        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resourceCost[i].CurrentAmount < resourceCost[i].CostAmount)
            {
                canPurchase = false;
                break;
            }          
        }

        if (canPurchase)
        {
            SelfCount++;
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource._resources[Buildings[Type].resourceCost[i]._AssociatedType].Amount -= resourceCost[i].CostAmount;
                resourceCost[i].CostAmount *= Mathf.Pow(CostMultiplier, SelfCount);                
                resourceCost[i]._UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[Buildings[Type].resourceCost[i]._AssociatedType].Amount, resourceCost[i].CostAmount);              
            }          
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
