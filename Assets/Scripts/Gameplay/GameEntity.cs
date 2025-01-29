using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct UiForResourceCost
{
    public TMP_Text CostNameText;
    public TMP_Text CostAmountText;

    public UiForResourceCost(TMP_Text costNameText, TMP_Text costAmountText)
    {
        CostNameText = costNameText;
        CostAmountText = costAmountText;
    }
}

[System.Serializable]
public struct ResourceCost
{
    public ResourceType AssociatedType;
    public float CostAmount;
    public float BaseCostAmount;
    public UiForResourceCost UiForResourceCost;

    // // Keeping track of CurrentAmount might be handled by another method or class
    [NonSerialized]
    public float CurrentAmount;

    public ResourceCost(ResourceType associatedType, float costAmount, float baseCostAmount, UiForResourceCost uiForResourceCost)
    {
        AssociatedType = associatedType;
        CostAmount = costAmount;
        BaseCostAmount = baseCostAmount;
        UiForResourceCost = uiForResourceCost;
        CurrentAmount = 0;
    }
}

[System.Serializable]
public struct TypesToUnlock
{
    // Arrays hold the unlockable types for various categories
    public ResourceType[] ResourceTypesToUnlock;
    public BuildingType[] BuildingTypesToUnlock;
    public ResearchType[] ResearchTypesToUnlock;
    public CraftingType[] CraftingTypesToUnlock;
    public WorkerType[] WorkerTypesToUnlock;

    // Flags to track which category is being unlocked
    public bool IsUnlockingResource;
    public bool IsUnlockingResearch;
    public bool IsUnlockingCrafting;
    public bool IsUnlockingBuilding;
    public bool IsUnlockingWorker;

    public TypesToUnlock(
        ResourceType[] resourceTypesToUnlock, BuildingType[] buildingTypesToUnlock,
        ResearchType[] researchTypesToUnlock, CraftingType[] craftingTypesToUnlock,
        WorkerType[] workerTypesToUnlock, bool isUnlockingResource, bool isUnlockingResearch,
        bool isUnlockingCrafting, bool isUnlockingBuilding, bool isUnlockingWorker)
    {
        ResourceTypesToUnlock = resourceTypesToUnlock;
        BuildingTypesToUnlock = buildingTypesToUnlock;
        ResearchTypesToUnlock = researchTypesToUnlock;
        CraftingTypesToUnlock = craftingTypesToUnlock;
        WorkerTypesToUnlock = workerTypesToUnlock;
        IsUnlockingResource = isUnlockingResource;
        IsUnlockingResearch = isUnlockingResearch;
        IsUnlockingCrafting = isUnlockingCrafting;
        IsUnlockingBuilding = isUnlockingBuilding;
        IsUnlockingWorker = isUnlockingWorker;
    }
}

public abstract class GameEntity : MonoBehaviour
{
    // Public fields (Pascal case)
    public GameObject PrefabResourceCost;
    public GameObject PrefabBodySpacer;
    public GameObject ObjBackground;
    public GameObject ObjProgressCircle;
    public GameObject ObjProgressCirclePanel;
    public GameObject ObjBtnMain;
    public GameObject ObjTxtHeader;
    public GameObject ObjBtnExpand;
    public GameObject ObjBtnCollapse;
    public GameObject ObjBody;

    public TMP_Text TxtDescription;
    public TMP_Text TxtHeader;

    public Transform TformDescription;
    public Transform TformObjBackground;
    public Transform TformTxtHeader;
    public Transform TformBtnMain;
    public Transform TformObjProgressCircle;
    public Transform TformProgressCirclePanel;
    public Transform TformBtnExpand;
    public Transform TformBtnCollapse;
    public Transform TformBody;
    public Transform TformObjMain;
    public Transform TformExpand;
    public Transform TformCollapse;

    public Image ImgMain;
    public Image ImgExpand;
    public Image ImgCollapse;
    public Image ImgProgressCircle;

    public Button BtnMain;

    public Color ColTxtHeader;

    private bool IsPurchaseableSet;
    private float currentFillCache;
    private string strCachedResourceCost;

    protected float timer = 0.1f;
    protected readonly float maxValue = 0.1f;
    protected float lastFillAmount;

    public ResourceCost[] ResourceCost;
    public TypesToUnlock TypesToUnlock;
    public bool IsUnlockableByResource;
    public int UnlockAmount, UnlocksRequired;
    public string ActualName;

    [NonSerialized] public bool IsUnlocked, IsFirstUnlocked, HasSeen = true, IsUnlockedByResource, IsPurchaseable;
    [NonSerialized] public GameObject ObjMainPanel;
    [NonSerialized] public Canvas Canvas;
    [NonSerialized] public GraphicRaycaster GraphicRaycaster;


    private void OnValidate()
    {
        if (TypesToUnlock.BuildingTypesToUnlock.Length != 0)
        {
            TypesToUnlock.IsUnlockingBuilding = true;
        }
        else
        {
            TypesToUnlock.IsUnlockingBuilding = false;
        }

        if (TypesToUnlock.CraftingTypesToUnlock.Length != 0)
        {
            TypesToUnlock.IsUnlockingCrafting = true;
        }
        else
        {
            TypesToUnlock.IsUnlockingCrafting = false;
        }

        if (TypesToUnlock.ResearchTypesToUnlock.Length != 0)
        {
            TypesToUnlock.IsUnlockingResearch = true;
        }
        else
        {
            TypesToUnlock.IsUnlockingResearch = false;
        }

        if (TypesToUnlock.WorkerTypesToUnlock.Length != 0)
        {
            TypesToUnlock.IsUnlockingWorker = true;
        }
        else
        {
            TypesToUnlock.IsUnlockingWorker = false;
        }

        if (TypesToUnlock.ResourceTypesToUnlock.Length != 0)
        {
            TypesToUnlock.IsUnlockingResource = true;
        }
        else
        {
            TypesToUnlock.IsUnlockingResource = false;
        }
    }
    protected virtual void InitializeObjects()
    {
        TformBody = transform.Find("Panel_Main/Body");

        #region Prefab Initializion

        PrefabResourceCost = Resources.Load<GameObject>("ResourceCost_Prefab/ResourceCost_Panel");
        PrefabBodySpacer = Resources.Load<GameObject>("ResourceCost_Prefab/Body_Spacer");

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            GameObject newObj = Instantiate(PrefabResourceCost, TformBody);

            if (i < ResourceCost.Length - 1)
            {
                Instantiate(PrefabBodySpacer, TformBody);
            }

            Transform _tformNewObj = newObj.transform;
            Transform _tformCostName = _tformNewObj.Find("Cost_Name_Panel/Text_CostName");
            Transform _tformCostAmount = _tformNewObj.Find("Cost_Amount_Panel/Text_CostAmount");

            ResourceCost[i].UiForResourceCost.CostNameText = _tformCostName.GetComponent<TMP_Text>();
            ResourceCost[i].UiForResourceCost.CostAmountText = _tformCostAmount.GetComponent<TMP_Text>();
        }

        #endregion


        GraphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        Canvas = gameObject.GetComponent<Canvas>();

        TformObjBackground = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/Background");
        TformDescription = transform.Find("Panel_Main/Body/Text_Description");
        TformTxtHeader = transform.Find("Panel_Main/Header_Panel/Text_Header");
        TformBtnMain = transform.Find("Panel_Main/Header_Panel/Button_Main");
        TformObjProgressCircle = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/ProgressCircle");
        TformProgressCirclePanel = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel");
        TformBtnCollapse = transform.Find("Panel_Main/Header_Panel/Button_Collapse");
        TformBtnExpand = transform.Find("Panel_Main/Header_Panel/Button_Expand");
        TformObjMain = transform.Find("Panel_Main");


        ObjBackground = TformObjBackground.gameObject;
        ObjMainPanel = TformObjMain.gameObject;
        TxtDescription = TformDescription.GetComponent<TMP_Text>();
        ObjProgressCircle = TformObjProgressCircle.gameObject;
        ImgProgressCircle = TformObjProgressCircle.GetComponent<Image>();
        ImgExpand = TformBtnExpand.GetComponent<Image>();
        ImgCollapse = TformBtnCollapse.GetComponent<Image>();
        ObjProgressCirclePanel = TformProgressCirclePanel.gameObject;
        ObjTxtHeader = TformTxtHeader.gameObject;
        ObjBtnMain = TformBtnMain.gameObject;
        ObjBtnExpand = TformBtnExpand.gameObject;
        ObjBtnCollapse = TformBtnCollapse.gameObject;
        ObjBody = TformBody.gameObject;
        TxtHeader = ObjTxtHeader.GetComponent<TMP_Text>();
        BtnMain = ObjBtnMain.GetComponent<Button>();
        ColTxtHeader = ObjTxtHeader.GetComponent<TMP_Text>().color;

        ObjBtnExpand.GetComponent<Button>().onClick.AddListener(OnExpandCloseAll);
        ObjBtnCollapse.GetComponent<Button>().onClick.AddListener(OnCollapse);
    }
    protected void OnExpandCloseAll()
    {
        if (UIManager.isCraftingVisible)
        {
            foreach (var craft in Craftable.Craftables)
            {
                if (craft.Value.ObjBody.activeSelf)
                {
                    craft.Value.ObjBody.SetActive(false);
                    craft.Value.ObjBtnCollapse.SetActive(false);
                    craft.Value.ObjBtnExpand.SetActive(true);
                }

            }
            ObjBtnExpand.SetActive(false);
            ObjBody.SetActive(true);
            ObjBtnCollapse.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(ObjBody.GetComponent<RectTransform>());
        }

        if (UIManager.isBuildingVisible)
        {
            foreach (var building in Building.Buildings)
            {
                if (building.Value.ObjBody.activeSelf)
                {
                    building.Value.ObjBody.SetActive(false);
                    building.Value.ObjBtnCollapse.SetActive(false);
                    building.Value.ObjBtnExpand.SetActive(true);
                }
            }
            ObjBtnExpand.SetActive(false);
            ObjBody.SetActive(true);
            ObjBtnCollapse.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(ObjBody.GetComponent<RectTransform>());
        }

        if (UIManager.isResearchVisible)
        {
            foreach (var research in Researchable.Researchables)
            {
                if (research.Value.ObjBody.activeSelf)
                {
                    research.Value.ObjBody.SetActive(false);
                    research.Value.ObjBtnCollapse.SetActive(false);
                    research.Value.ObjBtnExpand.SetActive(true);
                }
            }
            ObjBtnExpand.SetActive(false);
            ObjBody.SetActive(true);
            ObjBtnCollapse.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(ObjBody.GetComponent<RectTransform>());
        }
    }
    protected void OnCollapse()
    {
        ObjBtnExpand.SetActive(true);
        ObjBody.SetActive(false);
        ObjBtnCollapse.SetActive(false);
    }
    protected void Purchaseable()
    {
        string htmlValue = "#333333";

        BtnMain.interactable = true;

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            ColTxtHeader = darkGreyColor;
        }
    }
    protected void UnPurchaseable()
    {
        BtnMain.interactable = false;

        string htmlValue = "#D71C2A";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color redColor))
        {
            ColTxtHeader = redColor;
        }
    }
    private void InitialBuildingUnlock(Building building)
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            //Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
            ResourceCost[i].CostAmount *= Mathf.Pow(building.costMultiplier, building._selfCount);
            ResourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[ResourceCost[i].AssociatedType].amount), NumberToLetter.FormatNumber(ResourceCost[i].CostAmount));
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
        building.TxtHeader.text = string.Format("{0} ({1})", building.ActualName, building._selfCount);

        //building.UpdateResourceInfo();
    }
    public virtual void CheckIfPurchaseable()
    {
        if (IsUnlocked)
        {
            if (lastFillAmount != GetCurrentFill() && GetCurrentFill() != 1)
            {
                IsPurchaseable = false;
                UnPurchaseable();
            }
            else if (GetCurrentFill() == 1)
            {
                IsPurchaseable = true;
            }

            if (!IsPurchaseable)
            {
                IsPurchaseableSet = false;
            }
            else if (IsPurchaseable && !IsPurchaseableSet)
            {
                Purchaseable();
                IsPurchaseableSet = true;
            }

            lastFillAmount = GetCurrentFill();
        }
    }
    protected void CalculateResourceCosts(TMP_Text txt, float current, float cost, float amountPerSecond, float storageAmount)
    {
        string strResourceCost;
        if (amountPerSecond > 0 && cost > current)
        {
            float secondsLeft = (cost - current) / (amountPerSecond);
            TimeSpan timeSpan = TimeSpan.FromSeconds((double)(new decimal(secondsLeft)));

            if (storageAmount < cost)
            {
                strResourceCost = string.Format("{0:0.00}/{1:0.00}(<color=#D71C2A>Never</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost));
            }
            else
            {
                if (current >= cost)
                {
                    strResourceCost = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost));
                }
                else if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0 && timeSpan.Seconds < 1)
                {
                    strResourceCost = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>0.{2:%f}ms</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
                else if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0)
                {
                    strResourceCost = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%s}s</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
                else if (timeSpan.Days == 0 && timeSpan.Hours == 0)
                {
                    strResourceCost = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%m}m{2:%s}s</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
                else if (timeSpan.Days == 0)
                {
                    strResourceCost = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%h}h{2:%m}m</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
                else
                {
                    strResourceCost = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%d}d{2:%h}h</color>)", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost), timeSpan.Duration());
                }
            }

        }
        else
        {
            strResourceCost = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(current), NumberToLetter.FormatNumber(cost));
        }

        if (strResourceCost != strCachedResourceCost)
        {
            txt.text = strResourceCost;
        }
        strCachedResourceCost = strResourceCost;
    }
    protected float GetCurrentFill()
    {
        float add = 0;
        float div = 0;
        float fillAmount = 0;

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            add = ResourceCost[i].CurrentAmount;
            div = ResourceCost[i].CostAmount;
            if (add > div)
            {
                add = div;
            }
            fillAmount += add / div;
        }
        return fillAmount / ResourceCost.Length;
    }
    protected void CheckIfBuildingUnlocked()
    {
        foreach (var building in Building.Buildings)
        {
            if (building.Value.IsUnlocked)
            {
                InitialBuildingUnlock(building.Value);
                building.Value.ObjMainPanel.SetActive(true);

                if (UIManager.isBuildingVisible)
                {
                    building.Value.Canvas.enabled = true;
                    building.Value.GraphicRaycaster.enabled = true;
                    building.Value.HasSeen = true;
                }
                else if (building.Value.HasSeen)
                {
                    Building.isBuildingUnlockedEvent = true;
                    building.Value.HasSeen = false;
                    PointerNotification.leftAmount++;
                }
            }
        }
    }
    protected void CheckIfCraftingUnlocked()
    {
        foreach (var craftable in Craftable.Craftables)
        {
            if (craftable.Value.IsUnlocked)
            {
                if (UIManager.isBuildingVisible && craftable.Value.HasSeen)
                {
                    Craftable.isCraftableUnlockedEvent = true;
                    craftable.Value.HasSeen = false;
                    PointerNotification.rightAmount++;
                }
                else if (UIManager.isCraftingVisible)
                {
                    // This does run more than once each, but isn't a big deal
                    craftable.Value.ObjMainPanel.SetActive(true);
                    craftable.Value.Canvas.enabled = true;
                    craftable.Value.GraphicRaycaster.enabled = true;
                    craftable.Value.HasSeen = true;
                }
                else if (UIManager.isWorkerVisible && craftable.Value.HasSeen)
                {
                    Craftable.isCraftableUnlockedEvent = true;
                    craftable.Value.HasSeen = false;
                    PointerNotification.leftAmount++;
                }
                else if (UIManager.isResearchVisible && craftable.Value.HasSeen)
                {
                    Craftable.isCraftableUnlockedEvent = true;
                    craftable.Value.HasSeen = false;
                    PointerNotification.leftAmount++;
                }
            }
        }
    }
    protected void CheckIfWorkerUnlocked()
    {
        foreach (var worker in Worker.Workers)
        {
            if (worker.Value.IsUnlocked)
            {
                worker.Value.ObjMainPanel.SetActive(true);

                if (UIManager.isBuildingVisible && worker.Value.HasSeen)
                {
                    Worker.isWorkerUnlockedEvent = true;
                    worker.Value.HasSeen = false;
                    PointerNotification.rightAmount++;
                }
                else if (UIManager.isCraftingVisible && worker.Value.HasSeen)
                {
                    Worker.isWorkerUnlockedEvent = true;
                    worker.Value.HasSeen = false;
                    PointerNotification.rightAmount++;
                }
                else if (UIManager.isWorkerVisible)
                {
                    worker.Value.Canvas.enabled = true;
                    worker.Value.GraphicRaycaster.enabled = true;
                    worker.Value.HasSeen = true;
                }
                else if (UIManager.isResearchVisible && worker.Value.HasSeen)
                {
                    Worker.isWorkerUnlockedEvent = true;
                    worker.Value.HasSeen = false;
                    PointerNotification.leftAmount++;
                }
            }
        }
    }
    protected void CheckIfResearchUnlocked()
    {
        foreach (var researchable in Researchable.Researchables)
        {
            if (researchable.Value.IsUnlocked)
            {
                if (UIManager.isResearchVisible)
                {
                    researchable.Value.ObjMainPanel.SetActive(true);
                    researchable.Value.Canvas.enabled = true;
                    researchable.Value.GraphicRaycaster.enabled = true;
                    researchable.Value.HasSeen = true;
                }
                else if (researchable.Value.HasSeen)
                {
                    Researchable.isResearchableUnlockedEvent = true;
                    researchable.Value.HasSeen = false;
                    PointerNotification.rightAmount++;
                }
            }
        }
    }
    protected void CheckIfUnlockedDeprecated()
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
                    if (TypesToUnlock.IsUnlockingResearch)
                    {
                        CheckIfResearchUnlocked();
                    }
                    if (TypesToUnlock.IsUnlockingCrafting)
                    {
                        CheckIfCraftingUnlocked();
                    }
                    if (TypesToUnlock.IsUnlockingWorker)
                    {
                        CheckIfWorkerUnlocked();
                    }
                    if (TypesToUnlock.IsUnlockingBuilding)
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
        if (TypesToUnlock.IsUnlockingResource)
        {
            foreach (var resource in TypesToUnlock.ResourceTypesToUnlock)
            {
                Resource.Resources[resource].InitializeAmount();
                Resource.Resources[resource].IsUnlocked = true;
                Resource.Resources[resource].ObjMainPanel.SetActive(true);
                Resource.Resources[resource].Canvas.enabled = true;
                Resource.Resources[resource].GraphicRaycaster.enabled = true;
            }
        }
    }
    protected void UnlockWorkerJob()
    {
        if (TypesToUnlock.IsUnlockingWorker)
        {
            foreach (var worker in TypesToUnlock.WorkerTypesToUnlock)
            {
                Worker.Workers[worker].IsUnlocked = true;
                Worker.Workers[worker].ObjMainPanel.SetActive(true);

                if (UIManager.isWorkerVisible)
                {
                    Worker.Workers[worker].Canvas.enabled = true;
                    Worker.Workers[worker].GraphicRaycaster.enabled = true;
                    Worker.Workers[worker].HasSeen = true;
                }
                else if (UIManager.isResearchVisible)
                {
                    Worker.isWorkerUnlockedEvent = true;
                    Worker.Workers[worker].HasSeen = false;
                    PointerNotification.leftAmount++;
                    PointerNotification.HandleLeftAnim();
                }
                else
                {
                    Worker.isWorkerUnlockedEvent = true;
                    Worker.Workers[worker].HasSeen = false;
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
        if (TypesToUnlock.IsUnlockingCrafting)
        {
            foreach (CraftingType craft in TypesToUnlock.CraftingTypesToUnlock)
            {
                Craftable.Craftables[craft].UnlockAmount++;

                if (Craftable.Craftables[craft].UnlockAmount == Craftable.Craftables[craft].UnlocksRequired)
                {
                    Craftable.Craftables[craft].IsUnlocked = true;

                    if (UIManager.isBuildingVisible)
                    {
                        Craftable.isCraftableUnlockedEvent = true;
                        Craftable.Craftables[craft].HasSeen = false;
                        PointerNotification.rightAmount++;
                        PointerNotification.HandleRightAnim();
                    }
                    else if (!UIManager.isCraftingVisible)
                    {
                        Craftable.isCraftableUnlockedEvent = true;
                        Craftable.Craftables[craft].HasSeen = false;
                        PointerNotification.leftAmount++;
                        PointerNotification.HandleLeftAnim();
                    }
                    else
                    {
                        Craftable.Craftables[craft].ObjMainPanel.SetActive(true);
                        Craftable.Craftables[craft].Canvas.enabled = true;
                        Craftable.Craftables[craft].GraphicRaycaster.enabled = true;
                        Craftable.Craftables[craft].HasSeen = true;
                    }
                }
            }
        }
    }
    protected void UnlockBuilding()
    {
        if (TypesToUnlock.IsUnlockingBuilding)
        {
            foreach (BuildingType buildingType in TypesToUnlock.BuildingTypesToUnlock)
            {
                Building.Buildings[buildingType].UnlockAmount++;
                Building.Buildings[buildingType].ObjMainPanel.SetActive(true);

                if (Building.Buildings[buildingType].UnlockAmount == Building.Buildings[buildingType].UnlocksRequired)
                {
                    InitialBuildingUnlock(Building.Buildings[buildingType]);
                    Building.Buildings[buildingType].IsUnlocked = true;
                    Building.Buildings[buildingType].CheckIfPurchaseable();

                    if (!UIManager.isBuildingVisible)
                    {
                        Building.isBuildingUnlockedEvent = true;
                        Building.Buildings[buildingType].HasSeen = false;
                        PointerNotification.leftAmount++;
                        PointerNotification.HandleLeftAnim();
                    }
                    else
                    {
                        Building.Buildings[buildingType].Canvas.enabled = true;
                        Building.Buildings[buildingType].GraphicRaycaster.enabled = true;
                        Building.Buildings[buildingType].HasSeen = true;
                    }
                }
            }
        }
    }
    protected void UnlockResearchable()
    {
        if (TypesToUnlock.IsUnlockingResearch)
        {
            foreach (ResearchType research in TypesToUnlock.ResearchTypesToUnlock)
            {
                Researchable.Researchables[research].UnlockAmount++;

                if (Researchable.Researchables[research].UnlockAmount == Researchable.Researchables[research].UnlocksRequired)
                {
                    Researchable.Researchables[research].IsUnlocked = true;

                    if (UIManager.isResearchVisible)
                    {
                        Researchable.Researchables[research].ObjMainPanel.SetActive(true);
                        Researchable.Researchables[research].Canvas.enabled = true;
                        Researchable.Researchables[research].GraphicRaycaster.enabled = true;
                        Researchable.Researchables[research].HasSeen = true;
                    }
                    else
                    {
                        Researchable.isResearchableUnlockedEvent = true;
                        Researchable.Researchables[research].HasSeen = false;
                        PointerNotification.rightAmount++;
                        PointerNotification.HandleRightAnim();
                    }
                }
            }
        }
    }
    protected void UpdateResourceCostTexts()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            ResourceCost[i].CurrentAmount = Resource.Resources[ResourceCost[i].AssociatedType].amount;

            if (!ObjBtnExpand.activeSelf && IsUnlocked)
            {
                ResourceCost[i].UiForResourceCost.CostNameText.text = string.Format("{0}", ResourceCost[i].AssociatedType.ToString());
                CalculateResourceCosts(ResourceCost[i].UiForResourceCost.CostAmountText, ResourceCost[i].CurrentAmount, ResourceCost[i].CostAmount, Resource.Resources[ResourceCost[i].AssociatedType].amountPerSecond, Resource.Resources[ResourceCost[i].AssociatedType].storageAmount);
            }
        }

        if (GetCurrentFill() != currentFillCache && IsUnlocked)
        {
            ImgProgressCircle.fillAmount = GetCurrentFill();
        }
        currentFillCache = GetCurrentFill();
    }
}
