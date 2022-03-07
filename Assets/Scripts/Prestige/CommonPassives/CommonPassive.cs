using System.Collections.Generic;
using UnityEngine;

public enum CommonType
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

public class CommonPassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<CommonType, CommonPassive> CommonPassives = new Dictionary<CommonType, CommonPassive>();
    public CommonType Type;
    [System.NonSerialized] public string description;
    protected int _index;

    public virtual void InitializePermanentStat()
    {
        
    }
}

