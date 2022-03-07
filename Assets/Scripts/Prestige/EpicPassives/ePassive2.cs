//ePassive2: Increase production of ALL Workers by a certain %.
using System.Collections.Generic;

public class ePassive2 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float percentageAmount = 0.02f; // 2%
    private WorkerType workerTypeChosen;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
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