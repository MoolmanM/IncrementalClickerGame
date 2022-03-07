using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BoxCache : MonoBehaviour
{
    public static float cachedAllWorkerMultiplierAmount;

    public static Dictionary<CraftingType, float> cachedCraftableCostReduced = new Dictionary<CraftingType, float>();
    public static Dictionary<ResearchType, float> cachedResearchableCostReduced = new Dictionary<ResearchType, float>();
    public static Dictionary<BuildingType, float> cachedBuildingCostReduced = new Dictionary<BuildingType, float>();

    public static Dictionary<WorkerType, float> cachedWorkerMultiplierModified = new Dictionary<WorkerType, float>();
    public static Dictionary<BuildingType, float> cachedBuildingMultiplierModified = new Dictionary<BuildingType, float>();

    public static Dictionary<BuildingType, uint> cachedBuildingSelfCountModified = new Dictionary<BuildingType, uint>();

    public static uint cachedWorkerCountModified;
    public static float cachedResearchTimeReductionAmount;
    public static float cachedstoragePercentageAmount;

    [Button]
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
        ModifyCraftableCosts();

        // cPassive7
        ModifyResearchableCost();

        // cPassive8
        ModifyStorageLimit();
    }

    private void ModifyBuildingCosts()
    {
        foreach (var cachedBuilding in cachedBuildingCostReduced)
        {
            Building building = Building.Buildings[cachedBuilding.Key];
            for (int i = 0; i < building.resourceCost.Length; i++)
            {
                ResourceCost resourceCost = building.resourceCost[i];
                float oldResourceCost = resourceCost.initialCostAmount;
                float amountToDeduct = oldResourceCost * cachedBuilding.Value;
                float newResourceCost = resourceCost.initialCostAmount - amountToDeduct;
                resourceCost.costAmount = newResourceCost;

                Debug.Log(string.Format("Modified the cost amount of {0}  by {3}%,  from {1} to {2}", building.actualName, oldResourceCost, newResourceCost, cachedCraft.Value * 100));
            }

            if (!PermanentStats.buildingCostReduced.ContainsKey(building.Type))
            {
                PermanentStats.buildingCostReduced.Add(building.Type, cachedBuilding.Value);
            }
            else
            {
                PermanentStats.buildingCostReduced[building.Type] += cachedBuilding.Value;
            }
        }
    }
    private void ModifyAllWorkerMultiplier()
    {
        // I can probably merge this into the other worker multiplier function.
        // Execution will probably still be the same, but it will just be displayed different when you display all the stats.

        foreach (var worker in Worker.Workers)
        {
            //Worker worker = Worker.Workers[cachedWorker.Key];

            for (int i = 0; i < worker.Value._resourcesToIncrement.Length; i++)
            {
                WorkerResourcesToModify workerResourceToModify = worker.Value._resourcesToIncrement[i];

                float increaseAmount = workerResourceToModify.baseResourceMultiplier * cachedAllWorkerMultiplierAmount;
                workerResourceToModify.currentResourceMultiplier += increaseAmount;
                float differenceAmount = workerResourceToModify.currentResourceMultiplier - workerResourceToModify.baseResourceMultiplier;
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

            PermanentStats.allWorkerMultiplierAmount += cachedAllWorkerMultiplierAmount;
        }
    }
    private void ModifyStorageLimit()
    {
        StoragePile.storageAmountMultiplier += cachedstoragePercentageAmount;

        Debug.Log(string.Format("Increased storage limit by {0}", cachedstoragePercentageAmount*100));

        PermanentStats.storagePercentageAmount += cachedstoragePercentageAmount;
    }
    private void ModifyResearchableCost()
    {
        foreach (var cachedResearch in cachedResearchableCostReduced)
        {
            Researchable researchable = Researchable.Researchables[cachedResearch.Key];
            for (int i = 0; i < researchable.resourceCost.Length; i++)
            {
                ResourceCost resourceCost = researchable.resourceCost[i];
                float oldResourceCost = resourceCost.costAmount;
                float amountToDeduct = oldResourceCost * cachedResearch.Value;
                float newResourceCost = resourceCost.costAmount - amountToDeduct;
                resourceCost.costAmount = newResourceCost;

                Debug.Log(string.Format("Modified the cost amount of {0}  by {3}%,  from {1} to {2}", researchable.actualName, oldResourceCost, newResourceCost, cachedResearch.Value * 100));
            }

            if (!PermanentStats.researchableCostReduced.ContainsKey(cachedResearch.Key))
            {
                PermanentStats.researchableCostReduced.Add(cachedResearch.Key, cachedResearch.Value);
            }
            else
            {
                PermanentStats.researchableCostReduced[cachedResearch.Key] += cachedResearch.Value;
            }
        }
    }
    private void ModifyBuildingMultiplier()
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
                //building.ModifyDescriptionText();

                if (building.isUnlocked)
                {
                    float oldAmountPerSecond = Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond;
                    Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond += differenceAmount * building.ReturnSelfCount();
                    float newAmountPerSecond = Resource.Resources[buildingResourceToModify.resourceTypeToModify].amountPerSecond;
                    StaticMethods.ModifyAPSText(newAmountPerSecond, Resource.Resources[buildingResourceToModify.resourceTypeToModify].uiForResource.txtAmountPerSecond);
                    
                    Debug.Log(string.Format("Changed current amount per second of {0} from {1} to {2}, also building multiplier changed from {3} to {4}, difference: {5}", buildingResourceToModify.resourceTypeToModify, oldAmountPerSecond, newAmountPerSecond, buildingResourceToModify.baseResourceMultiplier, buildingResourceToModify.currentResourceMultiplier, differenceAmount));
                }
            }

            if (!PermanentStats.buildingMultiplierModified.ContainsKey(cachedBuilding.Key))
            {
                PermanentStats.buildingMultiplierModified.Add(cachedBuilding.Key, cachedBuilding.Value);
            }
            else
            {
                PermanentStats.buildingMultiplierModified[cachedBuilding.Key] += cachedBuilding.Value;
            }
        }
        //foreach (var building in cachedBuildingMultiplierModified)
        //{
        //    for (int i = 0; i < Building.Buildings[buildingTypeChosen].resourcesToIncrement.Count; i++)
        //    {
        //        BuildingResourcesToModify resourceTypeToModify = Building.Buildings[buildingTypeChosen].resourcesToIncrement[i];
        //        float oldResourceMultiplier = resourceTypeToModify.currentResourceMultiplier;
        //        float newResourceMultiplier = resourceTypeToModify.currentResourceMultiplier * percentageAmount;
        //        resourceTypeToModify.currentResourceMultiplier = newResourceMultiplier;

        //        multiplierModifiedAmount = newResourceMultiplier - oldResourceMultiplier;

        //        // After modifying this, you should probably also check how many workers there are currently assigned to that job.

        //        if (Building.Buildings[buildingTypeChosen].isUnlocked)
        //        {
        //            float oldAmountPerSecond = Resource.Resources[resourceTypeToModify.resourceTypeToModify].amountPerSecond;
        //            //Building.Buildings[buildingTypeChosen];
        //            // Re-modify amount per second
        //            float newAmountPerSecond = Resource.Resources[resourceTypeToModify.resourceTypeToModify].amountPerSecond += multiplierModifiedAmount;
        //            Resource.Resources[resourceTypeToModify.resourceTypeToModify].amountPerSecond = newAmountPerSecond;
        //            StaticMethods.ModifyAPSText(Resource.Resources[resourceTypeToModify.resourceTypeToModify].amountPerSecond, Resource.Resources[resourceTypeToModify.resourceTypeToModify].uiForResource.txtAmountPerSecond);

        //            Debug.Log(string.Format("Changed current amount per second of {0} from {1} to {2}", resourceTypeToModify.resourceTypeToModify, oldAmountPerSecond, newAmountPerSecond));
        //        }
        //        Building.Buildings[buildingTypeChosen].ReModifyDescription();
        //        Debug.Log(string.Format("Modified {0}'s resource multiplier amount from {1} to {2}", buildingTypeChosen, oldResourceMultiplier, newResourceMultiplier));
        //        description = string.Format("Modified {0}'s resource multiplier amount from {1} to {2}", buildingTypeChosen, oldResourceMultiplier, newResourceMultiplier);

        //        AddToPermanentList();
        //    }

        //    if (!PermanentStats.buildingMultiplierModified.ContainsKey(buildingTypeChosen))
        //    {
        //        PermanentStats.buildingMultiplierModified.Add(buildingTypeChosen, multiplierModifiedAmount);
        //    }
        //    else
        //    {
        //        PermanentStats.buildingMultiplierModified[buildingTypeChosen] += multiplierModifiedAmount;
        //    }
        //}
    }
    private void ModifyInitialWorkerCount()
    {
        Worker.TotalWorkerCount += cachedWorkerCountModified;
        Worker.AliveCount += cachedWorkerCountModified;

        Debug.Log(string.Format("Increased workers by {0}", cachedWorkerCountModified));
        // This should only happen after resetting the game
        // Also if you get this passive while doing the prestige, it needs to increase by this amount and the prestige amount additionally.
        // Which should be fine since it's all going to permanent and prestige stat classes to centralize everything.

        //Worker.TotalWorkerCount += 5;
    }
    private void ModifyInitialBuildingCount()
    {
        foreach (var cachedBuilding in cachedBuildingSelfCountModified)
        {
            Building.Buildings[cachedBuilding.Key].SetSelfCount(cachedBuilding.Value);
            Debug.Log(string.Format("Increased the initial self count of {0} by {1}", Building.Buildings[cachedBuilding.Key].actualName, cachedBuilding.Value));
        }
    
       // Don't actually think I need to add this to permanent stats. 
       // Foudn another way to handle this particular passive.
    }
    private void ModifyResearchTime()
    {
        // This should probably only happen on game reset.
        // Or if you can revert the previous research time reduction and reduce it by the new amount, that could also work but sounds hard to do.

        foreach (var research in Researchable.Researchables)
        {
            research.Value.ModifyTimeToCompleteResearch(cachedResearchTimeReductionAmount);
        }

        PermanentStats.researchTimeReductionAmount += cachedResearchTimeReductionAmount;
    }
    private void ModifyWorkerMultiplier()
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
                worker.ModifyDescriptionText();

                if (worker.workerCount > 0)
                {
                    float oldAmountPerSecond = Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond;
                    Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond += differenceAmount * worker.workerCount;
                    float newAmountPerSecond = Resource.Resources[workerResourceToModify.resourceTypeToModify].amountPerSecond;
                    StaticMethods.ModifyAPSText(newAmountPerSecond, Resource.Resources[workerResourceToModify.resourceTypeToModify].uiForResource.txtAmountPerSecond);
                    Debug.Log(string.Format("Changed current amount per second of {0} from {1} to {2}, also worker multiplier changed from {3} to {4}, difference: {5}", workerResourceToModify.resourceTypeToModify, oldAmountPerSecond, newAmountPerSecond, workerResourceToModify.baseResourceMultiplier, workerResourceToModify.currentResourceMultiplier, differenceAmount));
                }
            }

            if (!PermanentStats.workerMultiplierModified.ContainsKey(cachedWorker.Key))
            {
                PermanentStats.workerMultiplierModified.Add(cachedWorker.Key, cachedWorker.Value);
            }
            else
            {
                PermanentStats.workerMultiplierModified[cachedWorker.Key] += cachedWorker.Value;
            }
        }       
    }
    private void ModifyCraftableCosts()
    {
        foreach (var cachedCraft in cachedCraftableCostReduced)
        {
            Craftable craftable = Craftable.Craftables[cachedCraft.Key];
            for (int i = 0; i < craftable.resourceCost.Length; i++)
            {
                ResourceCost resourceCost = craftable.resourceCost[i];
                float oldResourceCost = resourceCost.costAmount;
                float amountToDeduct = oldResourceCost * cachedCraft.Value;
                float newResourceCost = resourceCost.costAmount - amountToDeduct;
                resourceCost.costAmount = newResourceCost;

                Debug.Log(string.Format("Modified the cost amount of {0}  by {3}%,  from {1} to {2}", craftable.actualName, oldResourceCost, newResourceCost, cachedCraft.Value * 100));
            }

            if (!PermanentStats.craftableCostReduced.ContainsKey(craftable.Type))
            {
                PermanentStats.craftableCostReduced.Add(craftable.Type, cachedCraft.Value);
            }
            else
            {
                PermanentStats.craftableCostReduced[craftable.Type] += cachedCraft.Value;
            }
        }
    }
}
