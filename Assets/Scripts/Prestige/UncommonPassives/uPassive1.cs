using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reduce time it takes to research stuff by a certain %.
public class uPassive1 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;
    private float permanentAmount = 0.023f, prestigeAmount = 0.115f;

    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);      
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
