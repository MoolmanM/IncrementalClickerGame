using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmers : Worker
{
    private Worker _worker;

    private void Awake()
    {
        _worker = GetComponent<Worker>();
        Workers.Add(_Type, _worker);
        //DisplayConsole();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<WorkerType, Worker> kvp in Workers)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
}
