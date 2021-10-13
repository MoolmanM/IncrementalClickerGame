using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class PotatoField : Building
{
    private Building _building;

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
    }
}
