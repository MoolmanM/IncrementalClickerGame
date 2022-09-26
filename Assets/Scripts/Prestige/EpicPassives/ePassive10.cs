using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive10: Increase storage limit of ALL storage Buildings by a certain %.

// For now it's obviously just one storage building, but eventually it'll be more.

public class ePassive10 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float permanentAmount = 0.01f, prestigeAmount = 0.05f;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);       
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
