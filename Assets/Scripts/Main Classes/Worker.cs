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
    public static Dictionary<WorkerType, Worker> _workers = new Dictionary<WorkerType, Worker>();
    public static uint TotalWorkerCount;
    public static uint AvailableWorkerCount;
    public static uint ChangeAmount = 1;

    public TMP_Text HeaderText, AvailableWorkerText;  
    //This can be protected/private.
    public uint WorkerCount;
    public WorkerType Type;

    public void OnPlusButton()
    {
        if (AvailableWorkerCount > 0)
        {
            if (IncrementSelect.OneSelected)
            {
                ChangeAmount = 1;
            }
            if (IncrementSelect.TenSelected)
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
            if (IncrementSelect.HundredSelected)
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
            if (IncrementSelect.MaxSelected)
            {
                ChangeAmount = AvailableWorkerCount;
            }
            AvailableWorkerCount -= ChangeAmount;
            WorkerCount += ChangeAmount;
            HeaderText.text = string.Format("{0} [{1}]", Type.ToString(), WorkerCount);
            AvailableWorkerText.text = string.Format("Available Workers: [{0}]", AvailableWorkerCount);
        }       
    }

    public void OnMinusButton()
    {
        if (WorkerCount > 0)
        {
            if (IncrementSelect.OneSelected)
            {
                ChangeAmount = 1;
            }
            if (IncrementSelect.TenSelected)
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
            if (IncrementSelect.HundredSelected)
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
            if (IncrementSelect.MaxSelected)
            {
                ChangeAmount = WorkerCount;
            }
            AvailableWorkerCount += ChangeAmount;
            WorkerCount -= ChangeAmount;
            HeaderText.text = string.Format("{0} [{1}]", Type.ToString(), WorkerCount);
            AvailableWorkerText.text = string.Format("Available Workers: [{0}]", AvailableWorkerCount);
        }     
    }
}
