using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reduce time it takes to research stuff by a certain %.
public class cPassive1 : CommonPassive
{
    private CommonPassive _commonPassive;
    private float permanentAmount = 0.001f, prestigeAmount = 0.005f;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
        
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
