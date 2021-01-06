using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public GameObject buildingMainPanel, craftingMainPanel, headerText, buildPanel, mainGroup, sideGroup;
    public Swipe swipeControls;
    public static int menuValue = 0;
    public Animation buildPanelAnimation;
    private int halfOfParent, cacheOfHalf;

    public void Start()
    {     
        BuildingsTabActive();
        if (mainGroup.transform.childCount % 2 != 0)
        {
            halfOfParent = (mainGroup.transform.childCount / 2) + 1;
            cacheOfHalf = halfOfParent;
        }
        else
        {
            halfOfParent = (mainGroup.transform.childCount / 2);
        }
        while (mainGroup.transform.childCount > cacheOfHalf)
        {
            mainGroup.transform.GetChild(Random.Range(0, mainGroup.transform.childCount)).SetParent(sideGroup.transform, false);
        }
    }

    public void LargeBuildingTabActive()
    {
        buildingMainPanel.SetActive(true);
        craftingMainPanel.SetActive(false);
        headerText.GetComponent<TextMeshProUGUI>().text = "Buildings:";
        sideGroup.SetActive(true);
        /*if (mainGroup.transform.childCount % 2 != 0)
        {
            halfOfParent = (mainGroup.transform.childCount / 2) + 1;
            cacheOfHalf = halfOfParent;
        }
        else
        {
            halfOfParent = (mainGroup.transform.childCount / 2);
        } */
        halfOfParent = (mainGroup.transform.childCount / 2);
        cacheOfHalf = halfOfParent;
        if (mainGroup.transform.childCount > cacheOfHalf)
        {
            mainGroup.transform.GetChild(Random.Range(0, mainGroup.transform.childCount + 1)).SetParent(sideGroup.transform, false);
        }

        mainGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(540, 80);
        buildPanel.transform.localPosition = new Vector2(0.01062012f, -37.1001f);
        buildPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(-0.02050781f, -74.12451f);
        buildPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.01062012f, -37.1001f);
    }
    public void BuildingsTabActive()
    {
        buildingMainPanel.SetActive(true);
        craftingMainPanel.SetActive(false);
        headerText.GetComponent<TextMeshProUGUI>().text = "Buildings:";
        //while (sideGroup.transform.childCount > 0)
        //{
        //    sideGroup.transform.GetChild(sideGroup.transform.childCount - 1).SetParent(mainGroup.transform, false);
        //}
        //sideGroup.SetActive(false);
        buildPanel.transform.localPosition = new Vector2(158.24f, -37.1001f);
        buildPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(-316.4792f, -74.12451f);
        buildPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(158.24f, -37.1001f);
        mainGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(763, 80);
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
        else if (menuValue == 0)
        {
            BuildingsTabActive();          
        }
        else if (menuValue == 1)
        {
            CraftingTabActive();
        }
    }
}
