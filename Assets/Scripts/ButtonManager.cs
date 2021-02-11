using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void OnGatherPotatoes()
    {
        GameManager.Instance.resourceList[0].inputValues.resourceAmount++;
    }
    public void OnGatherSticks()
    {
        GameManager.Instance.resourceList[1].inputValues.resourceAmount++;
    }
    public void OnCraftingWoodenHoe()
    {
        float _resourceAmount = GameManager.Instance.resourceList[1].inputValues.resourceAmount;
        float _costAmount = GameManager.Instance.craftingItemsList[0].resourceCosts[0].costAmount;

        if (_resourceAmount >= _costAmount)
        {
            //craftingItemsList[0].objects.headerText.text = "Wooden Hoe (Complete)";
            _resourceAmount -= _costAmount;
            GameManager.Instance.buildingList[0].objects.mainPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Not enough: " + GameManager.Instance.resourceList[1].name);
        }

    }
    public void OnCraftingWoodenAxe()
    {
        float _resourceAmount = GameManager.Instance.resourceList[1].inputValues.resourceAmount;
        float _costAmount = GameManager.Instance.craftingItemsList[1].resourceCosts[0].costAmount;

        if (_resourceAmount >= _costAmount)
        {
            //craftingItemsList[1].objects.headerText.text = "Wooden Axe (Complete)";
            _resourceAmount -= _costAmount;
            GameManager.Instance.buildingList[1].objects.mainPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Not enough: " + GameManager.Instance.resourceList[1].name);
        }
    }
    public void OnCraftingWoodenPickaxe()
    {
        float _resourceAmount = GameManager.Instance.resourceList[1].inputValues.resourceAmount;
        float _costAmount = GameManager.Instance.craftingItemsList[1].resourceCosts[0].costAmount;

        if (_resourceAmount >= _costAmount)
        {
            //craftingItemsList[2].objects.headerText.text = "Wooden Pickaxe (Complete)";
            _resourceAmount -= _costAmount;
            GameManager.Instance.buildingList[2].objects.mainPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Not enough: " + GameManager.Instance.resourceList[1].name);
        }
    }
    /*
    public void BuildingButtonTemplate(float _ResourceAmount, float _ResourceMaxStorage, float _costAmount, int _buildingAmount, float _buildingCostMultiplier, float _resourceName)
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
    */

    public void BuildPotatoField()
    {
        float _resourceAmount = GameManager.Instance.resourceList[0].inputValues.resourceAmount;
        float _costAmount = GameManager.Instance.buildingList[0].resourceCosts[0].costAmount;
        float _buildingAmount = GameManager.Instance.buildingList[0].inputValues.buildingAmount;

        if (_resourceAmount >= _costAmount)
        {
            _buildingAmount++;
            _resourceAmount -= _costAmount;
            _costAmount = _costAmount * Mathf.Pow(GameManager.Instance.buildingList[0].inputValues.buildingCostMultiplier, _buildingAmount);
        }
        else
        {
            Debug.Log("Not enough " + GameManager.Instance.resourceList[0].name);
        }
    }
    public void BuildWoodlot()
    {
        float _resourceAmount = GameManager.Instance.resourceList[1].inputValues.resourceAmount;
        float _costAmount = GameManager.Instance.buildingList[1].resourceCosts[0].costAmount;
        float _buildingAmount = GameManager.Instance.buildingList[1].inputValues.buildingAmount;

        if (_resourceAmount >= _costAmount)
        {
            _buildingAmount++;
            _resourceAmount -= _costAmount;
            _costAmount = _costAmount * Mathf.Pow(GameManager.Instance.buildingList[0].inputValues.buildingCostMultiplier, _buildingAmount);
        }
        else
        {
            Debug.Log("Not enough " + GameManager.Instance.resourceList[0].name);
        }
    }
    public void BuildDigSite()
    {
        float _resourceAmount = GameManager.Instance.resourceList[1].inputValues.resourceAmount;
        float _costAmount = GameManager.Instance.buildingList[2].resourceCosts[0].costAmount;
        float _buildingAmount = GameManager.Instance.buildingList[2].inputValues.buildingAmount;

        if (_resourceAmount >= _costAmount)
        {
            _buildingAmount++;
            _resourceAmount -= _costAmount;
            _costAmount = _costAmount * Mathf.Pow(GameManager.Instance.buildingList[0].inputValues.buildingCostMultiplier, _buildingAmount);
        }
        else
        {
            Debug.Log("Not enough " + GameManager.Instance.resourceList[0].name);
        }
    }
}
    /*
     
    public void BuildMakeshiftBed()
    {
        if ((resourceList[1].inputValues.resourceAmount >= buildingList[3].resourceCosts[0].costAmount) && (resourceList[2].inputValues.resourceAmount >= buildingList[3].resourceCosts[1].costAmount))
        {
            buildingList[3].inputValues.buildingAmount++;
            //Just reference buttonmanager.availableworkers etc and all the other fields here.
            //UIManager.instance.availableWorkers++;
            //maxWorkers++;
            //availableWorkerObject.text = string.Format("Available Workers: [{0}/{1}]", UIManager.instance.availableWorkers, maxWorkers);
            resourceList[1].inputValues.resourceAmount = resourceList[1].inputValues.resourceAmount - buildingList[3].resourceCosts[0].costAmount;
            resourceList[2].inputValues.resourceAmount = resourceList[2].inputValues.resourceAmount - buildingList[3].resourceCosts[0].costAmount;
            buildingList[3].resourceCosts[0].costAmount = buildingList[3].resourceCosts[0].costAmount * Mathf.Pow(buildingList[3].inputValues.buildingCostMultiplier, buildingList[3].inputValues.buildingAmount);
            buildingList[3].resourceCosts[1].costAmount = buildingList[3].resourceCosts[1].costAmount * Mathf.Pow(buildingList[3].inputValues.buildingCostMultiplier, buildingList[3].inputValues.buildingAmount);
            
            buildingList[3].objects.buildingNameText.text = string.Format("Makeshift Bed ({0})", buildingList[3].inputValues.buildingAmount);
            buildingList[3].resourceCosts[0].costAmountText.text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[1].inputValues.resourceAmount, buildingList[3].resourceCosts[0].costAmount);
            buildingList[3].resourceCosts[1].costAmountText.text = string.Format("{0:#0.00}/{1:#0.00}", resourceList[2].inputValues.resourceAmount, buildingList[3].resourceCosts[1].costAmount);
            
        }
        else
        {
            Debug.Log("Not enough " + resourceList[1].name);
        }
    }
    */
     

