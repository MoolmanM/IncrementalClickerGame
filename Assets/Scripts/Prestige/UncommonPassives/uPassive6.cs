using System.Collections.Generic;
using UnityEngine;

// Decrease cost of a random Craftable by a certain %.
public class uPassive6 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;
    private float percentageAmount = 0.023f; // 2.3%
    private CraftingType craftingTypeChosen;

    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);
    }
    private void ChooseRandomCraftable()
    {
        List<CraftingType> craftingTypesInCurrentRun = new List<CraftingType>();

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.isUnlocked)
            {
                craftingTypesInCurrentRun.Add(craft.Key);
            }
        }
        if (craftingTypesInCurrentRun.Count >= Prestige.workersUnlockedInPreviousRun.Count)
        {
            _index = Random.Range(0, craftingTypesInCurrentRun.Count);
            craftingTypeChosen = craftingTypesInCurrentRun[_index];
        }
        else
        {
            _index = Random.Range(0, Prestige.craftablesUnlockedInPreviousRun.Count);
            craftingTypeChosen = Prestige.craftablesUnlockedInPreviousRun[_index];
        }

        description = string.Format("Decrease the cost of crafting '{0}' by {1}%", Craftable.Craftables[craftingTypeChosen].actualName, percentageAmount * 100);

        AddToBoxCache();
    }
    private void AddToBoxCache()
    {
        if (!BoxCache.cachedCraftableCostReduced.ContainsKey(craftingTypeChosen))
        {
            BoxCache.cachedCraftableCostReduced.Add(craftingTypeChosen, percentageAmount);
        }
        else
        {
            BoxCache.cachedCraftableCostReduced[craftingTypeChosen] += percentageAmount;
        }
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        ChooseRandomCraftable();
    }
}
