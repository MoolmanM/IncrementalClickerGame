using System.Collections;
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

    Paper,
    StoneEquipment
    
}

public abstract class Researchable : MonoBehaviour
{
    public static Dictionary<ResearchType, Researchable> Researchables = new Dictionary<ResearchType, Researchable>();

    public static bool isUnlockedEvent;
    public ResearchType Type;
    public ResourceCost[] resourceCost;
    public GameObject objMainPanel;

    public GameObject objSpacerBelow;
    [System.NonSerialized] public int isResearched = 0;
    [System.NonSerialized] public int isUnlocked = 0;

    private string _IsResearchedString;

    protected BuildingType[] _buildingTypesToModify;
    protected ResourceType[] _resourceTypesToModify;
    protected ResearchType[] _researchTypesToModify;
    protected WorkerType[] _workerTypesToModify;
    protected float _timer = 0.1f;
    protected readonly float _maxValue = 0.1f;
    protected TMP_Text _txtDescription;
    protected Transform _tformDescription, _tformTxtHeader, _tformBtnMain, _tformProgressbar, _tformProgressbarPanel, _tformTxtHeaderUncraft, _tformExpand, _tformCollapse, _tformObjMain;
    protected Image _imgProgressbar, _imgMain, _imgExpand, _imgCollapse;
    protected GameObject _objProgressbarPanel, _objBtnMain, _objTxtHeader, _objTxtHeaderUncraft;

    
    public void SetInitialValues()
    {
        InitializeObjects();

        if (TimeManager.hasPlayedBefore)
        {
            isResearched = PlayerPrefs.GetInt(_IsResearchedString, isResearched);
        }


        if (isResearched == 1)
        {
            Researched();
        }
        else
        {
            MakeResearchableAgain();
        }
    }
    protected void CheckIfUnlocked()
    {
        if (GetCurrentFill() == 1f)
        {
            Purchaseable();
        }
        else
        {
            UnPurchaseable();
        }

        if (isUnlocked == 0)
        {
            if (GetCurrentFill() >= 0.8f)
            {
                isUnlocked = 1;
                if (UIManager.isResearchVisible)
                {
                    objMainPanel.SetActive(true);
                    objSpacerBelow.SetActive(true);
                }
                else
                {
                    isUnlockedEvent = true;
                }
            }
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
            }

            _imgProgressbar.fillAmount = GetCurrentFill();
            CheckIfUnlocked();
        }
    }
    public void OnResearch()
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
            isResearched = 1;
            Researched();
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            }

        }
    }
    protected virtual void Researched()
    {
        _objBtnMain.GetComponent<Button>().interactable = false;
        _objProgressbarPanel.SetActive(false);
        _objTxtHeader.SetActive(false);
        _objTxtHeaderUncraft.SetActive(true);

        string htmlValue = "#D4D4D4";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
        {
            _imgExpand.color = greyColor;
            _imgCollapse.color = greyColor;
        }
    } 
    private void MakeResearchableAgain()
    {
        _objBtnMain.GetComponent<Button>().interactable = true;
        _objProgressbarPanel.SetActive(true);
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
        _tformDescription = transform.Find("Panel_Main/Body/Description_Panel/Text_Description");
        _tformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        _tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        _tformProgressbar = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        _tformProgressbarPanel = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel");
        _tformTxtHeaderUncraft = transform.Find("Panel_Main/Header_Panel/Text_Header_Done");
        _tformExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");
        _tformCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformObjMain = transform.Find("Panel_Main");

        _txtDescription = _tformDescription.GetComponent<TMP_Text>();
        _objTxtHeader = _tformTxtHeader.gameObject;
        _objBtnMain = _tformBtnMain.gameObject;
        _imgProgressbar = _tformProgressbar.GetComponent<Image>();
        _objProgressbarPanel = _tformProgressbarPanel.gameObject;
        _objTxtHeaderUncraft = _tformTxtHeaderUncraft.gameObject;
        _imgExpand = _tformExpand.GetComponent<Image>();
        _imgCollapse = _tformCollapse.GetComponent<Image>();
        objMainPanel = _tformObjMain.gameObject;

        _IsResearchedString = Type.ToString() + "isCrafted";
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
    public void SetDescriptionText(string description)
    {
        _txtDescription.text = string.Format("{0}", description);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_IsResearchedString, isResearched);
    }
}

