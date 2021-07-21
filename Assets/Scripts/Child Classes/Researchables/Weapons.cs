using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Researchable
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
        SetDescriptionText("Unlocks spear for hunting.");
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
