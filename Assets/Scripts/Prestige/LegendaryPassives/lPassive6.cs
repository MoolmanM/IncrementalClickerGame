using System.Collections.Generic;
using UnityEngine;

// ePassive6: Decrease initial cost of a random Building.
public class lPassive6 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;

    private float percentageAmount = 0.01f; // 1.5%
    private BuildingType buildingTypeChosen;

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
    }
    private void ChooseRandomBuilding()
    {
        List<BuildingType> buildingTypesInCurrentRun = new List<BuildingType>();

        foreach (var building in Building.Buildings)
        {
            if (building.Value.isUnlocked)
            {
                buildingTypesInCurrentRun.Add(building.Key);
            }
        }
        if (buildingTypesInCurrentRun.Count >= Prestige.buildingsUnlockedInPreviousRun.Count)
        {
            _index = Random.Range(0, buildingTypesInCurrentRun.Count);
            buildingTypeChosen = buildingTypesInCurrentRun[_index];
        }
        else
        {
            _index = Random.Range(0, Prestige.buildingsUnlockedInPreviousRun.Count);
            buildingTypeChosen = Prestige.buildingsUnlockedInPreviousRun[_index];
        }

        AddToBoxCache();
    }
    private void AddToBoxCache()
    {
        if (!BoxCache.cachedBuildingCostReduced.ContainsKey(buildingTypeChosen))
        {
            BoxCache.cachedBuildingCostReduced.Add(buildingTypeChosen, percentageAmount);
        }
        else
        {
            BoxCache.cachedBuildingCostReduced[buildingTypeChosen] += percentageAmount;
        }
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        ChooseRandomBuilding();
    }
}
