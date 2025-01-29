using System.Collections.Generic;
using UnityEngine;

public enum CraftingType
{
    //I could maybe have different types here such as tools and then like refined crafting? For now it'll just be the name of each crafting option
    WoodenHoe,
    WoodenAxe,
    WoodenPickaxe,
    Paper,
    WoodenSpear,
    StoneHoe,
    StoneAxe,
    StonePickaxe,
    StoneSpear,
    TinHoe,
    TinAxe,
    TinPickaxe,
    TinSpear,
    CopperHoe,
    CopperAxe,
    CopperPickaxe,
    CopperSpear,
    BronzeHoe,
    BronzeAxe,
    BronzePickaxe,
    BronzeSpear,
    IronAxe,
    IronHoe,
    IronPickaxe,
    IronSpear,
    None
}

public class Craftable : GameEntity
{
    public static Dictionary<CraftingType, Craftable> Craftables = new Dictionary<CraftingType, Craftable>();
    public static bool isCraftableUnlockedEvent;

    public CraftingType Type;
    [System.NonSerialized] public bool isCrafted;

    private string _isCraftedString, _IsUnlockedString;

    // Reset variables

    public float permCostSubtraction, permAllCostSubtraction;

    public float prestigeCostSubtraction, prestigeAllCostSubtraction;

    private string _strPermCostSubtraction, _strPermAllCostSubtraction, _strPrestigeCostSubtraction, _strPrestigeAllCostSubtraction;
    private GameObject _objCrafted;
    private Transform _tformObjCrafted;

    public uint trackedCraftedAmount;

    public void ResetCraftable()
    {
        IsUnlocked = false;
        _objCrafted.SetActive(false);
        //ObjMainPanel.SetActive(false);
        Canvas.enabled = false;
        GraphicRaycaster.enabled = false;
        UnlockAmount = 0;
        IsUnlocked = false;
        IsUnlockedByResource = false;
        isCrafted = false;
        HasSeen = true;
        ModifyCost();
        MakeCraftableAgain();
        ObjProgressCircle.SetActive(true);
        ObjBackground.SetActive(true);
    }
    public void ModifyCost()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            ResourceCost[i].CostAmount = ResourceCost[i].BaseCostAmount;
            float subtractionAmount = ResourceCost[i].BaseCostAmount * ((prestigeAllCostSubtraction + permAllCostSubtraction) + (permCostSubtraction + prestigeCostSubtraction));
            prestigeAllCostSubtraction = 0;
            prestigeCostSubtraction = 0;
            ResourceCost[i].CostAmount -= subtractionAmount;
            Debug.Log(string.Format("Changed craft {0}'s cost from {1} to {2}", ActualName, ResourceCost[i].BaseCostAmount, ResourceCost[i].CostAmount));
        }
    }
    protected void SetInitialValues()
    {
        InitializeObjects();

        TxtHeader.text = string.Format("{0}", ActualName);

        //if (TimeManager.hasPlayedBefore)
        //{
        IsUnlocked = PlayerPrefs.GetInt(_IsUnlockedString) == 1 ? true : false;
        isCrafted = PlayerPrefs.GetInt(_isCraftedString) == 1 ? true : false;

        FetchPrestigeValues();
        //}
        // Not sure why this was here? No need to set it to false since it should already be false. especially if you haven't played already.
        //else
        //{
        //    isCrafted = false;
        //}
        if (IsUnlocked)
        {
            ObjMainPanel.SetActive(true);
            Canvas.enabled = false;
            GraphicRaycaster.enabled = false;
        }
        else
        {
            ObjMainPanel.SetActive(false);
            Canvas.enabled = false;
            GraphicRaycaster.enabled = false;
        }

        if (isCrafted)
        {
            _objCrafted.SetActive(true);
            BtnMain.interactable = false;
            ObjProgressCircle.SetActive(false);
            ObjBackground.SetActive(false);

            if (Menu.isCraftingHidden)
            {
                ObjMainPanel.SetActive(false);
            }
        }
        // This is probably also not needed, but it might be.
        //else
        //{
        //    MakeCraftableAgain();
        //}
    }
    public virtual void OnCraft()
    {
        bool canPurchase = true;

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (ResourceCost[i].CurrentAmount < ResourceCost[i].CostAmount)
            {
                canPurchase = false;
                break;
            }
        }

        if (canPurchase)
        {
            trackedCraftedAmount++;
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
                Resource.Resources[ResourceCost[i].AssociatedType].uiForResource.txtAmount.text = string.Format("{0:0.00}", NumberToLetter.FormatNumber(Resource.Resources[ResourceCost[i].AssociatedType].amount));
            }

            isCrafted = true;
            Crafted();
        }
    }
    protected void Crafted()
    {
        _objCrafted.SetActive(true);
        BtnMain.interactable = false;
        ObjProgressCircle.SetActive(false);
        ObjBackground.SetActive(false);

        //TxtHeader.text = string.Format("{0} (Crafted)", ActualName);

        //string htmlValue = "#D4D4D4";

        //if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
        //{
        //    ImgExpand.color = greyColor;
        //    ImgCollapse.color = greyColor;
        //}

        if (Menu.isCraftingHidden)
        {
            ObjMainPanel.SetActive(false);
        }

        UnlockCrafting();
        UnlockBuilding();
        UnlockResearchable();
        UnlockWorkerJob();
        UnlockResource();
    }
    protected void TestingNewCrafted()
    {
        BtnMain.interactable = false;
        ObjProgressCirclePanel.SetActive(false);
        TxtHeader.text = string.Format("{0} (Crafted)", ActualName);

        string htmlValue = "#D4D4D4";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
        {
            ImgExpand.color = greyColor;
            ImgCollapse.color = greyColor;
        }

        if (Menu.isCraftingHidden)
        {
            ObjMainPanel.SetActive(false);
        }

        UnlockCrafting();
        UnlockBuilding();
        UnlockResearchable();
        UnlockWorkerJob();
        UnlockResource();
    }
    public void MakeCraftableAgain()
    {
        // I don't think this is really needed, not until the player prestige at least.
        BtnMain.interactable = true;
        ObjProgressCirclePanel.SetActive(true);
        TxtHeader.text = string.Format("{0}", ActualName);

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            ImgExpand.color = darkGreyColor;
            ImgCollapse.color = darkGreyColor;
        }
    }
    protected override void InitializeObjects()
    {
        base.InitializeObjects();

        _isCraftedString = Type.ToString() + "isCrafted";
        _IsUnlockedString = Type.ToString() + "IsUnlocked";

        AssignPrestigeStrings();

        BtnMain.onClick.AddListener(OnCraft);

        _tformObjCrafted = transform.Find("Panel_Main/Header_Panel/Image_Crafted");
        _objCrafted = _tformObjCrafted.gameObject;
        _objCrafted.SetActive(false);
    }
    protected void SetDescriptionText(string description)
    {
        TxtDescription.text = string.Format("{0}", description);
    }
    protected void UnlockedViaResource()
    {
        if (IsUnlocked)
        {
            if (UIManager.isBuildingVisible && HasSeen)
            {
                isCraftableUnlockedEvent = true;
                HasSeen = false;
                PointerNotification.rightAmount++;
            }
            else if (UIManager.isCraftingVisible)
            {
                // This does run more than once each, but isn't a big deal
                ObjMainPanel.SetActive(true);
                Canvas.enabled = true;
                GraphicRaycaster.enabled = true;
                HasSeen = true;
            }
            else if (UIManager.isWorkerVisible && HasSeen)
            {
                isCraftableUnlockedEvent = true;
                HasSeen = false;
                PointerNotification.leftAmount++;
            }
            else if (UIManager.isResearchVisible && HasSeen)
            {
                isCraftableUnlockedEvent = true;
                HasSeen = false;
                PointerNotification.leftAmount++;
            }
        }
    }
    private void CheckIfUnlocked()
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

                    UnlockedViaResource();

                    PointerNotification.HandleRightAnim();
                    PointerNotification.HandleLeftAnim();
                }
            }
        }
    }
    private void AssignPrestigeStrings()
    {
        _strPermAllCostSubtraction = Type.ToString() + "permAllCostSubtraction";
        _strPermCostSubtraction = Type.ToString() + "permCostSubtraction";

        _strPrestigeAllCostSubtraction = Type.ToString() + "prestigeAllCostSubtraction";
        _strPrestigeCostSubtraction = Type.ToString() + "prestigeCostSubtraction";
    }
    private void SavePrestigeValues()
    {
        PlayerPrefs.SetFloat(_strPermAllCostSubtraction, permAllCostSubtraction);
        PlayerPrefs.SetFloat(_strPermCostSubtraction, permCostSubtraction);

        PlayerPrefs.SetFloat(_strPrestigeAllCostSubtraction, prestigeAllCostSubtraction);
        PlayerPrefs.SetFloat(_strPrestigeCostSubtraction, prestigeCostSubtraction);
    }
    private void FetchPrestigeValues()
    {
        permAllCostSubtraction = PlayerPrefs.GetFloat(_strPermAllCostSubtraction, permAllCostSubtraction);
        permCostSubtraction = PlayerPrefs.GetFloat(_strPermCostSubtraction, permCostSubtraction);

        prestigeAllCostSubtraction = PlayerPrefs.GetFloat(_strPrestigeAllCostSubtraction, prestigeAllCostSubtraction);
        prestigeCostSubtraction = PlayerPrefs.GetFloat(_strPrestigeCostSubtraction, prestigeCostSubtraction);
    }
    protected void Update()
    {
        if ((timer -= Time.deltaTime) <= 0)
        {
            timer = maxValue;
            if (!isCrafted)
            {
                CheckIfPurchaseable();
            }

            UpdateResourceCostTexts();
            CheckIfUnlocked();
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_IsUnlockedString, IsUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(_isCraftedString, isCrafted ? 1 : 0);

        SavePrestigeValues();
    }
}
