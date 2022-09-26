using TMPro;
using UnityEngine;

public class StoragePile : Building
{
    private Building _building;
    public static float storageAmountMultiplier;

    public static float permStorageAddition, prestigeStorageAddition;
    //private float _increaseStorageAmount;
    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
    }
    void Start()
    {
        storageAmountMultiplier = 0.1f;

        ModifyDescriptionText();

        // Do this in another script that happens after everything initializes.
    }
    protected override void ModifyDescriptionText()
    {
        string oldString;
        float modifyAmount;
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            modifyAmount = Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].storageAmount * storageAmountMultiplier;
            if (i > 0)
            {
                oldString = _txtDescription.text;

                _txtDescription.text = string.Format("{0} \nIncrease <color=#F3FF0A>{1}</color> storage by <color=#FF0AF3>{2}</color>.", oldString, resourcesToIncrement[i].resourceTypeToModify.ToString(), NumberToLetter.FormatNumber(modifyAmount));
            }
            else
            {
                _txtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> storage by <color=#FF0AF3>{1}</color>.", resourcesToIncrement[i].resourceTypeToModify.ToString(), NumberToLetter.FormatNumber(modifyAmount));
            }

        }

    }
    public override void OnBuild()
    {
        bool canPurchase = true;

        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resourceCost[i].currentAmount < resourceCost[i].costAmount)
            {
                canPurchase = false;
                break;
            }
        }

        if (canPurchase)
        {
            _selfCount++;
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
                resourceCost[i].costAmount *= Mathf.Pow(costMultiplier, _selfCount);
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[resourceCost[i].associatedType].amount), NumberToLetter.FormatNumber(resourceCost[i].costAmount));
            }
            ModifyStorage();
            //buildingContributionAPS = 0;
            //buildingContributionAPS = _selfCount * _resourceMultiplier;
            //UpdateResourceInfo();
            ModifyDescriptionText();
        }

        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
    }
    private void ModifyStorage()
    {
        for (int i = 0; i < resourcesToIncrement.Count; i++)
        {
            // default Multiplier will be 20%.
            Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].storageAmount += Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].storageAmount * storageAmountMultiplier;
        }
    }
}
