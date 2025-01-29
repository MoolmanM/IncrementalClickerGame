using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PermanentCache : BoxCache
{
    public static float permanentBoxAllWorkerMultiplierAddition;
    public static float permanentBoxAllBuildingMultiplierAddition;

    public static float permanentBoxAllCraftablesCostSubtraction;
    public static float permanentBoxAllResearchablesCostSubtraction;

    public static Dictionary<CraftingType, float> permanentBoxCraftableCostSubtraction = new();
    public static Dictionary<ResearchType, float> permanentBoxResearchableCostSubtraction = new();
    public static Dictionary<BuildingType, float> permanentBoxBuildingCostSubtraction = new();

    public static Dictionary<WorkerType, float> permanentBoxWorkerMultiplierAddition = new();
    public static Dictionary<BuildingType, float> permanentBoxBuildingMultiplierAddition = new();

    public static Dictionary<BuildingType, uint> permanentBoxBuildingCountAddition = new();

    public static uint permanentBoxWorkerCountAddition;
    public static float permanentBoxResearchTimeSubtraction;
    public static float permanentBoxStorageAddition;

    public GameObject content;

    protected virtual void ModifyBuildingCount(Dictionary<BuildingType, uint> buildingCountAddition, Dictionary<BuildingType, uint> statsValue)
    {
        if (buildingCountAddition.Count > 0.00f)
        {
            foreach (var building in buildingCountAddition)
            {
                //Debug.Log(string.Format("Modified building {0}'s count from {1} to {2}", Building.Buildings[building.Key].ActualName, Building.Buildings[building.Key].countAddition, building.Value));
                Building.Buildings[building.Key].permCountAddition += building.Value;

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
                Building.Buildings[building.Key].permMultiplierAddition += building.Value;

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

                Worker.Workers[worker.Key].permMultiplierAddition += worker.Value;

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

                Building.Buildings[building.Key].permCostSubtraction += building.Value;

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

                Craftable.Craftables[craftable.Key].permCostSubtraction += craftable.Value;

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

                Researchable.Researchables[researchable.Key].permCostSubtraction += researchable.Value;



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
        if (researchTimeSubtraction > 0)
        {
            foreach (var research in Researchable.Researchables)
            {
                research.Value.permTimeSubtraction += researchTimeSubtraction;
            }

            Debug.Log(string.Format("Modified research time from {0} to {1}", PermanentStats.nextResearchTimeSubtraction, researchTimeSubtraction));
            PermanentStats.nextResearchTimeSubtraction += researchTimeSubtraction;
        }
    }
    protected void ModifyStorageLimit(float storageAddition)
    {
        if (storageAddition > 0)
        {
            //Debug.Log(string.Format("Modified storage from {0} to {1}", StoragePile.permStorageAddition, storageAddition));
            //StoragePile.permStorageAddition += storageAddition;

            PermanentStats.nextStorageAddition += storageAddition;
        }
    }

    protected void ModifyWorkerCount(uint workerCountAddition)
    {
        if (workerCountAddition > 0)
        {
            Worker.permCountAddition += workerCountAddition;

            PermanentStats.nextWorkerCountAddition += workerCountAddition;
        }
    }
    protected void ModifyAllBuildingMultiplier(float allBuildingMultiplierAddition)
    {
        if (allBuildingMultiplierAddition > 0)
        {
            foreach (var building in Building.Buildings)
            {
                Building.Buildings[building.Key].permAllMultiplierAddition += allBuildingMultiplierAddition;
            }

            Debug.Log(string.Format("Modified all worker multi from {0} to {1}", PermanentStats.nextAllBuildingMultiplierAddition, allBuildingMultiplierAddition));
            PermanentStats.nextAllBuildingMultiplierAddition += allBuildingMultiplierAddition;
        }
    }
    protected void ModifyAllWorkerMultiplier(float allWorkerMultiplierAmountAddition)
    {
        if (allWorkerMultiplierAmountAddition > 0)
        {
            foreach (var worker in Worker.Workers)
            {
                Worker.Workers[worker.Key].permAllMultiplierAddition += allWorkerMultiplierAmountAddition;
            }

            Debug.Log(string.Format("Modified all worker multi from {0} to {1}", PermanentStats.nextAllWorkerMultiplierAddition, allWorkerMultiplierAmountAddition));
            PermanentStats.nextAllWorkerMultiplierAddition += allWorkerMultiplierAmountAddition;
        }
    }
    protected void ModifyAllResearchablesCost(float allResearchCostSubtraction)
    {
        if (allResearchCostSubtraction > 0)
        {
            foreach (var researchable in Researchable.Researchables)
            {
                Researchable.Researchables[researchable.Key].permCostSubtraction += allResearchCostSubtraction;
            }
            Debug.Log(string.Format("Modified all research cost from {0} to {1}", PermanentStats.nextAllResearchablesCostSubtraction, allResearchCostSubtraction));
            PermanentStats.nextAllResearchablesCostSubtraction += allResearchCostSubtraction;
        }
    }
    protected void ModifyAllCraftableCost(float allCraftableCostSubtraction)
    {
        if (allCraftableCostSubtraction > 0)
        {
            foreach (var craftable in Craftable.Craftables)
            {
                Craftable.Craftables[craftable.Key].permCostSubtraction += allCraftableCostSubtraction;
            }
            PermanentStats.nextAllCraftablesCostSubtraction += allCraftableCostSubtraction;
        }
    }
    public void ApplyPermanentStats()
    {
        ModifyResearchTime(permanentBoxResearchTimeSubtraction);
        ModifyWorkerMultiplier(permanentBoxWorkerMultiplierAddition, PermanentStats.nextWorkerMultiplierAddition);
        ModifyBuildingCount(permanentBoxBuildingCountAddition, PermanentStats.nextBuildingCountAddition);
        ModifyWorkerCount(permanentBoxWorkerCountAddition);
        ModifyBuildingMultiplier(permanentBoxBuildingMultiplierAddition, PermanentStats.nextBuildingMultiplierAddition);
        ModifyCraftableCost(permanentBoxCraftableCostSubtraction, PermanentStats.nextCraftableCostSubtraction);
        ModifyResearchableCost(permanentBoxResearchableCostSubtraction, PermanentStats.nextResearchableCostSubtraction);
        ModifyStorageLimit(permanentBoxStorageAddition);
        ModifyAllResearchablesCost(permanentBoxAllResearchablesCostSubtraction);
        ModifyAllCraftableCost(permanentBoxAllCraftablesCostSubtraction);
        ModifyAllBuildingMultiplier(permanentBoxAllBuildingMultiplierAddition);
        ModifyBuildingCost(permanentBoxBuildingCostSubtraction, PermanentStats.nextBuildingCostSubtraction);
        ModifyAllWorkerMultiplier(permanentBoxAllWorkerMultiplierAddition);



        ClearBoxCache();
    }
    private void ClearBoxCache()
    {
        permanentBoxAllWorkerMultiplierAddition = 0;
        permanentBoxAllBuildingMultiplierAddition = 0;
        permanentBoxAllCraftablesCostSubtraction = 0;
        permanentBoxAllResearchablesCostSubtraction = 0;
        permanentBoxWorkerCountAddition = 0;
        permanentBoxResearchTimeSubtraction = 0;
        permanentBoxStorageAddition = 0;

        permanentBoxCraftableCostSubtraction.Clear();
        permanentBoxResearchableCostSubtraction.Clear();
        permanentBoxBuildingCostSubtraction.Clear();
        permanentBoxWorkerMultiplierAddition.Clear();
        permanentBoxBuildingMultiplierAddition.Clear();
        permanentBoxBuildingCountAddition.Clear();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("permanentBoxAllWorkerMultiplierAddition", permanentBoxAllWorkerMultiplierAddition);
        PlayerPrefs.SetFloat("permanentBoxAllBuildingMultiplierAddition", permanentBoxAllBuildingMultiplierAddition);
        PlayerPrefs.SetFloat("permanentBoxAllCraftablesCostSubtraction", permanentBoxAllCraftablesCostSubtraction);
        PlayerPrefs.SetFloat("permanentBoxAllResearchablesCostSubtraction", permanentBoxAllResearchablesCostSubtraction);
        PlayerPrefs.SetFloat("permanentBoxResearchTimeSubtraction", permanentBoxResearchTimeSubtraction);
        PlayerPrefs.SetFloat("permanentBoxStorageAddition", permanentBoxStorageAddition);
        PlayerPrefs.SetInt("permanentBoxWorkerCountAddition", (int)permanentBoxWorkerCountAddition);

        SaveSystem.SavePermanentCache(this);
    }
    private void Start()
    {
        permanentBoxAllWorkerMultiplierAddition = PlayerPrefs.GetFloat("permanentBoxAllWorkerMultiplierAddition", permanentBoxAllWorkerMultiplierAddition);
        permanentBoxAllBuildingMultiplierAddition = PlayerPrefs.GetFloat("permanentBoxAllBuildingMultiplierAddition", permanentBoxAllBuildingMultiplierAddition);
        permanentBoxAllCraftablesCostSubtraction = PlayerPrefs.GetFloat("permanentBoxAllCraftablesCostSubtraction", permanentBoxAllCraftablesCostSubtraction);
        permanentBoxAllResearchablesCostSubtraction = PlayerPrefs.GetFloat("permanentBoxAllResearchablesCostSubtraction", permanentBoxAllResearchablesCostSubtraction);
        permanentBoxResearchTimeSubtraction = PlayerPrefs.GetFloat("permanentBoxResearchTimeSubtraction", permanentBoxResearchTimeSubtraction);
        permanentBoxStorageAddition = PlayerPrefs.GetFloat("permanentBoxStorageAddition", permanentBoxStorageAddition);
        permanentBoxWorkerCountAddition = (uint)PlayerPrefs.GetInt("permanentBoxWorkerCountAddition", (int)permanentBoxWorkerCountAddition);

        PlayerDataPermanentCache data = SaveSystem.LoadPermanentCache();

        permanentBoxCraftableCostSubtraction = data.craftableCostSubtraction;
        permanentBoxResearchableCostSubtraction = data.researchableCostSubtraction;
        permanentBoxBuildingCostSubtraction = data.buildingCostSubtraction;
        permanentBoxWorkerMultiplierAddition = data.workerMultiplierAddition;
        permanentBoxBuildingMultiplierAddition = data.buildingMultiplierAddition;
        permanentBoxBuildingCountAddition = data.buildingCountAddition;
    }
}
