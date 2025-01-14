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
        isUnlocked = true;
        objMainPanel.SetActive(true);
        canvas.enabled = true;
    }
}


