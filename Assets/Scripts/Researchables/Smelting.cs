using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelting : Researchable
{
    private Researchable _researchable;


    // Unlock after having 100 knowledge.
    // Will get back to this later.
    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);
        _timeToCompleteResearch = 10;

        _buildingTypesToModify = new BuildingType[1];
        _buildingTypesToModify[0] = BuildingType.Furnace;

        _researchTypesToModify = new ResearchType[1];
        _researchTypesToModify[0] = ResearchType.ManualEnergyProduction;

        _isUnlockableByResource = true;


        SetInitialValues();

    }
    void Start()
    {
        SetDescriptionText("Enables smelting ores into metals.");
    }
    protected override void UnlockBuilding()
    {
        base.UnlockBuilding();
    }

    protected override void UnlockCrafting()
    {
        // No crafting to unlock
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
