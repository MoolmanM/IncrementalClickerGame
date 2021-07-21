using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualEnergyProduction : Researchable
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
        SetDescriptionText("Enables you to craft a hamster wheel for energy production.");
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
