using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestScript : MonoBehaviour
{
    public GameObject mainPanel;

    [Button]
    public void SetActive()
    {
        mainPanel.SetActive(true);
    }
    [Button]
    public void SetInactive()
    {
        mainPanel.SetActive(false);
    }
}
