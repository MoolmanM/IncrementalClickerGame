using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reduce time it takes to research stuff by a certain %.
public class rPassive1 : RarePassive
{
    private RarePassive _rarePassive;
    private float permanentAmount = 0.0036f, prestigeAmount = 0.0180f;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);       
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
        AddToBoxCache(permanentAmount);
        ModifyStatDescription(permanentAmount);
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
