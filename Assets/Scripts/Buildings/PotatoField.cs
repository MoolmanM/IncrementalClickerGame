using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotatoField : Building
{
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        _resourceMultiplier = 0.10f;
        _costMultiplier = 1.15f;
        resourceTypeToModify = ResourceType.Food;
        SetInitialValues();

        //for (int i = 0; i < 2; i++)
        //{
        //    GameObject newObj = Instantiate(costPrefab, _tformBody);
        //    Debug.Log("Spawned stuff");
        //    _tformCostname = transform.Find("Panel_Main/Header_Panel/Button_Main");
        //    //_tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        //    //resourceCost[i].uiForResourceCost.textCostAmount
        //    // Okay so this works, but I need to assign the correct values to the texts.
        //}
    }
    private void Start()
    {  
        SetDescriptionText();
        //DisplayConsole();
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<BuildingType, Building> kvp in Buildings)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}
