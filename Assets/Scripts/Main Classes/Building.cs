using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct UiForResourceCost
{
    public TMP_Text textCostName;
    public TMP_Text textCostAmount;
}

[System.Serializable]
public struct ResourceCost
{
    public ResourceType associatedType;
    [System.NonSerialized] public float currentAmount;
    public float costAmount;
    public UiForResourceCost uiForResourceCost;
}

public enum BuildingType
{
    PotatoField,
    Woodlot,
    DigSite,
    MakeshiftBed,
    Furnace
}

public abstract class Building : MonoBehaviour
{
    public static Dictionary<BuildingType, Building> Buildings = new Dictionary<BuildingType, Building>();

    public BuildingType Type;
    public ResourceCost[] resourceCost;
    public GameObject objSpacerBelow;
    [NonSerialized] public bool isUnlocked, hasSeen = true;
    [NonSerialized] public GameObject objMainPanel;
    public static bool isUnlockedEvent;

    private string _selfCountString, _isUnlockedString;
    private string[] _costString;
    private GameObject _objBtnMain, _objBtnExpand, _objBtnCollapse, _objBody;

    protected float _resourceMultiplier, _costMultiplier;
    protected ResourceType resourceTypeToModify;
    protected Transform _tformTxtHeader, _tformDescription, _tformObjProgressCircle, _tformObjMain, _tformBtnMain, _tformBtnExpand, _tformBtnCollapse, _tformBody;
    protected TMP_Text _txtHeader, _txtDescription;
    protected Image _imgProgressbar;
    protected string _stringOriginalHeader;
    protected uint _selfCount;
    protected float _timer = 0.1f;
    protected readonly float _maxValue = 0.1f;

    public GameObject costPrefab;
    //public Transform transformParent;

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(_selfCountString, (int)_selfCount);

        for (int i = 0; i < resourceCost.Length; i++)
        {
            PlayerPrefs.SetFloat(_costString[i], resourceCost[i].costAmount);
        }
    }
    public void SetInitialValues()
    {
        InitializeObjects();

        if (TimeManager.hasPlayedBefore)
        {
            isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;
            _selfCount = (uint)PlayerPrefs.GetInt(_selfCountString, (int)_selfCount);

            for (int i = 0; i < resourceCost.Length; i++)
            {
                resourceCost[i].costAmount = PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
            }
        }
        _txtHeader.text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    protected void CheckIfPurchaseable()
    {
        if (GetCurrentFill() == 1f)
        {
            Purchaseable();
        }
        else
        {
            UnPurchaseable();
        }
    }

    private void InitializeObjects()
    {
        _tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        _tformDescription = transform.Find("Panel_Main/Body/Description_Panel/Text_Description");
        _tformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        _tformObjProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        _tformObjMain = transform.Find("Panel_Main");
        _tformBtnCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformBtnExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");
        _tformBody = transform.Find("Panel_Main/Body");

        _txtHeader = _tformTxtHeader.GetComponent<TMP_Text>();
        _txtDescription = _tformDescription.GetComponent<TMP_Text>();
        _imgProgressbar = _tformObjProgressCircle.GetComponent<Image>();
        objMainPanel = _tformObjMain.gameObject;
        _objBtnMain = _tformBtnMain.gameObject;
        _objBtnExpand = _tformBtnExpand.gameObject;
        _objBtnCollapse = _tformBtnCollapse.gameObject;
        _objBody = _tformBody.gameObject;


        _stringOriginalHeader = _txtHeader.text;

        _selfCountString = (Type.ToString() + "_selfCount");
        _isUnlockedString = (Type.ToString() + "isUnlocked");
        _costString = new string[resourceCost.Length];

        for (int i = 0; i < resourceCost.Length; i++)
        {

            _costString[i] = Type.ToString() + resourceCost[i].associatedType.ToString();
            PlayerPrefs.GetFloat(_costString[i], resourceCost[i].costAmount);
        }
    }
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
                
                //if (Resource.Resources[resourceCost[i].associatedType].amountPerSecond > 0)
                //{
                    // 3 per second 
                    // kort 60
                    //float secondsLeft = (resourceCost[i].costAmount - resourceCost[i].currentAmount) / (Resource.Resources[resourceCost[i].associatedType].amountPerSecond);
                    //TimeSpan span = TimeSpan.FromSeconds((double)(new decimal(secondsLeft)));

                    //if (resourceCost[i].currentAmount > resourceCost[i].costAmount)
                    //{
                    //    resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", resourceCost[i].currentAmount, resourceCost[i].costAmount);
                    //}
                    //else if (span.Days == 0 && span.Hours == 0 && span.Minutes == 0)
                    //{
                    //    resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}({2:%s}s)", resourceCost[i].currentAmount, resourceCost[i].costAmount, span.Duration());
                    //}
                    //else if (span.Days == 0 && span.Hours == 0)
                    //{
                    //    resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}({2:%m}m {2:%s}s)", resourceCost[i].currentAmount, resourceCost[i].costAmount, span.Duration());
                    //}
                    //else if (span.Days == 0)
                    //{
                    //    resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}({0:%h}h {0:%m}m)", resourceCost[i].currentAmount, resourceCost[i].costAmount, span.Duration());
                    //}
                    //else
                    //{
                    //    resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}({0:%d}d {0:%h}h)", resourceCost[i].currentAmount, resourceCost[i].costAmount, span.Duration());
                    //}

                    ShowResourceCostTime(resourceCost[i].uiForResourceCost.textCostAmount, resourceCost[i].currentAmount, resourceCost[i].costAmount, Resource.Resources[resourceCost[i].associatedType].amountPerSecond);
                //}
            }
            _imgProgressbar.fillAmount = GetCurrentFill();
            CheckIfPurchaseable();
        }
    }
    private void Start()
    {
        
    }
    public static void ShowResourceCostTime(TMP_Text txt, float current, float cost, float amountPerSecond)
    {
        if (amountPerSecond > 0)
        {
            float secondsLeft = (cost - current) / (amountPerSecond);
            TimeSpan timeSpan = TimeSpan.FromSeconds((double)(new decimal(secondsLeft)));

            if (current > cost)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}", current, cost);
            }
            else if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}({2:%s}s)", current, cost, timeSpan.Duration());
            }
            else if (timeSpan.Days == 0 && timeSpan.Hours == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}({2:%m}m {2:%s}s)", current, cost, timeSpan.Duration());
            }
            else if (timeSpan.Days == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}({0:%h}h {0:%m}m)", current, cost, timeSpan.Duration());
            }
            else
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}({0:%d}d {0:%h}h)", current, cost, timeSpan.Duration());
            }
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
    public virtual void OnBuild()
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
            _selfCount++;
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[Buildings[Type].resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
                resourceCost[i].costAmount *= Mathf.Pow(_costMultiplier, _selfCount);
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[Buildings[Type].resourceCost[i].associatedType].amount, resourceCost[i].costAmount);
            }
            ModifyResource();
        }

        _txtHeader.text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);   
    }
    private void Purchaseable()
    {
        ColorBlock cb = _objBtnMain.GetComponent<Button>().colors;
        cb.normalColor = new Color(0.2f, 0.2f, 0.2f, 0);
        cb.highlightedColor = new Color(0.2f, 0.2f, 0.2f, 0.05882353f);
        cb.pressedColor = new Color(0.2f, 0.2f, 0.2f, 0.1960784f);
        cb.selectedColor = new Color(0.2f, 0.2f, 0.2f, 0);
        _objBtnMain.GetComponent<Button>().colors = cb;

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            _txtHeader.color = darkGreyColor;
        }
    }
    private void UnPurchaseable()
    {
        ColorBlock cb = _objBtnMain.GetComponent<Button>().colors;
        cb.normalColor = new Color(0, 0, 0, 0.25f);
        cb.highlightedColor = new Color(0, 0, 0, 0.23f);
        cb.pressedColor = new Color(0, 0, 0, 0.3f);
        cb.selectedColor = new Color(0, 0, 0, 0.25f);
        _objBtnMain.GetComponent<Button>().colors = cb;

        string htmlValue = "#D71C2A";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color redColor))
        {
            _txtHeader.color = redColor;
        }
    }
    public virtual void SetDescriptionText()
    {
        Buildings[Type]._txtDescription.text = string.Format("Increases {0} yield by: {1:0.00}", Resource.Resources[resourceTypeToModify].Type.ToString(), _resourceMultiplier);
    }
    public void OnExpandCloseAll()
    {
        foreach (var obj in Buildings)
        {
            obj.Value._objBody.SetActive(false);
            obj.Value._objBtnCollapse.SetActive(false);
            obj.Value._objBtnExpand.SetActive(true);
        }
        _objBtnExpand.SetActive(false);
        _objBody.SetActive(true);
        _objBtnCollapse.SetActive(true);
        
    }
    protected virtual void ModifyResource()
    {
        Resource.Resources[resourceTypeToModify].amountPerSecond += _resourceMultiplier;
    }
}








