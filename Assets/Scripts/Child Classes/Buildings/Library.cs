using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Building
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
