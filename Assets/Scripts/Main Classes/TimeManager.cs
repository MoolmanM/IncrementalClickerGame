using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //public string seasonString;
    //private int day, year, season, seasonCount;
    //public void CalculateSeason()
    //{
    //    day++;
    //    if (seasonCount == 0)
    //    {
    //        seasonString = "Spring";
    //    }
    //    else if (seasonCount == 1)
    //    {
    //        seasonString = "Summer";
    //    }
    //    else if (seasonCount == 2)
    //    {
    //        seasonString = "Fall";
    //    }
    //    else if (seasonCount == 3)
    //    {
    //        seasonString = "Winter";
    //    }
    //    else
    //    {
    //        seasonCount = 0;
    //    }

    //    if (day == 100 && seasonCount == 3)
    //    {
    //        year++;
    //        seasonCount++;
    //        day = 0;
    //    }

    //    else if (day == 100)
    //    {
    //        seasonCount++;
    //        day = 0;
    //    }

    //}

    DateTime currentDate;

    private long temp;
    void Start()
    {
        //Store the current time when it starts
        currentDate = DateTime.Now;

        //Grab the old time from the player prefs as a long
        if (PlayerPrefs.GetString("sysString") == "")
        {
            return;
        }
        else
        {
            temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));
        }
        

        //Convert the old time from binary to a DataTime variable
        DateTime oldDate = DateTime.FromBinary(temp);
        //print("oldDate: " + oldDate);

        //Use the Subtract method and store the result as a timespan variable
        TimeSpan difference = currentDate.Subtract(oldDate);
        //print("Difference: " + difference);

    }

    void OnApplicationQuit()
    {
        //Savee the current system time as a string in the player prefs class
        PlayerPrefs.SetString("sysString", DateTime.Now.ToBinary().ToString());

        //print("Saving this date to prefs: " + DateTime.Now);
    }


}
