using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class TestMaths : MonoBehaviour
{
    public float prestigeCurrency, lifetimeCurrency, startingLifetimeCurrency;
    public TMP_Text txtTopCoinAmount;
    public float foodWeight, stoneWeight, lumberWeight, knowledgeWeight, leatherWeight, copperWeight, bronzeWeight;
    public uint workerWeight;

    public AutoBot autoBot;
    public Prestige prestige;

    public List<float> previousPrestige = new List<float>();
    public TMP_Text txtCoinCurrent, txtCoinPrevious;
    public bool isCoinPanelOpen;
    public float multiplierPerCoin;

    public float testLargeNumber;

    public bool isFillingResources, isCalculatingPrestigeCoins;
    public float maxStorageAmount;

    [Button]
    public void CalculatePrestigeCurrency()
    {
        if (isCalculatingPrestigeCoins)
        {
            foodWeight = Resource.Resources[ResourceType.Food].trackedAmount * maxStorageAmount / Resource.Resources[ResourceType.Food].baseStorageAmount ;
            stoneWeight = Resource.Resources[ResourceType.Stone].trackedAmount * maxStorageAmount / Resource.Resources[ResourceType.Stone].baseStorageAmount;
            lumberWeight = Resource.Resources[ResourceType.Lumber].trackedAmount * maxStorageAmount / Resource.Resources[ResourceType.Lumber].baseStorageAmount;
            knowledgeWeight = Resource.Resources[ResourceType.Knowledge].trackedAmount * maxStorageAmount / Resource.Resources[ResourceType.Knowledge].baseStorageAmount;
            leatherWeight = Resource.Resources[ResourceType.Leather].trackedAmount * maxStorageAmount / Resource.Resources[ResourceType.Leather].baseStorageAmount;
            copperWeight = Resource.Resources[ResourceType.Copper].trackedAmount * maxStorageAmount / Resource.Resources[ResourceType.Copper].baseStorageAmount;
            bronzeWeight = Resource.Resources[ResourceType.Bronze].trackedAmount * maxStorageAmount / Resource.Resources[ResourceType.Bronze].baseStorageAmount;
            workerWeight = Worker.trackedWorkerCount * 1000;

            lifetimeCurrency = foodWeight + stoneWeight + lumberWeight + workerWeight + knowledgeWeight + leatherWeight + copperWeight + bronzeWeight;


            prestigeCurrency = (lifetimeCurrency / Mathf.Pow((testLargeNumber / 9), 0.5f)) - (startingLifetimeCurrency / Mathf.Pow((testLargeNumber / 9), 0.5f));

            //prestigeCurrency = largeNumber * Mathf.Sqrt(lifetimeCurrency / Mathf.Pow(10, 15));
            txtTopCoinAmount.text = NumberToLetter.FormatNumber(prestigeCurrency);
        }

    }

    [Button]
    public void OnPrestige()
    {
        //float newNumber = testLargeNumber / 9;
        startingLifetimeCurrency = lifetimeCurrency;
        //lifetimeCurrency = (testLargeNumber / 9) * Mathf.Pow(prestigeCurrency, 2);
        foreach (var building in Building.Buildings)
        {
            building.Value.prestigeAllMultiplierAddition = prestigeCurrency * multiplierPerCoin; 
        }

        prestige.OnResetGame();
        previousPrestige.Add(prestigeCurrency);
        
        autoBot.craftedAmount = 0;
        autoBot.craftUnlockedAmount = 0;

        UpdatePreviousCoinText();
    }
    public void OnOpenPrestigeCoins()
    {
        UpdatePreviousCoinText();
        isCoinPanelOpen = true;
    }
    public void OnClosePretigeCoins()
    {
        isCoinPanelOpen = false;
    }
    private void UpdatePreviousCoinText()
    {
        for (int i = 0; i < previousPrestige.Count; i++)
        {
            string oldString;

            if (i > 0)
            {
                oldString = txtCoinPrevious.text;

                txtCoinPrevious.text = string.Format("{0} \nPrestige {1}: {2}.", oldString, i, NumberToLetter.FormatNumber(previousPrestige[i]));
            }
            else
            {
                txtCoinPrevious.text = string.Format("Prestige {0}: {1}.", i, NumberToLetter.FormatNumber(previousPrestige[i]));
            }
        }
    }
    private void UpdateCurrentCoinText()
    {
        if (isCoinPanelOpen)
        {
            txtCoinCurrent.text = NumberToLetter.FormatNumber(prestigeCurrency);
        }
        
    }
    public void Update()
    {
        CalculatePrestigeCurrency();
        FillAllResources();
        UpdateCurrentCoinText();
    }
    [Button]
    public void FillAllResources()
    {
        if (isFillingResources)
        {
            foreach (var resource in Resource.Resources)
            {
                if (resource.Value.IsUnlocked)
                {
                    resource.Value.trackedAmount += resource.Value.storageAmount - resource.Value.amount;
                    resource.Value.amount += resource.Value.storageAmount - resource.Value.amount;
                }
            }
        }

    }
}
