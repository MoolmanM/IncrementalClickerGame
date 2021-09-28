using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UncommonType
{
    Passive1,
    Passive2,
    Passive3,
    Passive4,
    Passive5,
}

public class UncommonPassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<UncommonType, UncommonPassive> UncommonPassives = new Dictionary<UncommonType, UncommonPassive>();
    public UncommonType Type;
    public string description;

    public virtual void ExecutePassive()
    {
        Debug.Log("This is something the passives will all do because they have that in common");
    }
}