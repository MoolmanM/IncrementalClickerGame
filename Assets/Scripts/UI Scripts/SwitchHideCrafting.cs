using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchHideCrafting : MonoBehaviour
{
    public static bool isCraftingHidden;

    private GameObject btnOn, btnOff;
    private Transform tformBtnOn, tformBtnOff;

    private string _stringIsCraftingHidden = "IsCraftingHidden";
    void Awake()
    {
        tformBtnOn = transform.Find("ButtonOn");
        tformBtnOff = transform.Find("ButtonOff");

        btnOn = tformBtnOn.gameObject;
        btnOff = tformBtnOff.gameObject;

        isCraftingHidden = PlayerPrefs.GetInt(_stringIsCraftingHidden) == 1 ? true : false;

        Debug.Log(isCraftingHidden);

        if (isCraftingHidden)
        {
            Debug.Log("Reached here");
            OnHideCrafting();
        }
        else
        {
            Debug.Log("Reached here");
            OnUnhideCrafting();
        }
    }
    public void OnHideCrafting()
    {
        isCraftingHidden = true;

        btnOn.SetActive(false);
        btnOff.SetActive(true);

        foreach (var craft in Craftable.Craftables)
        {
            Debug.Log("Reached here");
            if (craft.Value.isUnlocked)
            {
                Debug.Log("Reached here after if");
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }
        }
    }
    public void OnUnhideCrafting()
    {
        isCraftingHidden = false;

        btnOn.SetActive(true);
        btnOff.SetActive(false);

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.isUnlocked && craft.Value.isCrafted)
            {
                craft.Value.objMainPanel.SetActive(true);
                craft.Value.objSpacerBelow.SetActive(true);
            }
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsCraftingHidden, isCraftingHidden ? 1 : 0);
    }
}
