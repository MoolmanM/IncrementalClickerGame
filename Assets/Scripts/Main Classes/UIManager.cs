using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Swipe swipe;
    private uint swipeCount = 0, panelCount = 2;
    public GameObject buildingPanel, craftingPanel, workerPanel;

    private void Start()
    {
        swipeCount = 0;    
    }

    private void BuildingPanelActive()
    {
        buildingPanel.SetActive(true);
        craftingPanel.SetActive(false);
        workerPanel.SetActive(false);
    }

    private void CraftingPanelActive()
    {
        buildingPanel.SetActive(false);
        craftingPanel.SetActive(true);
        workerPanel.SetActive(false);
    }

    private void WorkerPanelActive()
    {
        buildingPanel.SetActive(false);
        craftingPanel.SetActive(false);
        workerPanel.SetActive(true);
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
        else if (swipeCount == 2)
        {
            WorkerPanelActive();
        }
        else
        {
            Debug.Log("This shouldn't happen");
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
