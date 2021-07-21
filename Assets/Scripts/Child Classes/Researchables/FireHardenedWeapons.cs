using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHardenedWeapons : Researchable
{
    private Researchable _researchable;

    // Unlock after having 100 knowledge.
    // This will increase the efficiency of hunting.

    // I could also unlock maybe the craft of fire hardened spear.
    // And then could also craft it 
    // Could also introduce coal?
    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);

        SetInitialValues();
    }
    void Start()
    {
        SetDescriptionText("Enables crafting of stone tools.");
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
