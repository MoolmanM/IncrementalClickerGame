using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnmOptions : ButtonAnimation
{
    public GameObject objSettingsPanel;
    public GameObject[] objOptionsElements;
    public override void OnAnimationEnd()
    {
        foreach(var ui in objOptionsElements)
        {
            ui.SetActive(true);
        }
        objSettingsPanel.SetActive(false);
        base.OnAnimationEnd();
    }
}
