using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Start next/each run with a certain number of Workers.
public class rPassive4 : RarePassive
{
    private RarePassive _rarePassive;
    private uint amountToIncrease = 4;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);

        if (amountToIncrease > 1)
        {
            description = string.Format("Gain {0} additional workers", amountToIncrease);
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