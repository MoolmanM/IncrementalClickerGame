using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using JetBrains.Annotations;

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
    [Button]
    private void Calculate(int n, float b, float r, int k, int c)
    {
        for (int i = 0; i < 25; i++)
        {
            
        }
        test = 10 * Mathf.Pow(costMultiplier, 24) / 2;
        Debug.Log(test);
    }

}