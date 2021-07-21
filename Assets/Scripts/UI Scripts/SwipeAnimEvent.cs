using UnityEngine;

public class SwipeAnimEvent : MonoBehaviour
{
    public void SwipedRight()
    {
        UIManager.swipeCount--;
    }
    public void SwipedLeft()
    {
        UIManager.swipeCount++;
    }
}
