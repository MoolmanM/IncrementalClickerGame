using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hut : Building
{
    private Building _building;
    //public static new uint _selfCount;
    public Events events;

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
    }
    public override void OnBuild()
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
            _selfCount++;
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
                ResourceCost[i].CostAmount *= Mathf.Pow(costMultiplier, _selfCount);                
                ResourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[ResourceCost[i].AssociatedType].amount, ResourceCost[i].CostAmount);              
            }
            events.GenerateWorker();
        }

        TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);
    }
    protected override void ModifyDescriptionText()
    {
        TxtDescription.text = string.Format("Increases population by 1");
    }
}
