using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    public string name;
    public Objects objects;
    public List<ResourceCosts> resourceCosts = new List<ResourceCosts>();
    public ValuesToEnter valuesToEnter;
    public ProgressFillValues progressFillValues;


    [System.Serializable]
    public class ResourceCosts
    {
        public string resourceName;
        public float costAmount;
        public GameObject costNameText, costAmountText;
    }

    //[System.Serializable]
    public class ProgressFillValues
    {
        public float[] maxValuesArray;
        public float[] currentValuesArray;
    }

    [System.Serializable]
    public class Objects
    {
        public UnityEngine.UI.Image progressBar;
        public GameObject mainPanel, body, buildingNameText, descriptionText, particleEffect;
    }

    [System.Serializable]
    public class ValuesToEnter
    {
        public string descriptionString;
        public float buildingResourceMultiplier, buildingCostMultiplier;
        public int buildingAmount;
    }
}
