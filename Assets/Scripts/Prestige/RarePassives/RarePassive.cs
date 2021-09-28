using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RareType
{
    Passive1,
    Passive2,
    Passive3,
    Passive4,
    Passive5,
}

public class RarePassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<RareType, RarePassive> RarePassives = new Dictionary<RareType, RarePassive>();
    public RareType Type;
    public string description;

    public virtual void ExecutePassive()
    {
        Debug.Log("This is something the passives will all do because they have that in common");
    }
}
