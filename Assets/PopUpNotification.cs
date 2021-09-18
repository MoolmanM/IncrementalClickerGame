using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class PopUpNotification : MonoBehaviour
{
    public static GameObject popUpBad, popUpGood, popUpInfo, popUpError;
    public static Animator animator;
    public static TMP_Text txtGood, txtBad, txtInfo, txtError;

    private Transform tformBad, tformInfo, tformGood, tformError, tformTxtBad, tformTxtInfo, tformTxtGood, tformTxtError;

    private void Awake()
    {
        tformTxtBad = transform.Find("Popup_MessageBadInfo/Text_Message");
        tformTxtInfo = transform.Find("Popup_MessageInfo/Text_Message");
        tformTxtGood = transform.Find("Popup_MessageGoodInfo/Text_Message");
        tformTxtError = transform.Find("Popup_MessageError/Text_Message");

        txtBad = tformTxtBad.GetComponent<TMP_Text>();
        txtInfo = tformTxtInfo.GetComponent<TMP_Text>();
        txtGood = tformTxtGood.GetComponent<TMP_Text>();
        txtError = tformTxtError.GetComponent<TMP_Text>();

        animator = gameObject.GetComponent<Animator>();

        tformBad = transform.Find("Popup_MessageBadInfo");
        tformInfo = transform.Find("Popup_MessageInfo");
        tformGood = transform.Find("Popup_MessageGoodInfo");
        tformError = transform.Find("Popup_MessageError");

        popUpBad = tformBad.gameObject;
        popUpInfo = tformInfo.gameObject;
        popUpGood = tformGood.gameObject;
        popUpError = tformError.gameObject;

        popUpBad.SetActive(false);
        popUpInfo.SetActive(false);
        popUpGood.SetActive(false);
        popUpError.SetActive(false);
    }

    private static void PopUpBad()
    {
        popUpBad.SetActive(true);
        popUpGood.SetActive(false);
        popUpInfo.SetActive(false);
        popUpError.SetActive(false);
    }
    private static void PopUpInfo()
    {
        popUpBad.SetActive(false);
        popUpGood.SetActive(false);
        popUpInfo.SetActive(true);
        popUpError.SetActive(false);
    }
    private static void PopUpGood()
    {
        popUpBad.SetActive(false);
        popUpGood.SetActive(true);
        popUpInfo.SetActive(false);
        popUpError.SetActive(false);
    }
    private static void PopUpError()
    {
        popUpBad.SetActive(false);
        popUpGood.SetActive(false);
        popUpInfo.SetActive(false);
        popUpError.SetActive(true);
    }
    public static void HandleActivePopUp()
    {
        if (Events.eventBad)
        {
            PopUpBad();
        }
        else if (Events.eventError)
        {
            PopUpError();
        }
        else if (Events.eventGood)
        {
            PopUpGood();
        }
        else
        {
            PopUpInfo();
        }
    }
    private static bool IsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
    public static void HandleAnim()
    {
        if (Events.eventHappened == true)
        {
            HandleActivePopUp();
            if (IsPlaying(animator, "Display"))
            {
                animator.Play("DisplayAgain", -1, 0);
                //animator.SetTrigger("DisplayAgain");
            }
            else if (IsPlaying(animator, "PopDown"))
            {
                animator.SetTrigger("PopUp");
            }
            else
            {
                animator.SetTrigger("PopUp");
            }

            //if (IsPlaying(animator, "Display"))
            //{
            //    animator.Play("Notification_Display", -1, 0);
            //}
            //else if (IsPlaying(animator, "Notification_Down"))
            //{
            //    animator.SetTrigger("Start_Notification");
            //}
            //else
            //{
            //    animator.SetTrigger("Start_Notification");
            //}
            Events.eventHappened = false;
            Events.eventGood = false;
            Events.eventBad = false;
            Events.eventInfo = false;
            Events.eventError = false;
        }
    }
}
