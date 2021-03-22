using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct UiForResourceCost
{
    public TMP_Text CostNameText;
    public TMP_Text CostAmountText;
}

[System.Serializable]
public struct ResourceCost
{
    public ResourceType _AssociatedType;
    [System.NonSerialized] public float CurrentAmount;
    public float CostAmount;
    public UiForResourceCost _UiForResourceCost;
}

public enum BuildingType
{
    PotatoField,
    Woodlot,
    DigSite,
    MakeshiftBed
}

public abstract class Building : MonoBehaviour
{
    public static Dictionary<BuildingType, Building> Buildings = new Dictionary<BuildingType, Building>();
    public BuildingType Type;
    public float ResourceMultiplier, CostMultiplier;
    public ResourceType ResourceTypeToModify;
    public ResourceCost[] ResourceCost;
    public GameObject ObjSpacerBelow;
    [NonSerialized] public int IsUnlocked = 0;
    [NonSerialized] public GameObject ObjMainPanel;

    private string _selfCountString, _isUnlockedString;
    private string[] _costString;


    protected Transform TformTxtHeader, TformDescription, TformProgressbar;
    protected TMP_Text TxtHeader, TxtDescription;
    protected Image ImgProgressbar;
    protected string OriginalHeaderString;   
    protected uint SelfCount;
    protected float IncrementAmount;
    protected float Timer = 0.1f;
    protected readonly float MaxValue = 0.1f;

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isUnlockedString, IsUnlocked);
        PlayerPrefs.SetInt(_selfCountString, (int)SelfCount);

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            PlayerPrefs.SetFloat(_costString[i], ResourceCost[i].CostAmount);
        }
    }
    public void SetInitialValues()
    {
        InitializeObjects();

        

        if (TimeManager.hasPlayedBefore)
        {
            IsUnlocked = PlayerPrefs.GetInt(_isUnlockedString, IsUnlocked);
            SelfCount = (uint)PlayerPrefs.GetInt(_selfCountString, (int)SelfCount);

            for (int i = 0; i < ResourceCost.Length; i++)
            {
                ResourceCost[i].CostAmount = PlayerPrefs.GetFloat(_costString[i], ResourceCost[i].CostAmount);
            }
        }

        TxtHeader.text = string.Format("{0} ({1})", OriginalHeaderString, SelfCount);      
    }
    protected void CheckIfUnlocked()
    {
        if (IsUnlocked == 1)
        {
            ObjMainPanel.SetActive(true);
            ObjSpacerBelow.SetActive(true);

            //Debug.Log("Cost String: " + _costString);
            //Debug.Log("Type: " + Type.ToString());
            //Debug.Log("Resource Cost, Type: " + ResourceCost[0]._AssociatedType.ToString());
        }
        else
        {
            ObjMainPanel.SetActive(false);
            ObjSpacerBelow.SetActive(false);
        }
    }
    private void InitializeObjects()
    {
        TformDescription = transform.Find("Body/Description_Panel/Text_Description");
        TformTxtHeader = transform.Find("Header_Panel/Text_Header");
        TformProgressbar = transform.Find("Header_Panel/Progress_Circle_Panel/ProgressCircle");

        TxtHeader = TformTxtHeader.GetComponent<TMP_Text>();
        TxtDescription = TformDescription.GetComponent<TMP_Text>();
        ImgProgressbar = TformProgressbar.GetComponent<Image>();

        ObjMainPanel = gameObject;

        OriginalHeaderString = TxtHeader.text;
        
        _selfCountString = (Type.ToString() + "SelfCount");
        _isUnlockedString = (Type.ToString() + "IsUnlocked");
        _costString = new string[ResourceCost.Length];

        for (int i = 0; i < ResourceCost.Length; i++)
        {

            _costString[i] = Type.ToString() + ResourceCost[i]._AssociatedType.ToString();
            PlayerPrefs.GetFloat(_costString[i], ResourceCost[i].CostAmount);
        }
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
    public virtual void Build()
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
            SelfCount++;
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource._resources[Buildings[Type].ResourceCost[i]._AssociatedType].Amount -= ResourceCost[i].CostAmount;
                ResourceCost[i].CostAmount *= Mathf.Pow(CostMultiplier, SelfCount);
                ResourceCost[i]._UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[Buildings[Type].ResourceCost[i]._AssociatedType].Amount, ResourceCost[i].CostAmount);
            }
            IncrementAmount += ResourceMultiplier;
            Resource._resources[ResourceTypeToModify].AmountPerSecond += ResourceMultiplier;
        }

        TxtHeader.text = string.Format("{0} ({1})", OriginalHeaderString, SelfCount);   
    }
    public virtual void SetDescriptionText()
    {
        Buildings[Type].TxtDescription.text = string.Format("Increases {0} yield by: {1:0.00}", Resource._resources[ResourceTypeToModify].Type.ToString(), ResourceMultiplier);
    }  
}








