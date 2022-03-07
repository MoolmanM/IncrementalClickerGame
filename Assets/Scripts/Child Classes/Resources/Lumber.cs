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
        isUnlocked = true;
        objMainPanel.SetActive(true);
        canvas.enabled = true;
    }
}
