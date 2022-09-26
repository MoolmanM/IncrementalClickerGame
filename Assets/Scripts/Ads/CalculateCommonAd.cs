using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculateCommonAd : MonoBehaviour
{
    public bool isPanelActive, hasTimerStarted;
    public float timeToRefresh;
    public float _timeRemaining;
    private string strTimeLeft, strCache;
    public DateTime leftDate;
    public TMP_Text txtTimeLeft;
    public GameObject[] objAdChestNotifications;
    public Button btnCommonAd;

    protected float _timer = 0.1f;
    protected readonly float _maxValue = 1f;

    private void Start()
    {
        long currentTicks = DateTime.Now.Ticks;
        TimeSpan currentSpan = new TimeSpan(currentTicks);
        long leftTicks = leftDate.Ticks;
        TimeSpan leftSpan = new TimeSpan(leftTicks);

        float diffAmount = (float)currentSpan.TotalSeconds - (float)leftSpan.TotalSeconds;

        if (diffAmount - _timeRemaining <= 0)
        {
            foreach (var item in objAdChestNotifications)
            {
                item.SetActive(true);
            }
        }
        else
        {
            _timeRemaining -= diffAmount;
        }
    }
    public void SetPanelInActive()
    {
        isPanelActive = false;
    }
    public void SetPanelActive()
    {
        isPanelActive = true;
        hasTimerStarted = true;
    }
    public void AdCompleted()
    {
        // This all should happen after watching the ad successfully.
        if (!hasTimerStarted)
        {
            foreach (var item in objAdChestNotifications)
            {
                item.SetActive(false);
            }
            hasTimerStarted = true;
            isPanelActive = true;
            _timeRemaining += timeToRefresh;
            TimeSpan originaltime = TimeSpan.FromSeconds(timeToRefresh);

            strTimeLeft = " ";
            strCache = "";

            btnCommonAd.interactable = false;
        }
    }
    private void UpdateTimerText(float timeRemaining)
    {
        TimeSpan timeLeft = TimeSpan.FromSeconds((double)(new decimal(timeRemaining)));

        if (timeLeft.Days == 0 && timeLeft.Hours == 0 && timeLeft.Minutes == 0)
        {
            strCache = string.Format("<b>{0:%s}s</b>", timeLeft.Duration());
        }
        else if (timeLeft.Days == 0 && timeLeft.Hours == 0)
        {
            strCache = string.Format("<b>{0:%m}m {0:%s}s</b>", timeLeft.Duration());
        }
        else if (timeLeft.Days == 0)
        {
            strCache = string.Format("<b>{0:%h}h {0:%m}m</b>", timeLeft.Duration());
        }
        else
        {
            strCache = string.Format("<b>{0:%d}d {0:%h}h</b>", timeLeft.Duration());
        }

        if (strCache != strTimeLeft)
        {
            txtTimeLeft.text = strCache;
        }

        strTimeLeft = strCache;
    }
    void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;

            if (isPanelActive)
            {
                if (_timeRemaining > 0)
                {
                    _timeRemaining -= 1;
                    UpdateTimerText(_timeRemaining);
                }
                else
                {
                    txtTimeLeft.text = "Open";
                    hasTimerStarted = false;
                    isPanelActive = false;
                    _timeRemaining = 0;
                    foreach (var item in objAdChestNotifications)
                    {
                        item.SetActive(true);
                    }
                    btnCommonAd.interactable = true;
                }
            }
            else
            {
                _timeRemaining -= 1;
                if (_timeRemaining <= 0)
                {
                    txtTimeLeft.text = "Open";
                    hasTimerStarted = false;
                    isPanelActive = false;
                    _timeRemaining = 0;
                    foreach (var item in objAdChestNotifications)
                    {
                        item.SetActive(true);
                    }
                    btnCommonAd.interactable = true;
                }
                
            }
        }
    }
    private void OnApplicationQuit()
    {
        leftDate = DateTime.Now;
        long elapsedTicks = leftDate.Ticks;
        TimeSpan leftSpan = new TimeSpan(elapsedTicks);

        PlayerPrefs.SetFloat("Common_Ad_Left_Date", (float)leftSpan.TotalSeconds);
        PlayerPrefs.SetFloat("Common_Ad_Time_Remaining", _timeRemaining);
    }
}
