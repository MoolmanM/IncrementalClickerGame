using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TooltipScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool buttonPressed;
    public GameObject objTooltip;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        objTooltip.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        objTooltip.SetActive(false);
    }
}
