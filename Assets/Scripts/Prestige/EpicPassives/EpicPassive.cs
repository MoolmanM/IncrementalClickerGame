using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EpicType
{
    Passive1,
    Passive2,
    Passive3,
    Passive4,
    Passive5,
}

public class EpicPassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<EpicType, EpicPassive> EpicPassives = new Dictionary<EpicType, EpicPassive>();
    public EpicType Type;
    public string description;

    public virtual void ExecutePassive()
    {
        Debug.Log("This is something the passives will all do because they have that in common");
    }
}