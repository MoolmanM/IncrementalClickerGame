using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive9: Decrease cost of ALL Researchables by a certain %.
public class ePassive9 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float permanentAmount = 0.01f, prestigeAmount = 0.05f;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive); 
    }
    private void AddToBoxCache(float percentageAmount)
    {
        BoxCache.cachedAllResearchablesCostReduced += percentageAmount;       
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Decrease the cost of all Researchables by {0}%", percentageAmount * 100);
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
