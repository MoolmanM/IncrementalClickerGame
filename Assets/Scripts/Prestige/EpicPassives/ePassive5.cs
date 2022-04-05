using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ePassive5 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float permanentAmount = 0.01f, prestigeAmount = 0.05f;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
    }
    private void AddToBoxCache()
    {
        Debug.Log("This is ePassive5(Currently out of order)");
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}
