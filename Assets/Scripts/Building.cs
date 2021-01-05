using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    public string name;
    public float buildingResourceMultiplier, buildingCostMultiplier;
    public int buildingAmount;
    public UnityEngine.UI.Image progressBar;
    public GameObject mainPanel, body, buildingName, description;
    public List<ResourceCosts> resourceCosts = new List<ResourceCosts>();

    [System.Serializable]
    public class ResourceCosts
    {
        public string resourceName;
        public float costAmount;
        public GameObject resourceNameText, costAmountText;
    }
}
