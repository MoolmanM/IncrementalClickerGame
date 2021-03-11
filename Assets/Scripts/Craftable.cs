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

    public CraftingType type;
    public ResourceCost[] resourceCost;
    public TMP_Text descriptionText, craftName;
    public GameObject buttonMain;
    public BuildingType buildingTypeToActivate;

    private float _timer = 0.1f, maxValue = 0.1f;

    public virtual void UpdateCraftingElements()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            for (int i = 0; i < resourceCost.Length; i++)
            {
                _craftables[type].resourceCost[i].currentAmount = Resource._resources[_craftables[type].resourceCost[i].associatedType].amount;
                _craftables[type].resourceCost[i].uiForBuilding.costAmountText.text = string.Format("{0:0.00}/{1:0.00}", _craftables[type].resourceCost[i].currentAmount, _craftables[type].resourceCost[i].costAmount);
                _craftables[type].resourceCost[i].uiForBuilding.costNameText.text = string.Format("{0}", _craftables[type].resourceCost[i].associatedType.ToString());
                
            }
        }
    }

    public virtual void Craft()
    {
        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (!_craftables.TryGetValue(type, out Craftable associatedResource) || associatedResource.resourceCost[i].currentAmount < associatedResource.resourceCost[i].costAmount)
            {
                return;
            }

            Resource._resources[_craftables[type].resourceCost[i].associatedType].amount -= associatedResource.resourceCost[i].costAmount;
            
            _craftables[type] = associatedResource;
            _craftables[type].craftName.text = string.Format("{0} (Crafted)", _craftables[type].craftName.text);
            Destroy(buttonMain);
            Building._buildings[buildingTypeToActivate].mainBuildingPanel.SetActive(true);
        }
        
    }

    public void SetDescriptionText(string description)
    {
        _craftables[type].descriptionText.text = string.Format("{0}", description);
    }
}
