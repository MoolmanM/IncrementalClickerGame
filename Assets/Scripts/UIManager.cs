using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public GameObject buildingMainPanel, craftingMainPanel;
    public Swipe swipeControls;
    public static int menuValue;
    private int minSwipeValue, maxSwipeValue;
    public Animator buildPanelAnim, SettingsPanelAnim;

    public void Start()
    {     
        BuildingsTabActive();
        minSwipeValue = 0;
        maxSwipeValue = 1;
    }

    public void BuildingsTabActive()
    {
        buildingMainPanel.SetActive(true);
        craftingMainPanel.SetActive(false);
    }
    public void CraftingTabActive()
    {
        buildingMainPanel.SetActive(false);
        craftingMainPanel.SetActive(true);
    }
    private void Swiping()
    {
        if (menuValue < minSwipeValue)
        {
            menuValue = minSwipeValue;
        }
        if (menuValue > maxSwipeValue)
        {
            menuValue = maxSwipeValue;
        }

        if (swipeControls.SwipeLeft && (menuValue < maxSwipeValue))
        {        
            buildPanelAnim.SetTrigger("hasSwipedLeft");
        }
        else if (swipeControls.SwipeRight && (menuValue > minSwipeValue))
        {        
            buildPanelAnim.SetTrigger("hasSwipedRight");
        }

    }

    public void OnSettingsButton()
    {
        SettingsPanelAnim.SetTrigger("Enter");
    }

    public void OnBackButton()
    {
        SettingsPanelAnim.SetTrigger("Exit");
    }
    private void Update()
    {
        Swiping();
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
