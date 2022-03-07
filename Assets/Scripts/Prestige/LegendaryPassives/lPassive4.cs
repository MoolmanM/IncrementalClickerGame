// ePassive4: Start next/ each run with a certain number of Workers.
public class lPassive4 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;

    private uint amountToIncrease = 5;

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);

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