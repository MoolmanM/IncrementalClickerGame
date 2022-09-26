using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive7: Increase production of ALL production Buildings by a certain %.
public class ePassive7 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float permanentAmount = 0.01f, prestigeAmount = 0.05f;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);    
    }
    private void AddToPrestigeCache(float percentageAmount)
    {
        PrestigeCache.prestigeBoxAllBuildingMultiplierAddition += percentageAmount;    
    }
    private void AddToPermanentCache(float percentageAmount)
    {
        PermanentCache.permanentBoxAllBuildingMultiplierAddition += percentageAmount;
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Increase production of all Buildings by {0}%", percentageAmount * 100);
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