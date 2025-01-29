using System;
using System.Collections.Generic;
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

    Weapons,
    StoneEquipment,
    Fire,
    Cooking,
    Language,
    CopperMining,
    AnimalDomestication,
    AnimalHusbandry,
    Chariots,
    Metallurgy,
    BronzeAlloys,
    Writing,
    MechanicalEngineering,
    Sundial,
    Glassmaking,
    Woodworking,
    Housing,
    Storage,
    Compass,
    BronzeEquipment,
    None,
    // Books
    // Library
}

public abstract class Researchable : GameEntity
{
    public static Dictionary<ResearchType, Researchable> Researchables = new Dictionary<ResearchType, Researchable>();
    public static int researchSimulActive = 0, researchSimulAllowed = 1;
    public static bool hasReachedMaxSimulResearch, isResearchableUnlockedEvent;

    public ResearchType Type;
    //public uint testAmount;
    public float secondsToCompleteResearch, baseSecondsToCompleteResearch;
    [NonSerialized] public bool isResearched;

    public bool _isResearchStarted;
    private string _stringIsResearched, _stringResearchTimeRemaining, _stringIsResearchStarted, _stringIsUnlocked;
    private float _currentTimer, _researchTimeRemaining;
    private float timer = 0.1f;
    private readonly float maxValue = 0.1f;

    protected WorkerType[] _workerTypesToModify;
    protected Transform _tformImgProgressCircle, _tformObjCheckmark;

    // Reset Variables

    public float permTimeSubtraction, permAllCostSubtraction, permCostSubtraction;

    public float prestigeTimeSubtraction, prestigeAllCostSubtraction, prestigeCostSubtraction;

    private string _strPermTimeSubtraction, _strPermAllCostSubtraction, _strPermCostSubtraction, _strPrestigeTimeSubtraction, _strPrestigeAllCostSubtraction, _strPrestigeCostSubtraction;

    [SerializeField] private Sprite _pressedSprite;

    private Image imgResearchFill;
    private Transform tformResearchFill;

    public uint trackedResearchedAmount;
    //public float trackedResearchedTime;

    public void ResetResearchable()
    {
        IsUnlocked = false;
        Canvas.enabled = false;
        GraphicRaycaster.enabled = false;
        UnlockAmount = 0;
        IsUnlockedByResource = false;
        isResearched = false;
        _isResearchStarted = false;
        HasSeen = true;
        _currentTimer = 0f;
        TxtHeader.text = string.Format("{0}", ActualName);

        ModifyTimeToCompleteResearch();
        ModifyCost();
        MakeResearchableAgain();
    }
    public void ModifyCost()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            ResourceCost[i].CostAmount = ResourceCost[i].BaseCostAmount;
            float subtractionAmount = ResourceCost[i].BaseCostAmount * ((prestigeAllCostSubtraction + permAllCostSubtraction) + (prestigeCostSubtraction + permCostSubtraction));
            prestigeAllCostSubtraction = 0;
            prestigeCostSubtraction = 0;
            ResourceCost[i].CostAmount -= subtractionAmount;
            Debug.Log(string.Format("Changed research {0}'s cost from {1} to {2}", ActualName, ResourceCost[i].BaseCostAmount, ResourceCost[i].CostAmount));
        }
    }
    public void ModifyTimeToCompleteResearch()
    {
        secondsToCompleteResearch = baseSecondsToCompleteResearch;
        float subtractionAmount = baseSecondsToCompleteResearch * (prestigeTimeSubtraction + permTimeSubtraction);
        prestigeTimeSubtraction = 0;
        secondsToCompleteResearch -= subtractionAmount;
        Debug.Log(string.Format("Changed research {0}'s time from {1} to {2}", ActualName, baseSecondsToCompleteResearch, secondsToCompleteResearch));
    }
    public void SetInitialValues()
    {
        InitializeObjects();

        _isResearchStarted = PlayerPrefs.GetInt(_stringIsResearchStarted) == 1 ? true : false;
        isResearched = PlayerPrefs.GetInt(_stringIsResearched) == 1 ? true : false;
        _researchTimeRemaining = PlayerPrefs.GetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
        IsUnlocked = PlayerPrefs.GetInt(_stringIsUnlocked) == 1 ? true : false;

        FetchPrestigeValues();

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
                //ObjProgressCirclePanel.SetActive(false);
            }
        }

        else if (isResearched && !_isResearchStarted)
        {
            ObjProgressCircle.SetActive(false);
            ObjBackground.SetActive(false);
            TxtHeader.text = string.Format("{0}", ActualName);
            BtnMain.interactable = false;
            imgResearchFill.fillAmount = 1;

            if (Menu.isResearchHidden)
            {
                if (ObjMainPanel.activeSelf)
                {
                    ObjMainPanel.SetActive(false);
                    Canvas.enabled = false;
                    GraphicRaycaster.enabled = false;
                }
            }
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

                _researchTimeRemaining = secondsToCompleteResearch - _currentTimer;
                TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(_researchTimeRemaining)));
                float percentage = (_currentTimer / secondsToCompleteResearch);


                if (span.Days == 0 && span.Hours == 0 && span.Minutes == 0)
                {
                    TxtHeader.text = string.Format("{0} <color=#F3FF0A>[{1:0}%]</color> (<b>{2:%s}s</b>)", ActualName, percentage * 100, span.Duration());
                }
                else if (span.Days == 0 && span.Hours == 0)
                {
                    TxtHeader.text = string.Format("{0} <color=#F3FF0A>[{1:0}%]</color> (<b>{1:%m}m {1:%s}s</b>)", ActualName, percentage * 100, span.Duration());
                }
                else if (span.Days == 0)
                {
                    TxtHeader.text = string.Format("{0} <color=#F3FF0A>[{1:0}%]</color> (<b>{1:%h}h {1:%m}m {1:%s}s</b>)", ActualName, percentage * 100, span.Duration());
                }
                else
                {
                    TxtHeader.text = string.Format("{0} <color=#F3FF0A>[{1:0}%]</color> (<b>{1:%d}d {1:%h}h {1:%m}m {1:%s}s</b>)", ActualName, percentage * 100, span.Duration());
                }

                imgResearchFill.fillAmount = percentage;
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

                for (int i = 0; i < ResourceCost.Length; i++)
                {
                    if (ResourceCost[i].CurrentAmount < ResourceCost[i].CostAmount)
                    {
                        canPurchase = false;
                        break;
                    }
                }

                if (canPurchase)
                {
                    for (int i = 0; i < ResourceCost.Length; i++)
                    {
                        Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
                    }
                    ObjProgressCircle.SetActive(false);
                    ObjBackground.SetActive(false);
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

        trackedResearchedAmount++;
        researchSimulActive--;
        UnlockCrafting();
        UnlockBuilding();
        UnlockResearchable();
        UnlockWorkerJob();
        UnlockResource();

        ObjProgressCircle.SetActive(false);
        ObjBackground.SetActive(false);
        TxtHeader.text = string.Format("{0}", ActualName);
        BtnMain.interactable = false;
        imgResearchFill.fillAmount = 1;

        if (Menu.isResearchHidden)
        {
            if (ObjMainPanel.activeSelf)
            {
                ObjMainPanel.SetActive(false);
                Canvas.enabled = false;
                GraphicRaycaster.enabled = false;
            }
        }
    }
    public void MakeResearchableAgain()
    {
        // This will probably only happen after prestige.
        BtnMain.interactable = true;
        ObjProgressCirclePanel.SetActive(true);
        TxtHeader.text = string.Format("{0}", ActualName);

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            ImgExpand.color = darkGreyColor;
            ImgCollapse.color = darkGreyColor;
        }
    }
    public void StartResearching()
    {
        researchSimulActive++;
        _isResearchStarted = true;
        //ObjProgressCirclePanel.SetActive(false);
    }
    public void SetDescriptionText(string description)
    {
        TxtDescription.text = string.Format("{0}", description);
    }
    protected override void InitializeObjects()
    {
        base.InitializeObjects();

        _stringIsResearched = Type.ToString() + "IsResearched";
        _stringResearchTimeRemaining = Type.ToString() + "TimeRemaining";
        _stringIsResearchStarted = Type.ToString() + "IsResearchStarted";
        _stringIsUnlocked = Type.ToString() + "IsUnlocked";

        AssignPrestigeStrings();

        BtnMain.onClick.AddListener(OnResearch);

        tformResearchFill = transform.Find("Panel_Main/Header_Panel/Image_Research_Fill");
        imgResearchFill = tformResearchFill.GetComponent<Image>();
        //_tformObjCheckmark = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/Checkmark");
        //_objCheckmark = _tformObjCheckmark.gameObject;
        //_objCheckmark.SetActive(false);

        if (IsUnlocked)
        {
            ObjMainPanel.SetActive(true);
            Canvas.enabled = false;
            GraphicRaycaster.enabled = false;
        }
        else
        {
            ObjMainPanel.SetActive(false);
            Canvas.enabled = false;
            GraphicRaycaster.enabled = false;
        }
    }
    protected void UnlockedViaResource()
    {
        if (IsUnlocked)
        {
            if (UIManager.isResearchVisible)
            {
                ObjMainPanel.SetActive(true);
                Canvas.enabled = true;
                GraphicRaycaster.enabled = true;
                HasSeen = true;
            }
            else if (HasSeen)
            {
                isResearchableUnlockedEvent = true;
                HasSeen = false;
                PointerNotification.rightAmount++;
            }
        }
    }
    private void CheckIfUnlocked()
    {
        if (!IsUnlocked)
        {
            if (GetCurrentFill() >= 0.8f & !IsUnlockedByResource && IsUnlockableByResource)
            {
                IsUnlockedByResource = true;
                UnlockAmount++;

                if (UnlockAmount == UnlocksRequired)
                {
                    IsUnlocked = true;

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
        if ((timer -= Time.deltaTime) <= 0)
        {
            timer = maxValue;
            if (!isResearched || !_isResearchStarted)
            {
                CheckIfPurchaseable();
            }

            UpdateResourceCostTexts();
            CheckIfUnlocked();
        }

        UpdateResearchTimer();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsResearchStarted, _isResearchStarted ? 1 : 0);
        PlayerPrefs.SetInt(_stringIsResearched, isResearched ? 1 : 0);
        PlayerPrefs.SetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
        PlayerPrefs.SetInt(_stringIsUnlocked, IsUnlocked ? 1 : 0);

        SavePrestigeValues();
    }
}


