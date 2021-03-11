using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public TMP_Text DescriptionText, HeaderText;
    public GameObject ButtonMain;
    public BuildingType BuildingTypeToActivate;

    private float _timer = 0.1f, maxValue = 0.1f;

    public virtual void UpdateCraftingElements()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            for (int i = 0; i < ResourceCost.Length; i++)
            {
                _craftables[Type].ResourceCost[i].currentAmount = Resource._resources[_craftables[Type].ResourceCost[i].associatedType].Amount;
                _craftables[Type].ResourceCost[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", _craftables[Type].ResourceCost[i].currentAmount, _craftables[Type].ResourceCost[i].costAmount);
                _craftables[Type].ResourceCost[i].uiForBuilding.costNameText.text = string.Format("{0}", _craftables[Type].ResourceCost[i].associatedType.ToString());
                
            }
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
            Building._buildings[BuildingTypeToActivate].MainBuildingPanel.SetActive(true);
        }
        
    }

    public void SetDescriptionText(string description)
    {
        _craftables[Type].DescriptionText.text = string.Format("{0}", description);
    }
}
