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
    public BuildingType _Type;
    public float ResourceMultiplier, CostMultiplier;
    public ResourceType ResourceTypeToModify;
    public ResourceCost[] ResourceCost;
    public GameObject ObjSpacerBelow;
    [NonSerialized] public int IsUnlocked = 0;
    [NonSerialized] public GameObject ObjMainPanel;

    private string _selfCountString, _isUnlockedString;
  
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
    }
    public void SetInitialValues()
    {
        TformDescription = transform.Find("Body/Description_Panel/Text_Description");
        TformTxtHeader = transform.Find("Header_Panel/Text_Header");
        TformProgressbar = transform.Find("Header_Panel/Progress_Circle_Panel/ProgressCircle");

        TxtHeader = TformTxtHeader.GetComponent<TMP_Text>();
        TxtDescription = TformDescription.GetComponent<TMP_Text>();
        ImgProgressbar = TformProgressbar.GetComponent<Image>();

        ObjMainPanel = gameObject;

        OriginalHeaderString = TxtHeader.text;

        _selfCountString = (_Type.ToString() + "SC");
        _isUnlockedString = (_Type.ToString() + "Unlocked");

        if (TimeManager.hasPlayedBefore)
        {
            IsUnlocked = PlayerPrefs.GetInt(_isUnlockedString, IsUnlocked);
            SelfCount = (uint)PlayerPrefs.GetInt(_selfCountString, (int)SelfCount);
        }
        
        TxtHeader.text = string.Format("{0} ({1})", OriginalHeaderString, SelfCount);

        if (IsUnlocked == 1)
        {
            ObjMainPanel.SetActive(true);
            ObjSpacerBelow.SetActive(true);
        }
        else
        {
            ObjMainPanel.SetActive(false);
            ObjSpacerBelow.SetActive(false);
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
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (!Buildings.TryGetValue(_Type, out Building associatedResource) || associatedResource.ResourceCost[i].CurrentAmount < associatedResource.ResourceCost[i].CostAmount)
            {
                return;
            }

            SelfCount++;
            Resource._resources[Buildings[_Type].ResourceCost[i]._AssociatedType].Amount -= associatedResource.ResourceCost[i].CostAmount;
            associatedResource.ResourceCost[i].CostAmount *= Mathf.Pow(CostMultiplier, SelfCount);
            associatedResource.ResourceCost[i]._UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[Buildings[_Type].ResourceCost[i]._AssociatedType].Amount, associatedResource.ResourceCost[i].CostAmount);          
            Buildings[_Type] = associatedResource;

            //This seems to work but not sure for how long
            IncrementAmount += ResourceMultiplier;
            Resource._resources[ResourceTypeToModify].AmountPerSecond += ResourceMultiplier;
        }
        Buildings[_Type].TxtHeader.text = string.Format("{0} ({1})", OriginalHeaderString, SelfCount);      
    }
    public virtual void SetDescriptionText()
    {
        Buildings[_Type].TxtDescription.text = string.Format("Increases {0} yield by: {1:0.00}", Resource._resources[ResourceTypeToModify]._Type.ToString(), ResourceMultiplier);
    }  
}








