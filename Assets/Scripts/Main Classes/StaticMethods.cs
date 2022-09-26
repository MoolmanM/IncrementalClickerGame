using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaticMethods : MonoBehaviour
{
    public static void ModifyAPSText(float aps, TMP_Text txtAPS)
    {
        // FF0000
        // <color=#FFCBFA>{0}</color>
        // <color=#FF0000>
        if (aps < 0.0f)
        {
            txtAPS.text = string.Format("<color=#FF0000>{0:0.00}/sec</color>", aps);
        }
        else
        {
            txtAPS.text = string.Format("+{0:0.00}/sec", aps);
        }
    }
}
