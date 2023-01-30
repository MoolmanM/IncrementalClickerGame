using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NotificationManager : MonoBehaviour
{
    public GameObject objAchievementNotification, objEnterOptionsNotification;
    public void Start()
    {
        objAchievementNotification.SetActive(false);
        objEnterOptionsNotification.SetActive(false);
    }
}
