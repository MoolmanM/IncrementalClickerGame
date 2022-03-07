using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Increase storage limit of a random Storage Building by a certain %.
public class rPassive8 : RarePassive
{
    private RarePassive _rarePassive;
    private float percentageAmount = 0.1f; //10%

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);

        description = string.Format("Increase storage limit by {0}%", percentageAmount*100);
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedstoragePercentageAmount += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }

    // Increase initial storage
    // Increase storage permanently?
    // Eventually this passive can also loop through all 'storage' type Buildings.
}

