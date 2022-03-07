using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Start next/each run with a certain number of Workers.
public class cPassive4 : CommonPassive
{
    private CommonPassive _commonPassive;
    private uint amountToIncrease = 1;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);

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