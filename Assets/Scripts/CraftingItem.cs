using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingItem
{
    public string name;
    public UnityEngine.UI.Image progressBar;
    public GameObject mainPanel, craftName, body, description;
    public List<ResourceCosts> resourceCosts = new List<ResourceCosts>();



    [System.Serializable]
    public class ResourceCosts
    {
        public string resourceName;
        public float costAmount;
        public GameObject costAmountText;      
    }
}
