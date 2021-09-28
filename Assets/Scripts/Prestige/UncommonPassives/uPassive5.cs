using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uPassive5 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;

    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);
    }

    public override void ExecutePassive()
    {
        base.ExecutePassive();
    }
}
