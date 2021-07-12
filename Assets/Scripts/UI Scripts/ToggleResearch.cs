using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleResearch : MonoBehaviour
{
    public static int isResearchHidden;

    public GameObject buttonOn, buttonOff;

    private string _stringIsResearchHidden = "IsResearchHidden";
    void Awake()
    {
        PlayerPrefs.GetInt(_stringIsResearchHidden, isResearchHidden);
        if (isResearchHidden == 1)
        {
            ToggleResearchOff();
        }
        else
        {
            ToggleResearchOn();
        }
    }
    public void ToggleResearchOn()
    {
        isResearchHidden = 0;

        buttonOn.SetActive(false);
        buttonOff.SetActive(true);

        foreach (var research in Researchable.Researchables)
        {
            if (research.Value.isUnlocked)
            {
                research.Value.objMainPanel.SetActive(true);
                research.Value.objSpacerBelow.SetActive(true);
            }
        }
    }
    public void ToggleResearchOff()
    {
        isResearchHidden = 1;

        buttonOn.SetActive(true);
        buttonOff.SetActive(false);

        foreach (var research in Researchable.Researchables)
        {
            if (research.Value.isUnlocked && research.Value.isResearched)
            {
                research.Value.objMainPanel.SetActive(false);
                research.Value.objSpacerBelow.SetActive(false);
            }
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsResearchHidden, isResearchHidden);
    }

}
