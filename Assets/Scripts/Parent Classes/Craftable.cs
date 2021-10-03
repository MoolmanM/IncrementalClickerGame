using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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


}

public class Craftable : SuperClass
{
    public static Dictionary<CraftingType, Craftable> Craftables = new Dictionary<CraftingType, Craftable>();
    public static bool isCraftableUnlockedEvent;

    public CraftingType Type;
    [System.NonSerialized] public bool isCrafted;

    private string _isCraftedString, _isUnlockedString;

    public void ResetCraftable()
    {
        isUnlocked = false;
        objMainPanel.SetActive(false);
        objSpacerBelow.SetActive(false);
        unlockAmount = 0;
        isUnlockedByResource = false;
        isCrafted = false;
        hasSeen = true;
        MakeCraftableAgain();
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
            
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            }

        }
    }
    protected void Crafted()
    {
        _objBtnMain.GetComponent<Button>().interactable = false;
        _objProgressCircle.SetActive(false);
        _objTxtHeader.SetActive(false);
        _objTxtHeaderDone.SetActive(true);

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

        UnlockCrafting();
        UnlockBuilding();
        UnlockResearchable();
        UnlockWorkerJob();
        UnlockResource();
    }
    public void MakeCraftableAgain()
    {
        // I don't think this is really needed, not until the player prestige at least.
        _objBtnMain.GetComponent<Button>().interactable = true;
        _objProgressCircle.SetActive(true);
        _objTxtHeader.SetActive(true);
        _objTxtHeaderDone.SetActive(false);

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

        _objTxtHeaderDone = _tformTxtHeaderDone.gameObject;

        _objBtnMain.GetComponent<Button>().onClick.AddListener(OnCraft);
    }
    protected void SetDescriptionText(string description)
    {
        Craftables[Type]._txtDescription.text = string.Format("{0}", description);
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(_isCraftedString, isCrafted ? 1 : 0);
    }
}
