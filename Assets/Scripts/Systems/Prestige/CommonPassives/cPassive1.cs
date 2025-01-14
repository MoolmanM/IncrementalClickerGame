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
        AddToPermanentCache(permanentAmount);
        ModifyStatDescription(permanentAmount);
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
