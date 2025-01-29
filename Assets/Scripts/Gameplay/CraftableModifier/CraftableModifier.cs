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

public class CraftableModifier : Craftable
{
    public List<BuildingToMultiply> buildingToMultiply;
    public List<WorkerToMultiply> workerToMultiply;

    public BuildingToDeriveCostAmountFrom buildingToDeriveCostAmountFrom;
    private float newCostAmount;

    public string description;

    public override void OnCraft()
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
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
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
        foreach (var ResourceCost in Building.Buildings[buildingToDeriveCostAmountFrom.buildingType].ResourceCost)
        {
            newCostAmount += ResourceCost.BaseCostAmount * Mathf.Pow(Building.Buildings[buildingToDeriveCostAmountFrom.buildingType].costMultiplier, buildingToDeriveCostAmountFrom.selfCountAmount);
        }

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            ResourceCost[i].CostAmount = newCostAmount;
        }
    }
    protected void InitializeDescriptionText()
    {
        if (workerToMultiply.Count != 0 && buildingToMultiply.Count != 0)
        {
            foreach (var worker in workerToMultiply)
            {
                description += string.Format("Multiplies {0}'s efficiency by {1}", Worker.Workers[worker.workerType].ActualName, worker.multiplier);
                //SetDescriptionText(string.Format("Multiplies {0}'s efficiency by {1}", Worker.Workers[worker.workerType].ActualName, worker.multiplier));
            }
            foreach (var building in buildingToMultiply)
            {
                description += string.Format("\nMultiplies {0}'s production by {1}", Building.Buildings[building.buildingType].ActualName, building.multiplier);
                //SetDescriptionText(string.Format("Multiplies {0}'s production by {1}", Building.Buildings[building.buildingType].ActualName, building.multiplier));
            }
            SetDescriptionText(description);
        }
        else if (workerToMultiply.Count != 0)
        {
            foreach (var worker in workerToMultiply)
            {
                SetDescriptionText(string.Format("Multiplies {0}'s efficiency by {1}", Worker.Workers[worker.workerType].ActualName, worker.multiplier));
            }
        }
        else if (buildingToMultiply.Count != 0)
        {
            foreach (var building in buildingToMultiply)
            {
                SetDescriptionText(string.Format("Multiplies {0}'s production by {1}", Building.Buildings[building.buildingType].ActualName, building.multiplier));
            }
        }

        //if (workerToMultiply.Count != 0)
        //{
        //    foreach (var worker in workerToMultiply)
        //    {
        //        SetDescriptionText(string.Format("Multiplies {0}'s effiency by {1}", Worker.Workers[worker.workerType].ActualName, worker.multiplier));
        //    }
        //}
        //else if (buildingToMultiply.Count != 0)
        //{
        //    foreach (var building in buildingToMultiply)
        //    {
        //        SetDescriptionText(string.Format("Multiplies {0}'s production by {1}", Building.Buildings[building.buildingType].ActualName, building.multiplier));
        //    }
        //}
    }
}
