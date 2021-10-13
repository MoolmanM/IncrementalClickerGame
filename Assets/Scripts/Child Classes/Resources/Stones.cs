using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stones : Resource
{
    private Resource _resource;

    void Awake()
    {
        _resource = GetComponent<Resource>();
        Resources.Add(Type, _resource);
        SetInitialValues();
    }
    void Update()
    {
        UpdateResources();
    }
}
