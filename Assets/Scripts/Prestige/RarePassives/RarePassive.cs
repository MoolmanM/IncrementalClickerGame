using System.Collections.Generic;
using UnityEngine;

public enum RareType
{
    Passive1,
    Passive2,
    Passive3,
    Passive4,
    Passive5,
    Passive6,
    Passive7,
    Passive8
}

public class RarePassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<RareType, RarePassive> RarePassives = new Dictionary<RareType, RarePassive>();
    public RareType Type;
    [System.NonSerialized] public string description;
    protected int _index;

    public virtual void InitializePermanentStat()
    {

    }
}
