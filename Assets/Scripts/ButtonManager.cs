using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public List<CraftingItem> craftingItemsList = new List<CraftingItem>();
    public List<Building> buildingList = new List<Building>();
    public List<Resource> resourceList = new List<Resource>();

    //Create method in UImanager with parameters so I can just call the method lets say UpdateText(And insert the values that needs to be added to the text.)
    

    public void OnGatherPotatoes()
    {
        resourceList[0].inputValues.resourceAmount++;
        /*
        if (resourceList[0].valuesToEnter.resourceAmount >= resourceList[0].valuesToEnter.resourceMaxStorage)
        {
            resourceList[0].objects.resourceAmountText.text = string.Format("{0:#.00}", resourceList[0].valuesToEnter.resourceMaxStorage);
        }
        else
        {
            
            resourceList[0].objects.resourceAmountText.text = string.Format("{0:#.00}", resourceList[0].valuesToEnter.resourceAmount);
        }

        buildingList[0].resourceCosts[0].costAmountText.text = string.Format("{0:#.00}/{1:#.00}", resourceList[0].valuesToEnter.resourceAmount, buildingList[0].resourceCosts[0].costAmount);
        */
    }
    public void OnGatherSticks()
    {
        resourceList[1].inputValues.resourceAmount++;

        /*
        craftingItemsList[1].objects.mainPanel.SetActive(true);
        if (resourceList[1].valuesToEnter.resourceAmount >= resourceList[1].valuesToEnter.resourceMaxStorage)
        {
            resourceList[1].objects.resourceAmountText.text = string.Format("{0:#.00}", resourceList[0].valuesToEnter.resourceMaxStorage);
        }
        else
        {
            
            resourceList[1].objects.resourceAmountText.text = string.Format("{0:#.00}", resourceList[1].valuesToEnter.resourceAmount);
        }
        buildingList[1].resourceCosts[0].costAmountText.text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].valuesToEnter.resourceAmount, buildingList[1].resourceCosts[0].costAmount);
        buildingList[2].resourceCosts[0].costAmountText.text = string.Format("{0:#.00}/{1:#.00}", resourceList[1].valuesToEnter.resourceAmount, buildingList[2].resourceCosts[0].costAmount);
        //GetCurrentFill(resourceList[1].valuesToEnter.resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount, craftingItemsList[0].objects.progressBar, craftingItemsList[0].objects.particleEffect);
        */
    }
    public void OnCraftingWoodenHoe()
    {
        if (resourceList[1].valuesToEnter.resourceAmount >= craftingItemsList[0].resourceCosts[0].costAmount)
        {
            craftingItemsList[0].objects.headerText.text = "Wooden Hoe (Complete)";
            resourceList[1].valuesToEnter.resourceAmount -= craftingItemsList[0].resourceCosts[0].costAmount;
            resourceList[1].objects.resourceAmountText.text = string.Format("{0}", resourceList[1].valuesToEnter.resourceAmount);
            buildingList[0].objects.mainPanel.SetActive(true);
        }
        else
        {
            //Not enough 'insert stick name variable'!
        }

    }
    public void OnCraftingWoodenAxe()
    {
        if (resourceList[1].valuesToEnter.resourceAmount >= craftingItemsList[1].resourceCosts[0].costAmount)
        {
            craftingItemsList[1].objects.headerText.text = "Wooden Axe (Complete)";
            resourceList[1].valuesToEnter.resourceAmount -= craftingItemsList[0].resourceCosts[0].costAmount;
            resourceList[1].objects.resourceAmountText.text = string.Format("{0}", resourceList[1].valuesToEnter.resourceAmount);
            buildingList[1].objects.mainPanel.SetActive(true);
        }
        else
        {
            //Not enough 'insert stick name variable'!
        }
    }
    public void OnCraftingWoodenPickaxe()
    {
        if (resourceList[1].valuesToEnter.resourceAmount >= craftingItemsList[2].resourceCosts[0].costAmount)
        {
            craftingItemsList[2].objects.headerText.text = "Wooden Pickaxe (Complete)";
            resourceList[1].valuesToEnter.resourceAmount -= craftingItemsList[2].resourceCosts[0].costAmount;
            resourceList[1].objects.resourceAmountText.text = string.Format("{0}", resourceList[1].valuesToEnter.resourceAmount);
            buildingList[2].objects.mainPanel.SetActive(true);
        }
        else
        {
            //Not enough 'insert stick name variable'!
        }
    }

    //Seems this isn't going to work, have to code every button independently. what a shame.
    public void BuildingButtonTemplate(float _ResourceAmount, float _ResourceMaxStorage, float _costAmount, int _buildingAmount, float _buildingCostMultiplier, string _resourceName)
    {
        if (_ResourceAmount >= _costAmount)
        {
            _buildingAmount++;
            _ResourceAmount -= _costAmount;
            _costAmount = _costAmount * Mathf.Pow(_buildingCostMultiplier, _buildingAmount);
        }
        else
        {
            Debug.Log("Not enough " + _resourceName);
        }
    }

    public void BuildPotatoField()
    {
        
        if (resourceList[0].valuesToEnter.resourceAmount >= buildingList[0].resourceCosts[0].costAmount)
        {
            buildingList[0].valuesToEnter.buildingAmount++;
            resourceList[0].valuesToEnter.resourceAmount = resourceList[0].valuesToEnter.resourceAmount - buildingList[0].resourceCosts[0].costAmount;
            buildingList[0].resourceCosts[0].costAmount = buildingList[0].resourceCosts[0].costAmount * Mathf.Pow(buildingList[0].inputValues.buildingCostMultiplier, buildingList[0].valuesToEnter.buildingAmount);
            buildingList[0].objects.buildingNameText.text = string.Format("Potato Field ({0})", buildingList[0].valuesToEnter.buildingAmount);
            buildingList[0].resourceCosts[0].costAmountText.text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[0].valuesToEnter.resourceAmount, buildingList[0].resourceCosts[0].costAmount);
        }
        else
        {
            Debug.Log("Not enough food!");
        }
    }
    public void BuildWoodlot()
    {
        if (resourceList[1].valuesToEnter.resourceAmount >= buildingList[1].resourceCosts[0].costAmount)
        {
            buildingList[1].valuesToEnter.buildingAmount++;
            resourceList[1].valuesToEnter.resourceAmount = resourceList[1].valuesToEnter.resourceAmount - buildingList[1].resourceCosts[0].costAmount;
            buildingList[1].resourceCosts[0].costAmount = buildingList[1].resourceCosts[0].costAmount * Mathf.Pow(buildingList[1].valuesToEnter.buildingCostMultiplier, buildingList[1].valuesToEnter.buildingAmount);
            buildingList[1].objects.buildingNameText.text = string.Format("Wood-lot ({0})", buildingList[1].valuesToEnter.buildingAmount);
            buildingList[1].resourceCosts[0].costAmountText.text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[1].valuesToEnter.resourceAmount, buildingList[1].resourceCosts[0].costAmount);
        }
        else
        {
            Debug.Log("Not enough sticks!");
        }
    }
    public void BuildDigSite()
    {
        if (resourceList[1].valuesToEnter.resourceAmount >= buildingList[2].resourceCosts[0].costAmount)
        {
            buildingList[2].valuesToEnter.buildingAmount++;
            resourceList[1].valuesToEnter.resourceAmount = resourceList[1].valuesToEnter.resourceAmount - buildingList[2].resourceCosts[0].costAmount;
            buildingList[2].resourceCosts[0].costAmount = buildingList[2].resourceCosts[0].costAmount * Mathf.Pow(buildingList[2].valuesToEnter.buildingCostMultiplier, buildingList[2].valuesToEnter.buildingAmount);
            buildingList[2].objects.buildingNameText.text = string.Format("Dig Site ({0})", buildingList[2].valuesToEnter.buildingAmount);
            buildingList[2].resourceCosts[0].costAmountText.text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[1].valuesToEnter.resourceAmount, buildingList[2].resourceCosts[0].costAmount);
        }
        else
        {
            Debug.Log("Not enough sticks!");
        }
    }
    public void BuildMakeshiftBed()
    {
        Debug.Log("Clicked");
        if ((resourceList[1].valuesToEnter.resourceAmount >= buildingList[3].resourceCosts[0].costAmount) && (resourceList[2].valuesToEnter.resourceAmount >= buildingList[3].resourceCosts[0].costAmount))
        {
            Debug.Log("Reached here");
            buildingList[3].valuesToEnter.buildingAmount++;
            //Just reference buttonmanager.availableworkers etc and all the other fields here.
            //availableWorkers++;
            //maxWorkers++;
            //availableWorkerObject.text = string.Format("Available Workers: [{0}/{1}]", availableWorkers, maxWorkers);
            resourceList[1].valuesToEnter.resourceAmount = resourceList[1].valuesToEnter.resourceAmount - buildingList[3].resourceCosts[0].costAmount;
            resourceList[2].valuesToEnter.resourceAmount = resourceList[2].valuesToEnter.resourceAmount - buildingList[3].resourceCosts[0].costAmount;
            buildingList[3].resourceCosts[0].costAmount = buildingList[3].resourceCosts[0].costAmount * Mathf.Pow(buildingList[3].valuesToEnter.buildingCostMultiplier, buildingList[3].valuesToEnter.buildingAmount);
            buildingList[3].resourceCosts[1].costAmount = buildingList[3].resourceCosts[1].costAmount * Mathf.Pow(buildingList[3].valuesToEnter.buildingCostMultiplier, buildingList[3].valuesToEnter.buildingAmount);
            buildingList[3].objects.buildingNameText.text = string.Format("Makeshift Bed ({0})", buildingList[3].valuesToEnter.buildingAmount);
            buildingList[3].resourceCosts[0].costAmountText.text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[1].valuesToEnter.resourceAmount, buildingList[3].resourceCosts[0].costAmount);
            buildingList[3].resourceCosts[1].costAmountText.text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[2].valuesToEnter.resourceAmount, buildingList[3].resourceCosts[1].costAmount);
        }
        else
        {
            Debug.Log("Not enough sticks && stones");
        }
    }
}
