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
        float foodWeight = Resource.Resources[ResourceType.Food].amount * 0.01f;
        float stoneWeight = Resource.Resources[ResourceType.Stone].amount * 0.1f;
        float lumberWeight = Resource.Resources[ResourceType.Lumber].amount * 0.01f;
        uint workerWeight = Worker.TotalWorkerCount * 10;


        lifetimeCurrency = foodWeight + stoneWeight + lumberWeight + workerWeight;
        lifetimeCurrency *= 100;
        if (lifetimeCurrency > highestLifetimeCurrency)
        {
            prestigeCurrency = 500000 * Mathf.Sqrt(lifetimeCurrency / Mathf.Pow(10, 15));
            highestLifetimeCurrency = lifetimeCurrency;
        }
        else
        {
            prestigeCurrency = 500000 * Mathf.Sqrt(highestLifetimeCurrency / Mathf.Pow(10, 15));
        }


        

        
    }
    
}
