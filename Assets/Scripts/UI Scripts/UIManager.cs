using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
    private void UpdateNotificationPanel()
    {
        PointerNotification.textLeft.text = PointerNotification.leftAmount.ToString();
        PointerNotification.textRight.text = PointerNotification.rightAmount.ToString();
        if (PointerNotification.leftAmount > 0)
        {
            PointerNotification.objLeftPointer.SetActive(true);
        }
        else
        {
            PointerNotification.objLeftPointer.SetActive(false);
        }
        if (PointerNotification.rightAmount > 0)
        {
            PointerNotification.objRightPointer.SetActive(true);
        }
        else
        {
            PointerNotification.objRightPointer.SetActive(false);
        }
    }
    private void BuildingPanelActive()
    {
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

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
            if(!craft.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var researchable in Researchable.Researchables)
        {
            researchable.Value.objMainPanel.SetActive(false);
            researchable.Value.objSpacerBelow.SetActive(false);
            if (!researchable.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var worker in Worker.Workers)
        {
            worker.Value.objMainPanel.SetActive(false);
            worker.Value.objSpacerBelow.SetActive(false);
            if (!worker.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var building in Building.Buildings)
        {
            building.Value.hasSeen = true;
            if (building.Value.isUnlocked)
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
        UpdateNotificationPanel();
    }
    private void CraftingPanelActive()
    {
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

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

            if (!building.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
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

            if (!worker.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var craft in Craftable.Craftables)
        {
            craft.Value.hasSeen = true;
            if (craft.Value.isUnlocked)
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
        UpdateNotificationPanel();
    }
    private void WorkerPanelActive()
    {
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

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

            if (!building.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
            
        }

        foreach (var craft in Craftable.Craftables)
        {
            craft.Value.objMainPanel.SetActive(false);
            craft.Value.objSpacerBelow.SetActive(false);

            if (!craft.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var researchable in Researchable.Researchables)
        {
            researchable.Value.objMainPanel.SetActive(false);
            researchable.Value.objSpacerBelow.SetActive(false);
        }

        foreach (var worker in Worker.Workers)
        {
            worker.Value.hasSeen = true;
            if (worker.Value.isUnlocked)
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
        UpdateNotificationPanel();
    }
    private void ResearchPanelActive()
    {
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

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

            if (!craft.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var building in Building.Buildings)
        {
            building.Value.objMainPanel.SetActive(false);
            building.Value.objSpacerBelow.SetActive(false);

            if (!building.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var worker in Worker.Workers)
        {
            worker.Value.objMainPanel.SetActive(false);
            worker.Value.objSpacerBelow.SetActive(false);

            if (!worker.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var researchable in Researchable.Researchables)
        {
            researchable.Value.hasSeen = true;
            if (researchable.Value.isUnlocked)
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
        UpdateNotificationPanel();
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
