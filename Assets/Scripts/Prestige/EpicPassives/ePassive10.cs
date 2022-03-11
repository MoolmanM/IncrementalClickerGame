using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive10: Increase storage limit of ALL storage Buildings by a certain %.

// For now it's obviously just one storage building, but eventually it'll be more.

public class ePassive10 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float percentageAmount = 0.01f; // 1%

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);

        description = string.Format("Increase storage limit by {0}%", percentageAmount * 100);
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
}
