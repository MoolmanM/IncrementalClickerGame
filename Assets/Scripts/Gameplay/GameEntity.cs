using System;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameEntity : MonoBehaviour
{
    public ResourceCost[] ResourceCosts;
    public TypesToUnlock UnlockableTypes; 
    public bool IsUnlockableByResource; 
    public int UnlockAmount, UnlocksRequired;
    public string EntityName; 

    [NonSerialized] public bool IsUnlocked, IsFirstUnlocked, HasSeen = true, IsUnlockedByResource, IsPurchasable; 
    [NonSerialized] public GameObject MainPanel; 
    [NonSerialized] public Canvas EntityCanvas; 
    [NonSerialized] public GraphicRaycaster Raycaster; 

    private float _lastFillAmount, _currentFillCache;
    private string _cachedResourceCostText;
    private bool _isPurchasableSet;

    private static readonly UnityEngine.Color DarkGrayColor = ColorUtility.TryParseHtmlString("#333333", out var color) ? color : UnityEngine.Color.grey;
    private static readonly UnityEngine.Color RedColor = ColorUtility.TryParseHtmlString("#D71C2A", out var color) ? color : UnityEngine.Color.red;

    private void OnValidate()
    {
        SetUnlockingFlags();
    }

    private void SetUnlockingFlags()
    {
        UnlockableTypes.IsUnlockingBuilding = UnlockableTypes.BuildingTypesToUnlock.Length > 0;
        UnlockableTypes.IsUnlockingCrafting = UnlockableTypes.CraftingTypesToUnlock.Length > 0;
        UnlockableTypes.IsUnlockingResearch = UnlockableTypes.ResearchTypesToUnlock.Length > 0;
        UnlockableTypes.IsUnlockingWorker = UnlockableTypes.WorkerTypesToUnlock.Length > 0;
        UnlockableTypes.IsUnlockingResource = UnlockableTypes.ResourceTypesToUnlock.Length > 0;
    }

    protected virtual void InitializeObjects()
    {
        InitializeResourceCostPrefabs();
        CacheUIComponents();
        ConfigureUIEventListeners();
    }

    private void InitializeResourceCostPrefabs()
    {
        var resourceCostPrefab = Resources.Load<GameObject>("ResourceCost_Prefab/ResourceCost_Panel");
        var bodySpacerPrefab = Resources.Load<GameObject>("ResourceCost_Prefab/Body_Spacer");

        foreach (var cost in ResourceCosts)
        {
            var newObj = Instantiate(resourceCostPrefab, transform);

            var costNameTransform = newObj.transform.Find("Cost_Name_Panel/Text_CostName");
            var costAmountTransform = newObj.transform.Find("Cost_Amount_Panel/Text_CostAmount");

            cost.UiForResourceCost.TextCostName = costNameTransform.GetComponent<TMP_Text>();
            cost.UiForResourceCost.TextCostAmount = costAmountTransform.GetComponent<TMP_Text>();
        }
    }

    private void CacheUIComponents()
    {
        Raycaster = GetComponent<GraphicRaycaster>();
        EntityCanvas = GetComponent<Canvas>();
        MainPanel = transform.Find("Panel_Main").gameObject;
    }

    private void ConfigureUIEventListeners()
    {
        var expandButton = transform.Find("Panel_Main/Header_Panel/Button_Expand").GetComponent<Button>();
        var collapseButton = transform.Find("Panel_Main/Header_Panel/Button_Collapse").GetComponent<Button>();

        expandButton.onClick.AddListener(OnExpandCloseAll);
        collapseButton.onClick.AddListener(OnCollapse);
    }

    protected void OnExpandCloseAll()
    {
        ToggleAllEntities(UIManager.IsBuildingVisible, Building.Buildings);
        ToggleAllEntities(UIManager.IsCraftingVisible, Craftable.Craftables);
        ToggleAllEntities(UIManager.IsResearchVisible, Researchable.Researchables);

        LayoutRebuilder.ForceRebuildLayoutImmediate(MainPanel.GetComponent<RectTransform>());
    }

    private void ToggleAllEntities(bool isVisible, IDictionary<EntityType, GameEntity> entities)
    {
        if (!isVisible) return;

        foreach (var entity in entities.Values)
        {
            entity.MainPanel.SetActive(false);
            entity.Raycaster.enabled = false;
        }
        MainPanel.SetActive(true);
    }

    protected void OnCollapse()
    {
        MainPanel.SetActive(false);
    }

    public virtual void CheckIfPurchasable()
    {
        if (!IsUnlocked) return;

        var currentFill = GetCurrentFill();
        if (Math.Abs(currentFill - _lastFillAmount) > Mathf.Epsilon)
        {
            IsPurchasable = currentFill >= 1;
            UpdatePurchasableState();
            _lastFillAmount = currentFill;
        }
    }

    private void UpdatePurchasableState()
    {
        if (IsPurchasable && !_isPurchasableSet)
        {
            SetPurchasableState(true, DarkGrayColor);
        }
        else if (!IsPurchasable && _isPurchasableSet)
        {
            SetPurchasableState(false, RedColor);
        }
    }

    private void SetPurchasableState(bool interactable, UnityEngine.Color color)
    {
        _isPurchasableSet = interactable;
        var mainButton = MainPanel.GetComponent<Button>();
        mainButton.interactable = interactable;
        var headerText = MainPanel.GetComponentInChildren<TMP_Text>();
        headerText.color = color;
    }

    protected float GetCurrentFill()
    {
        if (ResourceCosts.Length == 0) return 0;

        var totalFill = 0f;
        foreach (var cost in ResourceCosts)
        {
            var fill = Mathf.Min(cost.CurrentAmount / cost.CostAmount, 1);
            totalFill += fill;
        }
        return totalFill / ResourceCosts.Length;
    }

    protected void UnlockEntity<T>(T entityType, IDictionary<T, GameEntity> entityDictionary) where T : Enum
    {
        foreach (var entity in entityDictionary.Values)
        {
            if (!entity.IsUnlocked) continue;

            entity.MainPanel.SetActive(true);
            entity.EntityCanvas.enabled = true;
            entity.Raycaster.enabled = true;
        }
    }

    protected void UpdateResourceCostTexts()
    {
        foreach (var cost in ResourceCosts)
        {
            cost.CurrentAmount = Resource.Resources[cost.AssociatedType].Amount;
            cost.UiForResourceCost.TextCostName.text = cost.AssociatedType.ToString();

            CalculateResourceCosts(
                cost.UiForResourceCost.TextCostAmount,
                cost.CurrentAmount,
                cost.CostAmount,
                Resource.Resources[cost.AssociatedType].AmountPerSecond,
                Resource.Resources[cost.AssociatedType].StorageAmount
            );
        }

        var currentFill = GetCurrentFill();
        if (Math.Abs(currentFill - _currentFillCache) > Mathf.Epsilon)
        {
            MainPanel.GetComponentInChildren<Image>().fillAmount = currentFill;
            _currentFillCache = currentFill;
        }
    }

    private void CalculateResourceCosts(TMP_Text text, float current, float cost, float rate, float storage)
    {
        string result;
        if (rate > 0 && cost > current)
        {
            float time = (cost - current) / rate;
            result = time > 0 ? $"{current}/{cost} ({time:F1}s)" : $"{current}/{cost} (Never)";
        }
        else
        {
            result = $"{current}/{cost}";
        }

        if (text.text != result)
        {
            text.text = result;
        }
    }
}
