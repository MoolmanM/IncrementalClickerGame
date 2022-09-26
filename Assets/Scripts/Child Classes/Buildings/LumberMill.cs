using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LumberMill : Building
{
    private Building _building;
    public float testMultiplierAmount;

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();       
    }
    [Button]
    public void MultiplyMultiplier()
    {
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            BuildingResourcesToModify buildingResourcesToModify = resourcesToIncrement[i];
            buildingResourcesToModify.currentResourceMultiplier *= testMultiplierAmount;        
            resourcesToIncrement[i] = buildingResourcesToModify;
            ModifyDescriptionText();

            if (CalculateAdBoost.isAdBoostActivated)
            {
                //Debug.Log("Multiplier: " + resourcesToIncrement[i].currentResourceMultiplier + " Self Count: " + _selfCount);
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond *= testMultiplierAmount;
            }
            else
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond *= testMultiplierAmount;
            }
            StaticMethods.ModifyAPSText(Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        }

    }
}