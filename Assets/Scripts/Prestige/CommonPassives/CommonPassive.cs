using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum CommonType
{
    Passive1,
    Passive2,
    Passive3,
    Passive4,
    Passive5,
}

public class CommonPassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<CommonType, CommonPassive> CommonPassives = new Dictionary<CommonType, CommonPassive>();
    public CommonType Type;
    public string description;
    protected int _index;
    //public ResourceType resourceTypeToModify;

    public virtual void ExecutePassive()
    {
        Debug.Log("Common passive executed");
    }
    public virtual void GenerateRandomResource()
    {
        _index = Random.Range(0, Prestige.resourcesUnlockedInPreviousRun.Count);
    }
    public virtual void GenerateRandomBuilding()
    {
        Debug.Log("Generate Random building");
    }
}

