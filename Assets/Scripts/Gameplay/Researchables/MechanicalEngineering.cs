using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalEngineering : Researchable
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
        SetDescriptionText("The gears are starting to turn");
    }
}
