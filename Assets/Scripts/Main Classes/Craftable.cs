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
    StoneHoe,
    StoneAxe,
    StonePickaxe
}

public abstract class Craftable : MonoBehaviour
{
    public static Dictionary<CraftingType, Craftable> Craftables = new Dictionary<CraftingType, Craftable>();

    public CraftingType Type;
    public ResourceCost[] resourceCost;
    public GameObject ObjSpacerBelow;
    [System.NonSerialized] public int IsCrafted = 0;
    [System.NonSerialized] public int IsUnlocked = 0;
    [System.NonSerialized] public GameObject ObjMainPanel;
    public float averageAmount;

    private string _isCraftedString;

    protected BuildingType[] BuildingTypesToModify;
    protected ResourceType[] ResourceTypesToModify, resourcesRequiredForUnlocking;
    protected WorkerType[] WorkerTypesToModify;
    protected float Timer = 0.1f;
    protected readonly float MaxValue = 0.1f;
    protected TMP_Text TxtDescription;
    protected Transform TformDescription, TformTxtHeader, TformBtnMain, TformProgressbar, TformProgressbarPanel, TformTxtHeaderUncraft, TformExpand, TformCollapse;      
    protected Image ImgProgressbar, ImgMain, ImgExpand, ImgCollapse;
    protected GameObject ObjProgressbarPanel, ObjBtnMain, ObjTxtHeader, ObjTxtHeaderUncraft;

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isCraftedString, IsCrafted);
    }
    public void SetInitialValues()
    {
        InitializeObjects();
   
        if (TimeManager.hasPlayedBefore)
        {
            IsCrafted = PlayerPrefs.GetInt(_isCraftedString, IsCrafted);
        }
        else
        {
            IsCrafted = 0;
        }
        

        if (IsCrafted == 1)
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
        if (UIManagerV2.isCraftingPanelAtive == true)
        {
            if (IsUnlocked == 0)
            {
                ObjMainPanel.GetComponent<CanvasGroup>().alpha = 0;
                ObjMainPanel.GetComponent<CanvasGroup>().interactable = false;
                ObjMainPanel.GetComponent<CanvasGroup>().ignoreParentGroups = false;
                ObjSpacerBelow.SetActive(false);
                return;
            }
            else
            {
                ObjMainPanel.GetComponent<CanvasGroup>().alpha = 1;
                ObjMainPanel.GetComponent<CanvasGroup>().interactable = true;
                ObjMainPanel.GetComponent<CanvasGroup>().ignoreParentGroups = true;
                ObjSpacerBelow.SetActive(true);
            }
        }
        else
        {
            ObjMainPanel.GetComponent<CanvasGroup>().alpha = 0;
            ObjMainPanel.GetComponent<CanvasGroup>().interactable = false;
            ObjMainPanel.GetComponent<CanvasGroup>().ignoreParentGroups = false;
            ObjSpacerBelow.SetActive(false);
        }
    }
    public virtual void UpdateResourceCosts()
    {
        if ((Timer -= Time.deltaTime) <= 0)
        {
            Timer = MaxValue;

            for (int i = 0; i < resourceCost.Length; i++)
            {
                Craftables[Type].resourceCost[i].CurrentAmount = Resource._resources[Craftables[Type].resourceCost[i]._AssociatedType].Amount;
                Craftables[Type].resourceCost[i]._UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Craftables[Type].resourceCost[i].CurrentAmount, Craftables[Type].resourceCost[i].CostAmount);
                Craftables[Type].resourceCost[i]._UiForResourceCost.CostNameText.text = string.Format("{0}", Craftables[Type].resourceCost[i]._AssociatedType.ToString());              
            }
            ImgProgressbar.fillAmount =  GetCurrentFill();
            if (GetCurrentFill() >= 0.8f)
            {
                IsUnlocked = 1;
            }
            CheckIfUnlocked();
        }
    }
    public void OnCraft()
    {
        bool canPurchase = true;

        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resourceCost[i].CurrentAmount < resourceCost[i].CostAmount)
            {
                canPurchase = false;
                break;
            }
        }

        if (canPurchase)
        {
            IsCrafted = 1;
            Crafted();
            UnlockBuilding();
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource._resources[resourceCost[i]._AssociatedType].Amount -= resourceCost[i].CostAmount;
            }

        }
    }
    protected virtual void Crafted()
    {
        ObjBtnMain.GetComponent<Button>().interactable = false;
        ObjProgressbarPanel.SetActive(false);
        ObjTxtHeader.SetActive(false);
        ObjTxtHeaderUncraft.SetActive(true);

        string htmlValue = "#D4D4D4";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color greyColor))
        {
            ImgExpand.color = greyColor;
            ImgCollapse.color = greyColor;
        }       
    }
    protected virtual void UnlockBuilding()
    {
        foreach (var building in BuildingTypesToModify)
        {
            Building.Buildings[building].IsUnlocked = 1;
            Building.Buildings[building].ObjMainPanel.SetActive(true);
            Building.Buildings[building].ObjSpacerBelow.SetActive(true);
        }
    }
    private void InitializeObjects()
    {
        TformDescription = transform.Find("Body/Text_Description");
        TformTxtHeader = transform.Find("Header_Panel/Text_Header");
        TformBtnMain = transform.Find("Header_Panel/Button_Main");
        TformProgressbar = transform.Find("Header_Panel/Progress_Circle_Panel/ProgressCircle");
        TformProgressbarPanel = transform.Find("Header_Panel/Progress_Circle_Panel");
        TformTxtHeaderUncraft = transform.Find("Header_Panel/Text_Header_Uncraftable");
        TformExpand = transform.Find("Header_Panel/Button_Expand");
        TformCollapse = transform.Find("Header_Panel/Button_Collapse");

        TxtDescription = TformDescription.GetComponent<TMP_Text>();
        ObjTxtHeader = TformTxtHeader.gameObject;
        ObjBtnMain = TformBtnMain.gameObject;
        ImgProgressbar = TformProgressbar.GetComponent<Image>();
        ObjProgressbarPanel = TformProgressbarPanel.gameObject;
        ObjTxtHeaderUncraft = TformTxtHeaderUncraft.gameObject;
        ImgExpand = TformExpand.GetComponent<Image>();
        ImgCollapse = TformCollapse.GetComponent<Image>();

        ObjMainPanel = gameObject;

        _isCraftedString = Type.ToString() + "IsCrafted";

        
    }
    private void MakeCraftableAgain()
    {
        ObjBtnMain.GetComponent<Button>().interactable = true;
        ObjProgressbarPanel.SetActive(true);
        ObjTxtHeader.SetActive(true);
        ObjTxtHeaderUncraft.SetActive(false);

        string htmlValue = "#333333";

        if (ColorUtility.TryParseHtmlString(htmlValue, out Color darkGreyColor))
        {
            ImgExpand.color = darkGreyColor;
            ImgCollapse.color = darkGreyColor;
        }
    }
    public float GetCurrentFill()
    {
        float add = 0;
        float div = 0;
        float fillAmount = 0;

        for (int i = 0; i < resourceCost.Length; i++)
        {
            add = resourceCost[i].CurrentAmount;
            div = resourceCost[i].CostAmount;
            if (add > div)
            {
                add = div;
            }
            fillAmount += add / div;
        }
        return fillAmount / resourceCost.Length;
    }
    public void SetDescriptionText(string description)
    {
        Craftables[Type].TxtDescription.text = string.Format("{0}", description);
    }
}
