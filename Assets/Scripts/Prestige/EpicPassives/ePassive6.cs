using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ePassive6: Decrease initial cost of a random Building.
public class ePassive6 : EpicPassive
{
    private EpicPassive _epicPassive;

    private float percentageAmount = 0.01f; // 1%
    private BuildingType buildingTypeChosen;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
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
        if (_selfCountIncreaseAmount > 1)
        {
            description = string.Format("Start each run with an additional {0} {1}'s, when you unlock it", _selfCountIncreaseAmount, Building.Buildings[buildingTypeChosen].actualName);
        }
        else
        {
            description = string.Format("Start each run with an additional {1}, when you unlock it", _selfCountIncreaseAmount, Building.Buildings[buildingTypeChosen].actualName);
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
