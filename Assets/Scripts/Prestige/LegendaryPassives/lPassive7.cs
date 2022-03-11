//ePassive7: Increase production of ALL production Buildings by a certain %.
public class lPassive7 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;
    private float percentageAmount = 0.025f; // 2.5%

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
        description = string.Format("Increase production of all Buildings by {0}%", percentageAmount * 100);
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedAllBuildingMultiplierAmount += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}