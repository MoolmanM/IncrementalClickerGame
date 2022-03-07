using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ePassive4: Start next/ each run with a certain number of Workers.
public class ePassive4 : EpicPassive
{
    private EpicPassive _epicPassive;

    private uint amountToIncrease = 1;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);

        if (amountToIncrease > 1)
        {
            description = string.Format("Gains {0} additional workers", amountToIncrease);
        }
        else
        {
            description = string.Format("Gain an additional worker", amountToIncrease);
        }
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedWorkerCountModified += amountToIncrease;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}