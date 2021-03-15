using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public TMP_Text seasonText, goneForText;
    public GameObject _welcomePanel;
    private DateTime currentDate;
    public static bool hasPlayedBefore;
    public static TimeSpan difference;
    private string seasonString;
    private int day, year, seasonCount;
    private float _timer = 0.1f;
    private readonly float maxValue = 5f;
    private long temp;

    public void CalculateSeason()
    {
        day++;
        if (seasonCount == 0)
        {
            seasonString = "Spring";
        }
        else if (seasonCount == 1)
        {
            seasonString = "Summer";
        }
        else if (seasonCount == 2)
        {
            seasonString = "Fall";
        }
        else if (seasonCount == 3)
        {
            seasonString = "Winter";
        }
        else
        {
            seasonCount = 0;
        }

        if (day == 100 && seasonCount == 3)
        {
            year++;
            seasonCount++;
            day = 0;
        }

        else if (day == 100)
        {
            seasonCount++;
            day = 0;
        }
        seasonText.text = string.Format("Year {0} - {1}, day {2}", year, seasonString, day);
    }  
    public void Awake()
    {
        SetLaunchValues();
        if (hasPlayedBefore)
        {
            _welcomePanel.SetActive(true);         
        }
        else
        {
            _welcomePanel.SetActive(false);
        }
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
        currentDate = DateTime.Now;

        //Grab the old time from the player prefs as a long
        if (PlayerPrefs.GetString("sysString") == "")
        {
            hasPlayedBefore = false;
            return;
        }
        else
        {
            hasPlayedBefore = true;
            temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));
        }


        //Convert the old time from binary to a DataTime variable
        DateTime oldDate = DateTime.FromBinary(temp);
        //print("oldDate: " + oldDate);

        //Use the Subtract method and store the result as a timespan variable
        difference = currentDate.Subtract(oldDate);
        //print("Difference: " + difference);
        //Debug.Log(Resource._resources[ResourceType.Sticks].AmountPerSecond);

        //Make the parts where the time is bold?
        if (difference.Days == 0 && difference.Hours == 0 && difference.Minutes == 0)
        {
            goneForText.text = string.Format("You were gone for {0:%s}s \n While you were gone, you earned:", difference.Duration());
        }
        else if (difference.Days == 0 && difference.Hours == 0)
        {
            goneForText.text = string.Format("You were gone for {0:%m}m {0:%s}s\nWhile you were gone, you earned:", difference.Duration());
        }
        else if (difference.Days == 0)
        {
            goneForText.text = string.Format("You were gone for {0:%h}h {0:%m}m {0:%s}s\nWhile you were gone, you earned:", difference.Duration());
        }
        else
        {
            goneForText.text = string.Format("You were gone for {0:%d}d {0:%h}h {0:%m}m {0:%s}s\nWhile you were gone, you earned:", difference.Duration());
        }

        
    }
    public static void GetAFKResource(ResourceType type)
    {
        float differenceAmount = (float)(difference.TotalSeconds * Resource._resources[type].AmountPerSecond);       
        Resource._resources[type].TxtEarned.text = string.Format("{0}: {1:0.00}", type, differenceAmount);

        if ((differenceAmount + Resource._resources[type].Amount) > Resource._resources[type].StorageAmount)
        {
            Resource._resources[type].Amount = Resource._resources[type].StorageAmount;
            Resource._resources[type]._UiForResource.TxtAmount.text = string.Format("{0}", Resource._resources[type].Amount);
        }
        else
        {
            Resource._resources[type].Amount += differenceAmount;
        }
        //Resource._resources[type].uiForResource.amount.text = string.Format("{0}", Resource._resources[type].Amount);
    }
    private void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            CalculateSeason();
        }
    }
}
