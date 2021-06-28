using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public Swipe _Swipe;
    private uint _swipeCount = 0;
    private readonly uint _panelCount = 3;
    public GameObject[] buildingUI, craftUI, workerUI, researchUI, settingsUI;
    public static bool isBuildingVisible, isCraftingVisible, isWorkerVisible, isResearchVisible;

    private void Start()
    {
        _swipeCount = 0;
        foreach(var _settingsUI in settingsUI)
        {
            _settingsUI.SetActive(false);
        }
        
        BuildingPanelActive();
        HideCompletedResearch();
    }
    private void BuildingPanelActive()
    {
        isBuildingVisible = true;
        isCraftingVisible = false;
        isResearchVisible = false;
        isWorkerVisible = false;

        foreach (var _buildingUI in buildingUI)
        {
            _buildingUI.SetActive(true);
        }

        foreach (var _workerUI in workerUI)
        {
            _workerUI.SetActive(false);
        }
        foreach (var _researchUI in researchUI)
        {
            _researchUI.SetActive(false);
        }

        foreach (var _craftUI in craftUI)
        {
            _craftUI.SetActive(false);
        }

        foreach (var craft in Craftable.Craftables)
        {
            craft.Value.objMainPanel.SetActive(false);
            craft.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var researchable in Researchable.Researchables)
        {
            researchable.Value.objMainPanel.SetActive(false);
            researchable.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var worker in Worker.Workers)
        {
            worker.Value.objMainPanel.SetActive(false);
            worker.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var building in Building.Buildings)
        {
            if (building.Value.isUnlocked == 1)
            {
                building.Value.objMainPanel.SetActive(true);
                building.Value.objSpacerBelow.SetActive(true);
            }
            else
            {
                building.Value.objMainPanel.SetActive(false);
                building.Value.objSpacerBelow.SetActive(false);
            }
        }
    }
    private void CraftingPanelActive()
    {
        isBuildingVisible = false;
        isCraftingVisible = true;
        isResearchVisible = false;
        isWorkerVisible = false;

        foreach (var _buildingUI in buildingUI)
        {
            _buildingUI.SetActive(false);
        }

        foreach (var _workerUI in workerUI)
        {
            _workerUI.SetActive(false);
        }
        foreach (var _researchUI in researchUI)
        {
            _researchUI.SetActive(false);
        }

        foreach (var _craftUI in craftUI)
        {
            _craftUI.SetActive(true);
        }

        foreach (var building in Building.Buildings)
        {
            building.Value.objMainPanel.SetActive(false);
            building.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var researchable in Researchable.Researchables)
        {
            researchable.Value.objMainPanel.SetActive(false);
            researchable.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var worker in Worker.Workers)
        {
            worker.Value.objMainPanel.SetActive(false);
            worker.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.isUnlocked == 1)
            {
                craft.Value.objMainPanel.SetActive(true);
                craft.Value.objSpacerBelow.SetActive(true);
            }
            else
            {
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }
        }
    }
    private void WorkerPanelActive()
    {
        isBuildingVisible = false;
        isCraftingVisible = false;
        isResearchVisible = false;
        isWorkerVisible = true;

        foreach (var _buildingUI in buildingUI)
        {
            _buildingUI.SetActive(false);
        }

        foreach (var _workerUI in workerUI)
        {
            _workerUI.SetActive(true);
        }
        foreach (var _researchUI in researchUI)
        {
            _researchUI.SetActive(false);
        }

        foreach (var _craftUI in craftUI)
        {
            _craftUI.SetActive(false);
        }

        foreach (var building in Building.Buildings)
        {
            building.Value.objMainPanel.SetActive(false);
            building.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var craft in Craftable.Craftables)
        {
            craft.Value.objMainPanel.SetActive(false);
            craft.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var researchable in Researchable.Researchables)
        {
            researchable.Value.objMainPanel.SetActive(false);
            researchable.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var worker in Worker.Workers)
        {
            if (worker.Value.isUnlocked == 1)
            {
                worker.Value.objMainPanel.SetActive(true);
                worker.Value.objSpacerBelow.SetActive(true);
            }
            else
            {
                worker.Value.objMainPanel.SetActive(false);
                worker.Value.objSpacerBelow.SetActive(false);
            }
        }
    }
    private void ResearchPanelActive()
    {
        isBuildingVisible = false;
        isCraftingVisible = false;
        isResearchVisible = true;
        isWorkerVisible = false;

        foreach (var _buildingUI in buildingUI)
        {
            _buildingUI.SetActive(false);
        }

        foreach (var _workerUI in workerUI)
        {
            _workerUI.SetActive(false);
        }
        foreach (var _researchUI in researchUI)
        {
            _researchUI.SetActive(true);
        }

        foreach (var _craftUI in craftUI)
        {
            _craftUI.SetActive(false);
        }

        foreach (var craft in Craftable.Craftables)
        {
            craft.Value.objMainPanel.SetActive(false);
            craft.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var building in Building.Buildings)
        {
            building.Value.objMainPanel.SetActive(false);
            building.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var worker in Worker.Workers)
        {
            worker.Value.objMainPanel.SetActive(false);
            worker.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var researchable in Researchable.Researchables)
        {
            if (researchable.Value.isUnlocked == 1)
            {
                researchable.Value.objMainPanel.SetActive(true);
                researchable.Value.objSpacerBelow.SetActive(true);
            }
            else
            {
                researchable.Value.objMainPanel.SetActive(false);
                researchable.Value.objSpacerBelow.SetActive(false);
            }
        }
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
    public void HideCompletedResearch()
    {
        foreach (var researchable in Researchable.Researchables)
        {
            //Debug.Log(researchable);
        }
    }
    void Update()
    {
        SwipeCountHandler();
    }
}
