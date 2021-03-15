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
    public BuildingType BuildingTypeToModify;
    public ResourceType ResourceTypeToModify;
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

        if (TimeManager.hasPlayedBefore)
        {
            IsCrafted = PlayerPrefs.GetInt(_isCraftedString, IsCrafted);
        }
        

        if (IsCrafted == 1)
        {
            FinishedCrafting();
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

    protected virtual void Craft()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (!Craftables.TryGetValue(Type, out Craftable associatedResource) || associatedResource.ResourceCost[i].CurrentAmount < associatedResource.ResourceCost[i].CostAmount)
            {
                return;
            }

            Resource._resources[Craftables[Type].ResourceCost[i]._AssociatedType].Amount -= associatedResource.ResourceCost[i].CostAmount;

            Craftables[Type] = associatedResource;

            IsCrafted = 1;
            FinishedCrafting();
            
        }

    }

    protected void FinishedCrafting()
    {
        ObjBtnMain.GetComponent<Button>().interactable = false;
        ObjProgressbarPanel.SetActive(false);
        ObjTxtHeader.SetActive(false);
        Color greyColor = new Color(0xD4, 0xD4, 0xD4, 0xFF);
        ImgExpand.color = greyColor;
        ImgCollapse.color = greyColor;
        Building.Buildings[BuildingTypeToModify].IsUnlocked = 1;
        //Building.Buildings[BuildingTypeToModify].MainBuildingPanel.SetActive(true);
        //Building.Buildings[BuildingTypeToModify].ObjSpacerBelow.SetActive(true);
        Resource._resources[ResourceTypeToModify].IsUnlocked = 1;
        //Resource._resources[ResourceTypeToModify].MainResourcePanel.SetActive(true);
        //Resource._resources[ResourceTypeToModify].spacerBelow.SetActive(true);
    }

    private void MakeCraftableAgain()
    {
        ObjBtnMain.GetComponent<Button>().interactable = true;
        ObjProgressbarPanel.SetActive(true);
        //uncraftableTextObject.SetActive(false);
        ObjTxtHeader.SetActive(true);
        Color darkDreyColor = new Color(0x33, 0x33, 0x33, 0xFF);
        ImgExpand.color = darkDreyColor;
        ImgCollapse.color = darkDreyColor;
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
