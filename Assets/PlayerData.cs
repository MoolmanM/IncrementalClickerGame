using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{ 
    public Dictionary<CraftingType, float> craftableCostReduced;
    public Dictionary<ResearchType, float> researchableCostReduced;
    public Dictionary<BuildingType, float> buildingCostReduced;

    public Dictionary<WorkerType, float> workerMultiplierModified;
    public Dictionary<BuildingType, float> buildingMultiplierModified;

    public Dictionary<BuildingType, uint> buildingSelfCountModified;

    public PlayerData(PermanentStats permanentStats)
    {
        craftableCostReduced = permanentStats.craftableCostReduced;
        researchableCostReduced = permanentStats.researchableCostReduced;
        buildingCostReduced = permanentStats.buildingCostReduced;
        workerMultiplierModified = permanentStats.workerMultiplierModified;
        buildingMultiplierModified = permanentStats.buildingMultiplierModified;
        buildingSelfCountModified = permanentStats.buildingSelfCountModified;
    }
}
