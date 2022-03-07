using System.Collections.Generic;
using UnityEngine;

// Increase production of a random production Building by a certain %.
public class rPassive5 : RarePassive
{
    private RarePassive _rarePassive;
    private float percentageAmount = 0.05f;
    private BuildingType buildingTypeChosen;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);
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

        description = string.Format("Increase production of the {0} by {1}%", Building.Buildings[buildingTypeChosen].actualName, percentageAmount * 100);

        AddToBoxCache();
    }
    private void AddToBoxCache()
    {
        if (!BoxCache.cachedBuildingMultiplierModified.ContainsKey(buildingTypeChosen))
        {
            BoxCache.cachedBuildingMultiplierModified.Add(buildingTypeChosen, percentageAmount);
        }
        else
        {
            BoxCache.cachedBuildingMultiplierModified[buildingTypeChosen] += percentageAmount;
        }
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        ChooseRandomBuilding();
    }
}
