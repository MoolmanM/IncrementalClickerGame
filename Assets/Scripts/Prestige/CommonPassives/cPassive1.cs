using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reduce time it takes to research stuff by a certain %.
public class cPassive1 : CommonPassive
{
    private CommonPassive _commonPassive;
    private float percentageAmount = 0.05f; //5%


    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
        description = "Reduces time it takes to research by " + percentageAmount;
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
