using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrestigeCache : BoxCache
{
    public static float prestigeBoxAllWorkerMultiplierAddition;
    public static float prestigeBoxAllBuildingMultiplierAddition;

    public static float prestigeBoxAllCraftablesCostSubtraction;
    public static float prestigeBoxAllResearchablesCostSubtraction;

    public static Dictionary<CraftingType, float> prestigeBoxCraftableCostSubtraction = new();
    public static Dictionary<ResearchType, float> prestigeBoxResearchableCostSubtraction = new();
    public static Dictionary<BuildingType, float> prestigeBoxBuildingCostSubtraction = new();

    public static Dictionary<WorkerType, float> prestigeBoxWorkerMultiplierAddition = new();
    public static Dictionary<BuildingType, float> prestigeBoxBuildingMultiplierAddition = new();

    public static Dictionary<BuildingType, uint> prestigeBoxBuildingCountAddition = new();

    public static uint prestigeBoxWorkerCountAddition;
    public static float prestigeBoxResearchTimeSubtraction;
    public static float prestigeBoxStorageAddition;

    //protected void DeductResearchTime(float researchTimeSubtraction)
    //{
    //    if (researchTimeSubtraction > 0.00f)
    //    {
    //        foreach (var research in Researchable.Researchables)
    //        {
    //            research.Value.prestigeTimeSubtraction -= researchTimeSubtraction;
    //        }
    //    }
    //}
    //protected void DeductStorageLimit(float storageAddition)
    //{
    //    if (storageAddition > 0.00f)
    //    {
    //        StoragePile.prestigeStorageAddition -= storageAddition;
    //    }
    //}
    //protected void DeductWorkerCount(uint workerCountAddition)
    //{
    //    if (workerCountAddition > 0.00f)
    //    {
    //        Worker.prestigeCountAddition += workerCountAddition;

    //        PrestigeStats.prestigeWorkerCountAddition -= workerCountAddition;
    //    }
    //}
    //protected void DeductAllBuildingMultiplier(float allBuildingMultiplierAddition)
    //{
    //    if (allBuildingMultiplierAddition > 0.00f)
    //    {
    //        foreach (var building in Building.Buildings)
    //        {
    //            Building.Buildings[building.Key].prestigeAllMultiplierAddition -= allBuildingMultiplierAddition;
    //        }
    //    }
    //}
    //protected void DeductAllWorkerMultiplier(float allWorkerMultiplierAmountAddition)
    //{
    //    if (allWorkerMultiplierAmountAddition > 0.00f)
    //    {
    //        foreach (var worker in Worker.Workers)
    //        {
    //            Worker.Workers[worker.Key].prestigeAllMultiplierAddition -= allWorkerMultiplierAmountAddition;
    //        }
    //    }
    //}
    //protected void DeductAllResearchablesCost(float allResearchCostSubtraction)
    //{
    //    if (allResearchCostSubtraction > 0.00f)
    //    {
    //        foreach (var researchable in Researchable.Researchables)
    //        {
    //            Researchable.Researchables[researchable.Key].prestigeCostSubtraction -= allResearchCostSubtraction;
    //        }
    //    }
    //}
    //protected void DeductAllCraftableCost(float allCraftableCostSubtraction)
    //{
    //    if (allCraftableCostSubtraction > 0.00f)
    //    {
    //        foreach (var craftable in Craftable.Craftables)
    //        {
    //            Craftable.Craftables[craftable.Key].prestigeCostSubtraction -= allCraftableCostSubtraction;
    //        }
    //    }
    //}

    protected virtual void ModifyBuildingCount(Dictionary<BuildingType, uint> buildingCountAddition, Dictionary<BuildingType, uint> statsValue)
    {
        if (buildingCountAddition.Count > 0.00f)
        {
            foreach (var building in buildingCountAddition)
            {
                //Debug.Log(string.Format("Modified building {0}'s count from {1} to {2}", Building.Buildings[building.Key].ActualName, Building.Buildings[building.Key].countAddition, building.Value));

                Building.Buildings[building.Key].prestigeCountAddition += building.Value;

                if (!statsValue.ContainsKey(building.Key))
                {
                    statsValue.Add(building.Key, building.Value);
                }
                else
                {
                    statsValue[building.Key] += building.Value;
                }
            }
        }
    }
    protected virtual void ModifyBuildingMultiplier(Dictionary<BuildingType, float> buildingMultiplierAddition, Dictionary<BuildingType, float> statsValue)
    {
        if (buildingMultiplierAddition.Count > 0.00f)
        {
            foreach (var building in buildingMultiplierAddition)
            {
                //Debug.Log(string.Format("Modified building {0}'s multi from {1} to {2}", Building.Buildings[building.Key].ActualName, Building.Buildings[building.Key].multiplierAddition, building.Value));
                Building.Buildings[building.Key].prestigeMultiplierAddition += building.Value;


                if (!statsValue.ContainsKey(building.Key))
                {
                    statsValue.Add(building.Key, building.Value);
                }
                else
                {
                    statsValue[building.Key] += building.Value;
                }
            }
        }
    }
    protected virtual void ModifyWorkerMultiplier(Dictionary<WorkerType, float> workerMultiplierAddition, Dictionary<WorkerType, float> statsValue)
    {
        if (workerMultiplierAddition.Count > 0.00f)
        {
            foreach (var worker in workerMultiplierAddition)
            {
                //Debug.Log(string.Format("Modified worker {0}'s multi from {1} to {2}", Worker.Workers[worker.Key].ActualName, Worker.Workers[worker.Key].multiplierAddition, worker.Value));
                Worker.Workers[worker.Key].prestigeMultiplierAddition += worker.Value;

                if (!statsValue.ContainsKey(worker.Key))
                {
                    statsValue.Add(worker.Key, worker.Value);
                }
                else
                {
                    statsValue[worker.Key] += worker.Value;
                }
            }
        }
    }
    protected virtual void ModifyBuildingCost(Dictionary<BuildingType, float> buildingCostSubtraction, Dictionary<BuildingType, float> statsValue)
    {
        if (buildingCostSubtraction.Count > 0.00f)
        {
            foreach (var building in buildingCostSubtraction)
            {
                //Debug.Log(string.Format("Modified building {0}'s cost from {1} to {2}", Building.Buildings[building.Key].ActualName, Building.Buildings[building.Key].costSubtraction, building.Value));
                Building.Buildings[building.Key].prestigeCostSubtraction += building.Value;

                if (!statsValue.ContainsKey(building.Key))
                {
                    statsValue.Add(building.Key, building.Value);
                }
                else
                {
                    statsValue[building.Key] += building.Value;
                }
            }
        }
    }
    protected virtual void ModifyCraftableCost(Dictionary<CraftingType, float> craftableCostSubtraction, Dictionary<CraftingType, float> statsValue)
    {
        if (craftableCostSubtraction.Count > 0.00f)
        {
            foreach (var craftable in craftableCostSubtraction)
            {
                //Debug.Log(string.Format("Modified research {0}'s cost from {1} to {2}", Craftable.Craftables[craftable.Key].ActualName, Craftable.Craftables[craftable.Key].costSubtraction, craftable.Value));
                Craftable.Craftables[craftable.Key].prestigeCostSubtraction += craftable.Value;

                if (!statsValue.ContainsKey(craftable.Key))
                {
                    statsValue.Add(craftable.Key, craftable.Value);
                }
                else
                {
                    statsValue[craftable.Key] += craftable.Value;
                }
            }
        }
    }
    protected virtual void ModifyResearchableCost(Dictionary<ResearchType, float> researchableCostSubtraction, Dictionary<ResearchType, float> statsValue)
    {
        if (researchableCostSubtraction.Count > 0.00f)
        {
            foreach (var researchable in researchableCostSubtraction)
            {
                //Debug.Log(string.Format("Modified research {0}'s cost from {1} to {2}", Researchable.Researchables[researchable.Key].ActualName, Researchable.Researchables[researchable.Key].costSubtraction, researchable.Value));
                Researchable.Researchables[researchable.Key].prestigeCostSubtraction += researchable.Value;

                if (!statsValue.ContainsKey(researchable.Key))
                {
                    statsValue.Add(researchable.Key, researchable.Value);
                }
                else
                {
                    statsValue[researchable.Key] += researchable.Value;
                }
            }
        }
    }

    protected void ModifyResearchTime(float researchTimeSubtraction)
    {
        if (researchTimeSubtraction > 0.00f)
        {
            foreach (var research in Researchable.Researchables)
            {
                research.Value.prestigeTimeSubtraction += researchTimeSubtraction;
            }

            Debug.Log(string.Format("Modified research time from {0} to {1}", PrestigeStats.prestigeResearchTimeSubtraction, researchTimeSubtraction));
            PrestigeStats.prestigeResearchTimeSubtraction += researchTimeSubtraction;
        }
    }
    protected void ModifyStorageLimit(float storageAddition)
    {
        if (storageAddition > 0.00f)
        {
            //Debug.Log(string.Format("Modified storage from {0} to {1}", StoragePile.prestigeStorageAddition, storageAddition));
            //StoragePile.prestigeStorageAddition += storageAddition;

            PrestigeStats.prestigeStorageAddition += storageAddition;
        }
    }
    protected void ModifyWorkerCount(uint workerCountAddition)
    {
        if (workerCountAddition > 0.00f)
        {
            Worker.prestigeCountAddition += workerCountAddition;

            PrestigeStats.prestigeWorkerCountAddition += workerCountAddition;
        }
    }
    protected void ModifyAllBuildingMultiplier(float allBuildingMultiplierAddition)
    {
        if (allBuildingMultiplierAddition > 0.00f)
        {
            foreach (var building in Building.Buildings)
            {
                Building.Buildings[building.Key].prestigeAllMultiplierAddition += allBuildingMultiplierAddition;
            }

            Debug.Log(string.Format("Modified all worker multi from {0} to {1}", PrestigeStats.prestigeAllBuildingMultiplierAddition, allBuildingMultiplierAddition));
            PrestigeStats.prestigeAllBuildingMultiplierAddition += allBuildingMultiplierAddition;
        }
    }
    protected void ModifyAllWorkerMultiplier(float allWorkerMultiplierAmountAddition)
    {
        if (allWorkerMultiplierAmountAddition > 0.00f)
        {
            foreach (var worker in Worker.Workers)
            {
                Worker.Workers[worker.Key].prestigeAllMultiplierAddition += allWorkerMultiplierAmountAddition;
            }

            Debug.Log(string.Format("Modified all worker multi from {0} to {1}", PrestigeStats.prestigeAllWorkerMultiplierAddition, allWorkerMultiplierAmountAddition));
            PrestigeStats.prestigeAllWorkerMultiplierAddition += allWorkerMultiplierAmountAddition;
        }
    }
    protected void ModifyAllResearchablesCost(float allResearchCostSubtraction)
    {
        if (allResearchCostSubtraction > 0.00f)
        {
            foreach (var researchable in Researchable.Researchables)
            {
                Researchable.Researchables[researchable.Key].prestigeCostSubtraction += allResearchCostSubtraction;
            }
            Debug.Log(string.Format("Modified all research cost from {0} to {1}", PrestigeStats.prestigeAllResearchablesCostSubtraction, allResearchCostSubtraction));
            PrestigeStats.prestigeAllResearchablesCostSubtraction += allResearchCostSubtraction;
        }
    }
    protected void ModifyAllCraftableCost(float allCraftableCostSubtraction)
    {
        if (allCraftableCostSubtraction > 0.00f)
        {
            foreach (var craftable in Craftable.Craftables)
            {
                Craftable.Craftables[craftable.Key].prestigeCostSubtraction += allCraftableCostSubtraction;
            }
            PrestigeStats.prestigeAllCraftablesCostSubtraction += allCraftableCostSubtraction;
        }
    }
    public void DeductValues()
    {
        //DeductResearchTime(prestigeBoxResearchTimeSubtraction);
        //DeductWorkerMultiplier(prestigeBoxWorkerMultiplierAddition, PrestigeStats.prestigeWorkerMultiplierAddition);
        //DeductBuildingCount(prestigeBoxBuildingCountAddition, PrestigeStats.prestigeBuildingCountAddition);
        //DeductWorkerCount(prestigeBoxWorkerCountAddition);
        //DeductBuildingMultiplier(prestigeBoxBuildingMultiplierAddition, PrestigeStats.prestigeBuildingMultiplierAddition);
        //DeductCraftableCost(prestigeBoxCraftableCostSubtraction, PrestigeStats.prestigeCraftableCostSubtraction);
        //DeductResearchableCost(prestigeBoxResearchableCostSubtraction, PrestigeStats.prestigeResearchableCostSubtraction);
        //DeductStorageLimit(prestigeBoxStorageAddition);
        //DeductAllResearchablesCost(prestigeBoxAllResearchablesCostSubtraction);
        //DeductAllCraftableCost(prestigeBoxAllCraftablesCostSubtraction);
        //DeductAllBuildingMultiplier(prestigeBoxAllBuildingMultiplierAddition);
        //DeductBuildingCost(prestigeBoxBuildingCostSubtraction, PrestigeStats.prestigeBuildingCostSubtraction);
        //DeductAllWorkerMultiplier(prestigeBoxAllWorkerMultiplierAddition);

        ClearBoxCache();

        PrestigeStats.prestigeAllWorkerMultiplierAddition = 0;
        PrestigeStats.prestigeAllBuildingMultiplierAddition = 0;
        PrestigeStats.prestigeAllCraftablesCostSubtraction = 0;
        PrestigeStats.prestigeAllResearchablesCostSubtraction = 0;
        PrestigeStats.prestigeWorkerCountAddition = 0;
        PrestigeStats.prestigeResearchTimeSubtraction = 0;
        PrestigeStats.prestigeStorageAddition = 0;
        PrestigeStats.prestigeCraftableCostSubtraction.Clear();
        PrestigeStats.prestigeResearchableCostSubtraction.Clear();
        PrestigeStats.prestigeBuildingCostSubtraction.Clear();
        PrestigeStats.prestigeWorkerMultiplierAddition.Clear();
        PrestigeStats.prestigeBuildingMultiplierAddition.Clear();
        PrestigeStats.prestigeBuildingCountAddition.Clear();

        // Here you should also make prestige null.
    }
    public void OnDoneButton()
    {
        ModifyResearchTime(prestigeBoxResearchTimeSubtraction);
        ModifyWorkerMultiplier(prestigeBoxWorkerMultiplierAddition, PrestigeStats.prestigeWorkerMultiplierAddition);
        ModifyBuildingCount(prestigeBoxBuildingCountAddition, PrestigeStats.prestigeBuildingCountAddition);
        ModifyWorkerCount(prestigeBoxWorkerCountAddition);
        ModifyBuildingMultiplier(prestigeBoxBuildingMultiplierAddition, PrestigeStats.prestigeBuildingMultiplierAddition);
        ModifyCraftableCost(prestigeBoxCraftableCostSubtraction, PrestigeStats.prestigeCraftableCostSubtraction);
        ModifyResearchableCost(prestigeBoxResearchableCostSubtraction, PrestigeStats.prestigeResearchableCostSubtraction);
        ModifyStorageLimit(prestigeBoxStorageAddition);
        ModifyAllResearchablesCost(prestigeBoxAllResearchablesCostSubtraction);
        ModifyAllCraftableCost(prestigeBoxAllCraftablesCostSubtraction);
        ModifyAllBuildingMultiplier(prestigeBoxAllBuildingMultiplierAddition);
        ModifyBuildingCost(prestigeBoxBuildingCostSubtraction, PrestigeStats.prestigeBuildingCostSubtraction);
        ModifyAllWorkerMultiplier(prestigeBoxAllWorkerMultiplierAddition);
    }
    public void ClearBoxCache()
    {
        prestigeBoxAllWorkerMultiplierAddition = 0;
        prestigeBoxAllBuildingMultiplierAddition = 0;
        prestigeBoxAllCraftablesCostSubtraction = 0;
        prestigeBoxAllResearchablesCostSubtraction = 0;
        prestigeBoxWorkerCountAddition = 0;
        prestigeBoxResearchTimeSubtraction = 0;
        prestigeBoxStorageAddition = 0;

        prestigeBoxCraftableCostSubtraction.Clear();
        prestigeBoxResearchableCostSubtraction.Clear();
        prestigeBoxBuildingCostSubtraction.Clear();
        prestigeBoxWorkerMultiplierAddition.Clear();
        prestigeBoxBuildingMultiplierAddition.Clear();
        prestigeBoxBuildingCountAddition.Clear();
    }
}
