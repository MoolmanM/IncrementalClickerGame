using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class cPassive4 : CommonPassive
{
    private CommonPassive _commonPassive;

    public static List<BuildingType> buildingTypesSelfCountToModify = new List<BuildingType>();
    private uint _selfCountAmount = 5;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
    }
    public override void ExecutePassive()
    {
        base.ExecutePassive();
        // Debug.Log("This for for Common 4 specifically");
        // Remember there has to be bad passives too. 

        // This should be something along the lines of start next run with x amount of buildings, workers or just resources.
        // Starting with x amount of buildings could be a very good one, because then you negate the exponential growth of the cost amount of that building.
        // Once again, all of this should only happen once that specific object, is actually unlocked in the next run.

        //With buildings maybe common should start next run with a random building
        // set to one selfcount .Where legendary is at 5 selfcount
        // It needs to also be a previous building that you've unlocked in a previous run.
        // How do I only 'activate' the passive only once when the building is unlocked in the next run?

        // I can have an extra method inside this passive, and then in the building I can run through a the CommonPassive class
        // And then activate that passive
        // Actually no, I don't think that will work, since the building will be randomly generated.
        // Unless I do that in every building

        // Unless I have a function that just runs in the unlocking script
        // Where it checks if the building in the list I will create here is equal to the type of that building that was unlocked.

        foreach (var buildingType in buildingTypesSelfCountToModify)
        {
            Building.Buildings[buildingType].SetSelfCount(_selfCountAmount);
        }
        
    }

    public override void GenerateRandomBuilding()
    {
        base.GenerateRandomBuilding();

        _index = Random.Range(0, Prestige.buildingsUnlockedInPreviousRun.Count);
        description = string.Format("Start next run with {0} of {1}, when you unlock it ", _selfCountAmount, Prestige.buildingsUnlockedInPreviousRun[_index].ToString());
    }
}