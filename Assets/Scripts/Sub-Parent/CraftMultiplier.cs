using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuildingToMultiply
{
    public BuildingType buildingType;
    public float multiplier;
    //public float selfCountAmount;
}

[System.Serializable]
public struct WorkerToMultiply
{
    public WorkerType workerType;
    public float multiplier;
    //public float selfCountAmount;
}

[System.Serializable]
public struct BuildingToDeriveCostAmountFrom
{
    public BuildingType buildingType;
    public uint selfCountAmount;
}

public class CraftMultiplier : Craftable
{
    public List<BuildingToMultiply> buildingToMultiply;
    public List<WorkerToMultiply> workerToMultiply;

    public BuildingToDeriveCostAmountFrom buildingToDeriveCostAmountFrom;
    private float newCostAmount;

    public string description;

    public override void OnCraft()
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
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            }
            isCrafted = true;
            Crafted();

            if (buildingToMultiply.Count != 0)
            {
                foreach (var building in buildingToMultiply)
                {
                    Building.Buildings[building.buildingType].MultiplyIncrementAmount(building.multiplier);
                }
            }

            if (workerToMultiply.Count != 0)
            {
                foreach (var worker in workerToMultiply)
                {
                    Worker.Workers[worker.workerType].MultiplyIncrementAmount(worker.multiplier, worker.workerType);
                }
            }
        }
    }
    protected void InitializeCostAmount()
    {
        foreach (var resourceCost in Building.Buildings[buildingToDeriveCostAmountFrom.buildingType].resourceCost)
        {
            newCostAmount += resourceCost.baseCostAmount * Mathf.Pow(Building.Buildings[buildingToDeriveCostAmountFrom.buildingType].costMultiplier, buildingToDeriveCostAmountFrom.selfCountAmount);
        }

        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].costAmount = newCostAmount;
        }
    }
    protected void InitializeDescriptionText()
    {
        if (workerToMultiply.Count != 0 && buildingToMultiply.Count != 0)
        {
            foreach (var worker in workerToMultiply)
            {
                description += string.Format("Multiplies {0}'s efficiency by {1}", Worker.Workers[worker.workerType].actualName, worker.multiplier);
                //SetDescriptionText(string.Format("Multiplies {0}'s efficiency by {1}", Worker.Workers[worker.workerType].actualName, worker.multiplier));
            }
            foreach (var building in buildingToMultiply)
            {
                description += string.Format("\nMultiplies {0}'s production by {1}", Building.Buildings[building.buildingType].actualName, building.multiplier);
                //SetDescriptionText(string.Format("Multiplies {0}'s production by {1}", Building.Buildings[building.buildingType].actualName, building.multiplier));
            }
            SetDescriptionText(description);
        }
        else if (workerToMultiply.Count != 0)
        {
            foreach (var worker in workerToMultiply)
            {
                SetDescriptionText(string.Format("Multiplies {0}'s efficiency by {1}", Worker.Workers[worker.workerType].actualName, worker.multiplier));
            }
        }
        else if (buildingToMultiply.Count != 0)
        {
            foreach (var building in buildingToMultiply)
            {
                SetDescriptionText(string.Format("Multiplies {0}'s production by {1}", Building.Buildings[building.buildingType].actualName, building.multiplier));
            }
        }

        //if (workerToMultiply.Count != 0)
        //{
        //    foreach (var worker in workerToMultiply)
        //    {
        //        SetDescriptionText(string.Format("Multiplies {0}'s effiency by {1}", Worker.Workers[worker.workerType].actualName, worker.multiplier));
        //    }
        //}
        //else if (buildingToMultiply.Count != 0)
        //{
        //    foreach (var building in buildingToMultiply)
        //    {
        //        SetDescriptionText(string.Format("Multiplies {0}'s production by {1}", Building.Buildings[building.buildingType].actualName, building.multiplier));
        //    }
        //}
    }
}
