using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Start next/each run with a certain number of Workers.
public class cPassive4 : CommonPassive
{
    private CommonPassive _commonPassive;
    private uint permanentAmount = 1, prestigeAmount = 5;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
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