using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualEnergyProduction : Researchable
{
    private Researchable _researchable;

    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);
        _timeToCompleteResearch = 10;

        _craftingTypesToModify = new CraftingType[1];
        _craftingTypesToModify[0] = CraftingType.HamsterWheel;

        SetInitialValues();
    }
    void Start()
    {
        SetDescriptionText("Enables you to craft a hamster wheel for energy production.");
    }
    protected override void UnlockBuilding()
    {
        // No building to unlock.
    }

    protected override void UnlockCrafting()
    {
        base.UnlockCrafting();
    }
    protected override void UnlockResearchable()
    {
        //base.UnlockResearchable();
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
