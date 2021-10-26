using UnityEngine;

public class SwipeAnimEvent : MonoBehaviour
{
    public void SwipedRight()
    {
        //UIManager.swipeCount--;
        //_uiManager.swipeCount--;
        //_uiManager.PanelHandler();
        UIManager.Instance.swipeCount--;
        UIManager.Instance.PanelHandler();
        //_uiManager.PanelHandler();
    }
    public void SwipedLeft()
    {
        UIManager.Instance.swipeCount++;
        UIManager.Instance.PanelHandler();
        //UIManager.swipeCount++;
        //_uiManager.PanelHandler();
    }
}
