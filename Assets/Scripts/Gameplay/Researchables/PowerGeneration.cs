
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGeneration : Researchable
{
    private Researchable _researchable;
    public Energy energy;

    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);

        SetInitialValues();

        if (isResearched)
        {
            energy.objIconPanel.SetActive(true);
        }
        else
        {
            energy.objIconPanel.SetActive(false);
        }
    }
    protected override void Researched()
    {
        base.Researched();

        energy.objIconPanel.SetActive(true);
    }
    void Start()
    {
        SetDescriptionText("Enables generation of power");
    }
}
