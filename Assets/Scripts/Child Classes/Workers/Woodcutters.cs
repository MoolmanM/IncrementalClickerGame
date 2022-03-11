using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutters : Worker
{
    private Worker _worker;

    void Awake()
    {
        _worker = GetComponent<Worker>();
        Workers.Add(Type, _worker);
        SetInitialValues();
    }

    //public override void ModifyMultiplier()
    //{
    //    for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //    {
    //        float newMultiplier = 0.15f;
    //        float difference = (newMultiplier - _resourcesToIncrement[i].currentResourceMultiplier) * workerCount;
    //        Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += difference;
    //        Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond);
    //        _resourcesToIncrement[i].currentResourceMultiplier = 0.15f;

    //        foreach (var resourceToIncrement in _resourcesToIncrement)
    //        {
    //            float workerAmountPerSecond = workerCount * resourceToIncrement.currentResourceMultiplier;
    //            Resource.Resources[resourceToIncrement.resourceTypeToModify].UpdateResourceInfo(gameObject, workerAmountPerSecond, resourceToIncrement.resourceTypeToModify);
    //        }
    //    }     
    //}
}
