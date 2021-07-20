using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static bool isBuildingVisible, isCraftingVisible, isWorkerVisible, isResearchVisible;
    public static uint swipeCount = 0;

    public Swipe _Swipe;
    public GameObject[] buildingUI, craftUI, workerUI, researchUI, settingsUI, gatheringUI;
    public Animator animMainPanel;

    private readonly uint _panelCount = 3;

    

    void Start()
    {
        swipeCount = 0;
        foreach(var _settingsUI in settingsUI)
        {
            _settingsUI.SetActive(false);
        }
        
        BuildingPanelActive();
    }
    private void UpdateNotificationPanel()
    {
        PointerNotification.HandleLeftAnim();
        PointerNotification.HandleRightAnim();
    }
    private void BuildingPanelActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
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
        foreach (var _gatheringUI in gatheringUI)
        {
            if (Menu.isGatheringHidden)
            {
                _gatheringUI.SetActive(false);
            }
            else
            {
                _gatheringUI.SetActive(true);
            }

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
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
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

            if (Menu.isCraftingHidden && craft.Value.isCrafted)
            {
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }
        }
        UpdateNotificationPanel();
    }
    private void WorkerPanelActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
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
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
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

            if (Menu.isResearchHidden && researchable.Value.isResearched)
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
        if (_Swipe.SwipeRight && (swipeCount >= 1))
        {
            //swipeCount--;
            animMainPanel.SetTrigger("hasSwipedRight");
        }
        else if (_Swipe.SwipeLeft && (swipeCount <= (_panelCount - 1)))
        {
            //swipeCount++;
            animMainPanel.SetTrigger("hasSwipedLeft");
        }
        #endregion

        //#region Sets Panels Active
        //if (_Swipe.SwipeRight || _Swipe.SwipeLeft)
        //{
        //    if (swipeCount == 0)
        //    {
        //        BuildingPanelActive();
        //    }
        //    else if (swipeCount == 1)
        //    {
        //        CraftingPanelActive();
        //    }
        //    else if (swipeCount == 2)
        //    {
        //        WorkerPanelActive();
        //    }
        //    else if (swipeCount == 3)
        //    {
        //        ResearchPanelActive();
        //    }
        //    else
        //    {
        //        Debug.LogError("This shouldn't happen");
        //    }
        //}
        //#endregion

        // I'll keep this for now, but I'm not 100% sure about it, what if the phone lags or a lagspike happens, this seems very susceptible to that.
        // One problem already with this is if you swipe excessively fast it just stays on the same panel.
        #region Sets Panels Active
        if (PointerNotification.IsPlaying(animMainPanel, "SwipeLeft") || PointerNotification.IsPlaying(animMainPanel, "SwipeRight"))
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
            else if (swipeCount == 3)
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
