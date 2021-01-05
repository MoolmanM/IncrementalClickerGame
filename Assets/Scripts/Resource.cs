using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource
{
    public string name;
    public float resourceMaxStorage, resourceAmount;
    public GameObject resourceAmountText, resourcePerSecond, resourceStorageText, mainPanel;
}
