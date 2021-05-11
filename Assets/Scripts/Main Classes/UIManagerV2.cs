using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerV2 : MonoBehaviour
{
    public Swipe _Swipe;
    private uint _swipeCount = 0;
    private readonly uint _panelCount = 3;
    public GameObject BuildingPanel, CraftingPanel, WorkerPanel, ResearchPanel;

    private void Start()
    {
        _swipeCount = 0;
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
}
