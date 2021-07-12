using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunters : Worker
{
    private Worker _worker;

    void Awake()
    {
        _worker = GetComponent<Worker>();
        Workers.Add(Type, _worker);
        SetInitialValues();
        resourceMultiplier = 0.10f;
        resourceTypeToModify = ResourceType.Pelts;

        // DisplayConsole();
        // Okay so hunters will modify both Food and Pelts.
        // But I need to modify the timer, and only start the timer every 500 seconds or whatever.
        // Will think about this later.
        // Orange progress bar like with research.
        // When I get the first pelt, it should unlock the resourceType Pelts.
        // Might have to have hunters completely seperate from the other workers, I'm not sure if this will work. To modify the time I need to modify the updateresources timer for
        // hunters alone. Which seems impossible?
    }

    private void DisplayConsole()
    {
        foreach (KeyValuePair<WorkerType, Worker> kvp in Workers)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
}
