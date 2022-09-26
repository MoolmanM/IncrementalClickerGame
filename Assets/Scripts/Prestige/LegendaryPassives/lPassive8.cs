//ePassive8: Decrease cost of ALL Craftables by a certain %.
public class lPassive8 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;
    private float permanentAmount = 0.025f, prestigeAmount = 0.125f;

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
        
    }
    private void AddToPrestigeCache(float percentageAmount)
    {
        PrestigeCache.prestigeBoxAllCraftablesCostSubtraction += percentageAmount;
    }
    private void AddToPermanentCache(float percentageAmount)
    {
        PermanentCache.permanentBoxAllCraftablesCostSubtraction += percentageAmount;
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Decrease cost of all Craftables by {0}%", percentageAmount * 100);
    }
    public override void InitializePermanentStat()
    {
        ModifyStatDescription(permanentAmount);
        AddToPermanentCache(permanentAmount);
    }
    public override void InitializePrestigeStat()
    {
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButton()
    {
        AddToPrestigeCache(prestigeAmount);
    }
}
