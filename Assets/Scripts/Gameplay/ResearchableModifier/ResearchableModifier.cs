using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchableModifier : Researchable
{
    public List<BuildingToMultiply> buildingToMultiply;
    public List<WorkerToMultiply> workerToMultiply;

    private string description;

    protected override void Researched()
    {
        base.Researched();

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
    }
}
