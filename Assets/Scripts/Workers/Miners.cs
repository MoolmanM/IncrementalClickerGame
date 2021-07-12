using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miners : Worker
{
    private Worker _worker;

    void Awake()
    {
        _worker = GetComponent<Worker>();
        Workers.Add(Type, _worker);
        SetInitialValues();
        resourceMultiplier = 0.10f;
        resourceTypeToModify = ResourceType.Stones;
        // DisplayConsole();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<WorkerType, Worker> kvp in Workers)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
}
