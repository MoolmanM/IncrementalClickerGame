using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Increase storage limit of a random Storage Building by a certain %.
public class cPassive8 : CommonPassive
{
    private CommonPassive _commonPassive;
    private float percentageAmount = 0.1f; //10%

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);

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

