using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Swipe _Swipe;
    private uint _swipeCount = 0, _panelCount = 3;
    public GameObject BuildingPanel, CraftingPanel, WorkerPanel, ResearchPanel;

    private void Start()
    {
        _swipeCount = 0;
    }
    private void OnApplicationQuit()
    {
        // Just before quitting it sets all objects active so everything can get saved to playerprefs or savefile.
        BuildingPanel.SetActive(true);
        CraftingPanel.SetActive(true);
        WorkerPanel.SetActive(true);
        ResearchPanel.SetActive(true);
    }
    private void BuildingPanelActive()
    {
        BuildingPanel.SetActive(true);
        CraftingPanel.SetActive(false);
        WorkerPanel.SetActive(false);
        ResearchPanel.SetActive(false);
    }
    private void CraftingPanelActive()
    {
        BuildingPanel.SetActive(false);
        CraftingPanel.SetActive(true);
        WorkerPanel.SetActive(false);
        ResearchPanel.SetActive(false);
    }
    private void WorkerPanelActive()
    {
        BuildingPanel.SetActive(false);
        CraftingPanel.SetActive(false);
        WorkerPanel.SetActive(true);
        ResearchPanel.SetActive(false);
    }
    private void ResearchPanelActive()
    {
        BuildingPanel.SetActive(false);
        CraftingPanel.SetActive(false);
        WorkerPanel.SetActive(false);
        ResearchPanel.SetActive(true);
    }
    private void SwipeCountHandler()
    {
        #region Actual Swiping
        if (_Swipe.SwipeRight && (_swipeCount >= 1))
        {
            _swipeCount--;
        }
        else if (_Swipe.SwipeLeft && (_swipeCount <= (_panelCount - 1)))
        {
            _swipeCount++;
        }
        #endregion

        #region Sets Panels Active
        if (_swipeCount == 0)
        {
            BuildingPanelActive();
        }
        else if (_swipeCount == 1)
        {
            CraftingPanelActive();
        }
        else if (_swipeCount == 2)
        {
            WorkerPanelActive();
        }
        else if (_swipeCount == 3)
        {
            ResearchPanelActive();
        }
        else
        {
            Debug.LogError("This shouldn't happen");
        }
        #endregion
    }
    void Update()
    {       
        SwipeCountHandler();
    }
}
