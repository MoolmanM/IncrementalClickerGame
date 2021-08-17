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
        
        _resourcesToIncrement = new ResourcesToModify[1];
        _resourcesToIncrement[0].resourceTypeToModify = ResourceType.Stones;
        _resourcesToIncrement[0].resourceMultiplier = 0.10f;
        SetInitialValues();
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<WorkerType, Worker> kvp in Workers)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
}
