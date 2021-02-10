using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    public TMP_Text availableWorkerText;
    public GameObject buildingMainPanel, craftingMainPanel, workerMainPanel;
    public Swipe swipeControls;
    public static int menuValue;
    private int minSwipeValue, maxSwipeValue;
    public Animator mainPanelAnim;
    

    public void Start()
    {     
        BuildingsTabActive();
        minSwipeValue = 0;
        maxSwipeValue = 2;
        InvokeRepeating("UpdateDay", 0, (float)5);
    }
    public void UpdateCostAmountTexts()
    {
        var _buildingList = GameManager.Instance.buildingList;
        var _resourceList = GameManager.Instance.resourceList;
        var _craftingList = GameManager.Instance.craftingItemsList;

        for (int b = 0; b < _buildingList.Count; b++)
        {
            _buildingList[b].progressFillValues.currentValuesArray = new float[_buildingList[b].resourceCosts.Count];
            for (int i = 0; i < _buildingList[b].progressFillValues.currentValuesArray.Length; i++)
            {
                for (int r = 0; r < _resourceList.Count; r++)
                {
                    if (_resourceList[r].name == _buildingList[b].resourceCosts[i].resourceName)
                    {
                        _buildingList[b].progressFillValues.currentValuesArray[i] = _resourceList[r].inputValues.resourceAmount;
                    }
                }

            }
        }

        for (int b = 0; b < _buildingList.Count; b++)
        {
            _buildingList[b].progressFillValues.maxValuesArray = new float[_buildingList[b].resourceCosts.Count];
            for (int i = 0; i < _buildingList[b].progressFillValues.maxValuesArray.Length; i++)
            {
                _buildingList[b].progressFillValues.maxValuesArray[i] = _buildingList[b].resourceCosts[i].costAmount;
            }
        }

        for (int c = 0; c < _craftingList.Count; c++)
        {
            for (int i = 0; i < _craftingList[c].resourceCosts.Count; i++)
            {
                _craftingList[c].resourceCosts[i].costNameText.GetComponent<TextMeshProUGUI>().text = _craftingList[c].resourceCosts[i].resourceName;

                for (int r = 0; r < _resourceList.Count; r++)
                {
                    _craftingList[c].resourceCosts[i].costAmountText.GetComponent<TextMeshProUGUI>().text = _resourceList[r].inputValues.resourceAmount + "/" + _craftingList[c].resourceCosts[i].costAmount;

                }
            }

        }

        for (int b = 0; b < _resourceList.Count; b++)
        {
            _buildingList[b].objects.descriptionText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: {1}/sec", _buildingList[b].inputValues.descriptionString, _buildingList[b].inputValues.buildingResourceMultiplier);
            for (int i = 0; i < _buildingList[b].resourceCosts.Count; i++)
            {
                _buildingList[b].resourceCosts[i].costNameText.GetComponent<TextMeshProUGUI>().text = _buildingList[b].resourceCosts[i].resourceName;
                for (int r = 0; r < _resourceList.Count; r++)
                {
                    _buildingList[b].resourceCosts[i].costAmountText.GetComponent<TextMeshProUGUI>().text = _resourceList[r].inputValues.resourceAmount + "/" + _buildingList[b].resourceCosts[i].costAmount;
                }
            }
        }

        _buildingList[3].objects.descriptionText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}", _buildingList[3].inputValues.descriptionString);
    }
    public void Link()
    {
        EventManager.onPerSecondTick += UpdatePerSecondText;
    }
    public void Unlink()
    {
        EventManager.onPerSecondTick -= UpdatePerSecondText;
    }
    void UpdatePerSecondText(int id)
    {
        List<Resource> _resourceList = GameManager.Instance.resourceList;

        _resourceList[id].objects.resourceAmountText.text = string.Format("{0}", _resourceList[id].inputValues.resourceAmount);
    }       
    public void BuildingsTabActive()
    {
        buildingMainPanel.SetActive(true);
        craftingMainPanel.SetActive(false);
        workerMainPanel.SetActive(false);
    }
    public void CraftingTabActive()
    {
        buildingMainPanel.SetActive(false);
        craftingMainPanel.SetActive(true);
        workerMainPanel.SetActive(false);
    }
    public void WorkerTabActive()
    {
        buildingMainPanel.SetActive(false);
        craftingMainPanel.SetActive(false);
        workerMainPanel.SetActive(true);
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
            mainPanelAnim.SetTrigger("hasSwipedLeft");
        }
        else if (swipeControls.SwipeRight && (menuValue > minSwipeValue))
        {
            mainPanelAnim.SetTrigger("hasSwipedRight");
        }

    }
    public void UpdateDay()
    {
        //seasonObject.GetComponent<TextMeshProUGUI>().text = string.Format("Year {0} - {1}, day {2}", year, seasonText, day);
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
        else if (menuValue == 2)
        {
            WorkerTabActive();
        }
    }

}
