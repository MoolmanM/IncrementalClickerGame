using System.Collections.Generic;
using UnityEngine;

public class LumberMill : Building
{
    private Building _building;

    public float test;

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
    }
}