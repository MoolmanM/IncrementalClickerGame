using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    public string name, descriptionString;
    public float buildingResourceMultiplier, buildingCostMultiplier;
    public int buildingAmount;
    public UnityEngine.UI.Image progressBar;
    public GameObject mainPanel, body, buildingName, description, particleEffect;
    public List<ResourceCosts> resourceCosts = new List<ResourceCosts>();
    public float[] maxValuesArray;
    public float[] currentValuesArray;

    [System.Serializable]
    public class ResourceCosts
    {
        public string resourceName;
        public float costAmount;
        public GameObject resourceNameText, costAmountText;
    }
}
