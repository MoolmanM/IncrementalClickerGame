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

        SetInitialValues();     
    }
    void Start()
    {      
        SetDescriptionText("Enables crafting of stone tools.");
    }
    protected override void Update()
    {
        base.Update();
        UpdateResearchTimer();
    }
}
