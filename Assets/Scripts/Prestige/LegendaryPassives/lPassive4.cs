// ePassive4: Start next/ each run with a certain number of Workers.
public class lPassive4 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;
    private uint permanentAmount = 7, prestigeAmount = 35;

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
    }
    private void AddToBoxCache(uint amountToIncrease)
    {
        BoxCache.cachedWorkerCountModified += amountToIncrease;
    }
    private void ModifyStatDescription(uint amountToIncrease)
    {
        if (amountToIncrease > 1)
        {
            description = string.Format("Gain {0} additional workers", amountToIncrease);
        }
        else
        {
            description = string.Format("Gain an additional worker", amountToIncrease);
        }
    }
    public override void InitializePermanentStat()
    {
        ModifyStatDescription(permanentAmount);
        AddToBoxCache(permanentAmount);
    }
    public override void InitializePrestigeStat()
    {
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButton()
    {
        AddToBoxCache(prestigeAmount);
    }
}
