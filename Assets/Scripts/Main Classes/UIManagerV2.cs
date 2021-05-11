using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerV2 : MonoBehaviour
{
    public Swipe _Swipe;
    private uint _swipeCount = 0;
    private readonly uint _panelCount = 3;
    public GameObject BuildingPanel, CraftingPanel, WorkerPanel, ResearchPanel;
    public static bool isBuildingPanelActive, isCraftingPanelAtive, isWorkerPanelActive, isResearchPanelActive;

    private void Start()
    {
        _swipeCount = 0;
        BuildingPanelActive();
    }

    private void BuildingPanelActive()
    {
        isBuildingPanelActive = true;
        isCraftingPanelAtive = false;
        isWorkerPanelActive = false;
        isResearchPanelActive = false;

        BuildingPanel.GetComponent<CanvasGroup>().alpha = 1;
        BuildingPanel.GetComponent<CanvasGroup>().interactable = true;
        BuildingPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        CraftingPanel.GetComponent<CanvasGroup>().alpha = 0;
        CraftingPanel.GetComponent<CanvasGroup>().interactable = false;
        CraftingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        WorkerPanel.GetComponent<CanvasGroup>().alpha = 0;
        WorkerPanel.GetComponent<CanvasGroup>().interactable = false;
        WorkerPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        ResearchPanel.GetComponent<CanvasGroup>().alpha = 0;
        ResearchPanel.GetComponent<CanvasGroup>().interactable = false;
        ResearchPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    private void CraftingPanelActive()
    {
        isBuildingPanelActive = false;
        isCraftingPanelAtive = true;
        isWorkerPanelActive = false;
        isResearchPanelActive = false;

        BuildingPanel.GetComponent<CanvasGroup>().alpha = 0;
        BuildingPanel.GetComponent<CanvasGroup>().interactable = false;
        BuildingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        CraftingPanel.GetComponent<CanvasGroup>().alpha = 1;
        CraftingPanel.GetComponent<CanvasGroup>().interactable = true;
        CraftingPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        WorkerPanel.GetComponent<CanvasGroup>().alpha = 0;
        WorkerPanel.GetComponent<CanvasGroup>().interactable = false;
        WorkerPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        ResearchPanel.GetComponent<CanvasGroup>().alpha = 0;
        ResearchPanel.GetComponent<CanvasGroup>().interactable = false;
        ResearchPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    private void WorkerPanelActive()
    {
        isBuildingPanelActive = false;
        isCraftingPanelAtive = false;
        isWorkerPanelActive = true;
        isResearchPanelActive = false;

        BuildingPanel.GetComponent<CanvasGroup>().alpha = 0;
        BuildingPanel.GetComponent<CanvasGroup>().interactable = false;
        BuildingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        CraftingPanel.GetComponent<CanvasGroup>().alpha = 0;
        CraftingPanel.GetComponent<CanvasGroup>().interactable = false;
        CraftingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        WorkerPanel.GetComponent<CanvasGroup>().alpha = 1;
        WorkerPanel.GetComponent<CanvasGroup>().interactable = true;
        WorkerPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        ResearchPanel.GetComponent<CanvasGroup>().alpha = 0;
        ResearchPanel.GetComponent<CanvasGroup>().interactable = false;
        ResearchPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    private void ResearchPanelActive()
    {
        isBuildingPanelActive = false;
        isCraftingPanelAtive = false;
        isWorkerPanelActive = false;
        isResearchPanelActive = true;

        BuildingPanel.GetComponent<CanvasGroup>().alpha = 0;
        BuildingPanel.GetComponent<CanvasGroup>().interactable = false;
        BuildingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        CraftingPanel.GetComponent<CanvasGroup>().alpha = 0;
        CraftingPanel.GetComponent<CanvasGroup>().interactable = false;
        CraftingPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        WorkerPanel.GetComponent<CanvasGroup>().alpha = 0;
        WorkerPanel.GetComponent<CanvasGroup>().interactable = false;
        WorkerPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        ResearchPanel.GetComponent<CanvasGroup>().alpha = 1;
        ResearchPanel.GetComponent<CanvasGroup>().interactable = true;
        ResearchPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
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
        if (_Swipe.SwipeRight || _Swipe.SwipeLeft)
        {
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
        }
        #endregion
    }
    void Update()
    {
        SwipeCountHandler();
    }
}
