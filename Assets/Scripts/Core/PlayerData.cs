using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataPermanent
{ 
    public Dictionary<CraftingType, float> nextCraftableCostSubtraction;
    public Dictionary<ResearchType, float> nextResearchableCostSubtraction;
    public Dictionary<BuildingType, float> nextBuildingCostSubtraction;
    public Dictionary<WorkerType, float> nextWorkerMultiplierAddition;
    public Dictionary<BuildingType, float> nextBuildingMultiplierAddition;
    public Dictionary<BuildingType, uint> nextBuildingCountAddition;

    public Dictionary<CraftingType, float> activeCraftableCostSubtraction;
    public Dictionary<ResearchType, float> activeResearchableCostSubtraction;
    public Dictionary<BuildingType, float> activeBuildingCostSubtraction;
    public Dictionary<WorkerType, float> activeWorkerMultiplierAddition;
    public Dictionary<BuildingType, float> activeBuildingMultiplierAddition;
    public Dictionary<BuildingType, uint> activeBuildingCountAddition;

    public PlayerDataPermanent()
    {
        nextCraftableCostSubtraction = PermanentStats.nextCraftableCostSubtraction;
        nextResearchableCostSubtraction = PermanentStats.nextResearchableCostSubtraction;
        nextBuildingCostSubtraction = PermanentStats.nextBuildingCostSubtraction;
        nextWorkerMultiplierAddition = PermanentStats.nextWorkerMultiplierAddition;
        nextBuildingMultiplierAddition = PermanentStats.nextBuildingMultiplierAddition;
        nextBuildingCountAddition = PermanentStats.nextBuildingCountAddition;

        activeCraftableCostSubtraction = PermanentStats.activeCraftableCostSubtraction;
        activeResearchableCostSubtraction = PermanentStats.activeResearchableCostSubtraction;
        activeBuildingCostSubtraction = PermanentStats.activeBuildingCostSubtraction;
        activeWorkerMultiplierAddition = PermanentStats.activeWorkerMultiplierAddition;
        activeBuildingMultiplierAddition = PermanentStats.activeBuildingMultiplierAddition;
        activeBuildingCountAddition = PermanentStats.activeBuildingCountAddition;
    }
}
[System.Serializable]
public class PlayerDataPrestige
{
    public Dictionary<CraftingType, float> craftableCostSubtraction;
    public Dictionary<ResearchType, float> researchableCostSubtraction;
    public Dictionary<BuildingType, float> buildingCostSubtraction;

    public Dictionary<WorkerType, float> workerMultiplierAddition;
    public Dictionary<BuildingType, float> buildingMultiplierAddition;

    public Dictionary<BuildingType, uint> buildingCountAddition;

    public PlayerDataPrestige(PrestigeStats prestigeStats)
    {
        craftableCostSubtraction = PrestigeStats.prestigeCraftableCostSubtraction;
        researchableCostSubtraction = PrestigeStats.prestigeResearchableCostSubtraction;
        buildingCostSubtraction = PrestigeStats.prestigeBuildingCostSubtraction;
        workerMultiplierAddition = PrestigeStats.prestigeWorkerMultiplierAddition;
        buildingMultiplierAddition = PrestigeStats.prestigeBuildingMultiplierAddition;
        buildingCountAddition = PrestigeStats.prestigeBuildingCountAddition;
    }
}
[System.Serializable]
public class PlayerDataPermanentCache
{
    public Dictionary<CraftingType, float> craftableCostSubtraction;
    public Dictionary<ResearchType, float> researchableCostSubtraction;
    public Dictionary<BuildingType, float> buildingCostSubtraction;

    public Dictionary<WorkerType, float> workerMultiplierAddition;
    public Dictionary<BuildingType, float> buildingMultiplierAddition;

    public Dictionary<BuildingType, uint> buildingCountAddition;

    public PlayerDataPermanentCache(PermanentCache permanentCache)
    {
        craftableCostSubtraction = PermanentCache.permanentBoxCraftableCostSubtraction;
        researchableCostSubtraction = PermanentCache.permanentBoxResearchableCostSubtraction;
        buildingCostSubtraction = PermanentCache.permanentBoxBuildingCostSubtraction;
        workerMultiplierAddition = PermanentCache.permanentBoxWorkerMultiplierAddition;
        buildingMultiplierAddition = PermanentCache.permanentBoxBuildingMultiplierAddition;
        buildingCountAddition = PermanentCache.permanentBoxBuildingCountAddition;
    }
}