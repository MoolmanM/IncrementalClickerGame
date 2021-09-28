using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LegendaryType
{
    Passive1,
    Passive2,
    Passive3,
    Passive4,
    Passive5,
}

public class LegendaryPassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<LegendaryType, LegendaryPassive> LegendaryPassives = new Dictionary<LegendaryType, LegendaryPassive>();
    public LegendaryType Type;
    public string description;

    public virtual void ExecutePassive()
    {
        Debug.Log("This is something the passives will all do because they have that in common");
    }
}
