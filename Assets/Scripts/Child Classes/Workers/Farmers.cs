using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmers : Worker
{
    private Worker _worker;

    void Awake()
    {
        _worker = GetComponent<Worker>();
        Workers.Add(Type, _worker);
        SetInitialValues();
    }
}
