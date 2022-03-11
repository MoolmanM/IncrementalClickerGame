using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reduce time it takes to research stuff by a certain %.
public class rPassive1 : RarePassive
{
    private RarePassive _rarePassive;
    private float percentageAmount = 0.0036f; // 0.36%


    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);
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
