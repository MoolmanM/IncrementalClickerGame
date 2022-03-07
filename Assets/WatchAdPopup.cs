using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System;

public class WatchAdPopup : MonoBehaviour
{
    public Image adAmountFillBar;
    public TMP_Text txtHeader, txtBody;
    public float _timeRemaining;
    public static bool isAdBoostActivated;
    public static float adBoostMultiplier = 2;
    [ShowInInspector] public static uint adBoostAmountWatched;
    public TimeSpan timeCached;
    public bool hasMultipliedAPS, isTimerRunning;
    public Button btnWatchAd;

    private void OnEnable()
    {
        //RewardedAdsButton.OnAdReward += OnWatchAd;
    }
    private void OnDisable()
    {
        //RewardedAdsButton.OnAdReward -= OnWatchAd;
    }
    private void Start()
    {
        _timeRemaining = PlayerPrefs.GetFloat("Boost_Time_Remaining", _timeRemaining);
        isAdBoostActivated = PlayerPrefs.GetInt("Is_Boost_Activated") == 1 ? true : false;
        adBoostAmountWatched = (uint)PlayerPrefs.GetInt("Amount_Boost_Button", (int)adBoostAmountWatched);


        if (_timeRemaining > 0)
        {
            isTimerRunning = true;
        }

        CalculateAdFillBar();
    }
    public void ModifyTextHeader()
    {
        // Call this when you click the PopupAd Button.
        txtHeader.text = string.Format("Watch an Ad to multiply the production of all your resources by {0}", adBoostMultiplier);
    }
    public void ModifyTextBody()
    {
        // Call this when you click the Popup Button
        if (adBoostAmountWatched == 0)
        {
            txtBody.text = string.Format("Click here to watch an ad to gain more resources for 4 hours.");
        }
        else
        {
            txtBody.text = string.Format("Click here to watch another Ad to increase your time by 4 Hours.");
        }
    }
    private void CalculateAdFillBar()
    {
        float add = 0;
        float div = 0;
        float fillAmount = 0;

        add = adBoostAmountWatched;
        div = 6;
        if (add > div)
        {
            add = div;
        }

        fillAmount += add / div;
        adAmountFillBar.fillAmount = fillAmount;
    }
    private void OnWatchAd()
    {
        AddFourHours();
        CalculateAdFillBar();
        if (adBoostAmountWatched == 6)
        {
            btnWatchAd.interactable = false;
        }
        // Load Ad
        // Then on succesful completion watching the Ad, do:
        // CalculateAdFillBar()
        // And then of course the other logic that actually gives you the boost.
    }
    [Button]
    private void AddFourHours()
    {
        if (adBoostAmountWatched >= 6)
        {
            Debug.Log("You've already viewed your max amount of ads for the day.");
        }
        else if (_timeRemaining + 14400 >= 86400)
        {
            _timeRemaining = 86400;
            adBoostAmountWatched++;
            Debug.Log("Boost can't be more than 24 hours");
        }
        else
        {
            adBoostAmountWatched++;
            _timeRemaining += 14400;
        }
        if (_timeRemaining > 0)
        {
            isTimerRunning = true;
        }
        else
        {
            isTimerRunning = false;
        }

        MultiplyAmountPerSecond();
    }
    [Button]
    private void AddFifteenSeconds()
    {
        if (adBoostAmountWatched >= 6)
        {
            Debug.Log("You've already viewed your max amount of ads for the day.");
        }
        else if (_timeRemaining + 15 >= 86400)
        {
            _timeRemaining = 86400;
            adBoostAmountWatched++;
            Debug.Log("Boost can't be more than 24 hours");
        }
        else
        {
            adBoostAmountWatched++;
            _timeRemaining += 15;
        }
        if (_timeRemaining > 0)
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
        if (_timeRemaining > 0 && !hasMultipliedAPS)
        {
            hasMultipliedAPS = true;
            foreach (var item in Resource.Resources)
            {
                if (item.Value.isUnlocked)
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
            if (item.Value.isUnlocked)
            {
                item.Value.amountPerSecond /= adBoostMultiplier;
                Debug.Log("Before: " + item.Value.amountPerSecond + " Text: " + item.Value.uiForResource.txtAmountPerSecond);
                StaticMethods.ModifyAPSText(item.Value.amountPerSecond, item.Value.uiForResource.txtAmountPerSecond);
                Debug.Log("After: " + item.Value.amountPerSecond + " Text: " + item.Value.uiForResource.txtAmountPerSecond);
            }
        }
    }
    [Button]
    private void RestartDay()
    {
        // This will happen once every 24 hours, At lets say 16:00.
        // Also need to save the amount boost button used. Also need to save boost time left of course.
        // Yes people will be able to exploit it by changing phone date/time, but that's irrelevant.
        adBoostAmountWatched = 0;
        if (btnWatchAd.interactable == false)
        {
            btnWatchAd.interactable = true;
        }
    }
    private void UpdateTimerText(float timeRemaining)
    {
        TimeSpan timeLeft = TimeSpan.FromSeconds((double)(new decimal(timeRemaining)));

        if (timeLeft.Seconds != timeCached.Seconds)
        {
            if (timeLeft.Days == 0 && timeLeft.Hours == 0 && timeLeft.Minutes == 0)
            {
                Debug.Log(string.Format("Boost Left: <b>{0:%s}s</b>", timeLeft.Duration()));
            }
            else if (timeLeft.Days == 0 && timeLeft.Hours == 0)
            {
                Debug.Log(string.Format("Boost Left: <b>{0:%m}m {0:%s}s</b>", timeLeft.Duration()));
            }
            else if (timeLeft.Days == 0)
            {
                Debug.Log(string.Format("Boost Left: <b>{0:%h}h {0:%m}m {0:%s}s</b>", timeLeft.Duration()));
            }
            else
            {
                Debug.Log(string.Format("Boost Left: <b>{0:%d}d {0:%h}h {0:%m}m {0:%s}s</b>", timeLeft.Duration()));
            }
        }

        timeCached = timeLeft;
    }
    void Update()
    {
        if (isTimerRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                if (isAdBoostActivated == false)
                {
                    isAdBoostActivated = true;
                }
                //UpdateTimerText(_timeRemaining);
            }
            else
            {
                isTimerRunning = false;
                isAdBoostActivated = false;
                DivideAmountPerSecond();
                hasMultipliedAPS = false;
                _timeRemaining = 0;
            }
        }


    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("Boost_Time_Remaining", _timeRemaining);
        PlayerPrefs.SetInt("Is_Boost_Activated", isAdBoostActivated ? 1 : 0);
        PlayerPrefs.SetInt("Amount_Ads_Watched", (int)adBoostAmountWatched);
    }
}
