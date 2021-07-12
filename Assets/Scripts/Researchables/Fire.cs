using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Researchable
{
    private Researchable _researchable;

    // Fire should unlock Cooking, Smelting and FireHardenedWeapons
    // Fire Hardened Weapons should only be unlocked if weapons and fire has been researched.
    // Unlock after having 100 knowledge.
    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);
        _timeToCompleteResearch = 60;

        _researchTypesToModify = new ResearchType[2];
        _researchTypesToModify[0] = ResearchType.Cooking;
        _researchTypesToModify[1] = ResearchType.FireHardenedWeapons;

        SetInitialValues();
    }
    void Start()
    {
        SetDescriptionText("Unlock new important technologies, such as cooking and more.");
    }
    protected override void UnlockBuilding()
    {
        // No Building
    }
    protected override void UnlockCrafting()
    {
        // No Crafting
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
