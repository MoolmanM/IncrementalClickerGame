using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public UIManager ui;

    private void Start()
    {
        //Should maybe make the uimanager a singleton.
        //But only assign stuff in there that REALLY doesn't fit in anywhere else. like this availableWorkerText.
        Debug.Log(ui.availableWorkerText);
    }
    public void increaseWorkers()
    {
        Worker.AvailableWorkerCount++;
        //ui.availableWorkerText.text = string.Format("Available Workers: [{0}]", Worker.AvailableWorkerCount);
    }
}
