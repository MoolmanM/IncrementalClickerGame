using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncrementSelect : MonoBehaviour
{
    public static bool IsOneSelected, IsTenSelected, IsHundredSelected, IsMaxSelected;
    public Button button1, button10, button100, buttonMax;
    public Button[] incrementButtons;
    void Start()
    {
        button1.onClick.Invoke();
        IsOneSelected = true;

        ToggleButtons(button1);
    }
    private void ToggleButtons(Button button)
    {
        foreach (var btn in incrementButtons)
        {
            if (btn.interactable == false)
            {
                btn.interactable = true;
            }
        }
        button.interactable = false;
    }
    public void On1()
    {
        IsOneSelected = true;
        IsTenSelected = false;
        IsHundredSelected = false;
        IsMaxSelected = false;

        ToggleButtons(button1);
    }

    public void On10()
    {
        IsOneSelected = false;
        IsTenSelected = true;
        IsHundredSelected = false;
        IsMaxSelected = false;

        ToggleButtons(button10);
    }

    public void On100()
    {
        IsOneSelected = false;
        IsTenSelected = false;
        IsHundredSelected = true;
        IsMaxSelected = false;

        ToggleButtons(button100);
    }

    public void OnMax()
    {
        IsOneSelected = false;
        IsTenSelected = false;
        IsHundredSelected = false;
        IsMaxSelected = true;

        ToggleButtons(buttonMax);
    }
}
