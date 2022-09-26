using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modifier_Headers : MonoBehaviour
{
    public GameObject objGroup;
    public GameObject objArrowDown, objArrowRight;

    private void Start()
    {
        InitializeButton();
        InitializeArrows();
        SwitchGroup();
    }
    private void InitializeArrows()
    {
        Transform tformArrowRight = transform.Find("Arrow_Right");
        Transform tformArrowDown = transform.Find("Arrow_Down");

        objArrowDown = tformArrowDown.gameObject;
        objArrowRight = tformArrowRight.gameObject;

        objArrowRight.SetActive(false);
    }
    private void ControlArrow()
    {
        if (objArrowDown.activeSelf)
        {
            objArrowDown.SetActive(false);
            objArrowRight.SetActive(true);
        }
        else
        {
            objArrowDown.SetActive(true);
            objArrowRight.SetActive(false);
        }
    }
    private void InitializeButton()
    {
        GetComponent<Button>().onClick.AddListener(SwitchGroup);
    }
    private void SwitchGroup()
    {
        if (objGroup.activeSelf)
        {
            objGroup.SetActive(false);
        }
        else
        {
            objGroup.SetActive(true);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(objGroup.GetComponent<RectTransform>());
        ControlArrow();
    }
}
