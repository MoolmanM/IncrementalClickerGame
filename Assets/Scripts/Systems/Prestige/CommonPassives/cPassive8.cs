using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Increase storage limit of a random Storage Building by a certain %.
public class cPassive8 : CommonPassive
{
    private CommonPassive _commonPassive;
    private float permanentAmount = 0.01f, prestigeAmount = 0.05f; // 1%

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);  
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

    // Increase initial storage
    // Increase storage permanently?
    // Eventually this passive can also loop through all 'storage' type Buildings.
}

