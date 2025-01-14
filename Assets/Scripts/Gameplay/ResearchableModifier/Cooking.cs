using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking : ResearchableModifier
{
    private Researchable _researchable;

    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);

        SetInitialValues();
    }
}
