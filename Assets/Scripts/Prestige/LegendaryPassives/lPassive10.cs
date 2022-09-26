//ePassive10: Increase storage limit of ALL storage Buildings by a certain %.

// For now it's obviously just one storage building, but eventually it'll be more.

public class lPassive10 : LegendaryPassive
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
        PrestigeCache.prestigeBoxStorageAddition += percentageAmount;
    }
    private void AddToPermanentCache(float percentageAmount)
    {
        PermanentCache.permanentBoxStorageAddition += percentageAmount;
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Increase storage limit by {0}%", percentageAmount * 100);
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
