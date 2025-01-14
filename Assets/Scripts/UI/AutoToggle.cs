//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
//Set your own Text in the Inspector window

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoToggle : MonoBehaviour
{
    public GameObject objToggle;
    private Toggle toggleAutoWorker;
    public TMP_Text txtAutoWorker;
    public static uint isAutoWorkerOn;
    public GameObject[] manualButtons;

    void Start()
    {
        isAutoWorkerOn = (uint)PlayerPrefs.GetInt("IsAutoWorkerOn", (int)isAutoWorkerOn);

        if (isAutoWorkerOn == 1)
        {
            toggleAutoWorker.isOn = true;
        }

        toggleAutoWorker = objToggle.GetComponent<Toggle>();

        toggleAutoWorker.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(toggleAutoWorker);
        });

        //Initialise the Text to say the first state of the Toggle
        //m_Text.text = "First Value : " + m_Toggle.isOn;
    }
    public void DisableManualButtons()
    {
        foreach(var button in manualButtons)
        {
            button.SetActive(false);
        }
    }
    public void EnableManualButtons()
    {
        foreach (var button in manualButtons)
        {
            button.SetActive(true);
        }
    }
    public void ToggleValueChanged(Toggle change)
    {
        //txtAutoWorker.text = "New Value : " + toggleAutoWorker.isOn;
        if (toggleAutoWorker.isOn)
        {
            isAutoWorkerOn = 1;
            AutoWorker.CalculateWorkers();
            AutoWorker.AutoAssignWorkers();
            DisableManualButtons();
        }
        else if(toggleAutoWorker.isOn == false)
        {
            isAutoWorkerOn = 0;
            EnableManualButtons();
        }
        else
        {
            Debug.LogError("This shouldn't happen");
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("IsAutoWorkerOn", (int)isAutoWorkerOn);
    }
}
