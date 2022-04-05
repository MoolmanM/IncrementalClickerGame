using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive1: Reduce time it takes to research by a certain %.

public class ePassive1 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float permanentAmount = 0.005f, prestigeAmount = 0.025f;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);       
    }
    private void AddToBoxCache(float percentageAmount)
    {
        BoxCache.cachedResearchTimeReductionAmount += percentageAmount;        
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Reduces time it takes to research by {0}%", percentageAmount * 100);
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
