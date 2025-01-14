using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ePassive4: Start next/ each run with a certain number of Workers.
public class ePassive4 : EpicPassive
{
    private EpicPassive _epicPassive;
    private uint permanentAmount = 5, prestigeAmount = 25;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
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