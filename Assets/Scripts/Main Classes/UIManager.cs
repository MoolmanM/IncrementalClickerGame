using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    /*
    public Swipe _Swipe;
    private uint _swipeCount = 0;
    private readonly uint _panelCount = 3;
    public GameObject BuildingPanel, CraftingPanel, WorkerPanel, ResearchPanel;
        
    private void Start()
    {
        _swipeCount = 0;
        BuildingPanelActive();
    }
    private void OnApplicationQuit()
    {
        // Just before quitting it sets all objects active so everything can get saved to playerprefs or savefile.
        BuildingPanel.SetActive(true);
        CraftingPanel.SetActive(true);
        WorkerPanel.SetActive(true);
        ResearchPanel.SetActive(true);
    }
    private void BuildingPanelActive()
    {
        BuildingPanel.SetActive(true);
        CraftingPanel.SetActive(false);
        WorkerPanel.SetActive(false);
        ResearchPanel.SetActive(false);
    }
    private void CraftingPanelActive()
    {
        BuildingPanel.SetActive(false);
        CraftingPanel.SetActive(true);
        WorkerPanel.SetActive(false);
        ResearchPanel.SetActive(false);
    }
    private void WorkerPanelActive()
    {
        BuildingPanel.SetActive(false);
        CraftingPanel.SetActive(false);
        WorkerPanel.SetActive(true);
        ResearchPanel.SetActive(false);
    }
    private void ResearchPanelActive()
    {
        BuildingPanel.SetActive(false);
        CraftingPanel.SetActive(false);
        WorkerPanel.SetActive(false);
        ResearchPanel.SetActive(true);
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
    //private void CheckIfUnlockedYet()
    //{
    //    foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftable.Craftables)
    //    {
    //        //Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));

    //        for (int i = 0; i < kvp.Value.resourceCost.Length; i++)
    //        {
    //            float[] amountsRequiredForUnlocking = new float[kvp.Value.resourceCost.Length];
    //            amountsRequiredForUnlocking[i] = kvp.Value.resourceCost[i].CostAmount * 0.8f;
    //            // Debug.Log(entry.Value + " " + entry.Value.resourceCost[i]._AssociatedType + " " + amountsRequiredForUnlocking[i]);

    //            if (Resource._resources[kvp.Value.resourceCost[i]._AssociatedType].Amount >= amountsRequiredForUnlocking[i])
    //            {
    //                kvp.Value.IsUnlocked = 1;
    //                kvp.Value.ObjMainPanel.SetActive(true);
    //                kvp.Value.ObjSpacerBelow.SetActive(true);
    //            }
    //        }
    //    }
    //}

    //private void SetAllActive()
    //{
    //    //foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftable.Craftables)
    //    //{
    //    //    //kvp.Value.IsUnlocked = 1;
    //    //}

    //    foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftable.Craftables)
    //    {
    //        //Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));

    //        for (int i = 0; i < kvp.Value.resourceCost.Length; i++)
    //        {
    //            float[] amountsRequiredForUnlocking = new float[kvp.Value.resourceCost.Length];
    //            amountsRequiredForUnlocking[i] = kvp.Value.resourceCost[i].CostAmount * 0.8f;
    //            // Debug.Log(entry.Value + " " + entry.Value.resourceCost[i]._AssociatedType + " " + amountsRequiredForUnlocking[i]);

    //            if (Resource._resources[kvp.Value.resourceCost[i]._AssociatedType].Amount >= amountsRequiredForUnlocking[i])
    //            {
    //                kvp.Value.IsUnlocked = 1;
    //                //kvp.Value.ObjMainPanel.SetActive(true);
    //                //kvp.Value.ObjSpacerBelow.SetActive(true);
    //            }
    //        }
    //    }
    //}
    void Update()
    {       
        SwipeCountHandler();
        // CheckIfUnlockedYet();
    }
    */
}
