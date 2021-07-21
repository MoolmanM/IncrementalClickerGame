using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelting : Researchable
{
    private Researchable _researchable;


    // Unlock after having 100 knowledge.
    // Will get back to this later.
    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);

        SetInitialValues();

    }
    void Start()
    {
        SetDescriptionText("Enables smelting ores into metals.");
    }
    void Update()
    {
        UpdateResearchTimer();
        UpdateResourceCosts();
    }
}
