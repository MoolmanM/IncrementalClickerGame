using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPassive1 : CommonPassive
{
    private CommonPassive _commonPassive;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
    }

    public override void ExecutePassive()
    {
        base.ExecutePassive();
        // So this passive willllllll increase storage amount but how?, increase storage upgrades by a certain percentage?
        // Increase initial storage?
        // Increase storage permanently?
        // Will start with the percentage option, for now default is 20%, so this will make that 30%.
        StoragePile.storageAmountMultiplier += 0.1f;
        // Increase with 10%
        // This passive can maybe also loop through all unlocked 'storage' buildings.
        
    }
}
