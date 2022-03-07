using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PermanentStats : MonoBehaviour
{
    public static float allWorkerMultiplierAmount;

    public static Dictionary<CraftingType, float> craftableCostReduced = new Dictionary<CraftingType, float>();
    public static Dictionary<ResearchType, float> researchableCostReduced = new Dictionary<ResearchType, float>();
    public static Dictionary<BuildingType, float> buildingCostReduced = new Dictionary<BuildingType, float>();

    public static Dictionary<WorkerType, float> workerMultiplierModified = new Dictionary<WorkerType, float>();
    public static Dictionary<BuildingType, float> buildingMultiplierModified = new Dictionary<BuildingType, float>();

    public static Dictionary<BuildingType, uint> buildingSelfCountModified = new Dictionary<BuildingType, uint>();

    public static uint workerCountModified;
    public static float researchTimeReductionAmount;
    public static float storagePercentageAmount;

    [Button]
    public void DisplayStatList()
    {
        //foreach (var item in Resource.Resources)
        //{
        //    if (item.Value.initialAmount > 0)
        //    {
        //        Debug.Log("Start each run with " + item.Value.initialAmount + item.Key);
        //    }
        //}
        foreach (var item in workerMultiplierModified)
        {
            Debug.Log(string.Format("{0} is increased by {1:0.00}/sec", item.Key, item.Value));
        }

        foreach (var item in buildingMultiplierModified)
        {
            Debug.Log(string.Format("{0} is increased by {1:0.00}/sec", item.Key, item.Value));
        }

        foreach (var item in craftableCostReduced)
        {
            Debug.Log(string.Format("Crafting Recipe, {0}'s cost is reduced by: {1}%", item.Key, item.Value * 100));
        }

        foreach (var item in researchableCostReduced)
        {
            Debug.Log(string.Format("Research Recipe, {0}'s cost is reduced by: {1}%", item.Key, item.Value * 100));
        }

        Debug.Log(string.Format("Research time is reduced by: {0}%", researchTimeReductionAmount * 100));
        Debug.Log(string.Format("Storage limit is increase by: {0}%", storagePercentageAmount * 100));

        foreach (var item in Building.Buildings)
        {
            if (item.Value.initialSelfCount > 0)
            {
                Debug.Log(string.Format("Start each run with an additional {0} {1}'s, when you unlock it", item.Value.initialSelfCount, item.Value.actualName));
                // And then once I create a class for the prestige stats, make it so that it only says:
                // "Start NEXT run with that amount of buildings, since it will change again after the next reset.
            }         
        }

        if (workerCountModified > 1)
        {
            Debug.Log(string.Format("Start each run with {0} additional workers", workerCountModified));
        }
        else
        {
            Debug.Log(string.Format("Start each run with an additional worker"));
        }
        
    }
}
