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
    public float initialCostAmount;
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
    public GameObject objSpacerBelow;

    public int unlockAmount, unlocksRequired;
    [System.NonSerialized] public bool isUnlocked, hasSeen = true, isUnlockedByResource;
    [System.NonSerialized] public GameObject objMainPanel;

    protected float _timer = 0.1f;
    protected readonly float _maxValue = 0.1f;
    protected GameObject _prefabResourceCost, _prefabBodySpacer, _objProgressCircle, _objBtnMain, _objTxtHeaderDone, _objTxtHeader, _objBtnExpand, _objBtnCollapse, _objBody;
    protected TMP_Text _txtDescription;
    protected Transform _tformDescription, _tformTxtHeader, _tformBtnMain, _tformObjProgressCircle, _tformProgressCirclePanel, _tformTxtHeaderDone, _tformBtnExpand, _tformBtnCollapse, _tformBody, _tformObjMain, _tformExpand, _tformCollapse;
    protected Image _imgMain, _imgExpand, _imgCollapse, _imgProgressCircle;

    void OnValidate()
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
        _objBtnCollapse.GetComponent<Button>().onClick.AddListener(OnCollapse);
    }
    protected void OnExpandCloseAll()
    {
        if (UIManager.isCraftingVisible)
        {
            foreach (var craft in Craftable.Craftables)
            {
                craft.Value._objBody.SetActive(false);
                craft.Value._objBtnCollapse.SetActive(false);
                craft.Value._objBtnExpand.SetActive(true);
            }
            _objBtnExpand.SetActive(false);
            _objBody.SetActive(true);
            _objBtnCollapse.SetActive(true);
        }

        if (UIManager.isBuildingVisible)
        {
            foreach (var building in Building.Buildings)
            {
                building.Value._objBody.SetActive(false);
                building.Value._objBtnCollapse.SetActive(false);
                building.Value._objBtnExpand.SetActive(true);
            }
            _objBtnExpand.SetActive(false);
            _objBody.SetActive(true);
            _objBtnCollapse.SetActive(true);
        }

        if (UIManager.isResearchVisible)
        {
            foreach (var research in Researchable.Researchables)
            {
                research.Value._objBody.SetActive(false);
                research.Value._objBtnCollapse.SetActive(false);
                research.Value._objBtnExpand.SetActive(true);
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
    protected void ShowResourceCostTime(TMP_Text txt, float current, float cost, float amountPerSecond, float storageAmount)
    {
        if (isUnlocked)
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
                if (UIManager.isBuildingVisible)
                {
                    building.Value.objMainPanel.SetActive(true);
                    building.Value.objSpacerBelow.SetActive(true);
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
                    craftable.Value.objSpacerBelow.SetActive(true);
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
                    worker.Value.objMainPanel.SetActive(true);
                    worker.Value.objSpacerBelow.SetActive(true);
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
                    researchable.Value.objSpacerBelow.SetActive(true);
                    researchable.Value.hasSeen = true;
                }
                else if(researchable.Value.hasSeen)
                {
                    Researchable.isResearchableUnlockedEvent = true;
                    researchable.Value.hasSeen = false;
                    PointerNotification.rightAmount++;
                }
            }
        }      
    }
    protected void CheckIfUnlocked()
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
                    CheckIfResearchUnlocked();
                    CheckIfCraftingUnlocked();
                    CheckIfWorkerUnlocked();
                    CheckIfBuildingUnlocked();
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
                Resource.Resources[resource].objSpacerBelow.SetActive(true);
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

                if (UIManager.isWorkerVisible)
                {
                    Worker.Workers[worker].objMainPanel.SetActive(true);
                    Worker.Workers[worker].objSpacerBelow.SetActive(true);
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
                        Craftable.Craftables[craft].objSpacerBelow.SetActive(true);
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
                
                if (Building.Buildings[buildingType].unlockAmount == Building.Buildings[buildingType].unlocksRequired)
                {
                    #region This is new and for testing
                    if (cPassive4.buildingTypesSelfCountToModify.Contains(buildingType))
                    {
                        Building.Buildings[buildingType].SetInitialAmountPerSecond();
                    }
                    #endregion

                    Building.Buildings[buildingType].isUnlocked = true;

                    if (!UIManager.isBuildingVisible)
                    {
                        Building.isBuildingUnlockedEvent = true;
                        Building.Buildings[buildingType].hasSeen = false;
                        PointerNotification.leftAmount++;
                        PointerNotification.HandleLeftAnim();
                    }
                    else
                    {
                        Building.Buildings[buildingType].objMainPanel.SetActive(true);
                        Building.Buildings[buildingType].objSpacerBelow.SetActive(true);
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
                        Researchable.Researchables[research].objSpacerBelow.SetActive(true);
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
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;

            for (int i = 0; i < resourceCost.Length; i++)
            {
                resourceCost[i].currentAmount = Resource.Resources[resourceCost[i].associatedType].amount;
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(resourceCost[i].currentAmount), NumberToLetter.FormatNumber(resourceCost[i].costAmount));
                resourceCost[i].uiForResourceCost.textCostName.text = string.Format("{0}", resourceCost[i].associatedType.ToString());

                ShowResourceCostTime(resourceCost[i].uiForResourceCost.textCostAmount, resourceCost[i].currentAmount, resourceCost[i].costAmount, Resource.Resources[resourceCost[i].associatedType].amountPerSecond, Resource.Resources[resourceCost[i].associatedType].storageAmount);
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
