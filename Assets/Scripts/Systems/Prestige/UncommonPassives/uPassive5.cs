using System.Collections.Generic;
using UnityEngine;

// Increase production of a random production Building by a certain %.
public class uPassive5 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;
    private BuildingType buildingTypeChosen;
    private float permanentAmount = 0.023f, prestigeAmount = 0.115f;

    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);
    }
    private void ChooseRandomBuilding()
    {
        List<BuildingType> buildingTypesInCurrentRun = new List<BuildingType>();

        foreach (var building in Building.Buildings)
        {
            if (building.Value.IsUnlocked && building.Value.resourcesToIncrement.Count > 0)
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
    private void AddToPrestigeCache(float percentageAmount)
    {
        if (!PrestigeCache.prestigeBoxBuildingMultiplierAddition.ContainsKey(buildingTypeChosen))
        {
            PrestigeCache.prestigeBoxBuildingMultiplierAddition.Add(buildingTypeChosen, percentageAmount);
        }
        else
        {
            PrestigeCache.prestigeBoxBuildingMultiplierAddition[buildingTypeChosen] += percentageAmount;
        }
    }
    private void AddToPermanentCache(float percentageAmount)
    {
        if (!PermanentCache.permanentBoxBuildingMultiplierAddition.ContainsKey(buildingTypeChosen))
        {
            PermanentCache.permanentBoxBuildingMultiplierAddition.Add(buildingTypeChosen, percentageAmount);
        }
        else
        {
            PermanentCache.permanentBoxBuildingMultiplierAddition[buildingTypeChosen] += percentageAmount;
        }
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Increase production of building '{0}' by {1}%", Building.Buildings[buildingTypeChosen].ActualName, percentageAmount * 100);
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
