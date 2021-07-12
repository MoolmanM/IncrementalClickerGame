using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum WorkerType
{
    Farmers,
    Woodcutters,
    Miners,
    Hunters,
    Scholars

        // So do we want hunters, for start. maybe just do for every worker there would be hunts automatically conducted. 
        // So every 500 seconds there's a hunt that lasts 500 seconds. When they leave it costs a certain amount of resources, depending on how many workers was assigned at that time.
        // Maybe have a timer somewhere that your hunters are hunting, maybe in the worker tab.
        // I can grey out the hunter main panel, Change text to "Hunting..." With a progress bar attached like I have in research panel.
        // Maybe just make the progress bar a different color such as orange.
        // And then when they come back you'll get a certain random amount of resources based on some sort of loot pool.
        // food 500 - 1000
        // Leather 0 - 2
        // Bones/tusks?
        // Pelts is the same thing as leather? Should I convert pelts to leather, can pelts be made into something else.
        // Tanning Rack - That seperates the pelt into fur and leather.
        // Should this just be in a new tab workshop where you refine your resources into resources a tier higher? or well in this case from pelt to - leater and fur
        // For example 1 pelt ---> 3 fur and 1 leather.
        // Maybe should have an animals killed variable on the hunts.
        // For each animal 100 - 200 food or whatever and 0 - 2 pelts. Something like that.
        // fur will of course be used for clothing
        // leather can be used for tents as well as various other stuff.
        // Hunter workerType should probably be unlocked when the player has crafted the Stone Spear? 
        // Could have different weapon tiers For example:
        // Wooden Spear - 100 - 200 0 - 2 pelts.
        // Stone Spear 120 - 220 0 - 2 pelts. 
        // Fire Hardended Spear 150 - 250, 0 - 2 pelts.
        // Which is why we should maybe have a weapons tab, but for now lets just do it in the crafting panel.
        // So unlock hunter workertype on the crafting of any weapon type. for in case the player decided to skip other weapon craftings.
}

public class Worker : MonoBehaviour
{
    public static Dictionary<WorkerType, Worker> Workers = new Dictionary<WorkerType, Worker>();

    public static uint TotalWorkerCount, UnassignedWorkerCount;
    public static bool isUnlockedEvent;

    [System.NonSerialized] public ResourceType resourceTypeToModify;
    [System.NonSerialized] public GameObject objMainPanel;
    [System.NonSerialized] public TMP_Text txtHeader;
    [System.NonSerialized] public bool isUnlocked, hasSeen = true;
    [System.NonSerialized] public float resourceMultiplier, incrementAmount;

    // Make workercount non serialized eventually, for now will use for debugging.
    public uint workerCount;

    public WorkerType Type;
    public GameObject objSpacerBelow;
    public TMP_Text txtAvailableWorkers;

    private uint _changeAmount = 1;
    private Transform _tformTxtHeader, _tformObjMainPanel;
    private string _workerString;

    protected void SetInitialValues()
    {
        InitializeObjects();        
    }
    protected void InitializeObjects()
    {
        _tformTxtHeader = transform.Find("Panel_Main/Text_Header");
        _tformObjMainPanel = transform.Find("Panel_Main");

        txtHeader = _tformTxtHeader.GetComponent<TMP_Text>();
        objMainPanel = _tformObjMainPanel.gameObject;

        _workerString = (Type.ToString() + "workerCount");

        workerCount = (uint)PlayerPrefs.GetInt(_workerString, (int)workerCount);
        UnassignedWorkerCount = (uint)PlayerPrefs.GetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        TotalWorkerCount = (uint)PlayerPrefs.GetInt("TotalWorkerCount", (int)TotalWorkerCount);


        txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
        txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", UnassignedWorkerCount);

        if (isUnlocked)
        {
            objMainPanel.SetActive(true);
            objSpacerBelow.SetActive(true);
        }
        else
        {
            objMainPanel.SetActive(false);
            objSpacerBelow.SetActive(false);
        }
        if (AutoToggle.isAutoWorkerOn == 1)
        {
            AutoWorker.CalculateWorkers();
            AutoWorker.AutoAssignWorkers();
        }      
    }
    public void OnPlusButton()
    {
        // amountPerSecond = 0;
        // amountPerSecond += workermultiplier 

        if (UnassignedWorkerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                _changeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (UnassignedWorkerCount < 10)
                {
                    _changeAmount = UnassignedWorkerCount;
                }
                else
                {
                    _changeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (UnassignedWorkerCount < 100)
                {
                    _changeAmount = UnassignedWorkerCount;
                }
                else
                {
                    _changeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                _changeAmount = UnassignedWorkerCount;
            }
            UnassignedWorkerCount -= _changeAmount;
            workerCount += _changeAmount;
            txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
            txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", UnassignedWorkerCount);

            incrementAmount = (_changeAmount * resourceMultiplier);
            Resource.Resources[resourceTypeToModify].amountPerSecond += incrementAmount;
        }       
    }
    public void OnMinusButton()
    {
        if (workerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                _changeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (workerCount < 10)
                {
                    _changeAmount = workerCount;
                }
                else
                {
                    _changeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (workerCount < 100)
                {
                    _changeAmount = workerCount;
                }
                else
                {
                    _changeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                _changeAmount = workerCount;
            }
            UnassignedWorkerCount += _changeAmount;
            workerCount -= _changeAmount;
            txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
            txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", UnassignedWorkerCount);

            incrementAmount = (_changeAmount * resourceMultiplier);
            Resource.Resources[resourceTypeToModify].amountPerSecond -= incrementAmount;
        }     
    }
    void OnApplicationQuit()
    {             
        PlayerPrefs.SetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        PlayerPrefs.SetInt(_workerString, (int)workerCount);
        PlayerPrefs.SetInt("TotalWorkerCount", (int)TotalWorkerCount);
    }
}
