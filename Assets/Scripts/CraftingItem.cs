using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingItem
{
    public string name;
    public Objects objects;
    public List<ResourceCosts> resourceCosts = new List<ResourceCosts>();
    public ProgressFillValues progressFillValues;


    [System.Serializable]
    public class Objects
    {
        public GameObject mainPanel, headerText, body, descriptionText, particleEffect;
        public UnityEngine.UI.Image progressBar;
    }

    //[System.Serializable]
    public class ProgressFillValues
    {
        public float[] maxValuesArray;
        public float[] currentValuesArray;
    }

    [System.Serializable]
    public class ResourceCosts
    {
        public string resourceName;
        public float costAmount;
        public GameObject costNameText, costAmountText;
    }

}
