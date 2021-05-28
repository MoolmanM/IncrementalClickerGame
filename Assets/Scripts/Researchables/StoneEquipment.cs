using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEquipment : Researchable
{
    private Researchable _researchable;

    // Unlock after having 100 knowledge.
    private void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);
        _timeToCompleteResearch = 60;
        _craftingTypesToModify = new CraftingType[3];
        _craftingTypesToModify[0] = CraftingType.StoneAxe;
        _craftingTypesToModify[1] = CraftingType.StoneHoe;
        _craftingTypesToModify[2] = CraftingType.StonePickaxe;

        SetInitialValues();
        
    }
    private void Start()
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
    private void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
