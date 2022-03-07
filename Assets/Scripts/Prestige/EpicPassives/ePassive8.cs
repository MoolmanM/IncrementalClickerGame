using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive8: Decrease cost of ALL Craftables by a certain %.
public class ePassive8 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float percentageAmount = 0.01f; //1%

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
        description = "Decrease the cost of all Craftables by " + percentageAmount;
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedAllCraftablesCostReduced += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}
