using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum WorkerType
{
    Farmers,
    Woodcutters,
    Miners
}

public class Worker : MonoBehaviour
{
    public static Dictionary<WorkerType, Worker> Workers = new Dictionary<WorkerType, Worker>();
    public static uint TotalWorkerCount;
    public static uint AvailableWorkerCount;
    public static uint ChangeAmount = 1;
    public uint WorkerCount;
    public WorkerType _Type;
    public TMP_Text TxtHeader, TxtAvailableWorkers;
    
    public void OnPlusButton()
    {
        if (AvailableWorkerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                ChangeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (AvailableWorkerCount < 10)
                {
                    ChangeAmount = AvailableWorkerCount;
                }
                else
                {
                    ChangeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (AvailableWorkerCount < 100)
                {
                    ChangeAmount = AvailableWorkerCount;
                }
                else
                {
                    ChangeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                ChangeAmount = AvailableWorkerCount;
            }
            AvailableWorkerCount -= ChangeAmount;
            WorkerCount += ChangeAmount;
            TxtHeader.text = string.Format("{0} [{1}]", _Type.ToString(), WorkerCount);
            TxtAvailableWorkers.text = string.Format("Available Workers: [{0}]", AvailableWorkerCount);
        }       
    }

    public void OnMinusButton()
    {
        if (WorkerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                ChangeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (WorkerCount < 10)
                {
                    ChangeAmount = WorkerCount;
                }
                else
                {
                    ChangeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (WorkerCount < 100)
                {
                    ChangeAmount = WorkerCount;
                }
                else
                {
                    ChangeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                ChangeAmount = WorkerCount;
            }
            AvailableWorkerCount += ChangeAmount;
            WorkerCount -= ChangeAmount;
            TxtHeader.text = string.Format("{0} [{1}]", _Type.ToString(), WorkerCount);
            TxtAvailableWorkers.text = string.Format("Available Workers: [{0}]", AvailableWorkerCount);
        }     
    }
}
