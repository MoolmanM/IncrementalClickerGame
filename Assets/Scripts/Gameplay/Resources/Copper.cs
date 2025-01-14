using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copper : Resource
{
    private Resource _resource;

    void Awake()
    {
        _resource = GetComponent<Resource>();
        Resources.Add(Type, _resource);
        SetInitialValues();
    }
}