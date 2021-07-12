using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Researchable
{
    private Researchable _researchable;

    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);
        _timeToCompleteResearch = 60;

        _craftingTypesToModify = new CraftingType[1];
        _craftingTypesToModify[0] = CraftingType.WoodenSpear;

        SetInitialValues();
    }
    void Start()
    {
        SetDescriptionText("Unlocks spear for hunting.");
    }
    protected override void UnlockBuilding()
    {
        // No building to unlock.
    }
    protected override void UnlockResearchable()
    {
        // No research to unlock.
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
