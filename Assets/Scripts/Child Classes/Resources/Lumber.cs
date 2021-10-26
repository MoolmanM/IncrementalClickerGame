    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lumber : Resource
{
    private Resource _resource;

    void Awake()
    {
        _resource = GetComponent<Resource>();
        Resources.Add(Type, _resource);
        isUnlocked = true;
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        SetInitialValues();
    }
}
