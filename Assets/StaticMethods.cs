using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct UiForResourceCost
{
<<<<<<< HEAD
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

public class SuperClass : MonoBehaviour
{
    public static Dictionary<BuildingType, Building> Buildings = new Dictionary<BuildingType, Building>();
    public static Dictionary<CraftingType, Craftable> Craftables = new Dictionary<CraftingType, Craftable>();
    public static Dictionary<ResearchType, Researchable> Researchables = new Dictionary<ResearchType, Researchable>();

    public static bool isUnlockedEvent;
    public ResourceCost[] resourceCost;
    public TypesToModify typesToModify;
    public GameObject objSpacerBelow;
    [System.NonSerialized] public bool isUnlocked,  hasSeen = true;
    [System.NonSerialized] public GameObject objMainPanel;
    public bool isUnlockableByResource;
    public int unlocksRequired, unlockAmount;

    protected string _isUnlockedString;
    protected GameObject _prefabResourceCost, _prefabBodySpacer;

    protected float _timer = 0.1f;
    protected readonly float _maxValue = 0.1f;
    protected TMP_Text _txtDescription;
    protected Transform _tformDescription, _tformTxtHeader, _tformBtnMain, _tformObjProgressCircle, _tformProgressbarPanel, _tformTxtHeaderUncraft, _tformBtnExpand, _tformBtnCollapse, _tformBody, _tformObjMain, _tformExpand, _tformCollapse;
    protected Image _imgProgressbar, _imgMain, _imgExpand, _imgCollapse;
    protected GameObject _objProgressCircle, _objBtnMain, _objTxtHeader, _objTxtHeaderUncraft, _objBtnExpand, _objBtnCollapse, _objBody;

    //void OnValidate()
    //{
    //    if (typesToModify.buildingTypesToModify.Length != 0)
    //    {
    //        typesToModify.isModifyingBuilding = true;
    //    }
    //    else
    //    {
    //        typesToModify.isModifyingBuilding = false;
    //    }

    //    if (typesToModify.craftingTypesToModify.Length != 0)
    //    {
    //        typesToModify.isModifyingCrafting = true;
    //    }
    //    else
    //    {
    //        typesToModify.isModifyingCrafting = false;
    //    }

    //    if (typesToModify.researchTypesToModify.Length != 0)
    //    {
    //        typesToModify.isModifyingResearch = true;
    //    }
    //    else
    //    {
    //        typesToModify.isModifyingResearch = false;
    //    }

    //    if (typesToModify.workerTypesToModify.Length != 0)
    //    {
    //        typesToModify.isModifyingWorker = true;
    //    }
    //    else
    //    {
    //        typesToModify.isModifyingWorker = false;
    //    }

    //    if (typesToModify.resourceTypesToModify.Length != 0)
    //    {
    //        typesToModify.isModifyingResource = true;
    //    }
    //    else
    //    {
    //        typesToModify.isModifyingResource = false;
    //    }
    //}
    protected void CheckIfPurchaseable()
=======
    void Awake()
>>>>>>> parent of 7cbddb0 (Backup)
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
    protected void UnlockWorkerJob()
    {
        if (typesToModify.isModifyingWorker)
        {
            PointerNotification.lastLeftAmount = PointerNotification.leftAmount;
            PointerNotification.lastRightAmount = PointerNotification.rightAmount;
            PointerNotification.rightAmount = 0;

            foreach (var worker in typesToModify.workerTypesToModify)
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

        //_isCraftedString = Type.ToString() + "isCrafted";
        //_isUnlockedString = (Type.ToString() + "isUnlocked");

        _objBtnExpand.GetComponent<Button>().onClick.AddListener(OnExpandCloseAll);
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
    protected void OnExpandCloseAll()
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
            _imgProgressbar.fillAmount = GetCurrentFill();
            CheckIfPurchaseable();
        }
    }
    protected void UnlockCrafting()
    {
        if (typesToModify.isModifyingCrafting)
        {
            foreach (CraftingType craft in typesToModify.craftingTypesToModify)
            {
                Craftables[craft].unlockAmount++;

                if (Craftables[craft].unlockAmount == Craftables[craft].unlocksRequired)
                {
                    Craftables[craft].isUnlocked = true;

                    if (UIManager.isBuildingVisible)
                    {
                        Craftable.isUnlockedEvent = true;
                        Craftables[craft].hasSeen = false;
                        PointerNotification.rightAmount++;
                    }
                    else if (!UIManager.isCraftingVisible)
                    {
                        Craftable.isUnlockedEvent = true;
                        Craftables[craft].hasSeen = false;
                        PointerNotification.leftAmount++;
                    }
                    else
                    {
                        Craftables[craft].objMainPanel.SetActive(true);
                        Craftables[craft].objSpacerBelow.SetActive(true);
                        Craftables[craft].hasSeen = true;
                    }
                }
            }

            PointerNotification.HandleRightAnim();
            PointerNotification.HandleLeftAnim();
        }
    }
    protected void UnlockBuilding()
    {
        if (typesToModify.isModifyingBuilding)
        {
            foreach (BuildingType building in typesToModify.buildingTypesToModify)
            {
                Buildings[building].unlockAmount++;

                if (Buildings[building].unlockAmount == Buildings[building].unlocksRequired)
                {
                    Buildings[building].isUnlocked = true;

                    if (!UIManager.isBuildingVisible)
                    {
                        Building.isUnlockedEvent = true;
                        Buildings[building].hasSeen = false;
                        PointerNotification.leftAmount++;
                    }
                    else
                    {
                        Buildings[building].objMainPanel.SetActive(true);
                        Buildings[building].objSpacerBelow.SetActive(true);
                        Buildings[building].hasSeen = true;
                    }
                }
            }

            PointerNotification.HandleRightAnim();
            PointerNotification.HandleLeftAnim();
        }
    }
    protected void UnlockResearchable()
    {
        if (typesToModify.isModifyingResearch)
        {
            foreach (ResearchType research in typesToModify.researchTypesToModify)
            {
                Researchables[research].unlockAmount++;

                if (Researchables[research].unlockAmount == Researchables[research].unlocksRequired)
                {
                    Researchables[research].isUnlocked = true;

                    if (UIManager.isResearchVisible)
                    {
                        Researchables[research].objMainPanel.SetActive(true);
                        Researchables[research].objSpacerBelow.SetActive(true);
                        Researchables[research].hasSeen = true;
                    }
                    else
                    {
                        Researchable.isUnlockedEvent = true;
                        Researchables[research].hasSeen = false;
                        PointerNotification.rightAmount++;
                    }
                }
            }

            PointerNotification.HandleRightAnim();
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
<<<<<<< HEAD
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
                        foreach (var craft in Craftables)
                        {
                            craft.Value.objMainPanel.SetActive(true);
                            craft.Value.objSpacerBelow.SetActive(true);
                        }

                    }
                    else if (UIManager.isBuildingVisible)
                    {
                        foreach (var building in Buildings)
                        {
                            building.Value.objMainPanel.SetActive(true);
                            building.Value.objSpacerBelow.SetActive(true);
                        }
                    }
                    else if (UIManager.isResearchVisible)
                    {
                        foreach (var research in Researchables)
                        {
                            research.Value.objMainPanel.SetActive(true);
                            research.Value.objSpacerBelow.SetActive(true);
                        }
                    }
                    else
                    {
                        isUnlockedEvent = true;
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
    void Update()
    {
        UpdateResourceCosts();
        CheckIfUnlocked();
        CheckIfPurchaseable();
    }
    //void Awake()
    //{
    //    foreach (var kvp in Researchable.Researchables)
    //    {
    //        if (kvp.Value.isUnlockableByResource)
    //        {
    //            kvp.Value.unlocksRequired++;
    //        }
    //        foreach (CraftingType type in kvp.Value.typesToModify.craftingTypesToModify)
    //        {
    //            Craftable.Craftables[type].unlocksRequired++;
    //        }
    //        foreach (ResearchType type in kvp.Value.typesToModify.researchTypesToModify)
    //        {
    //            Researchable.Researchables[type].unlocksRequired++;
    //        }
    //        foreach (BuildingType type in kvp.Value.typesToModify.buildingTypesToModify)
    //        {
    //            Building.Buildings[type].unlocksRequired++;
    //        }
    //    }

    //    foreach (var kvp in Building.Buildings)
    //    {
    //        if (kvp.Value.isUnlockableByResource)
    //        {
    //            kvp.Value.unlocksRequired++;
    //        }
    //        foreach (CraftingType type in kvp.Value.typesToModify.craftingTypesToModify)
    //        {
    //            Craftable.Craftables[type].unlocksRequired++;
    //        }
    //        foreach (ResearchType type in kvp.Value.typesToModify.researchTypesToModify)
    //        {
    //            Researchable.Researchables[type].unlocksRequired++;
    //        }
    //        foreach (BuildingType type in kvp.Value.typesToModify.buildingTypesToModify)
    //        {
    //            Building.Buildings[type].unlocksRequired++;
    //        }
    //    }

    //    foreach (var kvp in Craftable.Craftables)
    //    {
    //        if (kvp.Value.isUnlockableByResource)
    //        {
    //            kvp.Value.unlocksRequired++;
    //        }
    //        foreach (CraftingType type in kvp.Value.typesToModify.craftingTypesToModify)
    //        {
    //            Craftable.Craftables[type].unlocksRequired++;
    //        }
    //        foreach (ResearchType type in kvp.Value.typesToModify.researchTypesToModify)
    //        {
    //            Researchable.Researchables[type].unlocksRequired++;
    //        }
    //        foreach (BuildingType type in kvp.Value.typesToModify.buildingTypesToModify)
    //        {
    //            Building.Buildings[type].unlocksRequired++;
    //        }
    //    }
    //}
    //public static void UnlockCrafting(bool isModifying, CraftingType[] types)
    //{
    //    if (isModifying)
    //    {
    //        foreach (CraftingType craft in types)
    //        {
    //            Craftable.Craftables[craft].unlockAmount++;

    //            if (Craftable.Craftables[craft].unlockAmount == Craftable.Craftables[craft].unlocksRequired)
    //            {
    //                Craftable.Craftables[craft].isUnlocked = true;

    //                if (UIManager.isBuildingVisible)
    //                {
    //                    Craftable.isUnlockedEvent = true;
    //                    Craftable.Craftables[craft].hasSeen = false;
    //                    PointerNotification.rightAmount++;
    //                }
    //                else if (!UIManager.isCraftingVisible)
    //                {
    //                    Craftable.isUnlockedEvent = true;
    //                    Craftable.Craftables[craft].hasSeen = false;
    //                    PointerNotification.leftAmount++;
    //                }
    //                else
    //                {
    //                    Craftable.Craftables[craft].objMainPanel.SetActive(true);
    //                    Craftable.Craftables[craft].objSpacerBelow.SetActive(true);
    //                    Craftable.Craftables[craft].hasSeen = true;
    //                }
    //            }
    //        }

    //        PointerNotification.HandleRightAnim();
    //        PointerNotification.HandleLeftAnim();
    //    }
    //}
    //public static void UnlockBuilding(bool isModifying, BuildingType[] types)
    //{
    //    if (isModifying)
    //    {
    //        foreach (BuildingType building in types)
    //        {
    //            Building.Buildings[building].unlockAmount++;

    //            if (Building.Buildings[building].unlockAmount == Building.Buildings[building].unlocksRequired)
    //            {
    //                Building.Buildings[building].isUnlocked = true;

    //                if (!UIManager.isBuildingVisible)
    //                {
    //                    Building.isUnlockedEvent = true;
    //                    Building.Buildings[building].hasSeen = false;
    //                    PointerNotification.leftAmount++;
    //                }
    //                else
    //                {
    //                    Building.Buildings[building].objMainPanel.SetActive(true);
    //                    Building.Buildings[building].objSpacerBelow.SetActive(true);
    //                    Building.Buildings[building].hasSeen = true;
    //                }
    //            }
    //        }

    //        PointerNotification.HandleRightAnim();
    //        PointerNotification.HandleLeftAnim();
    //    }
    //}
    //public static void UnlockResearchable(bool isModifying, ResearchType[] types)
    //{
    //    if (isModifying)
    //    {
    //        foreach (ResearchType research in types)
    //        {
    //            Researchable.Researchables[research].unlockAmount++;

    //            if (Researchable.Researchables[research].unlockAmount == Researchable.Researchables[research].unlocksRequired)
    //            {
    //                Researchable.Researchables[research].isUnlocked = true;

    //                if (UIManager.isResearchVisible)
    //                {
    //                    Researchable.Researchables[research].objMainPanel.SetActive(true);
    //                    Researchable.Researchables[research].objSpacerBelow.SetActive(true);
    //                    Researchable.Researchables[research].hasSeen = true;
    //                }
    //                else
    //                {
    //                    Researchable.isUnlockedEvent = true;
    //                    Researchable.Researchables[research].hasSeen = false;
    //                    PointerNotification.rightAmount++;
    //                }
    //            }
    //        }

    //        PointerNotification.HandleRightAnim();
    //    }
    //}
    //public static void ShowResourceCostTime(TMP_Text txt, float current, float cost, float amountPerSecond)
    //{
    //    if (amountPerSecond > 0)
    //    {
    //        float secondsLeft = (cost - current) / (amountPerSecond);
    //        TimeSpan timeSpan = TimeSpan.FromSeconds((double)(new decimal(secondsLeft)));

    //        if (current >= cost)
    //        {
    //            txt.text = string.Format("{0:0.00}/{1:0.00}", current, cost);
    //        }
    //        else if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0)
    //        {
    //            txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%s}s</color>)", current, cost, timeSpan.Duration());
    //        }
    //        else if (timeSpan.Days == 0 && timeSpan.Hours == 0)
    //        {
    //            txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{2:%m}m {2:%s}s</color>)", current, cost, timeSpan.Duration());
    //        }
    //        else if (timeSpan.Days == 0)
    //        {
    //            txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{0:%h}h {0:%m}m</color>)", current, cost, timeSpan.Duration());
    //        }
    //        else
    //        {
    //            txt.text = string.Format("{0:0.00}/{1:0.00}(<color=#08F1FF>{0:%d}d {0:%h}h</color>)", current, cost, timeSpan.Duration());
    //        }
    //    }
    //}
    //public static void CheckIfUnlocked(bool isUnlocked, bool isUnlockableByResource, GameObject objMainPanel, GameObject objSpacerBelow, bool isUnlockedEvent, bool isVisible, ResourceCost[] costArray)
    //{
    //    if (!isUnlocked)
    //    {
    //        if (GetCurrentFill(costArray) >= 0.8f)
    //        {
    //            if (isUnlockableByResource)
    //            {
    //                isUnlocked = true;
    //                if (isVisible)
    //                {
    //                    objMainPanel.SetActive(true);
    //                    objSpacerBelow.SetActive(true);
    //                }
    //                else
    //                {
    //                    isUnlockedEvent = true;
    //                }
    //            }
    //        }
    //    }
    //}
    //public static void UpdateResourceCosts(ResourceCost[] costArray, Image imgProgressBar)
    //{
    //    if ((_timer -= Time.deltaTime) <= 0)
    //    {
    //        _timer = _maxValue;

    //        for (int i = 0; i < costArray.Length; i++)
    //        {
    //            costArray[i].currentAmount = Resource.Resources[costArray[i].associatedType].amount;
    //            costArray[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", costArray[i].currentAmount, costArray[i].costAmount);
    //            costArray[i].uiForResourceCost.textCostName.text = string.Format("{0}", costArray[i].associatedType.ToString());

    //            ShowResourceCostTime(costArray[i].uiForResourceCost.textCostAmount, costArray[i].currentAmount, costArray[i].costAmount, Resource.Resources[costArray[i].associatedType].amountPerSecond);
    //        }
    //        imgProgressBar.fillAmount = GetCurrentFill(costArray);
    //    }
    //}
    //public static float GetCurrentFill(ResourceCost[] costArray)
    //{
    //    float add = 0;
    //    float div = 0;
    //    float fillAmount = 0;

    //    for (int i = 0; i < costArray.Length; i++)
    //    {
    //        add = costArray[i].currentAmount;
    //        div = costArray[i].costAmount;
    //        if (add > div)
    //        {
    //            add = div;
    //        }
    //        fillAmount += add / div;
    //    }
    //    return fillAmount / costArray.Length;
    //}

    //private void Start()
    //{
    //    //Debug.Log((Mathf.Log(10) * ((4 * 10e12) * 6)) / 10);
    //    //Debug.Log((Mathf.Log10(20000000000000) - 6) /10); 

    //    // This is the Kardashev formula
    //    //This is earth's current point on the scale.
    //}
    //private void Update()
    //{
    //    // This seems to work perfectly.
    //    //if (Resource.Resources[ResourceType.Energy].amount > 0)
    //    //{
    //    //    Debug.Log("Kardashev Scale: " + (Mathf.Log10(Resource.Resources[ResourceType.Energy].amount) - 6) / 10);
    //    //}        
    //}
=======

>>>>>>> parent of 7cbddb0 (Backup)
}

