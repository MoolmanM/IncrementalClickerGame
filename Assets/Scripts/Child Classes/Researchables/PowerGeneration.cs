using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGeneration : Researchable
{
    private Researchable _researchable;
    public Energy energy;

    void Awake()
    {
        _researchable = GetComponent<Researchable>();
        Researchables.Add(Type, _researchable);

        SetInitialValues();

        if (isResearched)
        {
            energy.objIconPanel.SetActive(true);
        }
        else
        {
            energy.objIconPanel.SetActive(false);
        }
    }
    protected override void Researched()
    {
        isResearched = true;

        researchSimulActive--;
        UnlockCrafting();
        UnlockBuilding();
        UnlockResearchable();
        UnlockWorkerJob();
        UnlockResource();
        energy.objIconPanel.SetActive(true);

        _btnMain.interactable = false;
        _objProgressCircle.SetActive(false);
        _objBackground.SetActive(false);
        _objCheckmark.SetActive(true);
        _txtHeader.text = string.Format("{0}", actualName);

        if (Menu.isResearchHidden)
        {
            if (objMainPanel.activeSelf)
            {
                objMainPanel.SetActive(false);
                canvas.enabled = false;
                graphicRaycaster.enabled = false;
            }
        }
    }
    void Start()
    {
        SetDescriptionText("Enables generation of power");
    }
}
