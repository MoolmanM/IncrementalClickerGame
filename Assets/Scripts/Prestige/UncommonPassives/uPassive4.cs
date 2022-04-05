using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Start next/each run with a certain number of Workers.
public class uPassive4 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;
    private uint permanentAmount = 2, prestigeAmount = 10;

    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);         
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
        ModifyStatDescription(permanentAmount);
        AddToBoxCache(permanentAmount);
    }
    public override void InitializePrestigeButton()
    {
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeStat()
    {
        AddToBoxCache(prestigeAmount);
    }
}