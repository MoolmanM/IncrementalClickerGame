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
    public Dictionary<CraftingType, Craftable> _craftables = new Dictionary<CraftingType, Craftable>();

    public CraftingType Type;
    public ResourceCost[] ResourceCost;
    public BuildingType BuildingTypeToActivate;
    public ResourceType ResourceTypeToActivate;
    public GameObject SpacerAbove;

    protected float _timer = 0.1f;
    protected readonly float maxValue = 0.1f;
    protected TMP_Text DescriptionText, HeaderText;
    protected GameObject ButtonMain;
    protected Transform DescriptionTransform, HeaderTransform, ButtonTransform, progressCircleTransform;
    private string _isCraftedString;
    private int _isCrafted;
    protected Image progressCircle;

    public void SetInitialValues()
    {
        HeaderTransform = transform.Find("Header_Panel/Header_Text");
        HeaderText = HeaderTransform.GetComponent<TMP_Text>();
        DescriptionTransform = transform.Find("Body/Description_Panel/Description_Text");
        DescriptionText = DescriptionTransform.GetComponent<TMP_Text>();
        ButtonTransform = transform.Find("Header_Panel/Button_Main");
        ButtonMain = ButtonTransform.GetComponent<GameObject>();
        progressCircleTransform = transform.Find("Header_Panel/Progress_Circle_Panel/ProgressCircle");
        progressCircle = progressCircleTransform.GetComponent<Image>();
        _isCraftedString = Type.ToString() + "IsCrafted";
        PlayerPrefs.GetInt(_isCraftedString, _isCrafted);
        if (_isCrafted == 1)
        {
            Destroy(ButtonMain);
            _craftables[Type].HeaderText.text = string.Format("{0} (Crafted)", _craftables[Type].HeaderText.text);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(_isCraftedString, _isCrafted);
    }
    public virtual void UpdateResourceCosts()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            for (int i = 0; i < ResourceCost.Length; i++)
            {
                _craftables[Type].ResourceCost[i].currentAmount = Resource._resources[_craftables[Type].ResourceCost[i].associatedType].Amount;
                _craftables[Type].ResourceCost[i].UiForResourceCost.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", _craftables[Type].ResourceCost[i].currentAmount, _craftables[Type].ResourceCost[i].costAmount);
                _craftables[Type].ResourceCost[i].UiForResourceCost.costNameText.text = string.Format("{0}", _craftables[Type].ResourceCost[i].associatedType.ToString());              
            }
            GetCurrentFill();
        }
    }

    public virtual void Craft()
    {
        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (!_craftables.TryGetValue(Type, out Craftable associatedResource) || associatedResource.ResourceCost[i].currentAmount < associatedResource.ResourceCost[i].costAmount)
            {
                return;
            }

            Resource._resources[_craftables[Type].ResourceCost[i].associatedType].Amount -= associatedResource.ResourceCost[i].costAmount;
            
            _craftables[Type] = associatedResource;
            _craftables[Type].HeaderText.text = string.Format("{0} (Crafted)", _craftables[Type].HeaderText.text);
            Destroy(ButtonMain);
            Building._buildings[BuildingTypeToActivate].IsUnlocked = 1;
            Building._buildings[BuildingTypeToActivate].MainBuildingPanel.SetActive(true);
            Building._buildings[BuildingTypeToActivate].SpacerAbove.SetActive(true);
            Resource._resources[ResourceTypeToActivate].IsUnlocked = 1;
            Resource._resources[ResourceTypeToActivate].MainResourcePanel.SetActive(true);
            //Going to have to assign spacers to the resource panels too.
            _isCrafted = 1;
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

    public void SetDescriptionText(string description)
    {
        _craftables[Type].DescriptionText.text = string.Format("{0}", description);
    }
}
