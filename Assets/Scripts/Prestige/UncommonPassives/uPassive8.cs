using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Increase storage limit of a random Storage Building by a certain %.
public class uPassive8 : UncommonPassive
{
    private UncommonPassive _unommonPassive;
    private float percentageAmount = 0.023f; // 2.3%

    private void Awake()
    {
        _unommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _unommonPassive);

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

