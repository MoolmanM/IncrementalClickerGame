using System.Collections.Generic;
using UnityEngine;

// ePassive6: Decrease initial cost of a random Building.
public class ePassive6 : EpicPassive
{
    private EpicPassive _epicPassive;
    private BuildingType buildingTypeChosen;
    private float permanentAmount = 0.005f, prestigeAmount = 0.025f;

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
    private void AddToPrestigeCache(float percentageAmount)
    {
        if (!PrestigeCache.prestigeBoxBuildingCostSubtraction.ContainsKey(buildingTypeChosen))
        {
            PrestigeCache.prestigeBoxBuildingCostSubtraction.Add(buildingTypeChosen, percentageAmount);
        }
        else
        {
            PrestigeCache.prestigeBoxBuildingCostSubtraction[buildingTypeChosen] += percentageAmount;
        }
    }
    private void AddToPermanentCache(float percentageAmount)
    {
        if (!PermanentCache.permanentBoxBuildingCostSubtraction.ContainsKey(buildingTypeChosen))
        {
            PermanentCache.permanentBoxBuildingCostSubtraction.Add(buildingTypeChosen, percentageAmount);
        }
        else
        {
            PermanentCache.permanentBoxBuildingCostSubtraction[buildingTypeChosen] += percentageAmount;
        }
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Decrease cost of all Buildings by {0}%", percentageAmount * 100);
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
