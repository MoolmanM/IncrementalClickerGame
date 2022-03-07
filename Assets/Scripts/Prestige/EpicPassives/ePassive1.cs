using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive1: Reduce time it takes to research by a certain %.

public class ePassive1 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float percentageAmount = 0.05f; //5%

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
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
