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
        SetInitialValues();
        IsUnlocked = true;
        ObjMainPanel.SetActive(true);
        Canvas.enabled = true;
    }
}
