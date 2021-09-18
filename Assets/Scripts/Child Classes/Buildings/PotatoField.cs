using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotatoField : Building
{
    private Building _building;

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
    }
    void Start()
    {  
        SetDescriptionText();
    }
}
