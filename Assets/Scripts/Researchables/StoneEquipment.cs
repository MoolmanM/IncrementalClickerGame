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
        SetInitialValues();
    }
    private void Start()
    {
        
        SetDescriptionText("Enables crafting of stone equipment.");
    }
    protected override void Researched()
    {
        base.Researched();
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}
