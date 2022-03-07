using TMPro;
using UnityEngine;

public class UnlocksRequired : MonoBehaviour
{
    void Awake()
    {
        FillUnlocksRequired();
        SetupResourceInfos();
    }
    private void FillUnlocksRequired()
    {
        foreach (var kvp in Researchable.Researchables)
        {
            if (kvp.Value.isUnlockableByResource)
            {
                kvp.Value.unlocksRequired++;
            }
            foreach (CraftingType type in kvp.Value.typesToUnlock.craftingTypesToUnlock)
            {
                Craftable.Craftables[type].unlocksRequired++;
            }
            foreach (ResearchType type in kvp.Value.typesToUnlock.researchTypesToUnlock)
            {
                Researchable.Researchables[type].unlocksRequired++;
            }
            foreach (BuildingType type in kvp.Value.typesToUnlock.buildingTypesToUnlock)
            {
                Building.Buildings[type].unlocksRequired++;
            }
        }

        foreach (var kvp in Building.Buildings)
        {
            if (kvp.Value.isUnlockableByResource)
            {
                kvp.Value.unlocksRequired++;
            }
            foreach (CraftingType type in kvp.Value.typesToUnlock.craftingTypesToUnlock)
            {
                Craftable.Craftables[type].unlocksRequired++;
            }
            foreach (ResearchType type in kvp.Value.typesToUnlock.researchTypesToUnlock)
            {
                Researchable.Researchables[type].unlocksRequired++;
            }
            foreach (BuildingType type in kvp.Value.typesToUnlock.buildingTypesToUnlock)
            {
                Building.Buildings[type].unlocksRequired++;
            }
        }

        foreach (var kvp in Craftable.Craftables)
        {
            if (kvp.Value.isUnlockableByResource)
            {
                kvp.Value.unlocksRequired++;
            }
            foreach (CraftingType type in kvp.Value.typesToUnlock.craftingTypesToUnlock)
            {
                Craftable.Craftables[type].unlocksRequired++;
            }
            foreach (ResearchType type in kvp.Value.typesToUnlock.researchTypesToUnlock)
            {
                Researchable.Researchables[type].unlocksRequired++;
            }
            foreach (BuildingType type in kvp.Value.typesToUnlock.buildingTypesToUnlock)
            {
                Building.Buildings[type].unlocksRequired++;
            }
        }
    }
    private void SetupResourceInfos()
    {
        // Okay this all works, I need to either rethink the storage pile
        // Or just exclude storage pile here somehow

        // So I believe the building associated isn't needed since we 
        // Run it in resourceToIncrement anyways, so it's automatically the correct building and resource
        foreach (var resource in Resource.Resources)
        {
            foreach (var building in Building.Buildings)
            {
                foreach (var resourceToIncrement in building.Value.resourcesToIncrement)
                {
                    if (resourceToIncrement.resourceTypeToModify == resource.Key)
                    {
                        resource.Value.resourceInfoList.Add(new ResourceInfo() { name = building.Value.actualName, objModifiedBy = building.Value.gameObject });
                    }
                }

                foreach (var resourceToDecrement in building.Value.resourcesToDecrement)
                {
                    if (resourceToDecrement.resourceTypeToModify == resource.Key)
                    {
                        resource.Value.resourceInfoList.Add(new ResourceInfo() { name = building.Value.actualName, objModifiedBy = building.Value.gameObject });
                    }
                }
            }

            foreach (var worker in Worker.Workers)
            {
                foreach (var resourceToIncrement in worker.Value._resourcesToIncrement)
                {
                    if (resourceToIncrement.resourceTypeToModify == resource.Key)
                    {
                        resource.Value.resourceInfoList.Add(new ResourceInfo() { name = worker.Value.actualName, objModifiedBy = worker.Value.gameObject });
                    }
                }

                foreach (var resourceToDecrement in worker.Value._resourcesToDecrement)
                {
                    if (resourceToDecrement.resourceTypeToModify == resource.Key)
                    {
                        resource.Value.resourceInfoList.Add(new ResourceInfo() { name = worker.Value.actualName, objModifiedBy = worker.Value.gameObject });
                    }
                }
            }

            for (int i = 0; i < resource.Value.resourceInfoList.Count; i++)
            {
                ResourceInfo resourceInfo = resource.Value.resourceInfoList[i];
                if (i == 0)
                {
                    resourceInfo.uiForResourceInfo.objMainPanel = Instantiate(resource.Value.prefabObjTop, resource.Value.tformObjTooltipGroup);
                }
                else if (i == (resource.Value.resourceInfoList.Count - 1))
                {
                    resourceInfo.uiForResourceInfo.objMainPanel = Instantiate(resource.Value.prefabObjBot, resource.Value.tformObjTooltipGroup);
                }
                else
                {
                    resourceInfo.uiForResourceInfo.objMainPanel = Instantiate(resource.Value.prefabObjMid, resource.Value.tformObjTooltipGroup);
                }

                resourceInfo.uiForResourceInfo.tformNewObj = resourceInfo.uiForResourceInfo.objMainPanel.transform;

                resourceInfo.uiForResourceInfo.tformInfoName = resourceInfo.uiForResourceInfo.tformNewObj.Find("Object_Name");
                resourceInfo.uiForResourceInfo.tformInfoAmountPerSecond = resourceInfo.uiForResourceInfo.tformNewObj.Find("Object_APS");

                resourceInfo.uiForResourceInfo.textInfoName = resourceInfo.uiForResourceInfo.tformInfoName.GetComponent<TMP_Text>();
                resourceInfo.uiForResourceInfo.textInfoAmountPerSecond = resourceInfo.uiForResourceInfo.tformInfoAmountPerSecond.GetComponent<TMP_Text>();

                resourceInfo.uiForResourceInfo.textInfoName.text = resourceInfo.name;
                resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", resourceInfo.amountPerSecond);

                resource.Value.resourceInfoList[i] = resourceInfo;

                // So I actually think mainPanel can stay the same, just change the where you instantiate to top, bot or mid.
            }

            foreach (var resourceInfo in resource.Value.resourceInfoList)
            {
                resourceInfo.uiForResourceInfo.objMainPanel.SetActive(false);
                //resourceInfo.uiForResourceInfo.objSpacer.SetActive(false);
            }

            resource.Value.objTooltipGroup.SetActive(false);
        }


    }

    private void DeprecatedInstantiateResourceInfo()
    {
        //foreach (var resource in Resource.Resources)
        //{
        //    for (int i = 0; i < resource.Value.resourceInfoList.Count; i++)
        //    {
        //        ResourceInfo resourceInfo = resource.Value.resourceInfoList[i];
        //        resourceInfo.uiForResourceInfo.objMainPanel = Instantiate(resource.Value.prefabResourceInfoPanel, resource.Value.tformResourceTooltip);
        //        resourceInfo.uiForResourceInfo.objSpacer = Instantiate(resource.Value.prefabResourceInfoSpacer, resource.Value.tformResourceTooltip);
        //        resourceInfo.uiForResourceInfo.tformNewObj = resourceInfo.uiForResourceInfo.objMainPanel.transform;
        //        resourceInfo.uiForResourceInfo.tformInfoName = resourceInfo.uiForResourceInfo.tformNewObj.Find("Text_Name");
        //        resourceInfo.uiForResourceInfo.tformInfoAmountPerSecond = resourceInfo.uiForResourceInfo.tformNewObj.Find("Text_AmountPerSecond");

        //        resourceInfo.uiForResourceInfo.textInfoName = resourceInfo.uiForResourceInfo.tformInfoName.GetComponent<TMP_Text>();
        //        resourceInfo.uiForResourceInfo.textInfoAmountPerSecond = resourceInfo.uiForResourceInfo.tformInfoAmountPerSecond.GetComponent<TMP_Text>();

        //        resourceInfo.uiForResourceInfo.textInfoName.text = resourceInfo.name;
        //        resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", resourceInfo.amountPerSecond);

        //        resource.Value.resourceInfoList[i] = resourceInfo;

        //        // Okay so now I ONLY want to update the amount per second
        //        // And if it reaches zero. Or at least when the workercount reaches zero, I need to setactive(false)
        //        // BUT the problem is, how to know which amount per second to modify.
        //    }
        //}
    }

    private void InstantiateResourceInfo()
    {

    }
}

