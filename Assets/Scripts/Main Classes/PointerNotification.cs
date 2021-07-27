using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointerNotification : MonoBehaviour
{
    public static GameObject objLeftPointer, objRightPointer, objTextLeft, objTextRight;
    public static TMP_Text textLeft, textRight;
    public static uint leftAmount, rightAmount;
    public static Animator AnimRight, AnimLeft;

    public static uint lastLeftAmount, lastRightAmount;

    void Awake()
    {
        objLeftPointer = GameObject.Find("Pointer_NotificationLeft");
        objRightPointer = GameObject.Find("Pointer_NotificationRight");

        AnimRight = objRightPointer.GetComponent<Animator>();
        AnimLeft = objLeftPointer.GetComponent<Animator>();

        objTextLeft = GameObject.Find("Text_NotificationLeft");
        objTextRight = GameObject.Find("Text_NotificationRight");

        textLeft = objTextLeft.GetComponent<TMP_Text>();
        textRight = objTextRight.GetComponent<TMP_Text>();

        //objLeftPointer.SetActive(false);
        //objRightPointer.SetActive(false);
    }
    public static bool IsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
    public static bool IsLooping(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).loop)
            return true;
        else
            return false;
    }
    public static void HandleLeftAnim()
    {
        // The problem here is that the display again animation waits for one single loop to finish of the normal Display, where it should interupt it immediately.
        if (leftAmount > 0)
        {
            if (IsLooping(AnimLeft, "Display") && leftAmount > lastLeftAmount)
            {
                AnimLeft.SetTrigger("Display_Again");
                
            }
            else if(!IsLooping(AnimLeft, "Display"))
            {
                AnimLeft.SetTrigger("Up");
            }
        }
        else
        {
            if (!IsLooping(AnimLeft, "Idle"))
            {
                AnimLeft.SetTrigger("Down");
            }
        }

        textLeft.text = leftAmount.ToString();
    }
    public static void HandleRightAnim()
    {
        if (rightAmount > 0)
        {
            if (IsLooping(AnimRight, "Display") && rightAmount > lastRightAmount)
            {
                AnimRight.SetTrigger("Display_Again");
                //AnimRight.Play("Display_Again", -1, 0);
            }
            else if(!IsLooping(AnimRight, "Display"))
            {
                AnimRight.SetTrigger("Up");
            }
        }
        else
        {
            if (!IsLooping(AnimRight, "Idle"))
            {
                AnimRight.SetTrigger("Down");
            }
        }

        textRight.text = rightAmount.ToString();
    }
}
