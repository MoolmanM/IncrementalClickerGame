using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlocksRequired : MonoBehaviour
{
    void Awake()
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
}

