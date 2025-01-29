using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private uint overallBuildingBuilt;
    [Button]
    public void UpdateStats()
    {
        foreach (var resource in Resource.Resources)
        {
            if (resource.Value.trackedAmount > 0)
            {
                Debug.Log(resource.Value.Type + ": " + resource.Value.trackedAmount);
            }
        }
        foreach (var building in Building.Buildings)
        {
            if (building.Value.trackedBuiltAmount > 0)
            {
                overallBuildingBuilt += building.Value.trackedBuiltAmount;

                

                Debug.Log(building.Value.ActualName + " " + building.Value.trackedBuiltAmount);
            }
            
        }
        Debug.Log("overallBuildingBuilt buildings built: " + overallBuildingBuilt);
        Debug.Log("Overall workers had: " + Worker.trackedWorkerCount);
        overallBuildingBuilt = 0;
        // Remember to save these values to playerprefs and add the afk amount to these value as well.
        // Not needed to save them here, save them in the resources script.\
        // Should also track how many times you've built each building.
        // How many workers you've had worked for you over each prestige.
        // How many times you've researched certain stuff
        // And also an overallBuildingBuilt one for each thing for example
        // How many buildings you've built in general, how many times you've researched/crafted stuff in general.
        // How many ads watched?
        // How many gems collected?
        // How many times prestige
        // How much time spent in game.
        // So that will include getting tracked amounts for buildings, workers, research and crafting.
        // Also these tracked amounts should stay over prestiges.

        // FOR WORKERS

        // Track current amount of workers
        // Save total amount of amount of workers at the end of each prestige
        // then just take that total amount and += current amount
        // Should work

        // FOR RESEARCH

        // Track how many times you've researched in total.
        // Track how many times you've researched each thing?
        // Track total amount spent on research

        // FOR CRAFTING

        // Track how many times you've crafted in total.
        // Track how many times you've crafted each thing?


    }
}
