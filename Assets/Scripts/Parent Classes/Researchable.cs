using System;
using System.Collections.Generic;
using UnityEngine;

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
    FireHardenedWeapons,
    Smelting,
    //CopperMining,
    //TinMining,
    //IronMining,
    //BronzeAlloys,
    //TinEquipment,
    //CopperEquipment,
    //BronzeEquipment,
    //IronEquipment,
    //IronAlloys,
    MinorStorage,
    PowerGeneration,
    None
}

public abstract class Researchable : SuperClass
{
    public static Dictionary<ResearchType, Researchable> Researchables = new Dictionary<ResearchType, Researchable>();
    public static int researchSimulActive = 0, researchSimulAllowed = 1;
    public static bool hasReachedMaxSimulResearch, isResearchableUnlockedEvent;

    public ResearchType Type;
    //public uint testAmount;
    public float secondsToCompleteResearch, baseSecondsToCompleteResearch;
    [NonSerialized] public bool isResearched;

    private bool _isResearchStarted;
    private string _stringIsResearched, _stringResearchTimeRemaining, _stringIsResearchStarted, _stringIsUnlocked;
    private float _currentTimer, _researchTimeRemaining;
    private float timer = 0.1f;
    private readonly float maxValue = 0.1f;

    protected WorkerType[] _workerTypesToModify;
    protected Transform _tformImgProgressCircle, _tformObjCheckmark;
    protected GameObject _objCheckmark;

    // Reset Variables

    public float permTimeSubtraction, permAllCostSubtraction, permCostSubtraction;

    public float prestigeTimeSubtraction, prestigeAllCostSubtraction, prestigeCostSubtraction;

    private string _strPermTimeSubtraction, _strPermAllCostSubtraction, _strPermCostSubtraction, _strPrestigeTimeSubtraction, _strPrestigeAllCostSubtraction, _strPrestigeCostSubtraction;

    public void ResetResearchable()
    {
        isUnlocked = false;
        canvas.enabled = false;
        //objMainPanel.SetActive(false);
        graphicRaycaster.enabled = false;
        unlockAmount = 0;
        isUnlockedByResource = false;
        isResearched = false;
        _isResearchStarted = false;
        hasSeen = true;
        _currentTimer = 0f;
        _txtHeader.text = string.Format("{0}", actualName);

        ModifyTimeToCompleteResearch();
        ModifyCost();
        MakeResearchableAgain();
    }
    public void ModifyCost()
    {
        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].costAmount = resourceCost[i].baseCostAmount;
            float subtractionAmount = resourceCost[i].baseCostAmount * ((prestigeAllCostSubtraction + permAllCostSubtraction) + (prestigeCostSubtraction + permCostSubtraction));
            prestigeAllCostSubtraction = 0;
            prestigeCostSubtraction = 0;
            resourceCost[i].costAmount -= subtractionAmount;
            Debug.Log(string.Format("Changed research {0}'s cost from {1} to {2}", actualName, resourceCost[i].baseCostAmount, resourceCost[i].costAmount));
        }
    }
    public void ModifyTimeToCompleteResearch()
    {
        secondsToCompleteResearch = baseSecondsToCompleteResearch;
        float subtractionAmount = baseSecondsToCompleteResearch * (prestigeTimeSubtraction + permTimeSubtraction);
        prestigeTimeSubtraction = 0;
        secondsToCompleteResearch -= subtractionAmount;
        Debug.Log(string.Format("Changed research {0}'s time from {1} to {2}", actualName, baseSecondsToCompleteResearch, secondsToCompleteResearch));
    }
    public void SetInitialValues()
    {
        InitializeObjects();

        if (TimeManager.hasPlayedBefore)
        {
            _isResearchStarted = PlayerPrefs.GetInt(_stringIsResearchStarted) == 1 ? true : false;
            isResearched = PlayerPrefs.GetInt(_stringIsResearched) == 1 ? true : false;
            _researchTimeRemaining = PlayerPrefs.GetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
            isUnlocked = PlayerPrefs.GetInt(_stringIsUnlocked) == 1 ? true : false;

            FetchPrestigeValues();
        }

        if (!isResearched && _isResearchStarted)
        {
            if (_researchTimeRemaining <= TimeManager.difference.TotalSeconds)
            {
                _isResearchStarted = false;
                isResearched = true;
                Debug.Log("Research has been completed while you were gone");
                Researched();
            }
            else
            {
                secondsToCompleteResearch = _researchTimeRemaining - (float)TimeManager.difference.TotalSeconds;
                Debug.Log("You still have ongoing research");
                //_objProgressCirclePanel.SetActive(false);
            }
        }

        else if (isResearched && !_isResearchStarted)
        {
            Researched();
        }

    }
    public virtual void UpdateResearchTimer()
    {
        if (_isResearchStarted)
        {
            if ((timer -= Time.deltaTime) <= 0)
            {
                timer = maxValue;

                _currentTimer += 0.1f;

                //_imgResearchBar.fillAmount = _currentTimer / secondsToCompleteResearch;
                _researchTimeRemaining = secondsToCompleteResearch - _currentTimer;
                TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(_researchTimeRemaining)));

                if (span.Days == 0 && span.Hours == 0 && span.Minutes == 0)
                {
                    _txtHeader.text = string.Format("{0}\nResearching... (<b>{1:%s}s</b>)", actualName, span.Duration());
                }
                else if (span.Days == 0 && span.Hours == 0)
                {
                    _txtHeader.text = string.Format("{0}\nResearching... (<b>{1:%m}m {1:%s}s</b>)", actualName, span.Duration());
                }
                else if (span.Days == 0)
                {
                    _txtHeader.text = string.Format("{0}\nResearching... (<b>{1:%h}h {1:%m}m {1:%s}s</b>)", actualName, span.Duration());
                }
                else
                {
                    _txtHeader.text = string.Format("{0}\nResearching... (<b>{1:%d}d {1:%h}h {1:%m}m {1:%s}s</b>)", actualName, span.Duration());
                }
                CheckIfResearchIsComplete();
            }
        }

    }
    public void OnResearch()
    {
        if (researchSimulActive >= researchSimulAllowed)
        {
            hasReachedMaxSimulResearch = true;
        }
        else
        {
            if (!_isResearchStarted && !isResearched)
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
            _isResearchStarted = false;
            Researched();
        }
    }
    protected virtual void Researched()
    {
        isResearched = true;

        researchSimulActive--;
        UnlockCrafting();
        UnlockBuilding();
        UnlockResearchable();
        UnlockWorkerJob();
        UnlockResource();
        //_objProgressCirclePanel.SetActive(false);
        //_txtHeader.text = string.Format("{0} (Researched)", actualName);
        _btnMain.interactable = false;
        _objProgressCircle.SetActive(false);
        _objBackground.SetActive(false);
        _objCheckmark.SetActive(true);
        _txtHeader.text = string.Format("{0}", actualName);
        //string htmlValue = "#D4D4D4";

        //if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
        //{
        //    _imgExpand.color = greyColor;
        //    _imgCollapse.color = greyColor;
        //}


        if (Menu.isResearchHidden)
        {
            if (objMainPanel.activeSelf)
            {
                objMainPanel.SetActive(false);
                canvas.enabled = false;
                graphicRaycaster.enabled = false;
            }
        }
    }
    public void MakeResearchableAgain()
    {
        // This will probably only happen after prestige.
        _btnMain.interactable = true;
        _objProgressCirclePanel.SetActive(true);
        _txtHeader.text = string.Format("{0}", actualName);

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            _imgExpand.color = darkGreyColor;
            _imgCollapse.color = darkGreyColor;
        }
    }
    public void StartResearching()
    {
        researchSimulActive++;
        _isResearchStarted = true;
        //_objProgressCirclePanel.SetActive(false);
    }
    public void SetDescriptionText(string description)
    {
        _txtDescription.text = string.Format("{0}", description);
    }
    protected override void InitializeObjects()
    {
        base.InitializeObjects();

        _stringIsResearched = Type.ToString() + "IsResearched";
        _stringResearchTimeRemaining = Type.ToString() + "TimeRemaining";
        _stringIsResearchStarted = Type.ToString() + "IsResearchStarted";
        _stringIsUnlocked = Type.ToString() + "IsUnlocked";

        AssignPrestigeStrings();

        _btnMain.onClick.AddListener(OnResearch);

        _tformObjCheckmark = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/Checkmark");
        _objCheckmark = _tformObjCheckmark.gameObject;
        _objCheckmark.SetActive(false);

        if (isUnlocked)
        {
            objMainPanel.SetActive(true);
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
        }
        else
        {
            objMainPanel.SetActive(false);
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
        }
    }
    protected void UnlockedViaResource()
    {
        if (isUnlocked)
        {
            if (UIManager.isResearchVisible)
            {
                objMainPanel.SetActive(true);
                canvas.enabled = true;
                graphicRaycaster.enabled = true;
                hasSeen = true;
            }
            else if (hasSeen)
            {
                isResearchableUnlockedEvent = true;
                hasSeen = false;
                PointerNotification.rightAmount++;
            }
        }
    }
    private void CheckIfUnlocked()
    {
        if (!isUnlocked)
        {
            if (GetCurrentFill() >= 0.8f & !isUnlockedByResource && isUnlockableByResource)
            {
                isUnlockedByResource = true;
                unlockAmount++;

                if (unlockAmount == unlocksRequired)
                {
                    isUnlocked = true;

                    UnlockedViaResource();

                    PointerNotification.HandleRightAnim();
                    PointerNotification.HandleLeftAnim();
                }
            }
        }
    }
    private void AssignPrestigeStrings()
    {
        _strPermTimeSubtraction = Type.ToString() + "permTimeSubtraction";
        _strPermAllCostSubtraction = Type.ToString() + "permAllCostSubtraction";
        _strPermCostSubtraction = Type.ToString() + "permCostSubtraction";

        _strPrestigeTimeSubtraction = Type.ToString() + "prestigeTimeSubtraction";
        _strPrestigeAllCostSubtraction = Type.ToString() + "prestigeAllCostSubtraction";
        _strPrestigeCostSubtraction = Type.ToString() + "prestigeCostSubtraction";
    }
    private void SavePrestigeValues()
    {
        PlayerPrefs.SetFloat(_strPermTimeSubtraction, permTimeSubtraction);
        PlayerPrefs.SetFloat(_strPermAllCostSubtraction, permAllCostSubtraction);
        PlayerPrefs.SetFloat(_strPermCostSubtraction, permCostSubtraction);

        PlayerPrefs.SetFloat(_strPrestigeTimeSubtraction, prestigeTimeSubtraction);
        PlayerPrefs.SetFloat(_strPrestigeAllCostSubtraction, prestigeAllCostSubtraction);
        PlayerPrefs.SetFloat(_strPrestigeCostSubtraction, prestigeCostSubtraction);
    }
    private void FetchPrestigeValues()
    {
        permTimeSubtraction = PlayerPrefs.GetFloat(_strPermTimeSubtraction, permTimeSubtraction);
        permAllCostSubtraction = PlayerPrefs.GetFloat(_strPermAllCostSubtraction, permAllCostSubtraction);
        permCostSubtraction = PlayerPrefs.GetFloat(_strPermCostSubtraction, permCostSubtraction);

        prestigeTimeSubtraction = PlayerPrefs.GetFloat(_strPrestigeTimeSubtraction, prestigeTimeSubtraction);
        prestigeAllCostSubtraction = PlayerPrefs.GetFloat(_strPrestigeAllCostSubtraction, prestigeAllCostSubtraction);
        prestigeCostSubtraction = PlayerPrefs.GetFloat(_strPrestigeCostSubtraction, prestigeCostSubtraction);
    }
    protected void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;
            if (!isResearched || !_isResearchStarted)
            {
                CheckIfPurchaseable();
            }

            UpdateResourceCosts();
            CheckIfUnlocked();
        }

        UpdateResearchTimer();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsResearchStarted, _isResearchStarted ? 1 : 0);
        PlayerPrefs.SetInt(_stringIsResearched, isResearched ? 1 : 0);
        PlayerPrefs.SetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
        PlayerPrefs.SetInt(_stringIsUnlocked, isUnlocked ? 1 : 0);

        SavePrestigeValues();
    }
}


