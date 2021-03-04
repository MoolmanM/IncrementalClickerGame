using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sticks : MonoBehaviour
{
    public TMP_Text amountText;
    public TMP_Text amountPerSecondText;
    public void UpdateResource(float amount, float amountPerSecond)
    {
        amountText.text = string.Format("{0:0.00}", amount);
        amountPerSecondText.text = string.Format("+{0:0.00}/sec", amountPerSecond);
    }
}
