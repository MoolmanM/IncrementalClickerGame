using TMPro;
using UnityEngine;

public class StoragePile : StorageBuilding
{
    private Building _building;
    //public static float storageAmountMultiplier;

    //public static float permStorageAddition, prestigeStorageAddition;
    //private float _increaseStorageAmount;
    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
    }
    void Start()
    {
        //storageAmountMultiplier = 0.05f;

        ModifyDescriptionText();

        // Do this in another script that happens after everything initializes.
    }
    //protected override void ModifyDescriptionText()
    //{
    //    string oldString;
    //    float modifyAmount;
    //    for (int i = 0; i < resourcesToIncrement.Count; i++)
    //    {
    //        modifyAmount = Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].storageAmount * storageAmountMultiplier;
    //        if (i > 0)
    //        {
    //            oldString = TxtDescription.text;

    //            TxtDescription.text = string.Format("{0} \nIncrease <color=#F3FF0A>{1}</color> storage by <color=#FF0AF3>{2}</color>.", oldString, resourcesToIncrement[i].resourceTypeToModify.ToString(), NumberToLetter.FormatNumber(modifyAmount));
    //        }
    //        else
    //        {
    //            TxtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> storage by <color=#FF0AF3>{1}</color>.", resourcesToIncrement[i].resourceTypeToModify.ToString(), NumberToLetter.FormatNumber(modifyAmount));
    //        }

    //    }

    //}
    //public override void OnBuild()
    //{
    //    bool canPurchase = true;

    //    for (int i = 0; i < ResourceCost.Length; i++)
    //    {
    //        if (ResourceCost[i].CurrentAmount < ResourceCost[i].CostAmount)
    //        {
    //            canPurchase = false;
    //            break;
    //        }
    //    }

    //    if (canPurchase)
    //    {
    //        _selfCount++;
    //        for (int i = 0; i < ResourceCost.Length; i++)
    //        {
    //            Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
    //            ResourceCost[i].CostAmount *= Mathf.Pow(costMultiplier, _selfCount);
    //            ResourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[ResourceCost[i].AssociatedType].amount), NumberToLetter.FormatNumber(ResourceCost[i].CostAmount));
    //        }
    //        ModifyStorage();
    //        //buildingContributionAPS = 0;
    //        //buildingContributionAPS = _selfCount * _resourceMultiplier;
    //        //UpdateResourceInfo();
    //        ModifyDescriptionText();
    //    }

    //    TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);
    //}
    //private void ModifyStorage()
    //{
    //    for (int i = 0; i < resourcesToIncrement.Count; i++)
    //    {
    //        Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].storageAmount += Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].storageAmount * storageAmountMultiplier;
    //    }
    //}
}
