using System.Collections.Generic;
using UnityEngine;

public class BoxCacheDeprecated : MonoBehaviour
{
    /*
    public PermanentStats permanentStats;

    public static float cachedAllWorkerMultiplierAmount;
    public static float cachedAllBuildingMultiplierAmount;

    public static float cachedAllCraftablesCostReduced;
    public static float cachedAllResearchablesCostReduced;

    public static Dictionary<CraftingType, float> cachedCraftableCostReduced = new Dictionary<CraftingType, float>();
    public static Dictionary<ResearchType, float> cachedResearchableCostReduced = new Dictionary<ResearchType, float>();
    public static Dictionary<BuildingType, float> cachedBuildingCostReduced = new Dictionary<BuildingType, float>();

    public static Dictionary<WorkerType, float> cachedWorkerMultiplierModified = new Dictionary<WorkerType, float>();
    public static Dictionary<BuildingType, float> cachedBuildingMultiplierModified = new Dictionary<BuildingType, float>();

    public static Dictionary<BuildingType, uint> cachedBuildingSelfCountModified = new Dictionary<BuildingType, uint>();

    public static uint cachedWorkerCountModified;
    public static float cachedResearchTimeReductionAmount;
    public static float cachedstoragePercentageAmount;

    public void OnDoneButton()
    {
        // cPassive1
        ModifyResearchTime();

        // cPassive2
        ModifyWorkerMultiplier();

        // cPassive3
        ModifyInitialBuildingCount();

        // cPassive4
        ModifyInitialWorkerCount();

        // cPassive5
        ModifyBuildingMultiplier();

        // cPassive6
        ModifyCraftableCost();

        // cPassive7
        ModifyResearchableCost();

        // cPassive8
        ModifyStorageLimit();

        ModifyAllResearchablesCost();
        ModifyAllCraftableCost();
        ModifyAllBuildingMultiplier();
        ModifyBuildingCost();
        ModifyAllWorkerMultiplier();

        ClearBoxCache();
    }

    public void ClearBoxCache()
    {
        cachedAllWorkerMultiplierAmount = 0;
        cachedAllBuildingMultiplierAmount = 0;
        cachedAllCraftablesCostReduced = 0;
        cachedAllResearchablesCostReduced = 0;
        cachedWorkerCountModified = 0;
        cachedResearchTimeReductionAmount = 0;
        cachedstoragePercentageAmount = 0;

        cachedCraftableCostReduced.Clear();
        cachedResearchableCostReduced.Clear();
        cachedBuildingCostReduced.Clear();
        cachedWorkerMultiplierModified.Clear();
        cachedBuildingMultiplierModified.Clear();
        cachedBuildingSelfCountModified.Clear();
    }

    private void ModifyResearchTime()
    {
        // This should probably only happen on game reset.
        // Or if you can revert the previous research time reduction and reduce it by the new amount, that could also work but sounds hard to do.

        if (cachedResearchTimeReductionAmount > 0)
        {
            foreach (var research in Researchable.Researchables)
            {
                research.Value.ModifyTimeToCompleteResearch(cachedResearchTimeReductionAmount);
            }

            permanentStats.researchTimeReductionAmount += cachedResearchTimeReductionAmount;
        }

        // REVAMP

        ResetCache.resetResearchTimeReductionAmount += cachedResearchTimeReductionAmount;
    }
    private void ModifyStorageLimit()
    {
        if (cachedstoragePercentageAmount > 0)
        {
            StoragePile.storageAmountMultiplier += cachedstoragePercentageAmount;

            Debug.Log(string.Format("Increased storage limit by {0}%", cachedstoragePercentageAmount * 100));

            permanentStats.storagePercentageAmount += cachedstoragePercentageAmount;
        }

        // REVAMP 

        ResetCache.resetstoragePercentageAmount += cachedstoragePercentageAmount;
    }

    private void ModifyInitialWorkerCount()
    {
        if (cachedWorkerCountModified > 0)
        {
            Worker.InitialWorkerCount += cachedWorkerCountModified;

            permanentStats.workerCountModified += cachedWorkerCountModified;

            Debug.Log(string.Format("Increased workers by {0}", cachedWorkerCountModified));
        }

        // REVAMP 

        ResetCache.resetWorkerCountModified += cachedWorkerCountModified;
    }
    private void ModifyInitialBuildingCount()
    {
        if (cachedBuildingSelfCountModified.Count > 0)
        {
            foreach (var cachedBuilding in cachedBuildingSelfCountModified)
            {
                Building.Buildings[cachedBuilding.Key].SetSelfCount(cachedBuilding.Value);

                Debug.Log(string.Format("Increased the initial self count of {0} by {1}", Building.Buildings[cachedBuilding.Key].ActualName, cachedBuilding.Value));
            }
        }

        // Don't actually think I need to add this to permanent stats. 
        // Foudn another way to handle this particular passive.

        // REVAMP 

        if (cachedBuildingSelfCountModified.Count > 0)
        {
            foreach (var cachedBuilding in cachedBuildingSelfCountModified)
            {
                //Building.Buildings[cachedBuilding.Key].SetSelfCount(cachedBuilding.Value);

                //Debug.Log(string.Format("Increased the initial self count of {0} by {1}", Building.Buildings[cachedBuilding.Key].ActualName, cachedBuilding.Value));

                ResetCache.resetBuildingSelfCountModified.Add(cachedBuilding.Key, cachedBuilding.Value);
            }
        }
    }

    private void ModifyAllBuildingMultiplier()
    {
        if (cachedAllBuildingMultiplierAmount > 0)
        {
            foreach (var building in Building.Buildings)
            {
                //Building building = Building.Buildings[cachedBuilding.Key];

                for (int i = 0; i < building.Value.resourcesToIncrement.Count; i++)
                {
                    BuildingResourcesToModify buildingResourceToModify = building.Value.resourcesToIncrement[i];

                    float increaseAmount = buildingResourceToModify.baseResourceMultiplier * cachedAllBuildingMultiplierAmount;
                    buildingResourceToModify.currentResourceMultiplier += increaseAmount;
                    float differenceAmount = buildingResourceToModify.currentResourceMultiplier - buildingResourceToModify.baseResourceMultiplier;
                    building.Value.ModifyMultiplier(buildingResourceToModify.currentResourceMultiplier);
                    building.Value.UpdateDescription();

                    if (building.Value.IsUnlocked)
                    {
                        float oldAmountPerSecond = Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond;
                        Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond += differenceAmount * building.Value.ReturnSelfCount();
                        float newAmountPerSecond = Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond;
                        StaticMethods.ModifyAPSText(newAmountPerSecond, Resource.Resources[buildingResourceToModify.resourceTypeToModify].uiForResource.txtAmountPerSecond);

                        Debug.Log(string.Format("Changed current amount per second of {0} from {1} to {2}, also building multiplier changed from {3} to {4}, difference: {5}", buildingResourceToModify.resourceTypeToModify, oldAmountPerSecond, newAmountPerSecond, buildingResourceToModify.baseResourceMultiplier, buildingResourceToModify.currentResourceMultiplier, differenceAmount));
                    }
                }

                permanentStats.allBuildingMultiplierAmount += cachedAllBuildingMultiplierAmount;
            }
        }

        // REVAMP

        ResetCache.resetAllBuildingMultiplierAmount += cachedAllBuildingMultiplierAmount;
    }
    private void ModifyAllWorkerMultiplier()
    {
        // I can probably merge this into the other worker multiplier function.
        // Execution will probably still be the same, but it will just be displayed different when you display all the stats.
        if (cachedAllWorkerMultiplierAmount > 0)
        {
            foreach (var worker in Worker.Workers)
            {
                //Worker worker = Worker.Workers[cachedWorker.Key];

                for (int i = 0; i < worker.Value._resourcesToIncrement.Length; i++)
                {
                    WorkerResourcesToModify workerResourceToModify = worker.Value._resourcesToIncrement[i];

                    float increaseAmount = workerResourceToModify.baseResourceMultiplier * cachedAllWorkerMultiplierAmount;
                    workerResourceToModify.currentResourceMultiplier += increaseAmount;
                    float differenceAmount = workerResourceToModify.currentResourceMultiplier - workerResourceToModify.baseResourceMultiplier;
                    worker.Value.ModifyMultiplier(workerResourceToModify.currentResourceMultiplier);
                    worker.Value.ModifyDescriptionText();

                    if (worker.Value.workerCount > 0)
                    {
                        float oldAmountPerSecond = Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond;
                        Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond += differenceAmount * worker.Value.workerCount;
                        float newAmountPerSecond = Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond;
                        StaticMethods.ModifyAPSText(newAmountPerSecond, Resource.Resources[workerResourceToModify.resourceTypeToModify].uiForResource.txtAmountPerSecond);

                        Debug.Log(string.Format("Changed current amount per second of {0} from {1} to {2}, also worker multiplier changed from {3} to {4}, difference: {5}", workerResourceToModify.resourceTypeToModify, oldAmountPerSecond, newAmountPerSecond, workerResourceToModify.baseResourceMultiplier, workerResourceToModify.currentResourceMultiplier, differenceAmount));
                    }
                }

                permanentStats.allWorkerMultiplierAmount += cachedAllWorkerMultiplierAmount;
            }
        }

        // REVAMP

        ResetCache.resetAllWorkerMultiplierAmount += cachedAllWorkerMultiplierAmount;
    }
    private void ModifyBuildingMultiplier()
    {
        if (cachedBuildingMultiplierModified.Count > 0)
        {
            foreach (var cachedBuilding in cachedBuildingMultiplierModified)
            {
                Building building = Building.Buildings[cachedBuilding.Key];

                for (int i = 0; i < building.resourcesToIncrement.Count; i++)
                {
                    BuildingResourcesToModify buildingResourceToModify = building.resourcesToIncrement[i];

                    float increaseAmount = buildingResourceToModify.baseResourceMultiplier * cachedBuilding.Value;
                    buildingResourceToModify.currentResourceMultiplier += increaseAmount;
                    float differenceAmount = buildingResourceToModify.currentResourceMultiplier - buildingResourceToModify.baseResourceMultiplier;
                    building.ModifyMultiplier(buildingResourceToModify.currentResourceMultiplier);
                    building.UpdateDescription();

                    if (building.IsUnlocked)
                    {
                        float oldAmountPerSecond = Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond;
                        Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond += differenceAmount * building.ReturnSelfCount();
                        float newAmountPerSecond = Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond;
                        StaticMethods.ModifyAPSText(newAmountPerSecond, Resource.Resources[buildingResourceToModify.resourceTypeToModify].uiForResource.txtAmountPerSecond);

                        Debug.Log(string.Format("Changed current amount per second of {0} from {1} to {2}, also building multiplier changed from {3} to {4}, difference: {5}", buildingResourceToModify.resourceTypeToModify, oldAmountPerSecond, newAmountPerSecond, buildingResourceToModify.baseResourceMultiplier, buildingResourceToModify.currentResourceMultiplier, differenceAmount));
                    }
                }

                if (!permanentStats.buildingMultiplierModified.ContainsKey(cachedBuilding.Key))
                {
                    permanentStats.buildingMultiplierModified.Add(cachedBuilding.Key, cachedBuilding.Value);
                }
                else
                {
                    permanentStats.buildingMultiplierModified[cachedBuilding.Key] += cachedBuilding.Value;
                }
            }
        }

        // REVAMP 

        if (cachedBuildingMultiplierModified.Count > 0)
        {
            foreach (var cachedBuilding in cachedBuildingMultiplierModified)
            {
                ResetCache.resetBuildingMultiplierModified.Add(cachedBuilding.Key, cachedBuilding.Value);
            }
        }
    }
    private void ModifyWorkerMultiplier()
    {
        if (cachedWorkerMultiplierModified.Count > 0)
        {
            foreach (var cachedWorker in cachedWorkerMultiplierModified)
            {
                Worker worker = Worker.Workers[cachedWorker.Key];

                for (int i = 0; i < worker._resourcesToIncrement.Length; i++)
                {
                    WorkerResourcesToModify workerResourceToModify = worker._resourcesToIncrement[i];

                    float increaseAmount = workerResourceToModify.baseResourceMultiplier * cachedWorker.Value;
                    workerResourceToModify.currentResourceMultiplier += increaseAmount;
                    float differenceAmount = workerResourceToModify.currentResourceMultiplier - workerResourceToModify.baseResourceMultiplier;
                    worker.ModifyMultiplier(workerResourceToModify.currentResourceMultiplier);
                    worker.ModifyDescriptionText();

                    if (worker.workerCount > 0)
                    {
                        float oldAmountPerSecond = Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond;
                        Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond += differenceAmount * worker.workerCount;
                        float newAmountPerSecond = Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond;
                        StaticMethods.ModifyAPSText(newAmountPerSecond, Resource.Resources[workerResourceToModify.resourceTypeToModify].uiForResource.txtAmountPerSecond);

                        Debug.Log(string.Format("Changed current amount per second of {0} from {1} to {2}, also building multiplier changed from {3} to {4}, difference: {5}", workerResourceToModify.resourceTypeToModify, oldAmountPerSecond, newAmountPerSecond, workerResourceToModify.baseResourceMultiplier, workerResourceToModify.currentResourceMultiplier, differenceAmount));
                    }
                }

                if (!permanentStats.workerMultiplierModified.ContainsKey(cachedWorker.Key))
                {
                    permanentStats.workerMultiplierModified.Add(cachedWorker.Key, cachedWorker.Value);
                }
                else
                {
                    permanentStats.workerMultiplierModified[cachedWorker.Key] += cachedWorker.Value;
                }
            }
        }

        // REVAMP 

        if (cachedWorkerMultiplierModified.Count > 0)
        {
            foreach (var cachedWorker in cachedWorkerMultiplierModified)
            {
                ResetCache.resetWorkerMultiplierModified.Add(cachedWorker.Key, cachedWorker.Value);
            }
        }
    }

    private void ModifyAllResearchablesCost()
    {
        if (cachedResearchableCostReduced.Count > 0)
        {
            foreach (var researchable in Researchable.Researchables)
            {
                //Researchable researchable = Researchable.Researchables[cachedResearch.Key];
                for (int i = 0; i < researchable.Value.ResourceCost.Length; i++)
                {
                    ResourceCost ResourceCost = researchable.Value.ResourceCost[i];
                    float oldResourceCost = ResourceCost.costAmount;
                    float amountToDeduct = oldResourceCost * cachedAllResearchablesCostReduced;
                    float newResourceCost = ResourceCost.costAmount - amountToDeduct;
                    ResourceCost.costAmount = newResourceCost;
                    researchable.Value.UpdateResourceCostPassive(ResourceCost.costAmount);

                    Debug.Log(string.Format("Modified the cost amount of {0}  by {3}%,  from {1} to {2}", researchable.Value.ActualName, oldResourceCost, newResourceCost, cachedAllResearchablesCostReduced * 100));
                }

                permanentStats.allResearchablesCostReduced += cachedAllResearchablesCostReduced;
            }
        }

        // REVAMP 

        ResetCache.resetAllResearchablesCostReduced += cachedAllResearchablesCostReduced;
    }
    private void ModifyAllCraftableCost()
    {
        if (cachedCraftableCostReduced.Count > 0)
        {
            foreach (var craft in Craftable.Craftables)
            {
                Craftable craftable = Craftable.Craftables[craft.Key];
                for (int i = 0; i < craftable.ResourceCost.Length; i++)
                {
                    ResourceCost ResourceCost = craftable.ResourceCost[i];
                    float oldResourceCost = ResourceCost.costAmount;
                    float amountToDeduct = oldResourceCost * cachedAllCraftablesCostReduced;
                    float newResourceCost = ResourceCost.costAmount - amountToDeduct;
                    ResourceCost.costAmount = newResourceCost;
                    craftable.UpdateResourceCostPassive(ResourceCost.costAmount);

                    Debug.Log(string.Format("Modified the cost amount of {0}  by {3}%,  from {1} to {2}", craftable.ActualName, oldResourceCost, newResourceCost, cachedAllCraftablesCostReduced * 100));
                }

                permanentStats.allCraftablesCostReduced += cachedAllCraftablesCostReduced;
            }
        }

        // REVAMP

        ResetCache.resetAllCraftablesCostReduced += cachedAllCraftablesCostReduced;
    }
    private void ModifyBuildingCost()
    {
        if (cachedBuildingCostReduced.Count > 0)
        {
            foreach (var cachedBuilding in cachedBuildingCostReduced)
            {
                Building building = Building.Buildings[cachedBuilding.Key];
                for (int i = 0; i < building.ResourceCost.Length; i++)
                {
                    ResourceCost ResourceCost = building.ResourceCost[i];
                    float oldResourceCost = ResourceCost.BaseCostAmount;
                    float amountToDeduct = oldResourceCost * cachedBuilding.Value;
                    float newResourceCost = ResourceCost.BaseCostAmount - amountToDeduct;
                    ResourceCost.costAmount = newResourceCost;
                    building.UpdateResourceCostPassive(ResourceCost.costAmount);

                    if (cachedBuilding.Value > 0)
                    {
                        Debug.Log(string.Format("Modified the cost amount of {0}  by {3}%,  from {1} to {2}", building.ActualName, oldResourceCost, newResourceCost, cachedBuilding.Value * 100));
                    }

                }

                if (!permanentStats.buildingCostReduced.ContainsKey(building.Type))
                {
                    permanentStats.buildingCostReduced.Add(building.Type, cachedBuilding.Value);
                }
                else
                {
                    permanentStats.buildingCostReduced[building.Type] += cachedBuilding.Value;
                }
            }
        }

        // REVAMP

        if (cachedBuildingCostReduced.Count > 0)
        {
            foreach (var cachedBuilding in cachedBuildingCostReduced)
            {
                ResetCache.resetBuildingCostReduced.Add(cachedBuilding.Key, cachedBuilding.Value);
            }
        }
    }
    private void ModifyCraftableCost()
    {
        if (cachedCraftableCostReduced.Count > 0)
        {
            foreach (var cachedCraft in cachedCraftableCostReduced)
            {
                Craftable craftable = Craftable.Craftables[cachedCraft.Key];
                for (int i = 0; i < craftable.ResourceCost.Length; i++)
                {
                    ResourceCost ResourceCost = craftable.ResourceCost[i];
                    float oldResourceCost = ResourceCost.BaseCostAmount;
                    float amountToDeduct = oldResourceCost * cachedCraft.Value;
                    float newResourceCost = ResourceCost.costAmount - amountToDeduct;
                    ResourceCost.costAmount = newResourceCost;
                    craftable.UpdateResourceCostPassive(ResourceCost.costAmount);

                    if (cachedCraft.Value > 0)
                    {
                        Debug.Log(string.Format("Modified the cost amount of {0}  by {3}%,  from {1} to {2}", craftable.ActualName, oldResourceCost, newResourceCost, cachedCraft.Value * 100));
                    }


                }

                if (!permanentStats.craftableCostReduced.ContainsKey(craftable.Type))
                {
                    permanentStats.craftableCostReduced.Add(craftable.Type, cachedCraft.Value);
                }
                else
                {
                    permanentStats.craftableCostReduced[craftable.Type] += cachedCraft.Value;
                }
            }
        }

        // REVAMP

        if (cachedCraftableCostReduced.Count > 0)
        {
            foreach (var cachedCraft in cachedCraftableCostReduced)
            {
                ResetCache.resetCraftableCostReduced.Add(cachedCraft.Key, cachedCraft.Value);
            }
        }
    }
    private void ModifyResearchableCost()
    {
        if (cachedResearchableCostReduced.Count > 0)
        {
            foreach (var cachedResearch in cachedResearchableCostReduced)
            {
                Researchable researchable = Researchable.Researchables[cachedResearch.Key];
                for (int i = 0; i < researchable.ResourceCost.Length; i++)
                {
                    ResourceCost ResourceCost = researchable.ResourceCost[i];
                    float oldResourceCost = ResourceCost.costAmount;
                    float amountToDeduct = oldResourceCost * cachedResearch.Value;
                    float newResourceCost = ResourceCost.costAmount - amountToDeduct;
                    ResourceCost.costAmount = newResourceCost;
                    researchable.UpdateResourceCostPassive(ResourceCost.costAmount);

                    if (cachedResearch.Value > 0)
                    {
                        Debug.Log(string.Format("Modified the cost amount of {0}  by {3}%,  from {1} to {2}", researchable.ActualName, oldResourceCost, newResourceCost, cachedResearch.Value * 100));
                    }

                }

                if (!permanentStats.researchableCostReduced.ContainsKey(cachedResearch.Key))
                {
                    permanentStats.researchableCostReduced.Add(cachedResearch.Key, cachedResearch.Value);
                }
                else
                {
                    permanentStats.researchableCostReduced[cachedResearch.Key] += cachedResearch.Value;
                }
            }
        }

        // REVAMP 

        if (cachedResearchableCostReduced.Count > 0)
        {
            foreach (var cachedResearch in cachedResearchableCostReduced)
            {
                ResetCache.resetResearchableCostReduced.Add(cachedResearch.Key, cachedResearch.Value);
            }
        }
    }

    */
}

