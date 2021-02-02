﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public List<CraftingItem> craftingItemsList = new List<CraftingItem>();
    public List<Building> buildingList = new List<Building>();
    public List<Resource> resourceList = new List<Resource>();
    //private bool craftedWoodenHoe, craftedWoodenAxe, craftedWoodenPickaxe;
    public float globalMultiplier;
    public GameObject seasonObject;
    private string seasonText;
    private int day, year, seasonCount;
    public int availableWorkers, maxWorkers;
    public GameObject availableWorkerObject;
    public ParticleSystem particleCompletion;
    //Multiply every single resource with globalMultiplier, for in the future for testing and the player, when they watch ads.

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The GameManagger is NULL.");
            return _instance;
        }
    }
    private void Awake()
    {

        _instance = this;
    }
    private void OnValidate()
    {
        craftingItemsList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[1].resourceAmount + "/" + craftingItemsList[0].resourceCosts[0].costAmount;
        craftingItemsList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[1].resourceAmount + "/" + craftingItemsList[1].resourceCosts[0].costAmount;
        craftingItemsList[2].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[1].resourceAmount + "/" + craftingItemsList[2].resourceCosts[0].costAmount;
        buildingList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[0].resourceAmount + "/" + buildingList[0].resourceCosts[0].costAmount;
        buildingList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[1].resourceAmount + "/" + buildingList[1].resourceCosts[0].costAmount;
        buildingList[2].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[0].resourceAmount + "/" + buildingList[2].resourceCosts[0].costAmount;
        buildingList[0].description.GetComponent<TextMeshProUGUI>().text = "Increases food yield by " + buildingList[0].buildingResourceMultiplier + " /sec";
        buildingList[1].description.GetComponent<TextMeshProUGUI>().text = "Increases stick gathering by " + buildingList[1].buildingResourceMultiplier + " /sec";
        buildingList[2].description.GetComponent<TextMeshProUGUI>().text = "Increases stone gathering by " + buildingList[2].buildingResourceMultiplier + " /sec";
        buildingList[0].resourceCosts[0].resourceNameText.GetComponent<TextMeshProUGUI>().text = buildingList[0].resourceCosts[0].resourceName;
        buildingList[1].resourceCosts[0].resourceNameText.GetComponent<TextMeshProUGUI>().text = buildingList[1].resourceCosts[0].resourceName;
        buildingList[2].resourceCosts[0].resourceNameText.GetComponent<TextMeshProUGUI>().text = buildingList[2].resourceCosts[0].resourceName;
    }
    public void Start()
    {
        InvokeRepeating("UpdateResources", 0, (float)0.1);
        InvokeRepeating("UpdateDay", 0, (float)5);
    }
    public void OnGatherPotatoes()
    {
        if (resourceList[0].resourceAmount >= resourceList[0].resourceMaxStorage)
        {
            resourceList[0].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}", resourceList[0].resourceMaxStorage);
        }
        else
        {
            resourceList[0].resourceAmount++;
            resourceList[0].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}", resourceList[0].resourceAmount);
        }
        
        buildingList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[0].resourceAmount, buildingList[0].resourceCosts[0].costAmount);
    }
    public void OnGatherSticks()
    {
        craftingItemsList[1].mainPanel.SetActive(true);
        if (resourceList[1].resourceAmount >= resourceList[1].resourceMaxStorage)
        {
            resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}", resourceList[0].resourceMaxStorage);
        }
        else
        {
            resourceList[1].resourceAmount++;
            resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}", resourceList[1].resourceAmount);
        }
        buildingList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].resourceAmount, buildingList[1].resourceCosts[0].costAmount);
        buildingList[2].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].resourceAmount, buildingList[2].resourceCosts[0].costAmount);
        GetCurrentFill(resourceList[1].resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount, craftingItemsList[0].progressBar);
    }
    public void OnCraftingWoodenHoe()
    {
        if (resourceList[1].resourceAmount >= craftingItemsList[0].resourceCosts[0].costAmount)
        {
            craftingItemsList[0].craftName.GetComponent<TextMeshProUGUI>().text = "Wooden Hoe (Complete)";
            resourceList[1].resourceAmount -= craftingItemsList[0].resourceCosts[0].costAmount;
            resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", resourceList[1].resourceAmount);
            buildingList[0].mainPanel.SetActive(true);        
        }
        else
        {
            //Not enough 'insert stick name variable'!
        }

    }
    public void OnCraftingWoodenAxe()
    {
        if (resourceList[1].resourceAmount >= craftingItemsList[1].resourceCosts[0].costAmount)
        {
            craftingItemsList[1].craftName.GetComponent<TextMeshProUGUI>().text = "Wooden Axe (Complete)";
            resourceList[1].resourceAmount -= craftingItemsList[0].resourceCosts[0].costAmount;
            resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", resourceList[1].resourceAmount);
            buildingList[1].mainPanel.SetActive(true);          
        }
        else
        {
            //Not enough 'insert stick name variable'!
        }
    }
    public void OnCraftingWoodenPickaxe()
    {
        if (resourceList[1].resourceAmount >= craftingItemsList[2].resourceCosts[0].costAmount)
        {
            craftingItemsList[2].craftName.GetComponent<TextMeshProUGUI>().text = "Wooden Pickaxe (Complete)";
            resourceList[1].resourceAmount -= craftingItemsList[2].resourceCosts[0].costAmount;
            resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", resourceList[1].resourceAmount);
            buildingList[2].mainPanel.SetActive(true);
        }
        else
        {
            //Not enough 'insert stick name variable'!
        }
    }
    public void BuildPotatoField()
    {
        if (resourceList[0].resourceAmount >= buildingList[0].resourceCosts[0].costAmount)
        {            
            buildingList[0].buildingAmount++;          
            resourceList[0].resourceAmount = resourceList[0].resourceAmount - buildingList[0].resourceCosts[0].costAmount;
            buildingList[0].resourceCosts[0].costAmount = buildingList[0].resourceCosts[0].costAmount * Mathf.Pow(buildingList[0].buildingCostMultiplier, buildingList[0].buildingAmount);
            buildingList[0].buildingName.GetComponent<TextMeshProUGUI>().text = string.Format("Potato Field ({0})", buildingList[0].buildingAmount);
            buildingList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[0].resourceAmount, buildingList[0].resourceCosts[0].costAmount);
        }
        else
        {
            Debug.Log("Not enough food!");
        }
    }
    public void BuildWoodlot()
    {
        if (resourceList[1].resourceAmount >= buildingList[1].resourceCosts[0].costAmount)
        {
            buildingList[1].buildingAmount++;
            resourceList[1].resourceAmount = resourceList[1].resourceAmount - buildingList[1].resourceCosts[0].costAmount;
            buildingList[1].resourceCosts[0].costAmount = buildingList[1].resourceCosts[0].costAmount * Mathf.Pow(buildingList[1].buildingCostMultiplier, buildingList[1].buildingAmount);
            buildingList[1].buildingName.GetComponent<TextMeshProUGUI>().text = string.Format("Wood-lot ({0})", buildingList[1].buildingAmount);
            buildingList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[1].resourceAmount, buildingList[1].resourceCosts[0].costAmount);
        }
        else
        {
            Debug.Log("Not enough sticks!");
        }
    }
    public void BuildDigSite()
    {
        if (resourceList[1].resourceAmount >= buildingList[2].resourceCosts[0].costAmount)
        {
            buildingList[2].buildingAmount++;
            resourceList[1].resourceAmount = resourceList[1].resourceAmount - buildingList[2].resourceCosts[0].costAmount;
            buildingList[2].resourceCosts[0].costAmount = buildingList[2].resourceCosts[0].costAmount * Mathf.Pow(buildingList[2].buildingCostMultiplier, buildingList[2].buildingAmount);
            buildingList[2].buildingName.GetComponent<TextMeshProUGUI>().text = string.Format("Dig Site ({0})", buildingList[2].buildingAmount);
            buildingList[2].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[1].resourceAmount, buildingList[2].resourceCosts[0].costAmount);
        }
        else
        {
            Debug.Log("Not enough sticks!");
        }
    }
    public void BuildMakeshiftBed()
    {
        Debug.Log("Clicked");
        if ((resourceList[1].resourceAmount >= buildingList[3].resourceCosts[0].costAmount) && (resourceList[2].resourceAmount >= buildingList[3].resourceCosts[0].costAmount))
        {
            Debug.Log("Reached here");
            buildingList[3].buildingAmount++;
            availableWorkers++;
            maxWorkers++;
            availableWorkerObject.GetComponent<TextMeshProUGUI>().text = string.Format("Available Workers: [{0}/{1}]", availableWorkers, maxWorkers);
            resourceList[1].resourceAmount = resourceList[1].resourceAmount - buildingList[3].resourceCosts[0].costAmount;
            resourceList[2].resourceAmount = resourceList[1].resourceAmount - buildingList[3].resourceCosts[0].costAmount;
            buildingList[3].resourceCosts[0].costAmount = buildingList[3].resourceCosts[0].costAmount * Mathf.Pow(buildingList[3].buildingCostMultiplier, buildingList[3].buildingAmount);
            buildingList[3].resourceCosts[1].costAmount = buildingList[3].resourceCosts[1].costAmount * Mathf.Pow(buildingList[3].buildingCostMultiplier, buildingList[3].buildingAmount);
            buildingList[3].buildingName.GetComponent<TextMeshProUGUI>().text = string.Format("Makeshift Bed ({0})", buildingList[3].buildingAmount);
            buildingList[3].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[1].resourceAmount, buildingList[3].resourceCosts[0].costAmount);
            buildingList[3].resourceCosts[1].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[2].resourceAmount, buildingList[3].resourceCosts[1].costAmount);
        }
        else
        {
            Debug.Log("Not enough sticks && stones");
        }
    }
    public void GetCurrentFill(float current, float max, Image progressCircle)
    {
        float fillAmount = current / max;   
        progressCircle.fillAmount = fillAmount;

        if (progressCircle.fillAmount == 1)
        {
            particleCompletion.Play();
        }
    }
    public void GetCurrentFillTwo(float current, float current1, float max, float max1, Image progressCircle)
    {
        float fillAmount = current / max;
        float fillAmount1 = current1 / max1;
        if (fillAmount >= 1)
        {
            fillAmount = 1;
        }

        if (fillAmount1 >= 1)
        {
            fillAmount1 = 1;
        }
            progressCircle.fillAmount = (fillAmount + fillAmount1) / 2;
    }
    public void UpdateResources()
    {
        #region Food Resource
        float potatoFieldMultiplier = (float)(buildingList[0].buildingAmount * buildingList[0].buildingResourceMultiplier) + (WorkerManager.farmerWorkMultiplier * WorkerManager.farmerWorkerAmount);
        if (resourceList[0].resourceAmount >= (resourceList[0].resourceMaxStorage - potatoFieldMultiplier))
        {
            resourceList[0].resourceAmount = resourceList[0].resourceMaxStorage;
            resourceList[0].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[0].resourceMaxStorage);
        }
        else
        {
            resourceList[0].resourceAmount += potatoFieldMultiplier;
            resourceList[0].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[0].resourceAmount);
        }
        resourceList[0].resourcePerSecond.GetComponent<TextMeshProUGUI>().text = string.Format("+{0:#0.00} /sec", potatoFieldMultiplier);
        #endregion

        #region Wood Resource
        float woodlotMultiplier = (float)(buildingList[1].buildingAmount * buildingList[1].buildingResourceMultiplier) + (WorkerManager.woodcutterWorkMutliplier * WorkerManager.woodcutterWorkerAmount);
        if (resourceList[1].resourceAmount >= (resourceList[1].resourceMaxStorage - woodlotMultiplier))
        {
            resourceList[1].resourceAmount = resourceList[1].resourceMaxStorage;
            resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}", resourceList[1].resourceMaxStorage);
        }
        else
        {
            resourceList[1].resourceAmount += woodlotMultiplier;
            resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[1].resourceAmount);
        }
        resourceList[1].resourcePerSecond.GetComponent<TextMeshProUGUI>().text = string.Format("+{0:#0.00} /sec", woodlotMultiplier);
        #endregion

        #region Stone Resource
        float digSiteMultiplier = (float)(buildingList[2].buildingAmount * buildingList[2].buildingResourceMultiplier) + (WorkerManager.minerWorkMultiplier * WorkerManager.minerWorkerAmount);
        if (resourceList[2].resourceAmount >= (resourceList[2].resourceMaxStorage - digSiteMultiplier))
        {
            resourceList[2].resourceAmount = resourceList[2].resourceMaxStorage;
            resourceList[2].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[2].resourceMaxStorage);
        }
        else
        {
            resourceList[2].resourceAmount += digSiteMultiplier;
            resourceList[2].resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[2].resourceAmount);
        }
        resourceList[2].resourcePerSecond.GetComponent<TextMeshProUGUI>().text = string.Format("+{0:#0.00} /sec", digSiteMultiplier);
        #endregion

        #region Update Resource Costs 

        craftingItemsList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00} / {1}", resourceList[1].resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount);
        craftingItemsList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00} / {1}", resourceList[1].resourceAmount, craftingItemsList[1].resourceCosts[0].costAmount);
        craftingItemsList[2].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00} / {1}", resourceList[1].resourceAmount, craftingItemsList[2].resourceCosts[0].costAmount);
        buildingList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[0].resourceAmount, buildingList[0].resourceCosts[0].costAmount);
        buildingList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].resourceAmount, buildingList[1].resourceCosts[0].costAmount);
        buildingList[2].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].resourceAmount, buildingList[2].resourceCosts[0].costAmount);
        buildingList[3].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].resourceAmount, buildingList[3].resourceCosts[0].costAmount);
        buildingList[3].resourceCosts[1].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[2].resourceAmount, buildingList[3].resourceCosts[1].costAmount);

        #endregion

        #region GetFills
        GetCurrentFill(resourceList[0].resourceAmount, buildingList[0].resourceCosts[0].costAmount, buildingList[0].progressBar);
        GetCurrentFill(resourceList[1].resourceAmount, buildingList[1].resourceCosts[0].costAmount, buildingList[1].progressBar);
        GetCurrentFill(resourceList[1].resourceAmount, buildingList[2].resourceCosts[0].costAmount, buildingList[2].progressBar);
        GetCurrentFillTwo(resourceList[1].resourceAmount, resourceList[2].resourceAmount, buildingList[3].resourceCosts[0].costAmount, buildingList[3].resourceCosts[1].costAmount, buildingList[3].progressBar);
        GetCurrentFill(resourceList[1].resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount, craftingItemsList[0].progressBar);
        GetCurrentFill(resourceList[1].resourceAmount, craftingItemsList[1].resourceCosts[0].costAmount, craftingItemsList[1].progressBar);
        GetCurrentFill(resourceList[1].resourceAmount, craftingItemsList[2].resourceCosts[0].costAmount, craftingItemsList[2].progressBar);
        #endregion
    }
    public void UpdateDay()
    {
        day++;
        if (seasonCount == 0)
        {
            seasonText = "Spring";
        }    
        else if (seasonCount == 1)
        {
            seasonText = "Summer";
        }
        else if (seasonCount == 2)
        {
            seasonText = "Fall";
        }
        else if (seasonCount == 3)
        {
            seasonText = "Winter";
        }
        else
        {
            seasonCount = 0;
        }

        if (day == 100 && seasonCount == 3)
        {
            year++;
            seasonCount++;
            day = 0;
        }

        else if (day == 100)
        {
            seasonCount++;
            day = 0;
        }

        seasonObject.GetComponent<TextMeshProUGUI>().text = string.Format("Year {0} - {1}, day {2}", year, seasonText, day);
        

        
    }
}
