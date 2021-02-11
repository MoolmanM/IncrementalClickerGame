using UnityEngine;
using TMPro;
using System;

public class BuildingUI
{
    internal Action<ResourceType> OnBuildClicked;

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

    public void RegisterEvents(Building building)
    {

    }
}
