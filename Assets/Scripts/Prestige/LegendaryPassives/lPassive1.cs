using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lPassive1 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
    }

    public override void ExecutePassive()
    {
        base.ExecutePassive();
    }
}
