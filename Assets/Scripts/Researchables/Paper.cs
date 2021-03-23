using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : Researchable
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
        CheckIfUnlocked();
        SetDescriptionText("Enables students to study and gain knowledge.");
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
