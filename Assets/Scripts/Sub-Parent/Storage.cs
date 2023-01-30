using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StorageMultiply
{
    public ResourceType resourceType;
    public float multiplier;
}

public class Storage : Building
{
    public List<StorageMultiply> storageMultiply;

    void Start()
    {
        ModifyDescriptionText();
    }
    protected override void ModifyDescriptionText()
    {
        string oldString;
        float modifyAmount;
        for (int i = 0; i < storageMultiply.Count; i++)
        {
            modifyAmount = Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].storageAmount * storageMultiply[i].multiplier;
            if (i > 0)
            {
                oldString = _txtDescription.text;

                _txtDescription.text = string.Format("{0} \nIncrease <color=#F3FF0A>{1}</color> storage by <color=#FF0AF3>{2}</color>.", oldString, storageMultiply[i].resourceType.ToString(), NumberToLetter.FormatNumber(modifyAmount));
            }
            else
            {
                _txtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> storage by <color=#FF0AF3>{1}</color>.", storageMultiply[i].resourceType.ToString(), NumberToLetter.FormatNumber(modifyAmount));
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
            ModifyDescriptionText();
        }

        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
    }
    private void ModifyStorage()
    {
        for (int i = 0; i < storageMultiply.Count; i++)
        {
            Resource.Resources[storageMultiply[i].resourceType].storageAmount += Resource.Resources[storageMultiply[i].resourceType].storageAmount * storageMultiply[i].multiplier;
        }
    }
}
