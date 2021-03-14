using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct UiForResourceCost
{
    public TMP_Text costNameText;
    public TMP_Text costAmountText;
}

[System.Serializable]
public struct ResourceCost
{
    public ResourceType associatedType;
    [System.NonSerialized] public float currentAmount;
    public float costAmount;
    public UiForResourceCost UiForResourceCost;

    public float CurrentAmount { get; }
    public float CostAmount { get; }
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
    public static Dictionary<BuildingType, Building> _buildings = new Dictionary<BuildingType, Building>();
    public BuildingType Type;
    public float ResourceMultiplier, CostMultiplier;
    public ResourceType ResourceTypeToModify;
    public ResourceCost[] ResourceCost;
    public GameObject SpacerAbove;

    protected uint SelfCount;    
    protected float IncrementAmount;
    [NonSerialized] public GameObject MainBuildingPanel;
    public int IsUnlocked = 0;
    protected Transform HeaderTransform, DesriptionTransform, progressCircleTransform;
    protected TMP_Text HeaderText, DescriptionText;   
    protected string HeaderString;
    protected Image progressCircle;
    private string _selfCountString, _isUnlockedString;

    protected float _timer = 0.1f;
    protected readonly float maxValue = 0.1f;

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isUnlockedString, IsUnlocked);
        PlayerPrefs.SetInt(_selfCountString, (int)SelfCount);
    }

    public void SetInitialValues()
    {
        _selfCountString = (Type.ToString() + "SC");
        _isUnlockedString = (Type.ToString() + "Unlocked");

        //IncrementAmount = SelfCount * ResourceMultiplier;
        //Resource._resources[ResourceTypeToModify].AmountPerSecond += IncrementAmount;
        MainBuildingPanel = this.gameObject;
        HeaderTransform = transform.Find("Header_Panel/Header_Text");
        HeaderText = HeaderTransform.GetComponent<TMP_Text>();
        HeaderString = HeaderText.text;
        DesriptionTransform = transform.Find("Body/Description_Panel/Description_Text");
        DescriptionText = DesriptionTransform.GetComponent<TMP_Text>();
        progressCircleTransform = transform.Find("Header_Panel/Progress_Circle_Panel/ProgressCircle");
        progressCircle = progressCircleTransform.GetComponent<Image>();
        

        IsUnlocked = PlayerPrefs.GetInt(_isUnlockedString, IsUnlocked);
        SelfCount = (uint)PlayerPrefs.GetInt(_selfCountString, (int)SelfCount);
        HeaderText.text = string.Format("{0} ({1})", HeaderString, SelfCount);

        if (IsUnlocked == 1)
        {
            MainBuildingPanel.SetActive(true);
            SpacerAbove.SetActive(true);
        }
        else
        {
            MainBuildingPanel.SetActive(false);
            SpacerAbove.SetActive(false);
        }
    }
 
    public virtual void UpdateResourceCosts()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            for (int i = 0; i < ResourceCost.Length; i++)
            {
                ResourceCost[i].currentAmount = Resource._resources[ResourceCost[i].associatedType].Amount;
                ResourceCost[i].UiForResourceCost.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", ResourceCost[i].currentAmount, ResourceCost[i].costAmount);
                ResourceCost[i].UiForResourceCost.costNameText.text = string.Format("{0}", ResourceCost[i].associatedType.ToString());              
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
            add = ResourceCost[i].currentAmount;
            div = ResourceCost[i].costAmount;
            if (add > div)
            {
                add = div;
            }
            fillAmount += add / div;
        }
        fillAmount /= ResourceCost.Length;
        progressCircle.fillAmount = fillAmount;
    }

    public virtual void Build()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (!_buildings.TryGetValue(Type, out Building associatedResource) || associatedResource.ResourceCost[i].currentAmount < associatedResource.ResourceCost[i].costAmount)
            {
                return;
            }

            SelfCount++;
            Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount -= associatedResource.ResourceCost[i].costAmount;
            associatedResource.ResourceCost[i].costAmount *= Mathf.Pow(CostMultiplier, SelfCount);
            associatedResource.ResourceCost[i].UiForResourceCost.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource._resources[_buildings[Type].ResourceCost[i].associatedType].Amount, associatedResource.ResourceCost[i].costAmount);          
            _buildings[Type] = associatedResource;

            //This seems to work but not sure for how long
            IncrementAmount += ResourceMultiplier;
            Resource._resources[ResourceTypeToModify].AmountPerSecond += ResourceMultiplier;
        }
        _buildings[Type].HeaderText.text = string.Format("{0} ({1})", HeaderString, SelfCount);      
    }
    public virtual void SetDescriptionText()
    {
        _buildings[Type].DescriptionText.text = string.Format("Increases {0} yield by: {1:0.00}", Resource._resources[ResourceTypeToModify].Type.ToString(), IncrementAmount);
    }  
}








