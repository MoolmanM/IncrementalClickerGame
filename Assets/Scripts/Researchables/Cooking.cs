using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking : Researchable
{
    private Researchable _researchable;

    // Unlock after having 100 knowledge.
    // Cooking will probably just increase the food production per second amount.
    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);
        _timeToCompleteResearch = 60;

        SetInitialValues();
    }
    void Start()
    {
        SetDescriptionText("Increases food production.");
    }
    protected override void UnlockBuilding()
    {
        // No building to unlock.
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
