using UnityEngine;

public class SwipeAnimEvent : MonoBehaviour
{
    public UIManager uiManager;
    public void SwipedRight()
    {
        uiManager.swipeCount--;
        uiManager.PanelHandler();
    }
    public void SwipedLeft()
    {
        uiManager.swipeCount++;
        uiManager.PanelHandler();
    }
}
