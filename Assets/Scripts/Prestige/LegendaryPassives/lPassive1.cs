//ePassive1: Reduce time it takes to research by a certain %.

public class lPassive1 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;
    private float permanentAmount = 0.0065f, prestigeAmount = 0.0325f;

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
        
    }
    private void AddToPrestigeCache(float percentageAmount)
    {
        PrestigeCache.prestigeBoxResearchTimeSubtraction += percentageAmount;
    }
    private void AddToPermanentCache(float percentageAmount)
    {
        PermanentCache.permanentBoxResearchTimeSubtraction += percentageAmount;
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Reduce research time by {0}%", percentageAmount * 100);
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
