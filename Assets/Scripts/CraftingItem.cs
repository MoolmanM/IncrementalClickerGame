using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        public GameObject mainPanel, body, particleEffect;
        public TMP_Text headerText, descriptionText;
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
        public TMP_Text costNameText, costAmountText;
    }

}
