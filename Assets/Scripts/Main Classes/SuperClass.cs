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
    public float baseCostAmount;
    public UiForResourceCost uiForResourceCost;
}

[System.Serializable]
public struct TypesToUnlock
{
    public ResourceType[] resourceTypesToUnlock;
    public BuildingType[] buildingTypesToUnlock;
    public ResearchType[] researchTypesToUnlock;
    public CraftingType[] craftingTypesToUnlock;
    public WorkerType[] workerTypesToUnlock;
    public bool isUnlockingResource, isUnlockingResearch, isUnlockingCrafting, isUnlockingBuilding, isUnlockingWorker;
}
public abstract class SuperClass : MonoBehaviour
{
    public ResourceCost[] resourceCost;
    public TypesToUnlock typesToUnlock;
    public bool isUnlockableByResource;
    public int unlockAmount, unlocksRequired;
    public string actualName;

    [NonSerialized] public bool isUnlocked, isFirstUnlocked, hasSeen = true, isUnlockedByResource, isPurchaseable;
    [NonSerialized] public GameObject objMainPanel;
    [NonSerialized] public Canvas canvas;
    [NonSerialized] public GraphicRaycaster graphicRaycaster;

    protected float _timer = 0.1f, _lastFillAmount;
    protected readonly float _maxValue = 0.1f;
    protected GameObject _prefabResourceCost, _prefabBodySpacer, _objBackground, _objProgressCircle, _objProgressCirclePanel, _objBtnMain, _objTxtHeader, _objBtnExpand, _objBtnCollapse, _objBody;
    protected TMP_Text _txtDescription, _txtHeader;
    protected Transform _tformDescription, _tformObjBackground, _tformTxtHeader, _tformBtnMain, _tformObjProgressCircle, _tformProgressCirclePanel, _tformBtnExpand, _tformBtnCollapse, _tformBody, _tformObjMain, _tformExpand, _tformCollapse;
    protected Image _imgMain, _imgExpand, _imgCollapse, _imgProgressCircle;
    protected Button _btnMain;
    protected Color _colTxtHeader;
    protected bool _isPurchaseableSet;

    private float currentFillCache;

    private void OnValidate()
    {
        if (typesToUnlock.buildingTypesToUnlock.Length != 0)
        {
            typesToUnlock.isUnlockingBuilding = true;
        }
        else
        {
            typesToUnlock.isUnlockingBuilding = false;
        }

        if (typesToUnlock.craftingTypesToUnlock.Length != 0)
        {
            typesToUnlock.isUnlockingCrafting = true;
        }
        else
        {
            typesToUnlock.isUnlockingCrafting = false;
        }

        if (typesToUnlock.researchTypesToUnlock.Length != 0)
        {
            typesToUnlock.isUnlockingResearch = true;
        }
        else
        {
            typesToUnlock.isUnlockingResearch = false;
        }

        if (typesToUnlock.workerTypesToUnlock.Length != 0)
        {
            typesToUnlock.isUnlockingWorker = true;
        }
        else
        {
            typesToUnlock.isUnlockingWorker = false;
        }

        if (typesToUnlock.resourceTypesToUnlock.Length != 0)
        {
            typesToUnlock.isUnlockingResource = true;
        }
        else
        {
            typesToUnlock.isUnlockingResource = false;
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

            if (i < resourceCost.Length - 1)
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


        graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        canvas = gameObject.GetComponent<Canvas>();

        _tformObjBackground = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/Background");
        _tformDescription = transform.Find("Panel_Main/Body/Text_Description");
        _tformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        _tformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        _tformObjProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        _tformProgressCirclePanel = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel");
        _tformBtnCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        _tformBtnExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");
        _tformObjMain = transform.Find("Panel_Main");


        _objBackground = _tformObjBackground.gameObject;
        objMainPanel = _tformObjMain.gameObject;
        _txtDescription = _tformDescription.GetComponent<TMP_Text>();
        _objProgressCircle = _tformObjProgressCircle.gameObject;
        _imgProgressCircle = _tformObjProgressCircle.GetComponent<Image>();
        _imgExpand = _tformBtnExpand.GetComponent<Image>();
        _imgCollapse = _tformBtnCollapse.GetComponent<Image>();
        _objProgressCirclePanel = _tformProgressCirclePanel.gameObject;
        _objTxtHeader = _tformTxtHeader.gameObject;
        _objBtnMain = _tformBtnMain.gameObject;
        _objBtnExpand = _tformBtnExpand.gameObject;
        _objBtnCollapse = _tformBtnCollapse.gameObject;
        _objBody = _tformBody.gameObject;

        _txtHeader = _objTxtHeader.GetComponent<TMP_Text>();
        _btnMain = _objBtnMain.GetComponent<Button>();
        _colTxtHeader = _objTxtHeader.GetComponent<TMP_Text>().color;

        _objBtnExpand.GetComponent<Button>().onClick.AddListener(OnExpandCloseAll);
        _objBtnCollapse.GetComponent<Button>().onClick.AddListener(OnCollapse);
    }
    protected void OnExpandCloseAll()
    {
        if (UIManager.isCraftingVisible)
        {
            foreach (var craft in Craftable.Craftables)
            {
                if (craft.Value._objBody.activeSelf)
                {
                    craft.Value._objBody.SetActive(false);
                    craft.Value._objBtnCollapse.SetActive(false);
                    craft.Value._objBtnExpand.SetActive(true);
                }

            }
            _objBtnExpand.SetActive(false);
            _objBody.SetActive(true);
            _objBtnCollapse.SetActive(true);
        }

        if (UIManager.isBuildingVisible)
        {
            foreach (var building in Building.Buildings)
            {
                if (building.Value._objBody.activeSelf)
                {
                    building.Value._objBody.SetActive(false);
                    building.Value._objBtnCollapse.SetActive(false);
                    building.Value._objBtnExpand.SetActive(true);
                }
            }
            _objBtnExpand.SetActive(false);
            _objBody.SetActive(true);
            _objBtnCollapse.SetActive(true);
        }

        if (UIManager.isResearchVisible)
        {
            foreach (var research in Researchable.Researchables)
            {
                if (research.Value._objBody.activeSelf)
                {
                    research.Value._objBody.SetActive(false);
                    research.Value._objBtnCollapse.SetActive(false);
                    research.Value._objBtnExpand.SetActive(true);
                }
            }
            _objBtnExpand.SetActive(false);
            _objBody.SetActive(true);
            _objBtnCollapse.SetActive(true);
        }
    }
    protected void OnCollapse()
    {
        _objBtnExpand.SetActive(true);
        _objBody.SetActive(false);
        _objBtnCollapse.SetActive(false);
    }
    protected void Purchaseable()
    {
        string htmlValue = "#333333";

        _btnMain.interactable = true;

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            _colTxtHeader = darkGreyColor;
        }
    }
    protected void UnPurchaseable()
    {
        _btnMain.interactable = false;

        string htmlValue = "#D71C2A";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color redColor))
        {
            _colTxtHeader = redColor;
        }
    }
    private void InitialBuildingUnlock(Building building)
    {
        for (int i = 0; i < resourceCost.Length; i++)
        {
            //Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            resourceCost[i].costAmount *= Mathf.Pow(building.costMultiplier, building._selfCount);
            resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[resourceCost[i].associatedType].amount), NumberToLetter.FormatNumber(resourceCost[i].costAmount));
        }

        for (int i = 0; i < building.resourcesToIncrement.Count; i++)
        {
            if (CalculateAdBoost.isAdBoostActivated)
            {
                Resource.Resources[building.resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += building.resourcesToIncrement[i].currentResourceMultiplier * building._selfCount * CalculateAdBoost.adBoostMultiplier;
            }
            else
            {
                Resource.Resources[building.resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += building.resourcesToIncrement[i].currentResourceMultiplier * building._selfCount;
            }
            StaticMethods.ModifyAPSText(Resource.Resources[building.resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[building.resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        }
        building._txtHeader.text = string.Format("{0} ({1})", building.actualName, building._selfCount);

        //building.UpdateResourceInfo();
    }
    public virtual void CheckIfPurchaseable()
    {
        if (isUnlocked)
        {
            if (_lastFillAmount != GetCurrentFill() && GetCurrentFill() != 1)
            {
                isPurchaseable = false;
                UnPurchaseable();
            }
            else if (GetCurrentFill() == 1)
            {
                isPurchaseable = true;
            }

            if (!isPurchaseable)
            {
                _isPurchaseableSet = false;
            }
            else if (isPurchaseable && !_isPurchaseableSet)
            {
                Purchaseable();
                _isPurchaseableSet = true;
            }

            _lastFillAmount = GetCurrentFill();
        }
    }
    protected void ShowResourceCostTime(TMP_Text txt, float current, float cost, float amountPerSecond, float storageAmount)
    {
        if (amountPerSecond > 0 && cost > current)
        {
            float secondsLeft = (cost - current) / (amountPerSecond);
            TimeSpan timeSpan = TimeSpan.FromSeconds((double)(new decimal(secondsLeft)));

            if (storageAmount < cost)
            {
                txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#D71C2A>Never</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost));
            }
            else
            {
                if (current >= cost)
                {
                    txt.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost));
                }
                else if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0 && timeSpan.Seconds < 1)
                {
                    txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>0.{2:%f}ms</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
                else if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0)
                {
                    txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%s}s</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
                else if (timeSpan.Days == 0 && timeSpan.Hours == 0)
                {
                    txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%m}m{2:%s}s</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
                else if (timeSpan.Days == 0)
                {
                    txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%h}h{2:%m}m</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
                else
                {
                    txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%d}d{2:%h}h</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
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
    protected void CheckIfBuildingUnlocked()
    {
        foreach (var building in Building.Buildings)
        {
            if (building.Value.isUnlocked)
            {
                InitialBuildingUnlock(building.Value);
                building.Value.objMainPanel.SetActive(true);

                if (UIManager.isBuildingVisible)
                {                    
                    building.Value.canvas.enabled = true;
                    building.Value.graphicRaycaster.enabled = true;
                    building.Value.hasSeen = true;
                }
                else if (building.Value.hasSeen)
                {
                    Building.isBuildingUnlockedEvent = true;
                    building.Value.hasSeen = false;
                    PointerNotification.leftAmount++;
                }
            }
        }
    }
    protected void CheckIfCraftingUnlocked()
    {
        foreach (var craftable in Craftable.Craftables)
        {
            if (craftable.Value.isUnlocked)
            {
                if (UIManager.isBuildingVisible && craftable.Value.hasSeen)
                {
                    Craftable.isCraftableUnlockedEvent = true;
                    craftable.Value.hasSeen = false;
                    PointerNotification.rightAmount++;
                }
                else if (UIManager.isCraftingVisible)
                {
                    // This does run more than once each, but isn't a big deal
                    craftable.Value.objMainPanel.SetActive(true);
                    craftable.Value.canvas.enabled = true;
                    craftable.Value.graphicRaycaster.enabled = true;
                    craftable.Value.hasSeen = true;
                }
                else if (UIManager.isWorkerVisible && craftable.Value.hasSeen)
                {
                    Craftable.isCraftableUnlockedEvent = true;
                    craftable.Value.hasSeen = false;
                    PointerNotification.leftAmount++;
                }
                else if (UIManager.isResearchVisible && craftable.Value.hasSeen)
                {
                    Craftable.isCraftableUnlockedEvent = true;
                    craftable.Value.hasSeen = false;
                    PointerNotification.leftAmount++;
                }
            }
        }
    }
    protected void CheckIfWorkerUnlocked()
    {
        foreach (var worker in Worker.Workers)
        {
            if (worker.Value.isUnlocked)
            {
                worker.Value.objMainPanel.SetActive(true);

                if (UIManager.isBuildingVisible && worker.Value.hasSeen)
                {
                    Worker.isWorkerUnlockedEvent = true;
                    worker.Value.hasSeen = false;
                    PointerNotification.rightAmount++;
                }
                else if (UIManager.isCraftingVisible && worker.Value.hasSeen)
                {
                    Worker.isWorkerUnlockedEvent = true;
                    worker.Value.hasSeen = false;
                    PointerNotification.rightAmount++;
                }
                else if (UIManager.isWorkerVisible)
                {
                    worker.Value.canvas.enabled = true;
                    worker.Value.graphicRaycaster.enabled = true;
                    worker.Value.hasSeen = true;
                }
                else if (UIManager.isResearchVisible && worker.Value.hasSeen)
                {
                    Worker.isWorkerUnlockedEvent = true;
                    worker.Value.hasSeen = false;
                    PointerNotification.leftAmount++;
                }
            }
        }
    }
    protected void CheckIfResearchUnlocked()
    {
        foreach (var researchable in Researchable.Researchables)
        {
            if (researchable.Value.isUnlocked)
            {
                if (UIManager.isResearchVisible)
                {
                    researchable.Value.objMainPanel.SetActive(true);
                    researchable.Value.canvas.enabled = true;
                    researchable.Value.graphicRaycaster.enabled = true;
                    researchable.Value.hasSeen = true;
                }
                else if (researchable.Value.hasSeen)
                {
                    Researchable.isResearchableUnlockedEvent = true;
                    researchable.Value.hasSeen = false;
                    PointerNotification.rightAmount++;
                }
            }
        }
    }
    protected void CheckIfUnlockedDeprecated()
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
                    if (typesToUnlock.isUnlockingResearch)
                    {
                        CheckIfResearchUnlocked();
                    }
                    if (typesToUnlock.isUnlockingCrafting)
                    {
                        CheckIfCraftingUnlocked();
                    }
                    if (typesToUnlock.isUnlockingWorker)
                    {
                        CheckIfWorkerUnlocked();
                    }
                    if (typesToUnlock.isUnlockingBuilding)
                    {
                        CheckIfBuildingUnlocked();
                    }
                    
                    
                    
                    PointerNotification.HandleRightAnim();
                    PointerNotification.HandleLeftAnim();
                }
            }
        }
    }
    protected void UnlockResource()
    {
        if (typesToUnlock.isUnlockingResource)
        {
            foreach (var resource in typesToUnlock.resourceTypesToUnlock)
            {
                Resource.Resources[resource].InitializeAmount();
                Resource.Resources[resource].isUnlocked = true;
                Resource.Resources[resource].objMainPanel.SetActive(true);
                Resource.Resources[resource].canvas.enabled = true;
                Resource.Resources[resource].graphicRaycaster.enabled = true;
            }
        }
    }
    protected void UnlockWorkerJob()
    {
        if (typesToUnlock.isUnlockingWorker)
        {
            foreach (var worker in typesToUnlock.workerTypesToUnlock)
            {
                Worker.Workers[worker].isUnlocked = true;
                Worker.Workers[worker].objMainPanel.SetActive(true);

                if (UIManager.isWorkerVisible)
                {
                    Worker.Workers[worker].canvas.enabled = true;
                    Worker.Workers[worker].graphicRaycaster.enabled = true;
                    Worker.Workers[worker].hasSeen = true;
                }
                else if (UIManager.isResearchVisible)
                {
                    Worker.isWorkerUnlockedEvent = true;
                    Worker.Workers[worker].hasSeen = false;
                    PointerNotification.leftAmount++;
                    PointerNotification.HandleLeftAnim();
                }
                else
                {
                    Worker.isWorkerUnlockedEvent = true;
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
        if (typesToUnlock.isUnlockingCrafting)
        {
            foreach (CraftingType craft in typesToUnlock.craftingTypesToUnlock)
            {
                Craftable.Craftables[craft].unlockAmount++;

                if (Craftable.Craftables[craft].unlockAmount == Craftable.Craftables[craft].unlocksRequired)
                {
                    Craftable.Craftables[craft].isUnlocked = true;

                    if (UIManager.isBuildingVisible)
                    {
                        Craftable.isCraftableUnlockedEvent = true;
                        Craftable.Craftables[craft].hasSeen = false;
                        PointerNotification.rightAmount++;
                        PointerNotification.HandleRightAnim();
                    }
                    else if (!UIManager.isCraftingVisible)
                    {
                        Craftable.isCraftableUnlockedEvent = true;
                        Craftable.Craftables[craft].hasSeen = false;
                        PointerNotification.leftAmount++;
                        PointerNotification.HandleLeftAnim();
                    }
                    else
                    {
                        Craftable.Craftables[craft].objMainPanel.SetActive(true);
                        Craftable.Craftables[craft].canvas.enabled = true;
                        Craftable.Craftables[craft].graphicRaycaster.enabled = true;
                        Craftable.Craftables[craft].hasSeen = true;
                    }
                }
            }
        }
    }
    protected void UnlockBuilding()
    {
        if (typesToUnlock.isUnlockingBuilding)
        {
            foreach (BuildingType buildingType in typesToUnlock.buildingTypesToUnlock)
            {
                Building.Buildings[buildingType].unlockAmount++;
                Building.Buildings[buildingType].objMainPanel.SetActive(true);

                if (Building.Buildings[buildingType].unlockAmount == Building.Buildings[buildingType].unlocksRequired)
                {
                    InitialBuildingUnlock(Building.Buildings[buildingType]);
                    Building.Buildings[buildingType].isUnlocked = true;
                    Building.Buildings[buildingType].CheckIfPurchaseable();
                    
                    if (!UIManager.isBuildingVisible)
                    {
                        Building.isBuildingUnlockedEvent = true;
                        Building.Buildings[buildingType].hasSeen = false;
                        PointerNotification.leftAmount++;
                        PointerNotification.HandleLeftAnim();
                    }
                    else
                    {
                        Building.Buildings[buildingType].canvas.enabled = true;
                        Building.Buildings[buildingType].graphicRaycaster.enabled = true;
                        Building.Buildings[buildingType].hasSeen = true;
                    }
                }
            }
        }
    }
    protected void UnlockResearchable()
    {
        if (typesToUnlock.isUnlockingResearch)
        {
            foreach (ResearchType research in typesToUnlock.researchTypesToUnlock)
            {
                Researchable.Researchables[research].unlockAmount++;

                if (Researchable.Researchables[research].unlockAmount == Researchable.Researchables[research].unlocksRequired)
                {
                    Researchable.Researchables[research].isUnlocked = true;

                    if (UIManager.isResearchVisible)
                    {
                        Researchable.Researchables[research].objMainPanel.SetActive(true);
                        Researchable.Researchables[research].canvas.enabled = true;
                        Researchable.Researchables[research].graphicRaycaster.enabled = true;
                        Researchable.Researchables[research].hasSeen = true;
                    }
                    else
                    {
                        Researchable.isResearchableUnlockedEvent = true;
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
        for (int i = 0; i < resourceCost.Length; i++)
        {
            resourceCost[i].currentAmount = Resource.Resources[resourceCost[i].associatedType].amount;

            if (!_objBtnExpand.activeSelf && isUnlocked)
            {
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(resourceCost[i].currentAmount), NumberToLetter.FormatNumber(resourceCost[i].costAmount));
                resourceCost[i].uiForResourceCost.textCostName.text = string.Format("{0}", resourceCost[i].associatedType.ToString());
                ShowResourceCostTime(resourceCost[i].uiForResourceCost.textCostAmount, resourceCost[i].currentAmount, resourceCost[i].costAmount, Resource.Resources[resourceCost[i].associatedType].amountPerSecond, Resource.Resources[resourceCost[i].associatedType].storageAmount);
            }
        }

        //if (GetCurrentFill() >= 0.01f && GetCurrentFill() <= 1)
        //{
        //    Debug.Log("This happened");
        //    _imgProgressCircle.fillAmount = GetCurrentFill();
        //}

        //if (GetCurrentFill() != currentFillCache)
        //{
        //    _imgProgressCircle.fillAmount = GetCurrentFill();
        //}
        // This seems to be working very well, keeping the previous ones here incase something goes wrong.
        if (GetCurrentFill() != currentFillCache && isUnlocked)
        {
            _imgProgressCircle.fillAmount = GetCurrentFill();
        }
        currentFillCache = GetCurrentFill();


        //CheckIfUnlocked();
    }
}
