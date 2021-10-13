using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPassive2 : CommonPassive
{
    private CommonPassive _commonPassive;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
    }
    public override void ExecutePassive()
    {
        base.ExecutePassive();
        // If I want to increase a certain resource's production
        // I could run through all of them , check which ones are unlocked, and then choose one of those that have been unlocked in the previous run
        // And the nrandomly select one of them to increase.
        // It'll be weird if you can increase a resource you've never even seen before.
        // The problem with this is, it dilutes the pool a bit, which might just be fine.
        // And I'll have the set the description inside the for loop.
        // Resources might need a new variable, to check if it was unlocked. Or I should check which ones were unlocked when the game resets
        // And then just add them to a list, and then loop through them here. And as soon as the loop here finishes, I can remove them?

        
        Resource.Resources[Prestige.resourcesUnlockedInPreviousRun[_index]].amountPerSecond += 0.12f;

        // And this also needs to instantiate the resourceinfo prefab
    }
    public override void GenerateRandomResource()
    {
        base.GenerateRandomResource();

        description = string.Format("Increases {0}'s production by 0.12/sec", Prestige.resourcesUnlockedInPreviousRun[_index].ToString());

        // Then also, do I want to increase it via percentage, so this is currently just a flat amount
        // But if I do it percentage, the percentage will get recalculated everytime the amountpersecond changed.
        // So 1% of 10/sec will be 0.1/sec but 1% of 100 will be 1/sec. So it will scale higher with time.
        // So the player will have to do those calculation themselves.
        // But this will definitely make me modify some code in different places
        // (everywhere where amountpersecond gets modified)

        // Would also be really nice, if the increase in amountpersecond only started happening when that specific 
        // resource gets unlocked again in the next run.
    }
}
