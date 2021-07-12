using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointerNotification : MonoBehaviour
{
    public static GameObject objLeftPointer, objRightPointer, objTextLeft, objTextRight;
    public static TMP_Text textLeft, textRight;
    public static uint leftAmount, rightAmount;

    void Awake()
    {
        objLeftPointer = GameObject.Find("Pointer_NotificationLeft");
        objRightPointer = GameObject.Find("Pointer_NotificationRight");
        
        objTextLeft = GameObject.Find("Text_NotificationLeft");
        objTextRight = GameObject.Find("Text_NotificationRight");

        textLeft = objTextLeft.GetComponent<TMP_Text>();
        textRight = objTextRight.GetComponent<TMP_Text>();

        objLeftPointer.SetActive(false);
        objRightPointer.SetActive(false);
    }
}
