using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject buildingMainPanel, craftingMainPanel, headerText, buildPanel, layoutGroup;
    public Swipe swipeControls;
    public static int menuValue = 0;
    private float buildPanelY, buildPanelSizeDeltaY;
    public Animation buildPanelAnimation;
    public void Start()
    {
        BuildingsTabActive();
        buildPanelY = buildPanel.transform.localPosition.y;
        buildPanelSizeDeltaY = buildPanel.GetComponent<RectTransform>().sizeDelta.y;
    }

    public void LargeBuildingTabActive()
    {
        buildingMainPanel.SetActive(true);
        craftingMainPanel.SetActive(false);
        headerText.GetComponent<TextMeshProUGUI>().text = "Buildings:";
        buildPanel.transform.localPosition = new Vector2(-0.2293701f, buildPanelY);
        buildPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(-0.5004883f, buildPanelSizeDeltaY);
        buildPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.2293701f, buildPanelY);
        layoutGroup.GetComponent<GridLayoutGroup>().constraintCount = 2;
        layoutGroup.GetComponent<GridLayoutGroup>().cellSize = new Vector2(540, 80);
    }
    public void BuildingsTabActive()
    {
        buildingMainPanel.SetActive(true);
        craftingMainPanel.SetActive(false);
        headerText.GetComponent<TextMeshProUGUI>().text = "Buildings:";
        buildPanel.transform.localPosition = new Vector2(158, -37.1001f);
        buildPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(-316.9592f, -74.12451f);
        buildPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(158, -37.1001f);
        layoutGroup.GetComponent<GridLayoutGroup>().constraintCount = 1;
        layoutGroup.GetComponent<GridLayoutGroup>().cellSize = new Vector2(763, 80);
    }
    public void CraftingTabActive()
    {
        buildingMainPanel.SetActive(false);
        craftingMainPanel.SetActive(true);
        headerText.GetComponent<TextMeshProUGUI>().text = "Crafting:";
    }
    private void Swiping()
    {
        if (menuValue < -1)
        {
            menuValue = -1;
        }
        if (menuValue > 1)
        {
            menuValue = 1;
        }

        if (swipeControls.SwipeLeft)
        {
            buildPanelAnimation.Play("BuildPanelSwipeLeft");
            //buildPanelAnimation.GetComponent<AnimationEvent>.
            //menuValue++;
            
        }
        else if (swipeControls.SwipeRight)
        {
            menuValue--;
        }

    }

    private void Update()
    {
        Swiping();
        if (menuValue == -1)
        {
            LargeBuildingTabActive();
        }
        if (menuValue == 0)
        {
            BuildingsTabActive();          
        }
        else if (menuValue == 1)
        {
            CraftingTabActive();
        }
    }
}
