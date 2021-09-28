using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ePassive5 : EpicPassive
{
    private EpicPassive _epicPassive;

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
    }

    public override void ExecutePassive()
    {
        base.ExecutePassive();
    }
}
