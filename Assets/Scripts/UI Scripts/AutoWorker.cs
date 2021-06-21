//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
//Set your own Text in the Inspector window

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoWorker : MonoBehaviour
{
    public GameObject objToggle;
    private Toggle toggleAutoWorker;
    public TMP_Text txtAutoWorker;
    public static bool isAutoWorkerOn;

    void Start()
    {
        //Fetch the Toggle GameObject
        toggleAutoWorker = objToggle.GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        toggleAutoWorker.onValueChanged.AddListener(delegate 
        {
            ToggleValueChanged(toggleAutoWorker);
        });

        //Initialise the Text to say the first state of the Toggle
        //m_Text.text = "First Value : " + m_Toggle.isOn;
    }

    //Output the new state of the Toggle into Text
    public void ToggleValueChanged(Toggle change)
    {
        //txtAutoWorker.text = "New Value : " + toggleAutoWorker.isOn;
        if (toggleAutoWorker.isOn)
        {
            isAutoWorkerOn = true;
        }
        else if(toggleAutoWorker.isOn == false)
        {
            isAutoWorkerOn = false;
        }
        else
        {
            Debug.LogError("This shouldn't happen");
        }
    }
}
