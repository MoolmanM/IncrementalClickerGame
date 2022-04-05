using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Decrease cost of a random Craftable by a certain %.
public class cPassive6 : CommonPassive
{
    private CommonPassive _commonPassive;
    public CraftingType craftingTypeChosen;
    private float permanentAmount = 0.01f, prestigeAmount = 0.05f;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
    }
    public void ChooseRandomCrafting()
    {
        List<CraftingType> craftingTypesInCurrentRun = new List<CraftingType>();

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.isUnlocked)
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
    private void AddToBoxCache(float percentageAmount, CraftingType craftingType)
    {
        if (!BoxCache.cachedCraftableCostReduced.ContainsKey(craftingType))
        {
            BoxCache.cachedCraftableCostReduced.Add(craftingType, percentageAmount);
        }
        else
        {
            BoxCache.cachedCraftableCostReduced[craftingType] += percentageAmount;
        }
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Decrease the cost of crafting '{0}' by {1}%", Craftable.Craftables[craftingTypeChosen].actualName, percentageAmount * 100);
    }
    public override void InitializePermanentStat()
    {
        ChooseRandomCrafting();
        ModifyStatDescription(permanentAmount);
        AddToBoxCache(permanentAmount, craftingTypeChosen);
    }
    public override void InitializePrestigeStat()
    {
        ChooseRandomCrafting();
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButtonCrafting(CraftingType craftingType)
    {
        AddToBoxCache(prestigeAmount, craftingType);
    }
    public override CraftingType ReturnCraftingType()
    {
        return craftingTypeChosen;
    }
}
