using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource
{
    public string name;
    public Objects objects;
    public ValuesToEnter valuesToEnter;
    
    [System.Serializable]
    public class Objects
    {
        public GameObject resourceAmountText, resourcePerSecondText, resourceStorageText, mainPanel;
    }

    [System.Serializable]
    public class ValuesToEnter
    {
        public float resourceMaxStorage, resourceAmount;
    }
}
