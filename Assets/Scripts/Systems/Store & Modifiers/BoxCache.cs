using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCache : MonoBehaviour
{
    //protected virtual void DeductBuildingCount(Dictionary<BuildingType, uint> buildingCountAddition, Dictionary<BuildingType, uint> statsValue)
    //{
    //    if (buildingCountAddition.Count > 0.00f)
    //    {
    //        foreach (var building in buildingCountAddition)
    //        {         
    //            Building.Buildings[building.Key].countAddition -= building.Value;
    //        }
    //    }
    //}
    //protected virtual void DeductBuildingMultiplier(Dictionary<BuildingType, float> buildingMultiplierAddition, Dictionary<BuildingType, float> statsValue)
    //{
    //    if (buildingMultiplierAddition.Count > 0.00f)
    //    {
    //        foreach (var building in buildingMultiplierAddition)
    //        {            
    //            Building.Buildings[building.Key].multiplierAddition -= building.Value;
    //        }
    //    }
    //}
    //protected virtual void DeductWorkerMultiplier(Dictionary<WorkerType, float> workerMultiplierAddition, Dictionary<WorkerType, float> statsValue)
    //{
    //    if (workerMultiplierAddition.Count > 0.00f)
    //    {
    //        foreach (var worker in workerMultiplierAddition)
    //        {           
    //            Worker.Workers[worker.Key].multiplierAddition -= worker.Value;
    //        }
    //    }
    //}
    //protected virtual void DeductBuildingCost(Dictionary<BuildingType, float> buildingCostSubtraction, Dictionary<BuildingType, float> statsValue)
    //{
    //    if (buildingCostSubtraction.Count > 0.00f)
    //    {
    //        foreach (var building in buildingCostSubtraction)
    //        {             
    //            Building.Buildings[building.Key].costSubtraction -= building.Value;
    //        }
    //    }
    //}
    //protected virtual void DeductCraftableCost(Dictionary<CraftingType, float> craftableCostSubtraction, Dictionary<CraftingType, float> statsValue)
    //{
    //    if (craftableCostSubtraction.Count > 0.00f)
    //    {
    //        foreach (var craftable in craftableCostSubtraction)
    //        {              
    //            Craftable.Craftables[craftable.Key].costSubtraction -= craftable.Value;
    //        }
    //    }
    //}
    //protected virtual void DeductResearchableCost(Dictionary<ResearchType, float> researchableCostSubtraction, Dictionary<ResearchType, float> statsValue)
    //{
    //    if (researchableCostSubtraction.Count > 0.00f)
    //    {
    //        foreach (var researchable in researchableCostSubtraction)
    //        {              
    //            Researchable.Researchables[researchable.Key].costSubtraction -= researchable.Value;
    //        }
    //    }
    //}
    //protected virtual void ModifyBuildingCount(Dictionary<BuildingType, uint> buildingCountAddition, Dictionary<BuildingType, uint> statsValue)
    //{
    //    if (buildingCountAddition.Count > 0.00f)
    //    {
    //        foreach (var building in buildingCountAddition)
    //        {
    //            //Debug.Log(string.Format("Modified building {0}'s count from {1} to {2}", Building.Buildings[building.Key].ActualName, Building.Buildings[building.Key].countAddition, building.Value));
    //            Building.Buildings[building.Key].permCountAddition += building.Value;
    //            Building.Buildings[building.Key].prestigeCountAddition += building.Value;

    //            if (!statsValue.ContainsKey(building.Key))
    //            {
    //                statsValue.Add(building.Key, building.Value);
    //            }
    //            else
    //            {
    //                statsValue[building.Key] += building.Value;
    //            }
    //        }
    //    }
    //}
    //protected virtual void ModifyBuildingMultiplier(Dictionary<BuildingType, float> buildingMultiplierAddition, Dictionary<BuildingType, float> statsValue)
    //{
    //    if (buildingMultiplierAddition.Count > 0.00f)
    //    {
    //        foreach (var building in buildingMultiplierAddition)
    //        {
    //            //Debug.Log(string.Format("Modified building {0}'s multi from {1} to {2}", Building.Buildings[building.Key].ActualName, Building.Buildings[building.Key].multiplierAddition, building.Value));
    //            Building.Buildings[building.Key].prestigeMultiplierAddition += building.Value;
    //            Building.Buildings[building.Key].prestigeMultiplierAddition += building.Value;

    //            if (!statsValue.ContainsKey(building.Key))
    //            {
    //                statsValue.Add(building.Key, building.Value);
    //            }
    //            else
    //            {
    //                statsValue[building.Key] += building.Value;
    //            }
    //        }
    //    }
    //}
    //protected virtual void ModifyWorkerMultiplier(Dictionary<WorkerType, float> workerMultiplierAddition, Dictionary<WorkerType, float> statsValue)
    //{
    //    if (workerMultiplierAddition.Count > 0.00f)
    //    {
    //        foreach (var worker in workerMultiplierAddition)
    //        {
    //            //Debug.Log(string.Format("Modified worker {0}'s multi from {1} to {2}", Worker.Workers[worker.Key].ActualName, Worker.Workers[worker.Key].multiplierAddition, worker.Value));
    //            Worker.Workers[worker.Key].prestigeMultiplierAddition += worker.Value;
    //            Worker.Workers[worker.Key].permMultiplierAddition += worker.Value;

    //            if (!statsValue.ContainsKey(worker.Key))
    //            {
    //                statsValue.Add(worker.Key, worker.Value);
    //            }
    //            else
    //            {
    //                statsValue[worker.Key] += worker.Value;
    //            }
    //        }
    //    }
    //}
    //protected virtual void ModifyBuildingCost(Dictionary<BuildingType, float> buildingCostSubtraction, Dictionary<BuildingType, float> statsValue)
    //{
    //    if (buildingCostSubtraction.Count > 0.00f)
    //    {
    //        foreach (var building in buildingCostSubtraction)
    //        {
    //            //Debug.Log(string.Format("Modified building {0}'s cost from {1} to {2}", Building.Buildings[building.Key].ActualName, Building.Buildings[building.Key].costSubtraction, building.Value));
    //            Building.Buildings[building.Key].prestigeCostSubtraction += building.Value;
    //            Building.Buildings[building.Key].permCostSubtraction += building.Value;

    //            if (!statsValue.ContainsKey(building.Key))
    //            {
    //                statsValue.Add(building.Key, building.Value);
    //            }
    //            else
    //            {
    //                statsValue[building.Key] += building.Value;
    //            }
    //        }
    //    }
    //}
    //protected virtual void ModifyCraftableCost(Dictionary<CraftingType, float> craftableCostSubtraction, Dictionary<CraftingType, float> statsValue)
    //{
    //    if (craftableCostSubtraction.Count > 0.00f)
    //    {
    //        foreach (var craftable in craftableCostSubtraction)
    //        {
    //            //Debug.Log(string.Format("Modified research {0}'s cost from {1} to {2}", Craftable.Craftables[craftable.Key].ActualName, Craftable.Craftables[craftable.Key].costSubtraction, craftable.Value));
    //            Craftable.Craftables[craftable.Key].prestigeCostSubtraction += craftable.Value;
    //            Craftable.Craftables[craftable.Key].permCostSubtraction += craftable.Value;

    //            if (!statsValue.ContainsKey(craftable.Key))
    //            {
    //                statsValue.Add(craftable.Key, craftable.Value);
    //            }
    //            else
    //            {
    //                statsValue[craftable.Key] += craftable.Value;
    //            }
    //        }
    //    }
    //}
    //protected virtual void ModifyResearchableCost(Dictionary<ResearchType, float> researchableCostSubtraction, Dictionary<ResearchType, float> statsValue)
    //{
    //    if (researchableCostSubtraction.Count > 0.00f)
    //    {
    //        foreach (var researchable in researchableCostSubtraction)
    //        {
    //            //Debug.Log(string.Format("Modified research {0}'s cost from {1} to {2}", Researchable.Researchables[researchable.Key].ActualName, Researchable.Researchables[researchable.Key].costSubtraction, researchable.Value));
    //            Researchable.Researchables[researchable.Key].prestigeCostSubtraction += researchable.Value;
    //            Researchable.Researchables[researchable.Key].permCostSubtraction += researchable.Value;



    //            if (!statsValue.ContainsKey(researchable.Key))
    //            {
    //                statsValue.Add(researchable.Key, researchable.Value);
    //            }
    //            else
    //            {
    //                statsValue[researchable.Key] += researchable.Value;
    //            }
    //        }
    //    }
    //}
}
