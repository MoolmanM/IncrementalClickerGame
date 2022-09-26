using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class PointerEvent : UnityEngine.Events.UnityEvent<PointerEventData> { };

public class PopupEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PointerEvent OnPointerDownEvent, OnPointerUpEvent;
    public void OnPointerDown(PointerEventData data)
    {
        if (OnPointerDownEvent != null)
            OnPointerDownEvent.Invoke(data);
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (OnPointerUpEvent != null)
            OnPointerUpEvent.Invoke(data);
    }
}
