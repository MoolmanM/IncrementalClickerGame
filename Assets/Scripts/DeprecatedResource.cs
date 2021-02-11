using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DeprecatedResource
{
    public string name;
    public Objects objects;
    public InputValues inputValues;
    
    [System.Serializable]
    public class Objects
    {
        public GameObject mainPanel;
        public TMP_Text resourceAmountText, resourcePerSecondText, resourceStorageText;
    }

    [System.Serializable]
    public class InputValues
    {
        public float resourceMaxStorage, resourceAmount;
    }
}
