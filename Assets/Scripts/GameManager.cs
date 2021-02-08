using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoSingleton<GameManager>
{
    /*
    private float _timer;

    private void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;
            // run logic
        }
    }
    */

    public List<CraftingItem> craftingItemsList = new List<CraftingItem>();
    public List<Building> buildingList = new List<Building>();
    public List<Resource> resourceList = new List<Resource>();
    //private bool craftedWoodenHoe, craftedWoodenAxe, craftedWoodenPickaxe;
    public float globalMultiplier;
    public GameObject seasonObject;

    public int availableWorkers, maxWorkers;
    public GameObject availableWorkerObject;
    //Multiply every single resource with globalMultiplier, for in the future for testing and the player, when they watch ads.

    private void UpdateResourceTextsCode()
    {
        for (int b = 0; b < buildingList.Count; b++)
        {
            buildingList[b].progressFillValues.currentValuesArray = new float[buildingList[b].resourceCosts.Count];
            for (int i = 0; i < buildingList[b].progressFillValues.currentValuesArray.Length; i++)
            {
                for (int r = 0; r < resourceList.Count; r++)
                {
                    if (resourceList[r].name == buildingList[b].resourceCosts[i].resourceName)
                    {
                        buildingList[b].progressFillValues.currentValuesArray[i] = resourceList[r].valuesToEnter.resourceAmount;
                    }
                }

            }
        }

        for (int b = 0; b < buildingList.Count; b++)
        {
            buildingList[b].progressFillValues.maxValuesArray = new float[buildingList[b].resourceCosts.Count];
            for (int i = 0; i < buildingList[b].progressFillValues.maxValuesArray.Length; i++)
            {
                buildingList[b].progressFillValues.maxValuesArray[i] = buildingList[b].resourceCosts[i].costAmount;
            }
        }

        for (int c = 0; c < craftingItemsList.Count; c++)
        {
            for (int i = 0; i < craftingItemsList[c].resourceCosts.Count; i++)
            {
                craftingItemsList[c].resourceCosts[i].costNameText.GetComponent<TextMeshProUGUI>().text = craftingItemsList[c].resourceCosts[i].resourceName;

                for (int r = 0; r < resourceList.Count; r++)
                {
                    craftingItemsList[c].resourceCosts[i].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[r].valuesToEnter.resourceAmount + "/" + craftingItemsList[c].resourceCosts[i].costAmount;

                }
            }

        }

        for (int b = 0; b < buildingList.Count; b++)
        {
            buildingList[b].objects.descriptionText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: {1}/sec", buildingList[b].valuesToEnter.descriptionString, buildingList[b].valuesToEnter.buildingResourceMultiplier);
            for (int i = 0; i < buildingList[b].resourceCosts.Count; i++)
            {
                buildingList[b].resourceCosts[i].costNameText.GetComponent<TextMeshProUGUI>().text = buildingList[b].resourceCosts[i].resourceName;
                for (int r = 0; r < resourceList.Count; r++)
                {
                    buildingList[b].resourceCosts[i].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[r].valuesToEnter.resourceAmount + "/" + buildingList[b].resourceCosts[i].costAmount;
                }
            }
        }

        buildingList[3].objects.descriptionText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", buildingList[3].valuesToEnter.descriptionString);
    }
    private void OnValidate()
    {
        UpdateResourcesCode();
    }
    public void Start()
    {
        InvokeRepeating("UpdateResources", 0, (float)0.1);
        InvokeRepeating("UpdateDay", 0, (float)5);
    }
   
    public float GetCurrentFill(float[] maxValues, float[] currentValues, GameObject thisParticleEffect)
    {
        float add = 0;

        for (int i = 0; i < currentValues.Length; i++)
        {
            add += currentValues[i];
        }

        float div = 0;

        for (int i = 0; i < maxValues.Length; i++)
        {
            div += maxValues[i];
        }
        if (add == div)
        {
            thisParticleEffect.GetComponent<ParticleSystem>().Play();
        }
        return add / div;
    }
    public void UpdateResources()
    {
        UpdateResourceTextsCode();
        #region Food Resource
        float potatoFieldMultiplier = (float)(buildingList[0].valuesToEnter.buildingAmount * buildingList[0].valuesToEnter.buildingResourceMultiplier) + (WorkerManager.farmerWorkMultiplier * WorkerManager.farmerWorkerAmount);
        if (resourceList[0].valuesToEnter.resourceAmount >= (resourceList[0].valuesToEnter.resourceMaxStorage - potatoFieldMultiplier))
        {
            resourceList[0].valuesToEnter.resourceAmount = resourceList[0].valuesToEnter.resourceMaxStorage;
            resourceList[0].objects.resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[0].valuesToEnter.resourceMaxStorage);
        }
        else
        {
            resourceList[0].valuesToEnter.resourceAmount += potatoFieldMultiplier;
            resourceList[0].objects.resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[0].valuesToEnter.resourceAmount);
        }
        resourceList[0].objects.resourcePerSecondText.GetComponent<TextMeshProUGUI>().text = string.Format("+{0:#0.00} /sec", potatoFieldMultiplier);
        #endregion

        #region Wood Resource
        float woodlotMultiplier = (float)(buildingList[1].valuesToEnter.buildingAmount * buildingList[1].valuesToEnter.buildingResourceMultiplier) + (WorkerManager.woodcutterWorkMutliplier * WorkerManager.woodcutterWorkerAmount);
        if (resourceList[1].valuesToEnter.resourceAmount >= (resourceList[1].valuesToEnter.resourceMaxStorage - woodlotMultiplier))
        {
            resourceList[1].valuesToEnter.resourceAmount = resourceList[1].valuesToEnter.resourceMaxStorage;
            resourceList[1].objects.resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}", resourceList[1].valuesToEnter.resourceMaxStorage);
        }
        else
        {
            resourceList[1].valuesToEnter.resourceAmount += woodlotMultiplier;
            resourceList[1].objects.resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[1].valuesToEnter.resourceAmount);
        }
        resourceList[1].objects.resourcePerSecondText.GetComponent<TextMeshProUGUI>().text = string.Format("+{0:#0.00} /sec", woodlotMultiplier);
        #endregion

        #region Stone Resource
        float digSiteMultiplier = (float)(buildingList[2].valuesToEnter.buildingAmount * buildingList[2].valuesToEnter.buildingResourceMultiplier) + (WorkerManager.minerWorkMultiplier * WorkerManager.minerWorkerAmount);
        if (resourceList[2].valuesToEnter.resourceAmount >= (resourceList[2].valuesToEnter.resourceMaxStorage - digSiteMultiplier))
        {
            resourceList[2].valuesToEnter.resourceAmount = resourceList[2].valuesToEnter.resourceMaxStorage;
            resourceList[2].objects.resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[2].valuesToEnter.resourceMaxStorage);
        }
        else
        {
            resourceList[2].valuesToEnter.resourceAmount += digSiteMultiplier;
            resourceList[2].objects.resourceAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#0.00}", resourceList[2].valuesToEnter.resourceAmount);
        }
        resourceList[2].objects.resourcePerSecondText.GetComponent<TextMeshProUGUI>().text = string.Format("+{0:#0.00} /sec", digSiteMultiplier);
        #endregion

        #region Update Resource Costs 

        craftingItemsList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00} / {1:#.00}", resourceList[1].valuesToEnter.resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount);
        craftingItemsList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00} / {1:#.00}", resourceList[1].valuesToEnter.resourceAmount, craftingItemsList[1].resourceCosts[0].costAmount);
        craftingItemsList[2].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00} / {1:#.00}", resourceList[1].valuesToEnter.resourceAmount, craftingItemsList[2].resourceCosts[0].costAmount);
        buildingList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[0].valuesToEnter.resourceAmount, buildingList[0].resourceCosts[0].costAmount);
        buildingList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].valuesToEnter.resourceAmount, buildingList[1].resourceCosts[0].costAmount);
        buildingList[2].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].valuesToEnter.resourceAmount, buildingList[2].resourceCosts[0].costAmount);
        buildingList[3].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].valuesToEnter.resourceAmount, buildingList[3].resourceCosts[0].costAmount);
        buildingList[3].resourceCosts[1].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[2].valuesToEnter.resourceAmount, buildingList[3].resourceCosts[1].costAmount);

        #endregion

        #region GetFills

        for (int b = 0; b < buildingList.Count; b++)
        {
            buildingList[b].objects.progressBar.fillAmount = GetCurrentFill(buildingList[b].progressFillValues.maxValuesArray, buildingList[b].progressFillValues.currentValuesArray, buildingList[b].objects.particleEffect);
            for (int r = 0; r < resourceList.Count; r++)
            {
                for (int i = 0; i < buildingList[b].resourceCosts.Count; i++)
                {
                    for (int c = 0; c < craftingItemsList.Count; c++)
                    {
                        //craftingItemsList[c].objects.progressBar.fillAmount = GetCurrentFill(craftingItemsList[c].progressFillValues.maxValuesArray, craftingItemsList[c].progressFillValues.currentValuesArray, craftingItemsList[c].objects.particleEffect)
                    }
                    
                }
            }
        }

        
        /*(resourceList[0].valuesToEnter.resourceAmount, buildingList[0].resourceCosts[0].costAmount, buildingList[0].objects.progressBar, buildingList[0].objects.particleEffect);
        GetCurrentFill(resourceList[1].valuesToEnter.resourceAmount, buildingList[1].resourceCosts[0].costAmount, buildingList[1].objects.progressBar, buildingList[1].objects.particleEffect);
        GetCurrentFill(resourceList[1].valuesToEnter.resourceAmount, buildingList[2].resourceCosts[0].costAmount, buildingList[2].objects.progressBar, buildingList[2].objects.particleEffect);
        GetCurrentFillTwo(resourceList[1].valuesToEnter.resourceAmount, resourceList[2].valuesToEnter.resourceAmount, buildingList[3].resourceCosts[0].costAmount, buildingList[3].resourceCosts[1].costAmount, buildingList[3].objects.progressBar);
        GetCurrentFill(resourceList[1].valuesToEnter.resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount, craftingItemsList[0].objects.progressBar, craftingItemsList[0].objects.particleEffect);
        GetCurrentFill(resourceList[1].valuesToEnter.resourceAmount, craftingItemsList[1].resourceCosts[0].costAmount, craftingItemsList[1].objects.progressBar, craftingItemsList[1].objects.particleEffect);
        GetCurrentFill(resourceList[1].valuesToEnter.resourceAmount, craftingItemsList[2].resourceCosts[0].costAmount, craftingItemsList[2].objects.progressBar, craftingItemsList[2].objects.particleEffect);*/
        #endregion
    }
}
