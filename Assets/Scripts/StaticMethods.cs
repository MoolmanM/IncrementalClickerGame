using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaticMethods 
{
    public static float _timer = 0.1f;
    public static float _maxValue = 0.1f;

    void Awake()
    {
        foreach (var kvp in Researchable.Researchables)
        {
            if (kvp.Value.isUnlockableByResource)
            {
                kvp.Value.unlocksRequired++;
            }
            foreach (CraftingType type in kvp.Value.typesToModify.craftingTypesToModify)
            {
                Craftable.Craftables[type].unlocksRequired++;
            }
            foreach (ResearchType type in kvp.Value.typesToModify.researchTypesToModify)
            {
                Researchable.Researchables[type].unlocksRequired++;
            }
            foreach (BuildingType type in kvp.Value.typesToModify.buildingTypesToModify)
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
            foreach (CraftingType type in kvp.Value.typesToModify.craftingTypesToModify)
            {
                Craftable.Craftables[type].unlocksRequired++;
            }
            foreach (ResearchType type in kvp.Value.typesToModify.researchTypesToModify)
            {
                Researchable.Researchables[type].unlocksRequired++;
            }
            foreach (BuildingType type in kvp.Value.typesToModify.buildingTypesToModify)
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
            foreach (CraftingType type in kvp.Value.typesToModify.craftingTypesToModify)
            {
                Craftable.Craftables[type].unlocksRequired++;
            }
            foreach (ResearchType type in kvp.Value.typesToModify.researchTypesToModify)
            {
                Researchable.Researchables[type].unlocksRequired++;
            }
            foreach (BuildingType type in kvp.Value.typesToModify.buildingTypesToModify)
            {
                Building.Buildings[type].unlocksRequired++;
            }
        }
    }
    public static void UnlockCrafting(bool isModifying, CraftingType[] types)
    {
        if (isModifying)
        {
            foreach (CraftingType craft in types)
            {
                Craftable.Craftables[craft].unlockAmount++;

                if (Craftable.Craftables[craft].unlockAmount == Craftable.Craftables[craft].unlocksRequired)
                {
                    Craftable.Craftables[craft].isUnlocked = true;

                    if (UIManager.isBuildingVisible)
                    {
                        Craftable.isUnlockedEvent = true;
                        Craftable.Craftables[craft].hasSeen = false;
                        PointerNotification.rightAmount++;
                    }
                    else if (!UIManager.isCraftingVisible)
                    {
                        Craftable.isUnlockedEvent = true;
                        Craftable.Craftables[craft].hasSeen = false;
                        PointerNotification.leftAmount++;
                    }
                    else
                    {
                        Craftable.Craftables[craft].objMainPanel.SetActive(true);
                        Craftable.Craftables[craft].objSpacerBelow.SetActive(true);
                        Craftable.Craftables[craft].hasSeen = true;
                    }
                }
            }

            PointerNotification.HandleRightAnim();
            PointerNotification.HandleLeftAnim();
        }
    }
    public static void UnlockBuilding(bool isModifying, BuildingType[] types)
    {
        if (isModifying)
        {
            foreach (BuildingType building in types)
            {
                Building.Buildings[building].unlockAmount++;

                if (Building.Buildings[building].unlockAmount == Building.Buildings[building].unlocksRequired)
                {
                    Building.Buildings[building].isUnlocked = true;

                    if (!UIManager.isBuildingVisible)
                    {
                        Building.isUnlockedEvent = true;
                        Building.Buildings[building].hasSeen = false;
                        PointerNotification.leftAmount++;
                    }
                    else
                    {
                        Building.Buildings[building].objMainPanel.SetActive(true);
                        Building.Buildings[building].objSpacerBelow.SetActive(true);
                        Building.Buildings[building].hasSeen = true;
                    }
                }
            }

            PointerNotification.HandleRightAnim();
            PointerNotification.HandleLeftAnim();
        }
    }
    public static void UnlockResearchable(bool isModifying, ResearchType[] types)
    {
        if (isModifying)
        {
            foreach (ResearchType research in types)
            {
                Researchable.Researchables[research].unlockAmount++;

                if (Researchable.Researchables[research].unlockAmount == Researchable.Researchables[research].unlocksRequired)
                {
                    Researchable.Researchables[research].isUnlocked = true;

                    if (UIManager.isResearchVisible)
                    {
                        Researchable.Researchables[research].objMainPanel.SetActive(true);
                        Researchable.Researchables[research].objSpacerBelow.SetActive(true);
                        Researchable.Researchables[research].hasSeen = true;
                    }
                    else
                    {
                        Researchable.isUnlockedEvent = true;
                        Researchable.Researchables[research].hasSeen = false;
                        PointerNotification.rightAmount++;
                    }
                }
            }

            PointerNotification.HandleRightAnim();
        }
    }
    public static void ShowResourceCostTime(TMP_Text txt, float current, float cost, float amountPerSecond)
    {
        if (amountPerSecond > 0)
        {
            float secondsLeft = (cost - current) / (amountPerSecond);
            TimeSpan timeSpan = TimeSpan.FromSeconds((double)(new decimal(secondsLeft)));

            if (current >= cost)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}", current, cost);
            }
            else if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%s}s</color>)", current, cost, timeSpan.Duration());
            }
            else if (timeSpan.Days == 0 && timeSpan.Hours == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%m}m {2:%s}s</color>)", current, cost, timeSpan.Duration());
            }
            else if (timeSpan.Days == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{0:%h}h {0:%m}m</color>)", current, cost, timeSpan.Duration());
            }
            else
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{0:%d}d {0:%h}h</color>)", current, cost, timeSpan.Duration());
            }
        }
    }
    public static void CheckIfUnlocked(bool isUnlocked, bool isUnlockableByResource, GameObject objMainPanel, GameObject objSpacerBelow, bool isUnlockedEvent, bool isVisible, ResourceCost[] costArray)
    {
        if (!isUnlocked)
        {
            if (GetCurrentFill(costArray) >= 0.8f)
            {
                if (isUnlockableByResource)
                {
                    isUnlocked = true;
                    if (isVisible)
                    {
                        objMainPanel.SetActive(true);
                        objSpacerBelow.SetActive(true);
                    }
                    else
                    {
                        isUnlockedEvent = true;
                    }
                }
            }
        }
    }
    public static void UpdateResourceCosts(ResourceCost[] costArray, Image imgProgressBar)
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;

            for (int i = 0; i < costArray.Length; i++)
            {
                costArray[i].currentAmount = Resource.Resources[costArray[i].associatedType].amount;
                costArray[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", costArray[i].currentAmount, costArray[i].costAmount);
                costArray[i].uiForResourceCost.textCostName.text = string.Format("{0}", costArray[i].associatedType.ToString());

                ShowResourceCostTime(costArray[i].uiForResourceCost.textCostAmount, costArray[i].currentAmount, costArray[i].costAmount, Resource.Resources[costArray[i].associatedType].amountPerSecond);
            }
            imgProgressBar.fillAmount = GetCurrentFill(costArray);
        }
    }
    public static float GetCurrentFill(ResourceCost[] costArray)
    {
        float add = 0;
        float div = 0;
        float fillAmount = 0;

        for (int i = 0; i < costArray.Length; i++)
        {
            add = costArray[i].currentAmount;
            div = costArray[i].costAmount;
            if (add > div)
            {
                add = div;
            }
            fillAmount += add / div;
        }
        return fillAmount / costArray.Length;
    }

    private void Start()
    {
        //Debug.Log((Mathf.Log(10) * ((4 * 10e12) * 6)) / 10);
        //Debug.Log((Mathf.Log10(20000000000000) - 6) /10); 

        // This is the Kardashev formula
        //This is earth's current point on the scale.
    }
    private void Update()
    {
        // This seems to work perfectly.
        //if (Resource.Resources[ResourceType.Energy].amount > 0)
        //{
        //    Debug.Log("Kardashev Scale: " + (Mathf.Log10(Resource.Resources[ResourceType.Energy].amount) - 6) / 10);
        //}        
    }
}

