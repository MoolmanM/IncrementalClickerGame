using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ePassive7 : EpicPassive
{
    private EpicPassive _epicPassive;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
    }
    private void AddToBoxCache()
    {

    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}