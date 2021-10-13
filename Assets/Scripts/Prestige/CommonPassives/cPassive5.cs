using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class cPassive5 : CommonPassive
{
    private CommonPassive _commonPassive;
    private float percentageAmount = 0.2f;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
    }
    public override void ExecutePassive()
    {
        base.ExecutePassive();
        //  Debug.Log("This for for Common 5 specifically");

        // I think I do need to increase resources using percentages eventually, I think it'll be a nice addition
        // To the prestige system.

        // This one is just going to have you start next run with a certain amount of some resources.

        Resource.Resources[Prestige.resourcesUnlockedInPreviousRun[_index]].SetInitialAmount(percentageAmount);
    }

    public override void GenerateRandomResource()
    {
        base.GenerateRandomResource();

        description = string.Format("{0} starts with 20% of max storage", Prestige.resourcesUnlockedInPreviousRun[_index].ToString());

        // Higher rarities can go up by 20%, so legendary will have 100%
    }
}
