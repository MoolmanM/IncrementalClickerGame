using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static bool isBuildingVisible, isCraftingVisible, isWorkerVisible, isResearchVisible;
    public TMP_Text txtMainHeader;
    public uint swipeCount = 0;

    public Swipe _Swipe;
    public Canvas[] gatheringObjects, workerObjects;
    public Animator animMainPanel;
    public GameObject leftButton, rightButton;

    private readonly uint _panelCount = 3;

    void Start()
    {
        swipeCount = 0;
        UpdatePopNotication();
        SetStartObjects();

        if (leftButton.activeSelf)
        {
            leftButton.SetActive(false);
        }
        if (!rightButton.activeSelf)
        {
            rightButton.SetActive(true);
        }
    }
    private void SetStartObjects()
    {
        foreach (var gatheringObj in gatheringObjects)
        {
            if (!gatheringObj.enabled)
            {
                gatheringObj.enabled = true;
            }
        }

        foreach (var workerObject in workerObjects)
        {
            if (workerObject.enabled)
            {
                workerObject.enabled = false;
            }
        }

        isBuildingVisible = true;
    }
    private void UpdatePopNotication()
    {
        PointerNotification.HandleLeftAnim();
        PointerNotification.HandleRightAnim();
    }
    private void BuildingsOff()
    {
        foreach (var gatheringobject in gatheringObjects)
        {
            if (gatheringobject.enabled)
            {
                gatheringobject.enabled = false;
            }
        }

        foreach (var kvp in Building.Buildings)
        {
            if (kvp.Value.canvas.enabled)
            {
                kvp.Value.canvas.enabled = false;
                kvp.Value.graphicRaycaster.enabled = false;
            }
        }
    }
    private void CraftablesOff()
    {
        foreach (var kvp in Craftable.Craftables)
        {
            if (kvp.Value.canvas.enabled)
            {
                kvp.Value.canvas.enabled = false;
                kvp.Value.graphicRaycaster.enabled = false;
            }
        }
    }
    private void ResearchablesOff()
    {
        foreach (var kvp in Researchable.Researchables)
        {
            if (kvp.Value.canvas.enabled)
            {
                kvp.Value.canvas.enabled = false;
                kvp.Value.graphicRaycaster.enabled = false;
            }
        }
    }
    private void WorkersOff()
    {
        foreach (var workerObject in workerObjects)
        {
            if (workerObject.enabled)
            {
                workerObject.enabled = false;
            }
        }

        foreach (var kvp in Worker.Workers)
        {
            if (kvp.Value.canvas.enabled)
            {
                kvp.Value.canvas.enabled = false;
                kvp.Value.graphicRaycaster.enabled = false;
            }
        }
    }
    public void BuildingsActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        CraftablesOff();
        ResearchablesOff();
        WorkersOff();

        txtMainHeader.text = "Buildings:";

        if (leftButton.activeSelf)
        {
            leftButton.SetActive(false);
        }
        if (!rightButton.activeSelf)
        {
            rightButton.SetActive(true);
        }

        foreach (var gatheringObj in gatheringObjects)
        {
            if (!gatheringObj.enabled)
            {
                gatheringObj.enabled = true;
            }
        }

        foreach (var kvp in Building.Buildings)
        {
            kvp.Value.hasSeen = true;
            if (kvp.Value.isUnlocked && !kvp.Value.canvas.enabled && !kvp.Value.isFirstUnlocked)
            {
                kvp.Value.objMainPanel.SetActive(true);
                kvp.Value.canvas.enabled = true;
                kvp.Value.isFirstUnlocked = true;
                kvp.Value.graphicRaycaster.enabled = true;
            }
            else if (kvp.Value.isUnlocked && !kvp.Value.canvas.enabled)
            {
                kvp.Value.canvas.enabled = true;
                kvp.Value.graphicRaycaster.enabled = true;
            }
        }

        foreach (var kvp in Craftable.Craftables)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var kvp in Worker.Workers)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var kvp in Researchable.Researchables)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        isBuildingVisible = true;
        isCraftingVisible = false;
        isResearchVisible = false;
        isWorkerVisible = false;

        UpdatePopNotication();
    }
    public void CraftablesActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        ResearchablesOff();
        WorkersOff();
        BuildingsOff();

        txtMainHeader.text = "Crafting:";

        if (!leftButton.activeSelf)
        {
            leftButton.SetActive(true);
        }
        if (!rightButton.activeSelf)
        {
            rightButton.SetActive(true);
        }

        foreach (var kvp in Craftable.Craftables)
        {
            kvp.Value.hasSeen = true;
            if (kvp.Value.isUnlocked && !kvp.Value.canvas.enabled && !kvp.Value.isFirstUnlocked)
            {
                kvp.Value.objMainPanel.SetActive(true);
                kvp.Value.canvas.enabled = true;
                kvp.Value.isFirstUnlocked = true;
                kvp.Value.graphicRaycaster.enabled = true;
            }
            else if (kvp.Value.isUnlocked && !kvp.Value.canvas.enabled)
            {
                kvp.Value.canvas.enabled = true;
                kvp.Value.graphicRaycaster.enabled = true;
            }
        }

        foreach (var kvp in Building.Buildings)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var kvp in Worker.Workers)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        foreach (var kvp in Researchable.Researchables)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        isBuildingVisible = false;
        isCraftingVisible = true;
        isResearchVisible = false;
        isWorkerVisible = false;

        UpdatePopNotication();
    }
    public void WorkersActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        CraftablesOff();
        ResearchablesOff();
        BuildingsOff();

        txtMainHeader.text = "Jobs:";

        if (!leftButton.activeSelf)
        {
            leftButton.SetActive(true);
        }
        if (!rightButton.activeSelf)
        {
            rightButton.SetActive(true);
        }

        foreach (var workerObject in workerObjects)
        {
            if (!workerObject.enabled)
            {
                workerObject.enabled = true;
            }
        }

        foreach (var kvp in Worker.Workers)
        {
            kvp.Value.hasSeen = true;
            if (kvp.Value.isUnlocked && !kvp.Value.canvas.enabled && !kvp.Value.isFirstUnlocked)
            {
                kvp.Value.objMainPanel.SetActive(true);
                kvp.Value.canvas.enabled = true;
                kvp.Value.isFirstUnlocked = true;
                kvp.Value.graphicRaycaster.enabled = true;
            }
            else if (kvp.Value.isUnlocked && !kvp.Value.canvas.enabled)
            {
                kvp.Value.canvas.enabled = true;
                kvp.Value.graphicRaycaster.enabled = true;
            }
        }

        foreach (var kvp in Building.Buildings)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var kvp in Craftable.Craftables)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var kvp in Researchable.Researchables)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        isBuildingVisible = false;
        isCraftingVisible = false;
        isResearchVisible = false;
        isWorkerVisible = true;

        UpdatePopNotication();
    }
    public void ResearchablesActive()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        CraftablesOff();
        WorkersOff();
        BuildingsOff();

        txtMainHeader.text = "Research:";

        if (!leftButton.activeSelf)
        {
            leftButton.SetActive(true);
        }
        if (rightButton.activeSelf)
        {
            rightButton.SetActive(false);
        }

        foreach (var kvp in Researchable.Researchables)
        {
            kvp.Value.hasSeen = true;
            if (kvp.Value.isUnlocked && !kvp.Value.canvas.enabled && !kvp.Value.isFirstUnlocked)
            {
                kvp.Value.objMainPanel.SetActive(true);
                kvp.Value.canvas.enabled = true;
                kvp.Value.isFirstUnlocked = true;
                kvp.Value.graphicRaycaster.enabled = true;
            }
            else if (kvp.Value.isUnlocked && !kvp.Value.canvas.enabled)
            {
                kvp.Value.canvas.enabled = true;
                kvp.Value.graphicRaycaster.enabled = true;
            }
        }

        foreach (var kvp in Building.Buildings)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var kvp in Craftable.Craftables)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        foreach (var kvp in Worker.Workers)
        {
            if (!kvp.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        isBuildingVisible = false;
        isCraftingVisible = false;
        isResearchVisible = true;
        isWorkerVisible = false;

        UpdatePopNotication();
    }
    private void SwipeCountHandler()
    {
        if (_Swipe.SwipeRight && (swipeCount >= 1))
        {
            //swipeCount--;
            //PanelHandler();
            animMainPanel.SetTrigger("hasSwipedRight");
        }
        else if (_Swipe.SwipeLeft && (swipeCount <= (_panelCount - 1)))
        {
            //swipeCount++;
            //PanelHandler();
            animMainPanel.SetTrigger("hasSwipedLeft");
        }

    }
    public void PanelHandler()
    {
        if (swipeCount == 0)
        {
            BuildingsActive();
        }
        else if (swipeCount == 1)
        {
            CraftablesActive();
        }
        else if (swipeCount == 2)
        {
            WorkersActive();
        }
        else if (swipeCount == 3)
        {
            ResearchablesActive();
        }
    }
    void Update()
    {
        SwipeCountHandler();
    }
}
