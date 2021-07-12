using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEquipment : Researchable
{
    private Researchable _researchable;

    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);
        _timeToCompleteResearch = 60;

        _craftingTypesToModify = new CraftingType[4];
        _craftingTypesToModify[0] = CraftingType.StoneAxe;
        _craftingTypesToModify[1] = CraftingType.StoneHoe;
        _craftingTypesToModify[2] = CraftingType.StonePickaxe;
        _craftingTypesToModify[3] = CraftingType.StoneSpear;

        SetInitialValues();     
    }
    void Start()
    {      
        SetDescriptionText("Enables crafting of stone tools.");
    }
    protected override void UnlockBuilding()
    {
        // No building to unlock.
    }

    protected override void UnlockCrafting()
    {
        base.UnlockCrafting();
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
