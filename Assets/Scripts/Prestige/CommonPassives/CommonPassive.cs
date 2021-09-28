using System.Collections;
using System.Collections.Generic;
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
    public ResourceType resourceTypeToModify;

    public virtual void ExecutePassive()
    {
        // Before I work on these functions I need to first make sure that they only execute after I've pressed the buy buttons,
        // Or I can store all the functions in a sort of cache, and then execute them all after I've pressed the done button

        Debug.Log("This is for all the commons");
        //Resource.Resources[resourceTypeToModify].amountPerSecond += 
    }
}
