//ePassive2: Increase production of ALL Workers by a certain %.
using System.Collections.Generic;

public class lPassive2 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;
    private float percentageAmount = 0.025f; // 2.5%

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
        description = "Increase production of all Workers by " + percentageAmount;
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedAllWorkerMultiplierAmount += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}