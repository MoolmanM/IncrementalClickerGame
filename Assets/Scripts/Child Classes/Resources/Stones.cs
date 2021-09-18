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
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        SetInitialValues();
    }
    void Update()
    {
        UpdateResources();
    }
}
