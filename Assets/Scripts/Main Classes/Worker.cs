using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum WorkerType
{
    Farmers,
    Woodcutters,
    Miners,
    Scholars
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

    // Make workercount nonserialized eventually, for now will use for debugging.
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
    private void OnApplicationQuit()
    {             
        PlayerPrefs.SetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        PlayerPrefs.SetInt(_workerString, (int)workerCount);
        PlayerPrefs.SetInt("TotalWorkerCount", (int)TotalWorkerCount);
    }
}
