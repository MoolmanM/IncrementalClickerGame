using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct StorageMultiply
{
    public ResourceType resourceType;
    public float multiplier, baseMultiplier;
}

public class StorageBuilding : Building
{
    public List<StorageMultiply> storageMultiply = new List<StorageMultiply>();

    void Start()
    {
        ModifyDescriptionText();       
    }
    public void PrestigeModifyStorageMultiplier()
    {
        for (int i = 0; i < storageMultiply.Count; i++)
        {
            StorageMultiply storageResourceToModify = storageMultiply[i];
            storageResourceToModify.multiplier = storageResourceToModify.baseMultiplier;
            float additionAmount = storageResourceToModify.baseMultiplier * ((prestigeAllMultiplierAddition + permAllMultiplierAddition) + (permMultiplierAddition + prestigeMultiplierAddition));
            storageResourceToModify.multiplier += additionAmount;
            storageMultiply[i] = storageResourceToModify;
        }
        prestigeAllMultiplierAddition = 0;
        prestigeMultiplierAddition = 0;
    }
    public override void ResetBuilding()
    {
        isUnlocked = false;
        canvas.enabled = false;
        objMainPanel.SetActive(false);
        graphicRaycaster.enabled = false;
        unlockAmount = 0;
        _selfCount = 0;
        hasSeen = true;
        isUnlockedByResource = false;
        ModifySelfCount();
        PrestigeModifyStorageMultiplier();
        ModifyCost();
        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
        ModifyDescriptionText();
    }
    protected override void ModifyDescriptionText()
    {
        string oldString;
        for (int i = 0; i < storageMultiply.Count; i++)
        {
            if (i > 0)
            {
                oldString = _txtDescription.text;

                _txtDescription.text = string.Format("{0} \nIncrease <color=#F3FF0A>{1}</color> storage by <color=#FF0AF3>{2}</color>.", oldString, storageMultiply[i].resourceType.ToString(), NumberToLetter.FormatNumber(ModifyResourceStorageAmount()));
            }
            else
            {
                _txtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> storage by <color=#FF0AF3>{1}</color>.", storageMultiply[i].resourceType.ToString(), NumberToLetter.FormatNumber(ModifyResourceStorageAmount()));
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
            for (int i = 0; i < storageMultiply.Count; i++)
            {
                Resource.Resources[storageMultiply[i].resourceType].storageAmount += ModifyResourceStorageAmount();
            }
            ModifyDescriptionText();
        }

        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
    }
    private float ModifyResourceStorageAmount()
    {
        for (int i = 0; i < storageMultiply.Count; i++)
        {
            return Resource.Resources[storageMultiply[i].resourceType].baseStorageAmount * storageMultiply[i].multiplier;
        }
        return 0;
    }
}
