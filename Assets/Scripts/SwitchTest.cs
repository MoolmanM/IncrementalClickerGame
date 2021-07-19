using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchTest : MonoBehaviour
{
    private Transform tformBtnOn, tformBtnOff;
    private Button btnOn, btnOff;
    private void Awake()
    {
        tformBtnOn = transform.Find("ButtonOn");
        tformBtnOff = transform.Find("ButtonOff");

        btnOn = tformBtnOn.gameObject.GetComponent<Button>();
        btnOff = tformBtnOff.gameObject.GetComponent<Button>();

        btnOn.gameObject.SetActive(false);

        Debug.Log(btnOn);
    }
    public void OnSwitch()
    {
        if(btnOn.IsActive())
        {
            btnOff.onClick.Invoke();
        }
        else if(btnOff.IsActive())
        {
            btnOn.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("Uhh");
        }
    }
}
