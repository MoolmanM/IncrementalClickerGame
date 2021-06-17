using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeshiftBed : Building
{
    private Building _building; 
    public static new uint _selfCount;

    private void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
        _selfCount = 10;
    }
    private void Start()
    {
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
            if (resourceCost[i].currentAmount < resourceCost[i].costAmount)
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
                Resource.Resources[Buildings[Type].resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
                resourceCost[i].costAmount *= Mathf.Pow(_costMultiplier, _selfCount);                
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[Buildings[Type].resourceCost[i].associatedType].amount, resourceCost[i].costAmount);              
            }          
        }

        _txtHeader.text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    public override void SetDescriptionText()
    {
        _txtDescription.text = string.Format("Increases population by 1");
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}
