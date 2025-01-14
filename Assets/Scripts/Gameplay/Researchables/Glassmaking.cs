using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glassmaking : Researchable
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
        SetDescriptionText("Don't break it");
    }
}
