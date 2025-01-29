using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Start next/each run with a certain number of a random Building.
public class rPassive3 : RarePassive
{
    private RarePassive _rarePassive;
    private BuildingType buildingTypeChosen;
    private uint permanentAmount = 4, prestigeAmount = 10;

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
            if (building.Value.IsUnlocked)
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
    }
    private void AddToPrestigeCache(uint selfCountIncreaseAmount)
    {
        if (!PrestigeCache.prestigeBoxBuildingCountAddition.ContainsKey(buildingTypeChosen))
        {
            PrestigeCache.prestigeBoxBuildingCountAddition.Add(buildingTypeChosen, selfCountIncreaseAmount);
        }
        else
        {
            PrestigeCache.prestigeBoxBuildingCountAddition[buildingTypeChosen] += selfCountIncreaseAmount;
        }
    }
    private void AddToPermanentCache(uint selfCountIncreaseAmount)
    {
        if (!PermanentCache.permanentBoxBuildingCountAddition.ContainsKey(buildingTypeChosen))
        {
            PermanentCache.permanentBoxBuildingCountAddition.Add(buildingTypeChosen, selfCountIncreaseAmount);
        }
        else
        {
            PermanentCache.permanentBoxBuildingCountAddition[buildingTypeChosen] += selfCountIncreaseAmount;
        }
    }
    private void ModifyStatDescription(uint selfCountIncreaseAmount)
    {
        if (selfCountIncreaseAmount > 1)
        {
            description = string.Format("Start with {0} additional {1}'s", selfCountIncreaseAmount, Building.Buildings[buildingTypeChosen].ActualName);
        }
        else
        {
            description = string.Format("Start with an additional '{1}'", selfCountIncreaseAmount, Building.Buildings[buildingTypeChosen].ActualName);
        }
    }
    public override void InitializePermanentStat()
    {
        ChooseRandomBuilding();
        ModifyStatDescription(permanentAmount);
        AddToPermanentCache(permanentAmount);
    }
    public override void InitializePrestigeStat()
    {
        ChooseRandomBuilding();
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButtonBuilding(BuildingType buildingType)
    {
        AddToPrestigeCache(prestigeAmount);
    }
    public override BuildingType ReturnBuildingType()
    {
        return buildingTypeChosen;
    }
}
