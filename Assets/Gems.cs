using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gems : MonoBehaviour
{
    public static uint gemAmount;

    public void ModifyGemText()
    {
        GetComponent<TMP_Text>().text = gemAmount.ToString();
    }
}
