using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Start next/each run with a certain number of Workers.
public class rPassive4 : RarePassive
{
    private RarePassive _rarePassive;
    private uint permanentAmount = 4, prestigeAmount = 20;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);           
    }
    private void AddToBoxCache(uint workerIncreaseAmount)
    {
        BoxCache.cachedWorkerCountModified += workerIncreaseAmount;
    }
    private void ModifyStatDescription(uint workerIncreaseAmount)
    {
        if (workerIncreaseAmount > 1)
        {
            description = string.Format("Gain {0} additional workers", workerIncreaseAmount);
        }
        else
        {
            description = string.Format("Gain an additional worker", workerIncreaseAmount);
        }
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