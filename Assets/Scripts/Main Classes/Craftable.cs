using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public enum CraftingType
{
    //I could maybe have different types here such as tools and then like refined crafting? For now it'll just be the name of each crafting option
    WoodenHoe,
    WoodenAxe,
    WoodenPickaxe,
    WoodenSpear,
    StoneHoe,
    StoneAxe,
    StonePickaxe,
    StoneSpear,
    FireHardenedSpear,
    HamsterWheel
}

public abstract class Craftable : MonoBehaviour
{
    public static Dictionary<CraftingType, Craftable> Craftables = new Dictionary<CraftingType, Craftable>();

    public static bool isUnlockedEvent;
    public CraftingType Type;
    public ResourceCost[] resourceCost;
    public GameObject objSpacerBelow;
    [System.NonSerialized] public bool isUnlocked, isCrafted, hasSeen = true;
    [System.NonSerialized] public GameObject objMainPanel;
    public float averageAmount;
    public int unlocksRequired = 1, unlocksAmount;

    private string _isCraftedString, _isUnlockedString;
    private GameObject _prefabResourceCost, _prefabBodySpacer;

    protected bool isUnlockableByResource;
    protected BuildingType[] _buildingTypesToModify;
    protected ResourceType[] _resourceTypesToModify;
    protected WorkerType[] _workerTypesToModify;
    protected CraftingType[] _craftingTypesToModify;
    protected float _timer = 0.1f;
    protected readonly float _maxValue = 0.1f;
    protected TMP_Text _txtDescription;
    protected Transform _tformDescription, _tformTxtHeader, _tformBtnMain, _tformObjProgressCircle, _tformProgressbarPanel, _tformTxtHeaderUncraft, _tformBtnExpand, _tformBtnCollapse, _tformBody, _tformObjMain, _tformExpand, _tformCollapse;      
    protected Image _imgProgressbar, _imgMain, _imgExpand, _imgCollapse;
    protected GameObject _objProgressCircle, _objBtnMain, _objTxtHeader, _objTxtHeaderUncraft, _objBtnExpand, _objBtnCollapse, _objBody;

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(_isCraftedString, isCrafted ? 1 : 0);
    }
    public void SetInitialValues()
    {
        InitializeObjects();
   
        if (TimeManager.hasPlayedBefore)
        {
            isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;
            isCrafted = PlayerPrefs.GetInt(_isCraftedString) == 1 ? true : false;
        }
        else
        {
            isCrafted = false;
        }
        

        if (isCrafted)
        {
            Crafted();
        }
        else
        {
            MakeCraftableAgain();
        }
    }
    protected void CheckIfUnlocked()
    {    
        if (!isUnlocked)
        {
            if (GetCurrentFill() >= 0.8f)
            {
                if (isUnlockableByResource)
                {
                    isUnlocked = true;
                    if (UIManager.isCraftingVisible)
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
    public virtual void UpdateResourceCosts()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;

            for (int i = 0; i < resourceCost.Length; i++)
            {
                Craftables[Type].resourceCost[i].currentAmount = Resource.Resources[Craftables[Type].resourceCost[i].associatedType].amount;
                Craftables[Type].resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Craftables[Type].resourceCost[i].currentAmount, Craftables[Type].resourceCost[i].costAmount);
                Craftables[Type].resourceCost[i].uiForResourceCost.textCostName.text = string.Format("{0}", Craftables[Type].resourceCost[i].associatedType.ToString());

                Building.ShowResourceCostTime(Craftables[Type].resourceCost[i].uiForResourceCost.textCostAmount, Craftables[Type].resourceCost[i].currentAmount, Craftables[Type].resourceCost[i].costAmount, Resource.Resources[resourceCost[i].associatedType].amountPerSecond);
            }
            _imgProgressbar.fillAmount =  GetCurrentFill();
            CheckIfUnlocked();
            CheckIfPurchaseable();
        }
    }
    public void OnCraft()
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
            isCrafted = true;
            Crafted();
            UnlockBuilding();
            UnlockResource();
            UnlockWorkerJob();
            
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            }

        }
    }
    protected virtual void Crafted()
    {
        _objBtnMain.GetComponent<Button>().interactable = false;
        _objProgressCircle.SetActive(false);
        _objTxtHeader.SetActive(false);
        _objTxtHeaderUncraft.SetActive(true);

        string htmlValue = "#D4D4D4";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
        {
            _imgExpand.color = greyColor;
            _imgCollapse.color = greyColor;
        }

        if (Menu.isCraftingHidden)
        {
            objMainPanel.SetActive(false);
            objSpacerBelow.SetActive(false);
        }

    }
    private void MakeCraftableAgain()
    {
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
    protected virtual void UnlockBuilding()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.leftAmount = 0;
        foreach (var building in _buildingTypesToModify)
        {
            Building.Buildings[building].isUnlocked = true;
            Building.isUnlockedEvent = true;
            Building.Buildings[building].hasSeen = false;
        }

        foreach (var buildingMain in Building.Buildings)
        {
            if (!buildingMain.Value.hasSeen)
            {
                PointerNotification.leftAmount++;
            }         
        }

        PointerNotification.HandleLeftAnim();
    }
    protected virtual void UnlockWorkerJob()
    {
        PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
        PointerNotification.lastRightAmount = PointerNotification.rightAmount;
        PointerNotification.rightAmount = 0;
        foreach (var worker in _workerTypesToModify)
        {
            Worker.Workers[worker].isUnlocked = true;
            Worker.isUnlockedEvent = true;
            Worker.Workers[worker].hasSeen = false;
            AutoWorker.TotalWorkerJobs++;
            
            if (AutoToggle.isAutoWorkerOn == 1)
            {
                AutoWorker.CalculateWorkers();
                AutoWorker.AutoAssignWorkers();
            }
        }

        foreach (var workerMain in Worker.Workers)
        {
            if (!workerMain.Value.hasSeen)
            {
                PointerNotification.rightAmount++;
            }
        }

        PointerNotification.HandleRightAnim();
    }
    protected virtual void UnlockResource()
    {
        foreach (var resource in _resourceTypesToModify)
        {
            Resource.Resources[resource].isUnlocked = 1;
            Resource.Resources[resource].objMainPanel.SetActive(true);
            Resource.Resources[resource].objSpacerBelow.SetActive(true);
        }
    }
    protected virtual void UnlockCrafting()
    {
        foreach (var craft in _craftingTypesToModify)
        {
            if (Craftables[craft].unlocksAmount < Craftables[craft].unlocksRequired)
            {
                Craftables[craft].unlocksAmount++;
            }
            else
            {
                Craftables[craft].isUnlocked = true;
                Craftables[craft].hasSeen = true;
            }      
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

        _tformDescription = transform.Find("Panel_Main/Body/Text_Description");
        _tformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        _tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        _tformObjProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        _tformProgressbarPanel = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel");
        _tformTxtHeaderUncraft = transform.Find("Panel_Main/Header_Panel/Text_Header_Uncraftable");
        _tformBtnCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformBtnExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");      
        _tformObjMain = transform.Find("Panel_Main");

        _txtDescription = _tformDescription.GetComponent<TMP_Text>();
        _objTxtHeader = _tformTxtHeader.gameObject;
        _objBtnMain = _tformBtnMain.gameObject;
        _imgProgressbar = _tformObjProgressCircle.GetComponent<Image>();
        _objProgressCircle = _tformProgressbarPanel.gameObject;
        _objTxtHeaderUncraft = _tformTxtHeaderUncraft.gameObject;
        _imgExpand = _tformBtnExpand.GetComponent<Image>();
        _imgCollapse = _tformBtnCollapse.GetComponent<Image>();
        objMainPanel = _tformObjMain.gameObject;
        _objBtnExpand = _tformBtnExpand.gameObject;
        _objBtnCollapse = _tformBtnCollapse.gameObject;
        _objBody = _tformBody.gameObject;

        _isCraftedString = Type.ToString() + "isCrafted";
        _isUnlockedString = (Type.ToString() + "isUnlocked");
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
    public void OnExpandCloseAll()
    {
        foreach (var obj in Craftables)
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
        Craftables[Type]._txtDescription.text = string.Format("{0}", description);
    }
}
