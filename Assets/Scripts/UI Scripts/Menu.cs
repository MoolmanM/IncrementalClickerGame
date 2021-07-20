using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static bool isResearchHidden, isCraftingHidden, isGatheringHidden;

    public GameObject[] objGatheringButtons;

    private GameObject _objBtnOnGathering, _objBtnOffGathering, _objBtnOnCrafting, _objBtnOffCrafting, _objBtnOnResearch, _objBtnOffResearch;
    private Transform _tformBtnOffGathering, _tformBtnOnGathering, _tformBtnOffCrafting, _tformBtnOnCrafting, _tformBtnOffResearch, _tformBtnOnResearch;
    private string _stringIsResearchHidden = "IsResearchHidden", _stringIsCraftingHidden = "isCraftingHidden", _stringIsGatheringHidden = "isGatheringHidden";

    void Awake()
    {
        // Need to find the proper transforms now.
        _tformBtnOffGathering = transform.Find("Menu_Panel_Main/Options_Panel/Main_Panel/Switch_Gathering/ButtonOff");
        _tformBtnOnGathering = transform.Find("Menu_Panel_Main/Options_Panel/Main_Panel/Switch_Gathering/ButtonOn");
        _tformBtnOffCrafting = transform.Find("Menu_Panel_Main/Options_Panel/Main_Panel/Switch_Crafting/ButtonOff");
        _tformBtnOnCrafting = transform.Find("Menu_Panel_Main/Options_Panel/Main_Panel/Switch_Crafting/ButtonOn");
        _tformBtnOffResearch = transform.Find("Menu_Panel_Main/Options_Panel/Main_Panel/Switch_Research/ButtonOff");
        _tformBtnOnResearch = transform.Find("Menu_Panel_Main/Options_Panel/Main_Panel/Switch_Research/ButtonOn");

        _objBtnOnGathering = _tformBtnOnGathering.gameObject;
        _objBtnOffGathering = _tformBtnOffGathering.gameObject;
        _objBtnOnCrafting = _tformBtnOnCrafting.gameObject;
        _objBtnOffCrafting = _tformBtnOffCrafting.gameObject;
        _objBtnOnResearch = _tformBtnOnResearch.gameObject;
        _objBtnOffResearch = _tformBtnOffResearch.gameObject;

        isResearchHidden = PlayerPrefs.GetInt(_stringIsResearchHidden) == 1 ? true : false;
        isCraftingHidden = PlayerPrefs.GetInt(_stringIsCraftingHidden) == 1 ? true : false;
        isGatheringHidden = PlayerPrefs.GetInt(_stringIsGatheringHidden) == 1 ? true : false;

        if (isResearchHidden)
        {
            OffButtonResearch();
        }
        else
        {
            OnButtonResearch();
        }

        if (isCraftingHidden)
        {
            OffButtonCrafting();
        }
        else
        {
            OnButtonCrafting();
        }

        if (isGatheringHidden)
        {
            OffButtonGathering();
        }
        else
        {
            OnButtonGathering();
        }

    }
    public void OnButtonResearch()
    {
        isResearchHidden = false;

        _objBtnOnResearch.SetActive(false);
        _objBtnOffResearch.SetActive(true);

        foreach (var research in Researchable.Researchables)
        {
            if (research.Value.isUnlocked)
            {
                research.Value.objMainPanel.SetActive(true);
                research.Value.objSpacerBelow.SetActive(true);
            }
        }
    }
    public void OffButtonResearch()
    {
        isResearchHidden = true;

        _objBtnOnResearch.SetActive(true);
        _objBtnOffResearch.SetActive(false);

        foreach (var research in Researchable.Researchables)
        {
            if (research.Value.isUnlocked && research.Value.isResearched)
            {
                research.Value.objMainPanel.SetActive(false);
                research.Value.objSpacerBelow.SetActive(false);
            }
        }
    }
    public void OnButtonCrafting()
    {
        isCraftingHidden = false;

        _objBtnOnCrafting.SetActive(false);
        _objBtnOffCrafting.SetActive(true);

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.isUnlocked)
            {
                craft.Value.objMainPanel.SetActive(true);
                craft.Value.objSpacerBelow.SetActive(true);
            }
        }

    }
    public void OffButtonCrafting()
    {
        isCraftingHidden = true;

        _objBtnOnCrafting.SetActive(true);
        _objBtnOffCrafting.SetActive(false);

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.isUnlocked && craft.Value.isCrafted)
            {
                craft.Value.objMainPanel.SetActive(false);
                craft.Value.objSpacerBelow.SetActive(false);
            }
        }
    }
    public void OnButtonGathering()
    {
        isGatheringHidden = false;

        _objBtnOnGathering.SetActive(false);
        _objBtnOffGathering.SetActive(true);

        foreach (var obj in objGatheringButtons)
        {
            if (UIManager.isBuildingVisible)
            {
                obj.SetActive(true);
            }
        }
    }
    public void OffButtonGathering()
    {
        isGatheringHidden = true;

        _objBtnOnGathering.SetActive(true);
        _objBtnOffGathering.SetActive(false);

        foreach (var obj in objGatheringButtons)
        {
            obj.SetActive(false);
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsResearchHidden, isResearchHidden ? 1 : 0);
        PlayerPrefs.SetInt(_stringIsCraftingHidden, isCraftingHidden ? 1 : 0);
        PlayerPrefs.SetInt(_stringIsGatheringHidden, isGatheringHidden ? 1 : 0);
    }
}
