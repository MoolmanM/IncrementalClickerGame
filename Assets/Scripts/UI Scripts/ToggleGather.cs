using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGather : MonoBehaviour
{
    public GameObject[] gatherButtons;

    public void ToggleGatherOn()
    {
        foreach (var uiElement in gatherButtons)
        {
            uiElement.SetActive(true);
        }
    }

    public void ToggleGatherOff()
    {
        foreach (var uiElement in gatherButtons)
        {
            uiElement.SetActive(false);
        }
    }

}
