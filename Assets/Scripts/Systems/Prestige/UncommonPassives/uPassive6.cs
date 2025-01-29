using System.Collections.Generic;
using UnityEngine;

// Decrease cost of a random Craftable by a certain %.
public class uPassive6 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;
    private CraftingType craftingTypeChosen;
    private float permanentAmount = 0.023f, prestigeAmount = 0.115f;

    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);
    }
    public void ChooseRandomCrafting()
    {
        List<CraftingType> craftingTypesInCurrentRun = new List<CraftingType>();

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.IsUnlocked)
            {
                craftingTypesInCurrentRun.Add(craft.Key);
            }
        }
        if (craftingTypesInCurrentRun.Count >= Prestige.craftablesUnlockedInPreviousRun.Count)
        {
            _index = Random.Range(0, craftingTypesInCurrentRun.Count);
            craftingTypeChosen = craftingTypesInCurrentRun[_index];
        }
        else
        {
            _index = Random.Range(0, Prestige.craftablesUnlockedInPreviousRun.Count);
            craftingTypeChosen = Prestige.craftablesUnlockedInPreviousRun[_index];
        }
    }
    private void AddToPrestigeCache(float percentageAmount, CraftingType craftingType)
    {
        if (!PrestigeCache.prestigeBoxCraftableCostSubtraction.ContainsKey(craftingType))
        {
            PrestigeCache.prestigeBoxCraftableCostSubtraction.Add(craftingType, percentageAmount);
        }
        else
        {
            PrestigeCache.prestigeBoxCraftableCostSubtraction[craftingType] += percentageAmount;
        }
    }
    private void AddToPermanentCache(float percentageAmount, CraftingType craftingType)
    {
        if (!PermanentCache.permanentBoxCraftableCostSubtraction.ContainsKey(craftingType))
        {
            PermanentCache.permanentBoxCraftableCostSubtraction.Add(craftingType, percentageAmount);
        }
        else
        {
            PermanentCache.permanentBoxCraftableCostSubtraction[craftingType] += percentageAmount;
        }
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Decrease cost of crafting '{0}' by {1}%", Craftable.Craftables[craftingTypeChosen].ActualName, percentageAmount * 100);
    }
    public override void InitializePermanentStat()
    {
        ChooseRandomCrafting();
        ModifyStatDescription(permanentAmount);
        AddToPermanentCache(permanentAmount, craftingTypeChosen);
    }
    public override void InitializePrestigeStat()
    {
        ChooseRandomCrafting();
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButtonCrafting(CraftingType craftingType)
    {
        AddToPrestigeCache(prestigeAmount, craftingType);
    }
    public override CraftingType ReturnCraftingType()
    {
        return craftingTypeChosen;
    }
}
