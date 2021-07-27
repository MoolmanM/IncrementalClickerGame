using System;
using TMPro;
using UnityEngine;

public class StaticMethods : MonoBehaviour
{
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

}

