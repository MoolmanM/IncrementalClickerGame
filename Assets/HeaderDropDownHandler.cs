using UnityEngine;

public class HeaderDropDownHandler : MonoBehaviour
{
    public UIManager uiManager;

    public GameObject[] objSelectors = new GameObject[4];

    public void ButtonBuilding()
    {
        uiManager.swipeCount = 0;
        uiManager.PanelHandler();
    }

    public void ButtonCrafting()
    {
        uiManager.swipeCount = 1;
        uiManager.PanelHandler();
    }

    public void ButtonWorkers()
    {
        uiManager.swipeCount = 2;
        uiManager.PanelHandler();
    }

    public void ButtonResearch()
    {
        uiManager.swipeCount = 3;
        uiManager.PanelHandler();
    }
    public void ButtonDropDown()
    {
        foreach (var item in objSelectors)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
            }
            if (uiManager.swipeCount == 3)
            {
                objSelectors[3].SetActive(false);
            }
            else if (uiManager.swipeCount == 2)
            {
                objSelectors[2].SetActive(false);
            }
            else if (uiManager.swipeCount == 1)
            {
                objSelectors[1].SetActive(false);
            }
            else
            {
                objSelectors[0].SetActive(false);
            }
        }
    }
}
