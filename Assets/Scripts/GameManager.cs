using System.Collections;
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
          if (buildingList[0].resourceCosts[0].costAmount <= 1)
          {
              buildingList[0].mainPanel.SetActive(false);
          }
      }
      public void OnGatherPotatoes()
      {   
          resourceList[0].resourceAmount++;
          resourceList[0].resourceAmountText.GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(resourceList[0].resourceAmount * 100f) / 100f);
      }
      public void OnGatherSticks()
      {
          craftingItemsList[0].mainPanel.SetActive(true);
          if (resourceList[1].resourceAmount <= resourceList[1].resourceMaxStorage)
          {
              resourceList[1].resourceAmount++;
          }
          resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(resourceList[1].resourceAmount * 100f) / 100f);
          GetCurrentFill(resourceList[1].resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount, craftingItemsList[0].progressBar);
      }
      public void OnCraftingWoodenHoe()
      {
          if (resourceList[1].resourceAmount >= craftingItemsList[0].resourceCosts[0].costAmount)
          {
              craftingItemsList[0].craftName.GetComponent<TextMeshProUGUI>().text = "Wooden Hoe (Complete)";
              resourceList[1].resourceAmount -= craftingItemsList[0].resourceCosts[0].costAmount;
              resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(resourceList[1].resourceAmount * 100f) / 100f);
              buildingList[0].mainPanel.SetActive(true);
              craftingItemsList[0].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[1].resourceAmount + "/" + craftingItemsList[0].resourceCosts[0].costAmount;
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
              resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(resourceList[1].resourceAmount * 100f) / 100f);
              buildingList[1].mainPanel.SetActive(true);
              craftingItemsList[1].resourceCosts[0].costAmountText.GetComponent<TextMeshProUGUI>().text = resourceList[1].resourceAmount + "/" + craftingItemsList[1].resourceCosts[0].costAmount;
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
              resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = "" + (Mathf.Round(resourceList[1].resourceAmount * 100f) / 100f);
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
              resourceList[0].resourceAmount -= buildingList[0].resourceCosts[0].costAmount;
              buildingList[0].resourceCosts[0].costAmount = ((float)(buildingList[0].resourceCosts[0].costAmount * buildingList[0].buildingCostMultiplier));
              buildingList[0].buildingName.GetComponent<TextMeshProUGUI>().text = "Potato Field (" + buildingList[0].buildingAmount + ")";
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
              resourceList[1].resourceAmount -= buildingList[1].resourceCosts[0].costAmount;
              buildingList[1].resourceCosts[0].costAmount = ((float)(buildingList[1].resourceCosts[0].costAmount * buildingList[1].buildingCostMultiplier));
              buildingList[1].buildingName.GetComponent<TextMeshProUGUI>().text = "Wood-lot (" + buildingList[1].buildingAmount + ")";
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
              resourceList[1].resourceAmount -= buildingList[2].resourceCosts[0].costAmount;
              buildingList[2].resourceCosts[0].costAmount = ((float)(buildingList[2].resourceCosts[0].costAmount * buildingList[2].buildingCostMultiplier));
              buildingList[2].buildingName.GetComponent<TextMeshProUGUI>().text = "Dig Site (" + buildingList[2].buildingAmount + ")";
          }
          else
          {
              Debug.Log("Not enough sticks!");
          }
      }
      public void GetCurrentFill(float current, float max, Image progressCircle)
    {
        float fillAmount = current / max;
        progressCircle.fillAmount = fillAmount;
    }
      public void UpdateResources()
      {
        #region Food Resource
        float potatoFieldMultiplier = (float)(buildingList[0].buildingAmount * buildingList[0].buildingResourceMultiplier);
          if (resourceList[0].resourceAmount <= (resourceList[0].resourceMaxStorage - potatoFieldMultiplier) - 0.1)
          {
              resourceList[0].resourceAmount += potatoFieldMultiplier;
          }       
          else
          {
              resourceList[0].resourceAmount = resourceList[0].resourceMaxStorage;
          }
          resourceList[0].resourceAmountText.GetComponent<TextMeshProUGUI>().text = (Mathf.Round(resourceList[0].resourceAmount * 100f) / 100f) + "";
          resourceList[0].resourcePerSecond.GetComponent<TextMeshProUGUI>().text = (Mathf.Round(potatoFieldMultiplier * 100f) / 100f) + "/sec";
        #endregion

        #region Wood Resource
                float woodlotMultiplier = (float)(buildingList[1].buildingAmount * buildingList[1].buildingResourceMultiplier);
                  resourceList[1].resourceAmount += woodlotMultiplier;
                  resourceList[1].resourceAmountText.GetComponent<TextMeshProUGUI>().text = (Mathf.Round(resourceList[1].resourceAmount * 100f) / 100f) + "";
                  resourceList[1].resourcePerSecond.GetComponent<TextMeshProUGUI>().text = (Mathf.Round(woodlotMultiplier * 100f) / 100f) + "/sec";
                #endregion

        #region Stone Resource
        float digSiteMultiplier = (float)(buildingList[2].buildingAmount * buildingList[2].buildingResourceMultiplier);
        resourceList[2].resourceAmountText.GetComponent<TextMeshProUGUI>().text = (Mathf.Round(resourceList[2].resourceAmount * 100f) / 100f) + "";
        resourceList[2].resourcePerSecond.GetComponent<TextMeshProUGUI>().text = (Mathf.Round(digSiteMultiplier * 100f) / 100f) + "/sec";
        #endregion

        GetCurrentFill(resourceList[0].resourceAmount, buildingList[0].resourceCosts[0].costAmount, buildingList[0].progressBar);
          GetCurrentFill(resourceList[1].resourceAmount, buildingList[1].resourceCosts[0].costAmount, buildingList[1].progressBar);
          GetCurrentFill(resourceList[1].resourceAmount, buildingList[2].resourceCosts[0].costAmount, buildingList[2].progressBar);
          GetCurrentFill(resourceList[1].resourceAmount, craftingItemsList[0].resourceCosts[0].costAmount, craftingItemsList[0].progressBar);
          GetCurrentFill(resourceList[1].resourceAmount, craftingItemsList[1].resourceCosts[0].costAmount, craftingItemsList[1].progressBar);
          GetCurrentFill(resourceList[1].resourceAmount, craftingItemsList[2].resourceCosts[0].costAmount, craftingItemsList[2].progressBar);
      }
}
