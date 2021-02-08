using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoSingleton<GameManager>
{
    public List<CraftingItem> craftingItemsList = new List<CraftingItem>();
    public List<Building> buildingList = new List<Building>();
    public List<Resource> resourceList = new List<Resource>();
    public GameObject buildingMainPanel, craftingMainPanel, workerMainPanel;
    public Swipe swipeControls;
    public static int menuValue;
    private int minSwipeValue, maxSwipeValue;
    public Animator mainPanelAnim;
    private string seasonText;
    private int day, year, seasonCount;

    public void Start()
    {     
        BuildingsTabActive();
        minSwipeValue = 0;
        maxSwipeValue = 2;
        InvokeRepeating("UpdateDay", 0, (float)5);
    }
    public void BuildingsTabActive()
    {
        buildingMainPanel.SetActive(true);
        craftingMainPanel.SetActive(false);
        workerMainPanel.SetActive(false);
    }
    public void CraftingTabActive()
    {
        buildingMainPanel.SetActive(false);
        craftingMainPanel.SetActive(true);
        workerMainPanel.SetActive(false);
    }
    public void WorkerTabActive()
    {
        buildingMainPanel.SetActive(false);
        craftingMainPanel.SetActive(false);
        workerMainPanel.SetActive(true);
    }
    private void Swiping()
    {
        if (menuValue < minSwipeValue)
        {
            menuValue = minSwipeValue;
        }
        if (menuValue > maxSwipeValue)
        {
            menuValue = maxSwipeValue;
        }

        if (swipeControls.SwipeLeft && (menuValue < maxSwipeValue))
        {
            mainPanelAnim.SetTrigger("hasSwipedLeft");
        }
        else if (swipeControls.SwipeRight && (menuValue > minSwipeValue))
        {
            mainPanelAnim.SetTrigger("hasSwipedRight");
        }

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

        //seasonObject.GetComponent<TextMeshProUGUI>().text = string.Format("Year {0} - {1}, day {2}", year, seasonText, day);



    }
    public void UpdateObjectTexts()
    {
        #region Update Resource Costs 

        for (int c = 0; c < craftingItemsList.Count; c++)
        {
            for (int r = 0; r < resourceList.Count; r++)
            {
                for (int i = 0; i < craftingItemsList[c].resourceCosts.Count; i++)
                {
                    craftingItemsList[c].resourceCosts[i].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00} / {1:#.00}", resourceList[r].valuesToEnter.resourceAmount, craftingItemsList[c].resourceCosts[i].costAmount);
                }
            }
        }

        for (int b = 0; b < buildingList.Count; b++)
        {
            for (int r = 0; r < resourceList.Count; r++)
            {
                for (int i = 0; i < buildingList[b].resourceCosts.Count; i++)
                {
                    buildingList[b].resourceCosts[i].costAmountText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:#.00}/{1:#.00}", resourceList[r].valuesToEnter.resourceAmount, buildingList[b].resourceCosts[i].costAmount);
                }
            }
        }
        #endregion
    }
    private void Update()
    {
        Swiping();
        if (menuValue == 0)
        {
            BuildingsTabActive();          
        }
        else if (menuValue == 1)
        {
            CraftingTabActive();
        }
        else if (menuValue == 2)
        {
            WorkerTabActive();
        }
    }

}
