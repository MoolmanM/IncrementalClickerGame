using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reduce time it takes to research stuff by a certain %.
public class cPassive1 : CommonPassive
{
    private CommonPassive _commonPassive;
    private float percentageAmount = 0.001f; // 0.1%


    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
        description = string.Format("Reduces time it takes to research by {0}%", percentageAmount * 100);
    }   
    private void AddToBoxCache()
    {
        BoxCache.cachedResearchTimeReductionAmount += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}
