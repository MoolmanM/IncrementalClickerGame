using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ResearchType
{
    // Housing should maybe be after you have the 'Log' resource.
    // Hamster Wheel
    // Fire?
    // Grinding stone/Sharpening of tools (Increases production of everything that uses tools, or maybe just increase worker values.
    // Cooking of food.
    // Clothing, need to research first and then craft perhaps, this might go for a lot of things.
    // Power Lines, enables buildings to receive power.
    // Maybe have knowledge points as the currency to research things.
    // Maybe if you get x amount of workers. Make an event happen where you get someone who is quite smart. and this person gives you an instant 50 knowledge points. Which is just enouggh
    // to research paper, then after researching paper you can have students that study papers. 
    // And with that you'll gain knowledge.
    // Smelting
    // Need to probably unlock researchables through the same formula as unlocking crafting stuff. Except based on knowledge.

    //Paper,
    Weapons,
    StoneEquipment,
    Fire,
    Cooking,
    FireHardenedWeapons,
    Smelting,
    ManualEnergyProduction,
    CopperMining,
    TinMining,
    IronMining,
    BronzeAlloys,
    TinEquipment,
    CopperEquipment,
    BronzeEquipment,
    IronEquipment,
    IronAlloys
}

public abstract class Researchable : SuperClass
{
    public static Dictionary<ResearchType, Researchable> Researchables = new Dictionary<ResearchType, Researchable>();
    public static int researchSimulActive = 0, researchSimulAllowed = 1;

    public ResearchType Type;
    public uint testAmount;
    public float secondsToCompleteResearch;
    [System.NonSerialized] public bool isResearched;

    private bool isResearchStarted;
    private string _stringIsResearched, _stringResearchTimeRemaining, _stringIsResearchStarted;
    private float _currentTimer, _researchTimeRemaining;
    private float timer = 0.1f;
    private readonly float maxValue = 0.1f;

    protected WorkerType[] _workerTypesToModify;
    protected Transform _tformImgProgressCircle, _tformImgResearchBar, _tformProgressbarPanel, _tformTxtHeaderUncraft;
    protected Image _imgResearchBar;
    private string _stringHeader;

    public void SetInitialValues()
    {
        InitializeObjects();
        //isUnlocked = true;

        if (TimeManager.hasPlayedBefore)
        {
            isResearchStarted = PlayerPrefs.GetInt(_stringIsResearchStarted) == 1 ? true : false;
            isResearched = PlayerPrefs.GetInt(_stringIsResearched) == 1 ? true : false;
            _researchTimeRemaining = PlayerPrefs.GetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
        }

        if (!isResearched && isResearchStarted)
        {
            if (_researchTimeRemaining <= TimeManager.difference.TotalSeconds)
            {
                isResearchStarted = false;
                isResearched = true;
                Debug.Log("Research was completed while you were gone");
                Researched();
            }
            else
            {
                secondsToCompleteResearch = _researchTimeRemaining - (float)TimeManager.difference.TotalSeconds;
                Debug.Log("You still have ongoing research");
                _objProgressCircle.SetActive(false);
            }
        }

        else if (isResearched && !isResearchStarted)
        {
            Researched();
        }

    }
    public virtual void UpdateResearchTimer()
    {
        if (isResearchStarted)
        {
            if ((timer -= Time.deltaTime) <= 0)
            {
                timer = maxValue;

                _currentTimer += 0.1f;

                _imgResearchBar.fillAmount = _currentTimer / secondsToCompleteResearch;
                _researchTimeRemaining = secondsToCompleteResearch - _currentTimer;
                TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(_researchTimeRemaining)));

                if (span.Days == 0 && span.Hours == 0 && span.Minutes == 0)
                {
                    _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0}\n(<b>{1:%s}s</b>)", _stringHeader, span.Duration());
                }
                else if (span.Days == 0 && span.Hours == 0)
                {
                    _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0}\n(<b>{1:%m}m {1:%s}s</b>)", _stringHeader, span.Duration());
                }
                else if (span.Days == 0)
                {
                    _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0}\n(<b>{1:%h}h {1:%m}m {1:%s}s</b>)", _stringHeader, span.Duration());
                }
                else
                {
                    _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0}\n(<b>{1:%d}d {1:%h}h {1:%m}m {1:%s}s</b>)", _stringHeader, span.Duration());
                }
                CheckIfResearchIsComplete();
            }
        }

    }
    public void OnResearch()
    {
        if (researchSimulActive >= researchSimulAllowed)
        {
            Debug.Log(string.Format("You can only have {0} research active at the same time", researchSimulAllowed));
        }
        else
        {
            if (!isResearchStarted && !isResearched)
            {

                bool canPurchase = true;

                for (int i = 0; i < resourceCost.Length; i++)
                {
                    if (resourceCost[i].currentAmount < resourceCost[i].costAmount)
                    {
                        canPurchase = false;
                        break;
                    }
                }

                if (canPurchase)
                {
                    for (int i = 0; i < resourceCost.Length; i++)
                    {
                        Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
                    }
                    StartResearching();
                }
            }
        }


    }
    private void CheckIfResearchIsComplete()
    {
        if (_currentTimer >= secondsToCompleteResearch)
        {
            isResearchStarted = false;
            isResearched = true;
            Researched();
        }
    }
    private void Researched()
    {
        isResearched = true;
        if (Menu.isResearchHidden)
        {
            researchSimulActive--;
            UnlockCrafting();
            UnlockBuilding();
            UnlockResearchable();
            _objProgressCircle.SetActive(false);
            _objTxtHeader.SetActive(false);
            _objTxtHeaderDone.SetActive(true);

            string htmlValue = "#D4D4D4";

            if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
            {
                _imgExpand.color = greyColor;
                _imgCollapse.color = greyColor;
            }

            objMainPanel.SetActive(false);
            objSpacerBelow.SetActive(false);
        }
        else
        {
            researchSimulActive--;
            UnlockCrafting();
            UnlockBuilding();
            UnlockResearchable();
            _objProgressCircle.SetActive(false);
            _objTxtHeader.SetActive(false);
            _objTxtHeaderDone.SetActive(true);

            string htmlValue = "#D4D4D4";

            if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
            {
                _imgExpand.color = greyColor;
                _imgCollapse.color = greyColor;
            }
        }

    }
    private void MakeResearchableAgain()
    {
        // This will probably only happen after prestige.
        _objBtnMain.GetComponent<Button>().interactable = true;
        _objProgressCircle.SetActive(true);
        _objTxtHeader.SetActive(true);
        _objTxtHeaderDone.SetActive(false);

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            _imgExpand.color = darkGreyColor;
            _imgCollapse.color = darkGreyColor;
        }
    }
    public void GetTimeToCompleteResearch()
    {
        isResearchStarted = true;
        DateTime currentTime = DateTime.Now;
        //Debug.Log(currentTime);
        DateTime timeToCompletion = currentTime.AddSeconds(60);
        //Debug.Log(timeToCompletion);
        TimeSpan differenceAmount = timeToCompletion.Subtract(currentTime);
        //Debug.Log(differenceAmount + " " + differenceAmount.Seconds);
        secondsToCompleteResearch = differenceAmount.Seconds;
    }
    public void StartResearching()
    {
        researchSimulActive++;
        isResearchStarted = true;
        _objProgressCircle.SetActive(false);
    }
    public void SetDescriptionText(string description)
    {
        _txtDescription.text = string.Format("{0}", description);
    }
    protected override void InitializeObjects()
    {
        base.InitializeObjects();

        _tformImgResearchBar = transform.Find("Panel_Main/Header_Panel/Research_FillBar");

        _imgResearchBar = _tformImgResearchBar.GetComponent<Image>();

        _stringIsResearched = Type.ToString() + "IsResearched";
        _stringResearchTimeRemaining = (Type.ToString() + "TimeRemaining");
        _stringIsResearchStarted = (Type.ToString() + "IsResearchStarted");

        _stringHeader = _objTxtHeader.GetComponent<TMP_Text>().text; 
        _objTxtHeaderDone = _tformTxtHeaderDone.gameObject;
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsResearchStarted, isResearchStarted ? 1 : 0);
        PlayerPrefs.SetInt(_stringIsResearched, isResearched ? 1 : 0);
        PlayerPrefs.SetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
    }
}


