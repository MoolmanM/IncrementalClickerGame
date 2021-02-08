using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Building
{
    public string name;
    public Objects objects;
    public List<ResourceCosts> resourceCosts = new List<ResourceCosts>();
    public InputValues inputValues;
    public ProgressFillValues progressFillValues;


    [System.Serializable]
    public class ResourceCosts
    {
        public string resourceName;
        public float costAmount;
        public TMP_Text costNameText, costAmountText;
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
        public GameObject mainPanel, body, particleEffect;
        public TMP_Text buildingNameText, descriptionText;
    }

    [System.Serializable]
    public class InputValues
    {
        public string descriptionString;
        public float buildingResourceMultiplier, buildingCostMultiplier;
        public int buildingAmount;
    }
}
