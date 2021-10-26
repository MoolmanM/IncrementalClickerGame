using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static bool isBuildingVisible, isCraftingVisible, isWorkerVisible, isResearchVisible;
    public uint swipeCount = 0;

    public Swipe _Swipe;
    public GameObject[] buildingUI, craftUI, workerUI, researchUI, settingsUI, gatheringUI;
    public Animator animMainPanel;

    private readonly uint _panelCount = 3;

    private static UIManager _instance;

    public static UIManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        swipeCount = 0;
        foreach (var _settingsUI in settingsUI)
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
    public void BuildingPanelActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        foreach (var _buildingUI in buildingUI)
        {
            if (!_buildingUI.activeSelf)
            {
                _buildingUI.SetActive(true);
            }

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
            if (_workerUI.activeSelf)
            {
                _workerUI.SetActive(false);
            }
        }
        foreach (var _researchUI in researchUI)
        {
            if (_researchUI.activeSelf)
            {
                _researchUI.SetActive(false);
            }
        }

        foreach (var _craftUI in craftUI)
        {
            if (_craftUI.activeSelf)
            {
                _craftUI.SetActive(false);
            }
        }

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.objMainPanel.activeSelf)
            {
                craft.Value.objMainPanel.SetActive(false);
            }
            if (craft.Value.objSpacerBelow.activeSelf)
            {
                craft.Value.objSpacerBelow.SetActive(false);
            }
            if (!craft.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var researchable in Researchable.Researchables)
        {
            if (researchable.Value.objMainPanel.activeSelf)
            {
                researchable.Value.objMainPanel.SetActive(false);
            }
            if (researchable.Value.objSpacerBelow.activeSelf)
            {
                researchable.Value.objSpacerBelow.SetActive(false);
            }

            if (!researchable.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var worker in Worker.Workers)
        {
            if (worker.Value.objMainPanel.activeSelf)
            {
                worker.Value.objMainPanel.SetActive(false);
            }
            if (worker.Value.objSpacerBelow.activeSelf)
            {
                worker.Value.objSpacerBelow.SetActive(false);
            }

            if (!worker.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var building in Building.Buildings)
        {
            building.Value.hasSeen = true;
            if (building.Value.isUnlocked && !building.Value.objMainPanel.activeSelf)
            {
                building.Value.objMainPanel.SetActive(true);
                building.Value.objSpacerBelow.SetActive(true);
            }
            else if (!building.Value.isUnlocked && building.Value.objMainPanel.activeSelf)
            {
                building.Value.objMainPanel.SetActive(false);
                building.Value.objSpacerBelow.SetActive(false);
            }
        }
        UpdateNotificationPanel();
    }
    public void CraftingPanelActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        foreach (var _buildingUI in buildingUI)
        {
            if (_buildingUI.activeSelf)
            {
                _buildingUI.SetActive(false);
            }
        }

        foreach (var _workerUI in workerUI)
        {
            if (_workerUI.activeSelf)
            {
                _workerUI.SetActive(false);
            }
        }
        foreach (var _researchUI in researchUI)
        {
            if (_researchUI.activeSelf)
            {
                _researchUI.SetActive(false);
            }
        }

        foreach (var _craftUI in craftUI)
        {
            if (!_craftUI.activeSelf)
            {
                _craftUI.SetActive(true);
            }

        }

        foreach (var building in Building.Buildings)
        {
            if (building.Value.objMainPanel.activeSelf)
            {
                building.Value.objMainPanel.SetActive(false);
                building.Value.objSpacerBelow.SetActive(false);
            }

            if (!building.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var researchable in Researchable.Researchables)
        {
            if (researchable.Value.objMainPanel.activeSelf)
            {
                researchable.Value.objMainPanel.SetActive(false);
                researchable.Value.objSpacerBelow.SetActive(false);
            }

            if (!researchable.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var worker in Worker.Workers)
        {
            if (worker.Value.objMainPanel.activeSelf)
            {
                worker.Value.objMainPanel.SetActive(false);
                worker.Value.objSpacerBelow.SetActive(false);
            }

            if (!worker.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var craft in Craftable.Craftables)
        {
            craft.Value.hasSeen = true;

            if (craft.Value.isUnlocked && !craft.Value.objMainPanel.activeSelf)
            {
                craft.Value.objMainPanel.SetActive(true);
                craft.Value.objSpacerBelow.SetActive(true);
            }
            else if (!craft.Value.isUnlocked && craft.Value.objMainPanel.activeSelf)
            {
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }

            // I can probably include this in the above if, and just make it the first if to check. 
            // So that it will overwrite the rest.
            if (Menu.isCraftingHidden && craft.Value.isCrafted && !craft.Value.objMainPanel.activeSelf)
            {
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }
        }

        UpdateNotificationPanel();
    }
    public void WorkerPanelActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        foreach (var _buildingUI in buildingUI)
        {
            if (_buildingUI.activeSelf)
            {
                _buildingUI.SetActive(false);
            }
        }

        foreach (var _workerUI in workerUI)
        {
            if (!_workerUI.activeSelf)
            {
                _workerUI.SetActive(true);
            }
        }
        foreach (var _researchUI in researchUI)
        {
            if (_researchUI.activeSelf)
            {
                _researchUI.SetActive(false);
            }
        }

        foreach (var _craftUI in craftUI)
        {
            if (_craftUI.activeSelf)
            {
                _craftUI.SetActive(false);
            }
        }

        foreach (var building in Building.Buildings)
        {
            if (building.Value.objMainPanel.activeSelf)
            {
                building.Value.objMainPanel.SetActive(false);
                building.Value.objSpacerBelow.SetActive(false);
            }

            if (!building.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }

        }

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.objMainPanel.activeSelf)
            {
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }

            if (!craft.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var researchable in Researchable.Researchables)
        {
            if (researchable.Value.objMainPanel.activeSelf)
            {
                researchable.Value.objMainPanel.SetActive(false);
                researchable.Value.objSpacerBelow.SetActive(false);
            }

            if (!researchable.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var worker in Worker.Workers)
        {
            worker.Value.hasSeen = true;
            if (worker.Value.isUnlocked && !worker.Value.objMainPanel.activeSelf)
            {
                worker.Value.objMainPanel.SetActive(true);
                worker.Value.objSpacerBelow.SetActive(true);
            }
            else if (!worker.Value.isUnlocked && worker.Value.objMainPanel.activeSelf)
            { 
                worker.Value.objMainPanel.SetActive(false);
                worker.Value.objSpacerBelow.SetActive(false);
            }
        }
        UpdateNotificationPanel();
    }
    public void ResearchPanelActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        foreach (var _buildingUI in buildingUI)
        {
            if (_buildingUI.activeSelf)
            {
                _buildingUI.SetActive(false);
            }
        }
        foreach (var _workerUI in workerUI)
        {
            if (_workerUI.activeSelf)
            {
                _workerUI.SetActive(false);
            }
        }
        foreach (var _researchUI in researchUI)
        {
            if (!_researchUI.activeSelf)
            {
                _researchUI.SetActive(true);
            }
        }
        foreach (var _craftUI in craftUI)
        {
            if (_craftUI.activeSelf)
            {
                _craftUI.SetActive(false);
            }
        }

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.objMainPanel.activeSelf)
            {
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }

            if (!craft.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var building in Building.Buildings)
        {
            if (building.Value.objMainPanel.activeSelf)
            {
                building.Value.objMainPanel.SetActive(false);
                building.Value.objSpacerBelow.SetActive(false);
            }

            if (!building.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var worker in Worker.Workers)
        {
            if (worker.Value.objMainPanel.activeSelf)
            {
                worker.Value.objMainPanel.SetActive(false);
                worker.Value.objSpacerBelow.SetActive(false);
            }

            if (!worker.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var researchable in Researchable.Researchables)
        {
            researchable.Value.hasSeen = true;
            if (researchable.Value.isUnlocked && !researchable.Value.objMainPanel.activeSelf)
            {
                researchable.Value.objMainPanel.SetActive(true);
                researchable.Value.objSpacerBelow.SetActive(true);
            }
            else if (!researchable.Value.isUnlocked && researchable.Value.objMainPanel.activeSelf)
            {
                researchable.Value.objMainPanel.SetActive(false);
                researchable.Value.objSpacerBelow.SetActive(false);
            }

            if (Menu.isResearchHidden && researchable.Value.isResearched)
            {
                if (researchable.Value.objMainPanel.activeSelf)
                {

                    researchable.Value.objMainPanel.SetActive(false);
                    researchable.Value.objSpacerBelow.SetActive(false);
                }
            }
        }
        UpdateNotificationPanel();
    }
    private void SwipeCountHandler()
    {
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
    }
    private void SwipeCountHandlerWorking()
    {
        #region Actual Swiping
        if (_Swipe.SwipeRight && (swipeCount >= 1))
        {
            //swipeCount--;
            animMainPanel.SetTrigger("hasSwipedRight");
            Debug.Log("Check");

        }
        else if (_Swipe.SwipeLeft && (swipeCount <= (_panelCount - 1)))
        {
            //swipeCount++;
            Debug.Log("Check");
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
                isBuildingVisible = true;
                isCraftingVisible = false;
                isResearchVisible = false;
                isWorkerVisible = false;
            }
            else if (swipeCount == 1)
            {
                isBuildingVisible = false;
                isCraftingVisible = true;
                isResearchVisible = false;
                isWorkerVisible = false;
            }
            else if (swipeCount == 2)
            {
                isBuildingVisible = false;
                isCraftingVisible = false;
                isResearchVisible = false;
                isWorkerVisible = true;
            }
            else if (swipeCount == 3)
            {
                isBuildingVisible = false;
                isCraftingVisible = false;
                isResearchVisible = true;
                isWorkerVisible = false;
            }

            if (isBuildingVisible)
            {
                Debug.Log("How many times");
                BuildingPanelActive();
            }
            if (isCraftingVisible)
            {
                Debug.Log("How many times");
                CraftingPanelActive();
            }
            if (isWorkerVisible)
            {
                Debug.Log("How many times");
                WorkerPanelActive();
            }
            if (isResearchVisible)
            {
                Debug.Log("How many times");
                ResearchPanelActive();
            }
            //Debug.Log("Building : " + isBuildingVisible + " Crafting " + isCraftingVisible + " Worker: " + isWorkerVisible + " Research: " + isResearchVisible);
        }
        #endregion


    }
    public void PanelHandler()
    {
        if (swipeCount == 0)
        {
            isBuildingVisible = true;
            isCraftingVisible = false;
            isResearchVisible = false;
            isWorkerVisible = false;
        }
        else if (swipeCount == 1)
        {
            isBuildingVisible = false;
            isCraftingVisible = true;
            isResearchVisible = false;
            isWorkerVisible = false;
        }
        else if (swipeCount == 2)
        {
            isBuildingVisible = false;
            isCraftingVisible = false;
            isResearchVisible = false;
            isWorkerVisible = true;
        }
        else if (swipeCount == 3)
        {
            isBuildingVisible = false;
            isCraftingVisible = false;
            isResearchVisible = true;
            isWorkerVisible = false;
        }

        if (isBuildingVisible)
        {
            BuildingPanelActive();
        }
        if (isCraftingVisible)
        {
            CraftingPanelActive();
        }
        if (isWorkerVisible)
        {
            WorkerPanelActive();
        }
        if (isResearchVisible)
        {
            ResearchPanelActive();
        }
    }
    void Update()
    {
        SwipeCountHandler();
    }
}
