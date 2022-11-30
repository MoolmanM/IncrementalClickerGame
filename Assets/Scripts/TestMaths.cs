using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestMaths : MonoBehaviour
{
    public float prestigeCurrency, lifetimeCurrency, highestLifetimeCurrency;

    [Button]
    public void CalculatePrestigeCurrency()
    {
        float foodWeight = Resource.Resources[ResourceType.Food].amount;
        float stoneWeight = Resource.Resources[ResourceType.Stone].amount;
        float lumberWeight = Resource.Resources[ResourceType.Lumber].amount;
        uint workerWeight = Worker.TotalWorkerCount * 1000;

        lifetimeCurrency = foodWeight + stoneWeight + lumberWeight + workerWeight;
        lifetimeCurrency *= 100;
        if (lifetimeCurrency > highestLifetimeCurrency)
        {
            prestigeCurrency = 1000000 * Mathf.Sqrt(lifetimeCurrency / Mathf.Pow(10, 15));
            highestLifetimeCurrency = lifetimeCurrency;
        }
        else
        {
            prestigeCurrency = 1000000 * Mathf.Sqrt(highestLifetimeCurrency / Mathf.Pow(10, 15));
        }      
    }

    [Button]
    public void OnPrestige()
    {
        foreach (var building in Building.Buildings)
        {
            building.Value.prestigeAllMultiplierAddition = prestigeCurrency * 0.01f; 
        }
    }

    public void Update()
    {
        CalculatePrestigeCurrency();
    }
}
