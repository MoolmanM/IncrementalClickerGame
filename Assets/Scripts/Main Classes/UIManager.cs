using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Swipe swipe;
    private uint swipeCount = 0, panelCount = 1;
    public GameObject BuildingPanel, CraftingPanel;

    private void BuildingPanelActive()
    {
        BuildingPanel.SetActive(true);
        CraftingPanel.SetActive(false);
    }

    private void CraftingPanelActive()
    {
        BuildingPanel.SetActive(false);
        CraftingPanel.SetActive(true);
    }

    private void SwipeCountHandler()
    {
        if (swipeCount == 0)
        {
            BuildingPanelActive();
        }
        else if (swipeCount == 1)
        {
            CraftingPanelActive();
        }
    }
    void Update()
    {
        if (swipeCount >= panelCount)
        {
            swipeCount = panelCount;
        }
        if (swipeCount <= 0)
        {
            swipeCount = 0;
        }
        if (swipe.SwipeRight)
        {
            swipeCount--;
        }
        else if (swipe.SwipeLeft)
        {
            swipeCount++;
        }

        SwipeCountHandler();
    }
}
