using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderArrows : MonoBehaviour
{
    public Animator animMainPanel;
    public void OnRightButton()
    {
        animMainPanel.SetTrigger("hasSwipedLeft");
    }
    public void OnLeftButton()
    {        
        animMainPanel.SetTrigger("hasSwipedRight");
    }
}
