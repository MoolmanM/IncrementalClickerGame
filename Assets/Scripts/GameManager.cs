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
    private float globalMultiplier;
    public GameObject seasonObject;

    //Multiply every single resource with globalMultiplier, for in the future for testing and the player, when they watch ads.
    public void Start()
    {
        //InvokeRepeating("UpdateResources", 0, (float)0.1);
        //InvokeRepeating("UpdateDay", 0, (float)5);
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
    //Notice the two floats for worker amount and worker multiplier, can maybe turn that into only one float in the workerManager code.
    public void CalculateResource(int thisBuildingAmount, float thisBuildingMultipler, float thisWorkerMultipler, float thisWorkerAmount, float thisMaxStorage, float thisResourceAmount)
    {
        float thisResourceMultiplier = (thisBuildingMultipler * thisBuildingAmount) + (thisWorkerMultipler * thisWorkerAmount);
        if (thisResourceAmount >= (thisMaxStorage - thisResourceMultiplier))
        {
            thisResourceAmount = thisMaxStorage;        
        }
        else
        {
            thisResourceAmount += thisResourceMultiplier;         
        }
    }
    public void UpdateResources()
    {
        //UIManager.Instance.UpdateCostAmountTexts();
        CalculateResource(buildingList[0].inputValues.buildingAmount, buildingList[0].inputValues.buildingResourceMultiplier, WorkerManager.farmerWorkMultiplier, WorkerManager.farmerWorkerAmount, resourceList[0].inputValues.resourceMaxStorage, resourceList[0].inputValues.resourceAmount);
        CalculateResource(buildingList[1].inputValues.buildingAmount, buildingList[1].inputValues.buildingResourceMultiplier, WorkerManager.farmerWorkMultiplier, WorkerManager.farmerWorkerAmount, resourceList[1].inputValues.resourceMaxStorage, resourceList[1].inputValues.resourceAmount);
        CalculateResource(buildingList[2].inputValues.buildingAmount, buildingList[2].inputValues.buildingResourceMultiplier, WorkerManager.farmerWorkMultiplier, WorkerManager.farmerWorkerAmount, resourceList[2].inputValues.resourceMaxStorage, resourceList[2].inputValues.resourceAmount);

        #region GetFills
        /*
        for (int b = 0; b < buildingList.Count; b++)
        {
            buildingList[b].objects.progressBar.fillAmount = GetCurrentFill(buildingList[b].progressFillValues.maxValuesArray, buildingList[b].progressFillValues.currentValuesArray, buildingList[b].objects.particleEffect);
            for (int r = 0; r < resourceList.Count; r++)
            {
                for (int i = 0; i < buildingList[b].resourceCosts.Count; i++)
                {
                    for (int c = 0; c < craftingItemsList.Count; c++)
                    {
                        craftingItemsList[c].objects.progressBar.fillAmount = GetCurrentFill(craftingItemsList[c].progressFillValues.maxValuesArray, craftingItemsList[c].progressFillValues.currentValuesArray, craftingItemsList[c].objects.particleEffect);
                    }
                    
                }
            }
        }
        */

        
        /*(resourceList[0].inputValues.resourceAmount, buildingList[0].resourceCosts[0].costAmount, buildingList[0].objects.progressBar, buildingList[0].objects.particleEffect);
        GetCurrentFill(resourceList[1].inputValues.resourceAmount, buildingList[1].resourceCosts[0].costAmount, buildingList[1].objects.progressBar, buildingList[1].objects.particleEffect);
        GetCurrentFill(resourceList[1].inputValues.resourceAmount, buildingList[2].resourceCosts[0].costAmount, buildingList[2].objects.progressBar, buildingList[2].objects.particleEffect);
        GetCurrentFillTwo(resourceList[1].inputValues.resourceAmount, resourceList[2].inputValues.resourceAmount, buildingList[3].resourceCosts[0].costAmount, buildingList[3].resourceCosts[1].costAmount, buildingList[3].objects.progressBar);
        GetCurrentFill(resourceList[1].inputValues.resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount, craftingItemsList[0].objects.progressBar, craftingItemsList[0].objects.particleEffect);
        GetCurrentFill(resourceList[1].inputValues.resourceAmount, craftingItemsList[1].resourceCosts[0].costAmount, craftingItemsList[1].objects.progressBar, craftingItemsList[1].objects.particleEffect);
        GetCurrentFill(resourceList[1].inputValues.resourceAmount, craftingItemsList[2].resourceCosts[0].costAmount, craftingItemsList[2].objects.progressBar, craftingItemsList[2].objects.particleEffect);*/
        #endregion
    }
}
