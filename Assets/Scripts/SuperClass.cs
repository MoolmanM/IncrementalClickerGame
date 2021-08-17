using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

[System.Serializable]
public struct TypesToModify
{
    public ResourceType[] resourceTypesToModify;
    public BuildingType[] buildingTypesToModify;
    public ResearchType[] researchTypesToModify;
    public CraftingType[] craftingTypesToModify;
    public WorkerType[] workerTypesToModify;
    public bool isModifyingResource, isModifyingResearch, isModifyingCrafting, isModifyingBuilding, isModifyingWorker;
}
public abstract class SuperClass : MonoBehaviour
{
    public static bool isUnlockedEvent;

    public ResourceCost[] resourceCost;
    public TypesToModify typesToModify;
    public bool isUnlockableByResource;
    public GameObject objSpacerBelow;
    
    public int unlockAmount, unlocksRequired;
    [System.NonSerialized] public bool isUnlocked, hasSeen = true, isUnlockedByResource;
    [System.NonSerialized] public GameObject objMainPanel;

    protected float _timer = 0.1f;
    protected readonly float _maxValue = 0.1f;
    protected GameObject _prefabResourceCost, _prefabBodySpacer, _objProgressCircle, _objBtnMain, _objTxtHeader, _objTxtHeaderDone, _objBtnExpand, _objBtnCollapse, _objBody;
    protected TMP_Text _txtDescription;
    protected Transform _tformDescription, _tformTxtHeader, _tformBtnMain, _tformObjProgressCircle, _tformProgressCirclePanel, _tformTxtHeaderDone, _tformBtnExpand, _tformBtnCollapse, _tformBody, _tformObjMain, _tformExpand, _tformCollapse;
    protected Image _imgMain, _imgExpand, _imgCollapse, _imgProgressCircle;

    void OnValidate()
    {
        if (typesToModify.buildingTypesToModify.Length != 0)
        {
            typesToModify.isModifyingBuilding = true;
        }
        else
        {
            typesToModify.isModifyingBuilding = false;
        }

        if (typesToModify.craftingTypesToModify.Length != 0)
        {
            typesToModify.isModifyingCrafting = true;
        }
        else
        {
            typesToModify.isModifyingCrafting = false;
        }

        if (typesToModify.researchTypesToModify.Length != 0)
        {
            typesToModify.isModifyingResearch = true;
        }
        else
        {
            typesToModify.isModifyingResearch = false;
        }

        if (typesToModify.workerTypesToModify.Length != 0)
        {
            typesToModify.isModifyingWorker = true;
        }
        else
        {
            typesToModify.isModifyingWorker = false;
        }

        if (typesToModify.resourceTypesToModify.Length != 0)
        {
            typesToModify.isModifyingResource = true;
        }
        else
        {
            typesToModify.isModifyingResource = false;
        }
    }
    protected virtual void InitializeObjects()
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

        _tformDescription = transform.Find("Panel_Main/Body/Description_Panel/Text_Description");
        _tformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        _tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        _tformObjProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        _tformProgressCirclePanel = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel");
        _tformTxtHeaderDone = transform.Find("Panel_Main/Header_Panel/Text_Header_Done");
        _tformBtnCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformBtnExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");
        _tformObjMain = transform.Find("Panel_Main");

        objMainPanel = _tformObjMain.gameObject;
        _txtDescription = _tformDescription.GetComponent<TMP_Text>();
        _imgProgressCircle = _tformObjProgressCircle.GetComponent<Image>();
        _imgExpand = _tformBtnExpand.GetComponent<Image>();
        _imgCollapse = _tformBtnCollapse.GetComponent<Image>();
        _objProgressCircle = _tformProgressCirclePanel.gameObject;
        _objTxtHeader = _tformTxtHeader.gameObject;
        _objBtnMain = _tformBtnMain.gameObject;
        _objBtnExpand = _tformBtnExpand.gameObject;
        _objBtnCollapse = _tformBtnCollapse.gameObject;
        _objBody = _tformBody.gameObject;
      
        _objBtnExpand.GetComponent<Button>().onClick.AddListener(OnExpandCloseAll);
    }
    protected void OnExpandCloseAll()
    {
        foreach (var obj in Craftable.Craftables)
        {
            obj.Value._objBody.SetActive(false);
            obj.Value._objBtnCollapse.SetActive(false);
            obj.Value._objBtnExpand.SetActive(true);
        }
        _objBtnExpand.SetActive(false);
        _objBody.SetActive(true);
        _objBtnCollapse.SetActive(true);

    }
    protected void Purchaseable()
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
    protected void UnPurchaseable()
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
    protected void ShowResourceCostTime(TMP_Text txt, float current, float cost, float amountPerSecond)
    {
        if (amountPerSecond > 0)
        {
            float secondsLeft = (cost - current) / (amountPerSecond);
            TimeSpan timeSpan = TimeSpan.FromSeconds((double)(new decimal(secondsLeft)));

            if (current >= cost)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}", current, cost);
            }
            else if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%s}s</color>)", current, cost, timeSpan.Duration());
            }
            else if (timeSpan.Days == 0 && timeSpan.Hours == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%m}m {2:%s}s</color>)", current, cost, timeSpan.Duration());
            }
            else if (timeSpan.Days == 0)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{0:%h}h {0:%m}m</color>)", current, cost, timeSpan.Duration());
            }
            else
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{0:%d}d {0:%h}h</color>)", current, cost, timeSpan.Duration());
            }
        }
    }
    protected float GetCurrentFill()
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
    protected void CheckIfUnlocked()
    {
        if (!isUnlocked)
        {
            if (GetCurrentFill() >= 0.8f && isUnlockableByResource && !isUnlockedByResource)
            {
                unlockAmount++;
                isUnlockedByResource = true;
                if (unlockAmount == unlocksRequired)
                {
                    isUnlocked = true;
                    if (UIManager.isCraftingVisible)
                    {
                        objMainPanel.SetActive(true);
                        objSpacerBelow.SetActive(true);
                        hasSeen = true;
                    }
                    else if (UIManager.isBuildingVisible)
                    {
                        isUnlockedEvent = true;
                        hasSeen = false;
                        PointerNotification.rightAmount++;
                        
                    }
                    else
                    {
                        isUnlockedEvent = true;
                        hasSeen = false;
                        PointerNotification.leftAmount++;
                        
                    }
                    PointerNotification.HandleRightAnim();
                    PointerNotification.HandleLeftAnim();
                }
            }
        }
    }
    protected void UnlockResource()
    {
        if (typesToModify.isModifyingResource)
        {
            foreach (var resource in typesToModify.resourceTypesToModify)
            {
                Resource.Resources[resource].isUnlocked = true;
                Resource.Resources[resource].objMainPanel.SetActive(true);
                Resource.Resources[resource].objSpacerBelow.SetActive(true);
            }
        }
    } 
    protected void UnlockWorkerJob()
    {
        if (typesToModify.isModifyingWorker)
        {
            foreach (var worker in typesToModify.workerTypesToModify)
            {
                Worker.Workers[worker].isUnlocked = true;

                if (UIManager.isWorkerVisible)
                {
                    Worker.Workers[worker].objMainPanel.SetActive(true);
                    Worker.Workers[worker].objSpacerBelow.SetActive(true);
                    Worker.Workers[worker].hasSeen = true;
                }
                else if (UIManager.isResearchVisible)
                {
                    isUnlockedEvent = true;
                    Worker.Workers[worker].hasSeen = false;
                    PointerNotification.leftAmount++;
                    PointerNotification.HandleLeftAnim();
                }
                else
                {
                    isUnlockedEvent = true;
                    Worker.Workers[worker].hasSeen = false;
                    PointerNotification.rightAmount++;
                    PointerNotification.HandleRightAnim();
                }

                AutoWorker.TotalWorkerJobs++;

                if (AutoToggle.isAutoWorkerOn == 1)
                {
                    AutoWorker.CalculateWorkers();
                    AutoWorker.AutoAssignWorkers();
                }
            }                    
        }
    }
    protected void UnlockCrafting()
    {
        if (typesToModify.isModifyingCrafting)
        {
            foreach (CraftingType craft in typesToModify.craftingTypesToModify)
            {
                Craftable.Craftables[craft].unlockAmount++;

                if (Craftable.Craftables[craft].unlockAmount == Craftable.Craftables[craft].unlocksRequired)
                {
                    Craftable.Craftables[craft].isUnlocked = true;

                    if (UIManager.isBuildingVisible)
                    {
                        isUnlockedEvent = true;
                        Craftable.Craftables[craft].hasSeen = false;
                        PointerNotification.rightAmount++;
                        PointerNotification.HandleRightAnim();
                    }
                    else if (!UIManager.isCraftingVisible)
                    {
                        isUnlockedEvent = true;
                        Craftable.Craftables[craft].hasSeen = false;
                        PointerNotification.leftAmount++;
                        PointerNotification.HandleLeftAnim();
                    }
                    else
                    {
                        Craftable.Craftables[craft].objMainPanel.SetActive(true);
                        Craftable.Craftables[craft].objSpacerBelow.SetActive(true);
                        Craftable.Craftables[craft].hasSeen = true;
                    }
                }
            }                     
        }
    }
    protected void UnlockBuilding()
    {
        if (typesToModify.isModifyingBuilding)
        {
            foreach (BuildingType building in typesToModify.buildingTypesToModify)
            {
                Building.Buildings[building].unlockAmount++;

                if (Building.Buildings[building].unlockAmount == Building.Buildings[building].unlocksRequired)
                {
                    Building.Buildings[building].isUnlocked = true;

                    if (!UIManager.isBuildingVisible)
                    {
                        Building.isUnlockedEvent = true;
                        Building.Buildings[building].hasSeen = false;
                        PointerNotification.leftAmount++;
                        PointerNotification.HandleLeftAnim();
                    }
                    else
                    {
                        Building.Buildings[building].objMainPanel.SetActive(true);
                        Building.Buildings[building].objSpacerBelow.SetActive(true);
                        Building.Buildings[building].hasSeen = true;
                    }
                }
            }
        }
    }
    protected void UnlockResearchable()
    {
        if (typesToModify.isModifyingResearch)
        {
            foreach (ResearchType research in typesToModify.researchTypesToModify)
            {
                Researchable.Researchables[research].unlockAmount++;

                if (Researchable.Researchables[research].unlockAmount == Researchable.Researchables[research].unlocksRequired)
                {
                    Researchable.Researchables[research].isUnlocked = true;

                    if (UIManager.isResearchVisible)
                    {
                        Researchable.Researchables[research].objMainPanel.SetActive(true);
                        Researchable.Researchables[research].objSpacerBelow.SetActive(true);
                        Researchable.Researchables[research].hasSeen = true;
                    }
                    else
                    {
                        Researchable.isUnlockedEvent = true;
                        Researchable.Researchables[research].hasSeen = false;
                        PointerNotification.rightAmount++;
                        PointerNotification.HandleRightAnim();
                    }
                }
            }           
        }
    }
    protected void UpdateResourceCosts()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;

            for (int i = 0; i < resourceCost.Length; i++)
            {
                resourceCost[i].currentAmount = Resource.Resources[resourceCost[i].associatedType].amount;
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", resourceCost[i].currentAmount, resourceCost[i].costAmount);
                resourceCost[i].uiForResourceCost.textCostName.text = string.Format("{0}", resourceCost[i].associatedType.ToString());

                ShowResourceCostTime(resourceCost[i].uiForResourceCost.textCostAmount, resourceCost[i].currentAmount, resourceCost[i].costAmount, Resource.Resources[resourceCost[i].associatedType].amountPerSecond);
            }
            _imgProgressCircle.fillAmount = GetCurrentFill();
            CheckIfUnlocked();
        }
    }
    protected virtual void Update()
    {
        UpdateResourceCosts();
        CheckIfPurchaseable();
    }
}
