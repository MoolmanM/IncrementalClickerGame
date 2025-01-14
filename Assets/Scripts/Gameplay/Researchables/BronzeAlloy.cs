using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronzeAlloy : Researchable
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
        SetDescriptionText("88% copper and 12% tin");
    }
}
