using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reduce time it takes to research stuff by a certain %.
public class uPassive1 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;
    private float percentageAmount = 0.0023f; // 0.23%


    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);
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
