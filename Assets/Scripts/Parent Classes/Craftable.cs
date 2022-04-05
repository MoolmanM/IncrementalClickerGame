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
    FireHardenedSpear,
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

public class Craftable : SuperClass
{
    public static Dictionary<CraftingType, Craftable> Craftables = new Dictionary<CraftingType, Craftable>();
    public static bool isCraftableUnlockedEvent;

    public CraftingType Type;
    [System.NonSerialized] public bool isCrafted;

    private string _isCraftedString, _isUnlockedString;
    private GameObject _objCheckmark;
    private Transform _tformObjCheckmark;

    public void ResetCraftable()
    {
        isUnlocked = false;
        objMainPanel.SetActive(false);
        unlockAmount = 0;
        isUnlockedByResource = false;
        isCrafted = false;
        hasSeen = true;
        MakeCraftableAgain();
    }
    protected void SetInitialValues()
    {
        InitializeObjects();

        _txtHeader.text = string.Format("{0}", actualName);

        //if (TimeManager.hasPlayedBefore)
        //{
            isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;
            isCrafted = PlayerPrefs.GetInt(_isCraftedString) == 1 ? true : false;
        //}
        // Not sure why this was here? No need to set it to false since it should already be false. especially if you haven't played already.
        //else
        //{
        //    isCrafted = false;
        //}
        if (isUnlocked)
        {
            objMainPanel.SetActive(true);
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
        }
        else
        {
            objMainPanel.SetActive(false);
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
        }

        if (isCrafted)
        {
            _objCheckmark.SetActive(true);
            _btnMain.interactable = false;
            _objProgressCircle.SetActive(false);
            _objBackground.SetActive(false);

            if (Menu.isCraftingHidden)
            {
                objMainPanel.SetActive(false);
            }
        }
        // This is probably also not needed, but it might be.
        //else
        //{
        //    MakeCraftableAgain();
        //}
    }
    protected virtual void OnCraft()
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
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            }

            isCrafted = true;
            Crafted();
        }
    }
    protected void Crafted()
    {
        _objCheckmark.SetActive(true);
        _btnMain.interactable = false;
        _objProgressCircle.SetActive(false);
        _objBackground.SetActive(false);

        //_txtHeader.text = string.Format("{0} (Crafted)", actualName);

        //string htmlValue = "#D4D4D4";

        //if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
        //{
        //    _imgExpand.color = greyColor;
        //    _imgCollapse.color = greyColor;
        //}

        if (Menu.isCraftingHidden)
        {
            objMainPanel.SetActive(false);
        }

        UnlockCrafting();
        UnlockBuilding();
        UnlockResearchable();
        UnlockWorkerJob();
        UnlockResource();
    }
    protected void TestingNewCrafted()
    {
        _btnMain.interactable = false;
        _objProgressCirclePanel.SetActive(false);
        _txtHeader.text = string.Format("{0} (Crafted)", actualName);

        string htmlValue = "#D4D4D4";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
        {
            _imgExpand.color = greyColor;
            _imgCollapse.color = greyColor;
        }

        if (Menu.isCraftingHidden)
        {
            objMainPanel.SetActive(false);
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
        _btnMain.interactable = true;
        _objProgressCirclePanel.SetActive(true);
        _txtHeader.text = string.Format("{0}", actualName);

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            _imgExpand.color = darkGreyColor;
            _imgCollapse.color = darkGreyColor;
        }
    }
    protected override void InitializeObjects()
    {
        base.InitializeObjects();

        _isCraftedString = Type.ToString() + "isCrafted";
        _isUnlockedString = (Type.ToString() + "isUnlocked");

        _btnMain.onClick.AddListener(OnCraft);

        _tformObjCheckmark = transform.Find("Panel_Main/Header_Panel/Progress_Circle_Panel/Checkmark");
        _objCheckmark = _tformObjCheckmark.gameObject;
        _objCheckmark.SetActive(false);
    }
    protected void SetDescriptionText(string description)
    {
        Craftables[Type]._txtDescription.text = string.Format("{0}", description);
    }
    protected void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = _maxValue;
            if (!isCrafted)
            {
                CheckIfPurchaseable();
            }

            UpdateResourceCosts();
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(_isCraftedString, isCrafted ? 1 : 0);
    }
}
