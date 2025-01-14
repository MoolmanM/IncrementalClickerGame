using UnityEngine;

public class lPassive5 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
    }
    private void AddToBoxCache()
    {
        Debug.Log("This is lPassive5(Currently out of order)");
    }
    public override void InitializePermanentStat()
    {
        AddToBoxCache();
    }
}
