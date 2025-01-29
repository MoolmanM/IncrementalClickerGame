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
        IsUnlocked = false;
        Canvas.enabled = false;
        ObjMainPanel.SetActive(false);
        GraphicRaycaster.enabled = false;
        UnlockAmount = 0;
        _selfCount = 0;
        HasSeen = true;
        IsUnlockedByResource = false;
        ModifySelfCount();
        PrestigeModifyStorageMultiplier();
        ModifyCost();
        TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);
        ModifyDescriptionText();
    }
    protected override void ModifyDescriptionText()
    {
        string oldString;
        for (int i = 0; i < storageMultiply.Count; i++)
        {
            if (i > 0)
            {
                oldString = TxtDescription.text;

                TxtDescription.text = string.Format("{0} \nIncrease <color=#F3FF0A>{1}</color> storage by <color=#FF0AF3>{2}</color>.", oldString, storageMultiply[i].resourceType.ToString(), NumberToLetter.FormatNumber(ModifyResourceStorageAmount()));
            }
            else
            {
                TxtDescription.text = string.Format("Increase <color=#F3FF0A>{0}</color> storage by <color=#FF0AF3>{1}</color>.", storageMultiply[i].resourceType.ToString(), NumberToLetter.FormatNumber(ModifyResourceStorageAmount()));
            }
        }
    }
    public override void OnBuild()
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
            _selfCount++;
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
                ResourceCost[i].CostAmount *= Mathf.Pow(costMultiplier, _selfCount);
                ResourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", NumberToLetter.FormatNumber(Resource.Resources[ResourceCost[i].AssociatedType].amount), NumberToLetter.FormatNumber(ResourceCost[i].CostAmount));
            }
            for (int i = 0; i < storageMultiply.Count; i++)
            {
                Resource.Resources[storageMultiply[i].resourceType].storageAmount += ModifyResourceStorageAmount();
            }
            ModifyDescriptionText();
        }

        TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);
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
