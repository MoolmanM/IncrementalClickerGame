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

    public TMP_Text HeaderText;  
    //This can be protected/private.
    public uint WorkerCount;
    public WorkerType Type;

    public void OnPlusButton()
    {
        if (AvailableWorkerCount > 0)
        {
            AvailableWorkerCount--;
            WorkerCount++;
        }
        HeaderText.text = string.Format("{0} [{1}]", Type.ToString(), WorkerCount);
    }

    public void OnMinusButton()
    {
        if (WorkerCount > 0)
        {
            AvailableWorkerCount++;
            WorkerCount--;
        }
        HeaderText.text = string.Format("{0} [{1}]", Type.ToString(), WorkerCount);
    }
}
