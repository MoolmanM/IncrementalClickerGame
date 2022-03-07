using System.Collections.Generic;
using UnityEngine;

public enum UncommonType
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

public class UncommonPassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<UncommonType, UncommonPassive> UncommonPassives = new Dictionary<UncommonType, UncommonPassive>();
    public UncommonType Type;
    [System.NonSerialized] public string description;
    protected int _index;

    public virtual void InitializePermanentStat()
    {

    }
}