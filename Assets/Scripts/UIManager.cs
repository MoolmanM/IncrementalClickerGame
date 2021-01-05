using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject buildingMainPanel, craftingMainPanel, headerText;
    public Swipe swipeControls;
    private int menuValue = 0;
    public void Start()
    {
        BuildingsTabActive();
    }

    public void BuildingsTabActive()
    {
        buildingMainPanel.SetActive(true);
        craftingMainPanel.SetActive(false);
        headerText.GetComponent<TextMeshProUGUI>().text = "Buildings:";
    }
    public void CraftingTabActive()
    {
        buildingMainPanel.SetActive(false);
        craftingMainPanel.SetActive(true);
        headerText.GetComponent<TextMeshProUGUI>().text = "Crafting:";
    }
    private void Swiping()
    {
        if (menuValue < 0)
        {
            menuValue = 0;
        }
        if (menuValue > 1)
        {
            menuValue = 1;
        }

        if (swipeControls.SwipeLeft)
        {
            menuValue++;
        }
        else if (swipeControls.SwipeRight)
        {
            menuValue--;
        }
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
    /*public void HandleDropdownValue(int value)
    {

        if (value == 0)
        {
            Debug.Log(value);
            BuildingsTabActive();
            //make everything else not visible besides building tab
        }

        if (value == 1)
        {
            Debug.Log(value);
            CraftingTabActive();
            //make every not not visible besides crafting tab
        }

        if (value == 2)
        {
            Debug.Log(value);
            //make every tab not visible besides research tab
        }
    }*/
}
