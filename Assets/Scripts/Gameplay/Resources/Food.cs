using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



public class Food : Resource
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


