using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneEquipment : Researchable
{
    private Researchable _researchable;

    private void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);
    }
    private void Start()
    {
        SetInitialValues();
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
