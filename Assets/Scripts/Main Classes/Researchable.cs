using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    Smelting   
}

public abstract class Researchable : MonoBehaviour
{
    public static Dictionary<ResearchType, Researchable> Researchables = new Dictionary<ResearchType, Researchable>();
    public static int researchSimulActive = 0, researchSimulAllowed = 1;
    public static bool isUnlockedEvent;

    public ResearchType Type;
    public ResourceCost[] resourceCost;
    
    public GameObject objSpacerBelow;
    [System.NonSerialized] public GameObject objMainPanel;
    [System.NonSerialized] public bool isUnlocked, isResearched, hasSeen = true;
    public uint unlocksRequired = 1, unlocksAmount;

    private bool isResearchStarted;
    private string _stringIsResearched, _stringResearchTimeRemaining, _stringIsResearchStarted;
    private float _currentTimer, _researchTimeRemaining;
    private GameObject _prefabResourceCost, _prefabBodySpacer;
    private float timer = 0.1f;
    private readonly float maxValue = 0.1f;

    protected float _timeToCompleteResearch;
    protected BuildingType[] _buildingTypesToModify;
    protected ResourceType[] _resourceTypesToModify;
    protected ResearchType[] _researchTypesToModify;
    protected CraftingType[] _craftingTypesToModify;
    protected WorkerType[] _workerTypesToModify;
    protected float _timer = 0.1f;
    protected readonly float _maxValue = 0.1f;   
    protected TMP_Text _txtDescription;
    protected Transform _tformImgProgressCircle, _tformImgResearchBar, _tformDescription, _tformTxtHeader, _tformBtnMain, _tformObjProgressCircle, _tformProgressbarPanel, _tformTxtHeaderUncraft, _tformExpand, _tformCollapse, _tformObjMain, _tformBtnExpand, _tformBtnCollapse, _tformBody;
    protected Image _imgMain, _imgExpand, _imgCollapse, _imgResearchBar, _imgProgressCircle;
    protected GameObject _objProgressCircle, _objBtnMain, _objTxtHeader, _objTxtHeaderUncraft, _objBtnExpand, _objBtnCollapse, _objBody;
    private string _stringHeader;

    public void SetInitialValues()
    {
        InitializeObjects();
        isUnlocked = true;

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
                _timeToCompleteResearch = _researchTimeRemaining - (float)TimeManager.difference.TotalSeconds;
                Debug.Log("You still have ongoing research");
                _objProgressCircle.SetActive(false);
            }
        }

        else if(isResearched && !isResearchStarted)
        {
            Researched();
        }
        
    }
    //protected void CheckIfUnlocked()
    //{
    //    if (GetCurrentFill() == 1f)
    //    {
    //        Purchaseable();
    //    }
    //    else
    //    {
    //        UnPurchaseable();
    //    }

    //    if (isUnlocked == 0)
    //    {
    //        if (GetCurrentFill() >= 0.8f)
    //        {
    //            isUnlocked = 1;
    //            if (UIManager.isResearchVisible)
    //            {
    //                objMainPanel.SetActive(true);
    //                objSpacerBelow.SetActive(true);
    //            }
    //            else
    //            {
    //                isUnlockedEvent = true;
    //            }
    //        }
    //    }
    //}
    public virtual void UpdateResourceCosts()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;

            for (int i = 0; i < resourceCost.Length; i++)
            {
                resourceCost[i].currentAmount = Resource.Resources[resourceCost[i].associatedType].amount;
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", resourceCost[i].currentAmount, resourceCost[i].costAmount);
                resourceCost[i].uiForResourceCost.textCostName.text = string.Format("{0}", resourceCost[i].associatedType.ToString());
            }

            _imgProgressCircle.fillAmount = GetCurrentFill();
            //CheckIfUnlocked();
            
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

                _imgResearchBar.fillAmount = _currentTimer / _timeToCompleteResearch;
                _researchTimeRemaining = _timeToCompleteResearch - _currentTimer;  
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
        if (_currentTimer >= _timeToCompleteResearch)
        {
            isResearchStarted = false;
            isResearched = true;
            Researched();
        }
    }
    protected virtual void UnlockBuilding()
    {
        PointerNotification.leftAmount = 0;
        foreach (var building in _buildingTypesToModify)
        {           
            if (UIManager.isBuildingVisible)
            {
                Building.Buildings[building].isUnlocked = true;
                Building.Buildings[building].objMainPanel.SetActive(true);
                Building.Buildings[building].objSpacerBelow.SetActive(true);
                Building.Buildings[building].hasSeen = true;
            }   
            else
            {
                Building.Buildings[building].isUnlocked = true;
                Building.isUnlockedEvent = true;
                Building.Buildings[building].hasSeen = false;
            }
        }

        foreach (var buildingMain in Building.Buildings)
        {
            if (!buildingMain.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        if (PointerNotification.leftAmount > 0)
        {
            PointerNotification.objLeftPointer.SetActive(true);
            PointerNotification.textLeft.GetComponent<TMP_Text>().text = PointerNotification.leftAmount.ToString();
        }
    }
    protected virtual void UnlockCrafting()
    {
        PointerNotification.leftAmount = 0;
        foreach (CraftingType craft in _craftingTypesToModify)
        {           
            if (UIManager.isCraftingVisible)
            {
                Craftable.Craftables[craft].isUnlocked = true;
                Craftable.Craftables[craft].objMainPanel.SetActive(true);
                Craftable.Craftables[craft].objSpacerBelow.SetActive(true);
                Craftable.Craftables[craft].hasSeen = true;
            }
            else
            {
                Craftable.Craftables[craft].isUnlocked = true;
                Craftable.isUnlockedEvent = true;
                Craftable.Craftables[craft].hasSeen = false;
            }
            //Debug.Log(Craftable.Craftables[craft].Type + " " + Craftable.Craftables[craft].isUnlocked);       
        }

        foreach (var craftMain in Craftable.Craftables)
        {
            if (!craftMain.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }
        }

        if (PointerNotification.leftAmount > 0)
        {
            PointerNotification.objLeftPointer.SetActive(true);
            PointerNotification.textLeft.GetComponent<TMP_Text>().text = PointerNotification.leftAmount.ToString();
        }
    }
    protected virtual void UnlockResearchable()
    {
        // Need to check this code, might not be working very well. And might contain some bugs.
        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;

        foreach (ResearchType research in _researchTypesToModify)
        {
            if (Researchables[research].unlocksAmount < Researchables[research].unlocksRequired)
            {
                Researchables[research].unlocksAmount++;
            }
            else
            {
                if (UIManager.isResearchVisible)
                {
                    Researchables[research].isUnlocked = true;
                    Researchables[research].objMainPanel.SetActive(true);
                    Researchables[research].objSpacerBelow.SetActive(true);
                    Researchables[research].hasSeen = true;
                }
                else
                {
                    Researchables[research].isUnlocked = true;
                    isUnlockedEvent = true;
                    Researchables[research].hasSeen = false;
                }

            }
        }

        foreach (var researchMain in Researchables)
        {
            if (!researchMain.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        if (PointerNotification.leftAmount > 0)
        {
            PointerNotification.objLeftPointer.SetActive(true);
            PointerNotification.textLeft.GetComponent<TMP_Text>().text = PointerNotification.leftAmount.ToString();
        }
        if (PointerNotification.rightAmount > 0)
        {
            PointerNotification.objRightPointer.SetActive(true);
            PointerNotification.textRight.GetComponent<TMP_Text>().text = PointerNotification.rightAmount.ToString();
        }
    }
    protected virtual void Researched()
    {
        if (ToggleResearch.isResearchHidden == 1)
        {
            researchSimulActive--;
            UnlockCrafting();
            UnlockBuilding();
            _objProgressCircle.SetActive(false);
            _objTxtHeader.SetActive(false);
            _objTxtHeaderUncraft.SetActive(true);

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
            _objProgressCircle.SetActive(false);
            _objTxtHeader.SetActive(false);
            _objTxtHeaderUncraft.SetActive(true);

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
        _objTxtHeaderUncraft.SetActive(false);

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            _imgExpand.color = darkGreyColor;
            _imgCollapse.color = darkGreyColor;
        }
    }

    private void InitializeObjects()
    {
        _tformBody = transform.Find("Panel_Main/Body");

        #region Prefab Initializion

        _prefabResourceCost = Resources.Load<GameObject>("ResourceCost_Prefab/ResourceCost_Panel");
        _prefabBodySpacer = Resources.Load<GameObject>("ResourceCost_Prefab/Body_Spacer");

        for (int i = 0; i < resourceCost.Length; i++)
        {
            GameObject newObj = Instantiate(_prefabResourceCost, _tformBody);

            //This loop just makes sure that there is a never a body spacer underneath the last element(the last resource cost panel)
            for (int spacerI = i + 1; spacerI < resourceCost.Length; spacerI++)
            {
                Instantiate(_prefabBodySpacer, _tformBody);
            }

            Transform _tformNewObj = newObj.transform;
            Transform _tformCostName = _tformNewObj.Find("Cost_Name_Panel/Text_CostName");
            Transform _tformCostAmount = _tformNewObj.Find("Cost_Amount_Panel/Text_CostAmount");

            resourceCost[i].uiForResourceCost.textCostName = _tformCostName.GetComponent<TMP_Text>();
            resourceCost[i].uiForResourceCost.textCostAmount = _tformCostAmount.GetComponent<TMP_Text>();
        }

        #endregion

        _tformImgResearchBar = transform.Find("Panel_Main/Header_Panel/Research_FillBar");
        _tformDescription = transform.Find("Panel_Main/Body/Description_Panel/Text_Description");
        _tformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        _tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        _tformObjProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel");
        _tformImgProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        _tformTxtHeaderUncraft = transform.Find("Panel_Main/Header_Panel/Text_Header_Done");
        _tformExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");
        _tformCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformObjMain = transform.Find("Panel_Main");
        _tformBtnCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformBtnExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");

        _imgProgressCircle = _tformImgProgressCircle.GetComponent<Image>();
        _imgResearchBar = _tformImgResearchBar.GetComponent<Image>();
        _txtDescription = _tformDescription.GetComponent<TMP_Text>();
        _objTxtHeader = _tformTxtHeader.gameObject;
        _objBtnMain = _tformBtnMain.gameObject;
        _objProgressCircle = _tformObjProgressCircle.gameObject;
        _objTxtHeaderUncraft = _tformTxtHeaderUncraft.gameObject;
        _imgExpand = _tformExpand.GetComponent<Image>();
        _imgCollapse = _tformCollapse.GetComponent<Image>();
        objMainPanel = _tformObjMain.gameObject;
        _objBtnExpand = _tformBtnExpand.gameObject;
        _objBtnCollapse = _tformBtnCollapse.gameObject;
        _objBody = _tformBody.gameObject;
        _stringHeader = _objTxtHeader.GetComponent<TMP_Text>().text;

        _stringIsResearched = Type.ToString() + "isCrafted";
        _stringResearchTimeRemaining = Type.ToString() + "ResearchTimeRemaining";
        _stringIsResearchStarted = Type.ToString() + "IsResearchStarted";
    }
    private void Purchaseable()
    {
        ColorBlock cb = _objBtnMain.GetComponent<Button>().colors;
        cb.normalColor = new Color(0, 0, 0, 0);
        _objBtnMain.GetComponent<Button>().colors = cb;

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            _objTxtHeader.GetComponent<TMP_Text>().color = darkGreyColor;
        }
    }
    private void UnPurchaseable()
    {
        ColorBlock cb = _objBtnMain.GetComponent<Button>().colors;
        cb.normalColor = new Color(0, 0, 0, 0.25f);
        cb.highlightedColor = new Color(0, 0, 0, 0.23f);
        cb.pressedColor = new Color(0, 0, 0, 0.3f);
        cb.selectedColor = new Color(0, 0, 0, 0.23f);
        _objBtnMain.GetComponent<Button>().colors = cb;

        string htmlValue = "#D71C2A";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color redColor))
        {
            _objTxtHeader.GetComponent<TMP_Text>().color = redColor;
        }
    }
    public float GetCurrentFill()
    {
        float add = 0;
        float div = 0;
        float fillAmount = 0;

        for (int i = 0; i < resourceCost.Length; i++)
        {
            add = resourceCost[i].currentAmount;
            div = resourceCost[i].costAmount;
            if (add > div)
            {
                add = div;
            }
            fillAmount += add / div;
        }
        return fillAmount / resourceCost.Length;
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
        _timeToCompleteResearch = differenceAmount.Seconds;
    }
    public void StartResearching()
    {
        researchSimulActive++;
        isResearchStarted = true;
        _objProgressCircle.SetActive(false);     
    }
    public void OnExpandCloseAll()
    {
        foreach (var obj in Researchables)
        {
            obj.Value._objBody.SetActive(false);
            obj.Value._objBtnCollapse.SetActive(false);
            obj.Value._objBtnExpand.SetActive(true);
        }
        _objBtnExpand.SetActive(false);
        _objBody.SetActive(true);
        _objBtnCollapse.SetActive(true);

    }
    public void SetDescriptionText(string description)
    {
        _txtDescription.text = string.Format("{0}", description);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_stringIsResearchStarted, isResearchStarted ? 1 : 0);
        PlayerPrefs.SetInt(_stringIsResearched, isResearched ? 1 : 0);
        PlayerPrefs.SetFloat(_stringResearchTimeRemaining, _researchTimeRemaining);
    }
}


