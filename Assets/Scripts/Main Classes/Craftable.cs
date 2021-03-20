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
    WoodenPickaxe
}

public abstract class Craftable : MonoBehaviour
{
    public Dictionary<CraftingType, Craftable> Craftables = new Dictionary<CraftingType, Craftable>();

    public CraftingType Type;
    public ResourceCost[] ResourceCost;
    public BuildingType[] BuildingTypesToModify;
    public ResourceType[] ResourceTypesToModify;
    public WorkerType[] WorkerTypesToModify;
    public GameObject ObjSpacerBelow;
    [System.NonSerialized] public int IsCrafted = 0;

    private string _isCraftedString;

    protected float Timer = 0.1f;
    protected readonly float MaxValue = 0.1f;
    protected TMP_Text TxtDescription;
    protected Transform TformDescription, TformTxtHeader, TformBtnMain, TformProgressbar, TformProgressbarPanel, TformTxtHeaderUncraft, TformExpand, TformCollapse;      
    protected Image ImgProgressbar, ImgMain, ImgExpand, ImgCollapse;
    protected GameObject ObjProgressbarPanel, ObjBtnMain, ObjTxtHeader, ObjTxtHeaderUncraft;

    public void SetInitialValues()
    {
        InitializeObjects();
   
        if (TimeManager.hasPlayedBefore)
        {
            IsCrafted = PlayerPrefs.GetInt(_isCraftedString, IsCrafted);
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
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isCraftedString, IsCrafted);
    }
    public virtual void UpdateResourceCosts()
    {
        if ((Timer -= Time.deltaTime) <= 0)
        {
            Timer = MaxValue;

            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Craftables[Type].ResourceCost[i].CurrentAmount = Resource._resources[Craftables[Type].ResourceCost[i]._AssociatedType].Amount;
                Craftables[Type].ResourceCost[i]._UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Craftables[Type].ResourceCost[i].CurrentAmount, Craftables[Type].ResourceCost[i].CostAmount);
                Craftables[Type].ResourceCost[i]._UiForResourceCost.CostNameText.text = string.Format("{0}", Craftables[Type].ResourceCost[i]._AssociatedType.ToString());              
            }
            GetCurrentFill();
        }
    }

    public  void Craft()
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
            IsCrafted = 1;
            Crafted();
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource._resources[Craftables[Type].ResourceCost[i]._AssociatedType].Amount -= ResourceCost[i].CostAmount;
            }

        }
    }

    protected void Crafted()
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

        foreach (var building in BuildingTypesToModify)
        {
            Building.Buildings[building].IsUnlocked = 1;
            Building.Buildings[building].ObjMainPanel.SetActive(true);
            Building.Buildings[building].ObjSpacerBelow.SetActive(true);
        }    
        foreach (var resource in ResourceTypesToModify)
        {
            Resource._resources[resource].IsUnlocked = 1;
        }
        foreach (var worker in WorkerTypesToModify)
        {
            Worker.Workers[worker].IsUnlocked = 1;
            Worker.Workers[worker].objMainPanel.SetActive(true);
            Worker.Workers[worker].objSpacerBelow.SetActive(true);
        }
    }
    private void InitializeObjects()
    {
        TformDescription = transform.Find("Body/Description_Panel/Text_Description");
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

    public void GetCurrentFill()
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
        fillAmount /= ResourceCost.Length;
        ImgProgressbar.fillAmount = fillAmount;
    }

    public void SetDescriptionText(string description)
    {
        Craftables[Type].TxtDescription.text = string.Format("{0}", description);
    }
}
