using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public enum ResearchType
{
    // Housing should maybe be after you have the 'Log' resource.
    // Hamster Wheel
    // Fire?
    // Grinding stone/Sharpening of tools (Increases production of everything that uses tools, or maybe just increase worker values.
    // Cooking of food.
    // Clothing, need to research first and then craft perhaps, this might go for a lot of things.
    // Power Lines, enables buildings to receive power.
    // Maybe have knowledge points as the currency to research things.
    // Maybe if you get x amount of workers. Make an event happen where you get someone who is quite smart. and this person gives you an instant 50 knowledge points. Which is just enouggh
    // to research paper, then after researching paper you can have students that study papers. 
    // And with that you'll gain knowledge.

    Paper,
    StoneTools,
    // Smelting
}

public abstract class Researchable : MonoBehaviour
{
    public Dictionary<ResearchType, Researchable> Researchables = new Dictionary<ResearchType, Researchable>();

    public ResearchType Type;
    public ResourceCost[] ResourceCost;
    public BuildingType[] BuildingTypesToModify;
    public ResourceType[] ResourceTypesToModify;
    public GameObject ObjSpacerBelow;
    [System.NonSerialized] public int IsResearched = 0;

    private string _IsResearchedString;

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
            IsResearched = PlayerPrefs.GetInt(_IsResearchedString, IsResearched);
        }


        if (IsResearched == 1)
        {
            Researched();
        }
        else
        {
            MakeResearchableAgain();
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_IsResearchedString, IsResearched);
    }
    public virtual void UpdateResourceCosts()
    {
        if ((Timer -= Time.deltaTime) <= 0)
        {
            Timer = MaxValue;

            for (int i = 0; i < ResourceCost.Length; i++)
            {
                ResourceCost[i].CurrentAmount = Resource._resources[ResourceCost[i]._AssociatedType].Amount;
                ResourceCost[i]._UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", ResourceCost[i].CurrentAmount, ResourceCost[i].CostAmount);
                ResourceCost[i]._UiForResourceCost.CostNameText.text = string.Format("{0}", ResourceCost[i]._AssociatedType.ToString());
            }
            GetCurrentFill();
        }
    }

    public void OnResearch()
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
            IsResearched = 1;
            Researched();
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource._resources[ResourceCost[i]._AssociatedType].Amount -= ResourceCost[i].CostAmount;
            }

        }
    }

    protected virtual void Researched()
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

        _IsResearchedString = Type.ToString() + "IsCrafted";
    }
    private void MakeResearchableAgain()
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
        TxtDescription.text = string.Format("{0}", description);
    }
}

