using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static bool hasPlayedBefore;
    public static TimeSpan difference;
    public static int day, year, seasonCount;
    public TMP_Text txtGoneFor;
    //public TMP_Text seasonText, txtGoneFor;
    //public GameObject objWelcomePanel, objSpringImage, objWinterImage, objSummerImage, objFallImage;
    
    private DateTime _currentDate;   
    private long _temp;

    public static void ResetSeason()
    {
        day = 0;
        seasonCount = 0;
        year = 0;
    }
    private void CalculateSeason()
    {
        //day++;
        //if (seasonCount == 0)
        //{
        //    _seasonString = "Spring";
        //    objSpringImage.SetActive(true);
        //    objSummerImage.SetActive(false);
        //    objFallImage.SetActive(false);
        //    objWinterImage.SetActive(false);
        //}
        //else if (seasonCount == 1)
        //{
        //    _seasonString = "Summer";
        //    objSpringImage.SetActive(false);
        //    objSummerImage.SetActive(true);
        //    objFallImage.SetActive(false);
        //    objWinterImage.SetActive(false);
        //}
        //else if (seasonCount == 2)
        //{
        //    _seasonString = "Fall";
        //    objSpringImage.SetActive(false);
        //    objSummerImage.SetActive(false);
        //    objFallImage.SetActive(true);
        //    objWinterImage.SetActive(false);
        //}
        //else if (seasonCount == 3)
        //{
        //    _seasonString = "Winter";
        //    objSpringImage.SetActive(false);
        //    objSummerImage.SetActive(false);
        //    objFallImage.SetActive(false);
        //    objWinterImage.SetActive(true);
        //}
        //else
        //{
        //    seasonCount = 0;
        //}

        //if (day == 100 && seasonCount == 3)
        //{
        //    year++;
        //    seasonCount++;
        //    day = 0;
        //}

        //else if (day == 100)
        //{
        //    seasonCount++;
        //    day = 0;
        //}
        //seasonText.text = string.Format("Year {0} - {1}, day {2}", year, _seasonString, day);
    }  
    void OnEnable()
    {
        SetLaunchValues();
        //if (hasPlayedBefore)
        //{
        //    objWelcomePanel.SetActive(true);         
        //}
        //else
        //{
        //    objWelcomePanel.SetActive(false);
        //}
    }
    void OnApplicationQuit()
    {
        //Savee the current system time as a string in the player prefs class
        PlayerPrefs.SetString("sysString", DateTime.Now.ToBinary().ToString());

        //print("Saving this date to prefs: " + DateTime.Now);
    }
    private void SetLaunchValues()
    {
        //Store the current time when it stardifference
        _currentDate = DateTime.Now;

        //Grab the old time from the player prefs as a long
        if (PlayerPrefs.GetString("sysString") == "")
        {
            hasPlayedBefore = false;
            return;
        }
        else
        {
            hasPlayedBefore = true;
            _temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));
        }

        //Convert the old time from binary to a DataTime variable
        DateTime oldDate = DateTime.FromBinary(_temp);
        //print("oldDate: " + oldDate);

        //Use the Subtract method and store the result as a timespan variable
        difference = _currentDate.Subtract(oldDate);
        //print("Difference: " + difference);

        //Make the parts where the time is bold?
        if (difference.Days == 0 && difference.Hours == 0 && difference.Minutes == 0)
        {
            txtGoneFor.text = string.Format("You were gone for <b>{0:%s}s</b>", difference.Duration());
        }
        else if (difference.Days == 0 && difference.Hours == 0)
        {
            txtGoneFor.text = string.Format("You were gone for <b>{0:%m}m {0:%s}s</b>", difference.Duration());
        }
        else if (difference.Days == 0)
        {
            txtGoneFor.text = string.Format("You were gone for <b>{0:%h}h {0:%m}m {0:%s}s</b>", difference.Duration());
        }
        else
        {
            txtGoneFor.text = string.Format("You were gone for <b>{0:%d}d {0:%h}h {0:%m}m {0:%s}s</b>", difference.Duration());
        }

        //_currentHistoryLog = txtHistoryLog.text;
        //if (_currentHistoryLog == "")
        //{
        //    txtHistoryLog.text = string.Format("{0}<b>{1:t}</b>: {2}", _currentHistoryLog, DateTime.Now, notableEventString);
        //}
        //else
        //{
        //    txtHistoryLog.text = string.Format("{0}\n<b>{1:t}</b>: {2}", _currentHistoryLog, DateTime.Now, notableEventString);
        //    // How is expensive is this?

        //    Canvas.ForceUpdateCanvases();
        //    scrollViewObject.GetComponent<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition = 0f;

        //}

    }
}
