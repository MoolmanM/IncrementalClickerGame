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
            _selfCount++;
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].AssociatedType].amount -= resourceCost[i].CostAmount;
                resourceCost[i].CostAmount *= Mathf.Pow(costMultiplier, _selfCount);                
                resourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[resourceCost[i].AssociatedType].amount, resourceCost[i].CostAmount);              
            }
            events.GenerateWorker();
        }

        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
    }
    protected override void ModifyDescriptionText()
    {
        _txtDescription.text = string.Format("Increases population by 1");
    }
}
