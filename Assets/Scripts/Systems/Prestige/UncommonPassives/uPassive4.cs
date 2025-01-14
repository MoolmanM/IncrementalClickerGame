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
    private void AddToPrestigeCache(uint amountToIncrease)
    {
        PrestigeCache.prestigeBoxWorkerCountAddition += amountToIncrease;
    }
    private void AddToPermanentCache(uint amountToIncrease)
    {
        PermanentCache.permanentBoxWorkerCountAddition += amountToIncrease;
    }
    private void ModifyStatDescription(uint amountToIncrease)
    {
        if (amountToIncrease > 1)
        {
            description = string.Format("Gain {0} additional workers", amountToIncrease);
        }
        else
        {
            description = string.Format("Gain an additional worker", amountToIncrease);
        }
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