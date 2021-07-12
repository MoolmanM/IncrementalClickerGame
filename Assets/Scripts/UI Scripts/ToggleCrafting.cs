using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCrafting : MonoBehaviour
{
    public static int isCraftingHidden;

    public GameObject buttonOn, buttonOff;

    private string _stringIsCraftingHidden = "IsCraftingHidden";
    void Awake()
    {
        PlayerPrefs.GetInt(_stringIsCraftingHidden, isCraftingHidden);
        Debug.Log(isCraftingHidden);
        if (isCraftingHidden == 1)
        {
            Debug.Log("Reached here");
            ToggleCraftingOn();
        }
        else
        {
            Debug.Log("Reached here");
            ToggleCraftingOff();
        }
    }
    public void ToggleCraftingOn()
    {
        isCraftingHidden = 0;

        buttonOn.SetActive(false);
        buttonOff.SetActive(true);

        foreach (var craft in Craftable.Craftables)
        {
            Debug.Log("Reached here");
            if (craft.Value.isUnlocked)
            {
                Debug.Log("Reached here after if");
                craft.Value.objMainPanel.SetActive(true);
                craft.Value.objSpacerBelow.SetActive(true);
            }
        }
    }
    public void ToggleCraftingOff()
    {
        isCraftingHidden = 1;

        buttonOn.SetActive(true);
        buttonOff.SetActive(false);

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.isUnlocked && craft.Value.isCrafted)
            {
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsCraftingHidden, isCraftingHidden);
        PlayerPrefs.GetInt(_stringIsCraftingHidden, isCraftingHidden);
        Debug.Log(isCraftingHidden);
    }
}
