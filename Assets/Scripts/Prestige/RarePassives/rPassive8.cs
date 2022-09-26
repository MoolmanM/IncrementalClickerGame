using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Increase storage limit of a random Storage Building by a certain %.
public class rPassive8 : RarePassive
{
    private RarePassive _rarePassive;
    private float permanentAmount = 0.036f, prestigeAmount = 0.180f;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);      
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

