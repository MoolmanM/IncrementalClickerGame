using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculateAdBoost : MonoBehaviour
{
    public Slider adAmountFillBar;
    public TMP_Text txtHeader, txtBody, txtBoostLeft;
    public int timeremaining;
    public static bool isAdBoostActivated;
    public static float adBoostMultiplier = 2;
    public uint adBoostAmountWatched;
    public TimeSpan timeCached;
    public bool hasMultipliedAPS, isTimerRunning, hasDayRestarted;
    public Button btnWatchAd;
    public float timeDayRestarts = 57600;
    public bool hasDayPassed;
    public DateTime dateDayRestarted;
    public int secondsPassedToday;
    public bool isPopupActive;
    public GameObject boostNotification;

    protected float timer = 0.1f;
    protected readonly float maxValue = 1f;

    private void Start()
    {
        timeremaining = PlayerPrefs.GetInt("Boost_Time_Remaining", timeremaining);
        isAdBoostActivated = PlayerPrefs.GetInt("Is_Boost_Activated") == 1 ? true : false;
        adBoostAmountWatched = (uint)PlayerPrefs.GetInt("Amount_Boost_Button", (int)adBoostAmountWatched);


        if (timeremaining > 0)
        {
            isTimerRunning = true;
        }

        CalculateAdFillBar();
        CheckIfDayShouldRestart();
    }
    private void ModifyTextHeader()
    {
        txtHeader.text = string.Format("Watch an Ad to multiply the production of all your resources by {0}", adBoostMultiplier);
    }
    private void ModifyTextBody()
    {
        if (adBoostAmountWatched == 0)
        {
            txtBody.text = string.Format("Click here to watch an ad to gain more resources for 4 hours.");
        }
        else
        {
            txtBody.text = string.Format("Click here to watch another Ad to increase your time by 4 Hours.");
        }
    }
    public void OpenPopup()
    {
        ModifyTextBody();
        ModifyTextHeader();
        isPopupActive = true;
    }
    public void ClosePopup()
    {
        isPopupActive = false;
    }
    private void CalculateAdFillBar()
    {
        float add;

        add = adBoostAmountWatched;

        adAmountFillBar.value = add;
    }
    public void AdCompleted()
    {
        AddFourHours();
        CalculateAdFillBar();
        if (adBoostAmountWatched == 6)
        {
            btnWatchAd.interactable = false;
            boostNotification.SetActive(false);
        }
    }
    private void AddFourHours()
    {
        adBoostAmountWatched++;
        timeremaining += 14400;
        if (timeremaining > 0)
        {
            isTimerRunning = true;
        }
        else
        {
            isTimerRunning = false;
        }

        MultiplyAmountPerSecond();
    }
    private void MultiplyAmountPerSecond()
    {
        if (timeremaining > 0 && !hasMultipliedAPS)
        {
            hasMultipliedAPS = true;
            foreach (var item in Resource.Resources)
            {
                if (item.Value.IsUnlocked)
                {
                    item.Value.amountPerSecond *= adBoostMultiplier;
                    StaticMethods.ModifyAPSText(item.Value.amountPerSecond, item.Value.uiForResource.txtAmountPerSecond);
                }
            }
        }
    }
    private void DivideAmountPerSecond()
    {
        foreach (var item in Resource.Resources)
        {
            if (item.Value.IsUnlocked)
            {
                item.Value.amountPerSecond /= adBoostMultiplier;
                Debug.Log("Before: " + item.Value.amountPerSecond + " Text: " + item.Value.uiForResource.txtAmountPerSecond);
                StaticMethods.ModifyAPSText(item.Value.amountPerSecond, item.Value.uiForResource.txtAmountPerSecond);
                Debug.Log("After: " + item.Value.amountPerSecond + " Text: " + item.Value.uiForResource.txtAmountPerSecond);
            }
        }
    }
    private void CheckIfDayShouldRestart()
    {
        TimeSpan test = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        secondsPassedToday = (int)test.TotalSeconds;

        if (secondsPassedToday > timeDayRestarts && DateTime.Now.Day > dateDayRestarted.Day)
        {
            Debug.Log("Day Should restart");
            RestartDay();
        }
    }
    private void RestartDay()
    {
        adBoostAmountWatched = 0;
        if (btnWatchAd.interactable == false)
        {
            btnWatchAd.interactable = true;
        }
        if (boostNotification.activeSelf)
        {
            boostNotification.SetActive(true);
        }
        dateDayRestarted = DateTime.Now;
    }
    private void UpdateTimerText(float timeRemaining)
    {
        if (isPopupActive)
        {
            TimeSpan timeLeft = TimeSpan.FromSeconds((double)(new decimal(timeRemaining)));

            if (timeLeft.Seconds != timeCached.Seconds)
            {
                if (timeLeft.Days == 0 && timeLeft.Hours == 0 && timeLeft.Minutes == 0)
                {
                    txtBoostLeft.text = string.Format("Boost Left: <b>{0:%s}s</b>", timeLeft.Duration());
                }
                else if (timeLeft.Days == 0 && timeLeft.Hours == 0)
                {
                    txtBoostLeft.text = string.Format("Boost Left: <b>{0:%m}m {0:%s}s</b>", timeLeft.Duration());
                }
                else if (timeLeft.Days == 0)
                {
                    txtBoostLeft.text = string.Format("Boost Left: <b>{0:%h}h {0:%m}m {0:%s}s</b>", timeLeft.Duration());
                }
                else
                {
                    txtBoostLeft.text = string.Format("Boost Left: <b>{0:%d}d {0:%h}h {0:%m}m {0:%s}s</b>", timeLeft.Duration());
                }
            }

            timeCached = timeLeft;
        }      
    }
    void Update()
    {
        if ((timer -= Time.deltaTime) <= 0)
        {
            timer = maxValue;

            if (isTimerRunning)
            {
                if (timeremaining > 0)
                {
                    timeremaining -= 1;
                    if (isAdBoostActivated == false)
                    {
                        isAdBoostActivated = true;
                    }
                    //UpdateTimerText(timeremaining);
                }
                else
                {
                    isTimerRunning = false;
                    isAdBoostActivated = false;
                    DivideAmountPerSecond();
                    hasMultipliedAPS = false;
                    timeremaining = 0;
                }
            }

            TimeSpan test = DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            secondsPassedToday = (int)test.TotalSeconds;

            if (secondsPassedToday == timeDayRestarts && DateTime.Now > dateDayRestarted)
            {
                RestartDay();
            }

            UpdateTimerText(timeremaining);
        }

    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Boost_Time_Remaining", timeremaining);
        PlayerPrefs.SetInt("Is_Boost_Activated", isAdBoostActivated ? 1 : 0);
        PlayerPrefs.SetInt("Amount_Ads_Watched", (int)adBoostAmountWatched);
    }
}
