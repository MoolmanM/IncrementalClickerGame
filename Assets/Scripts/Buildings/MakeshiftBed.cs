using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeshiftBed : Building
{
    private Building _building;
    public TMP_Text availableWorkerText;

    private void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(_Type, _building);
        
    }
    private void Start()
    {
        SetInitialValues();        
        SetDescriptionText();
        // DisplayConsole();
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<BuildingType, Building> kvp in Buildings)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    public override void SetDescriptionText()
    {
        TxtDescription.text = string.Format("Increases population by 1");

        // availableWorkerText.text = string.Format("Available Workers: [{0}]", Worker.AvailableWorkerCount);
        // Need to do that in the override Build here.
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}
