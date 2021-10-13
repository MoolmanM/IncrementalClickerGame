using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPassive3 : CommonPassive
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
        // Debug.Log("This for for Common 3 specifically");

        // Should I just have it so that every passive does the same thing when the rarities get increases. But the amounts just get better.
        // And then the higher rarities maybe get some exclusives.

        // One of the higher rarity passives.
        // Can increase the amountpersecond of every single resource.
        // That has been unlocked in the previous run.

        // How would I go about increasing research speed?

        // Okay this research timer thing works perfectly.

        // So how should it scale between rarities. 
        // Common = 1,       5%
        // Uncommon = 2,       12%
        // Rare = 3,       18%
        // Epic = 4,       24%
        // Legendary = 5,       30%
        // This seems pretty good, I think it's good to have a max such as 30%, otherwise there might not be any time walls for the player.
        // Or maybe it should be possible? 
        // Maybe it should be possible to have a complimentary passives
        // Such as this one that decreases research speed by #%
        // And then another that says increase that by a further #%.
        // Dumb example but yeah

        // And then I still have to figure out what am I taking my prestige points of?
        // Like how do you gain prestige points
        // The easiest thing would be total amount of workers = prestige points.
        // Or I can put a weight on every single resource, and then add them together when resetting the game
        // and bob's your uncle.
        // Or I can use energy in some way
        // And then if I for example use workers, The player needs to unlock a specific point during a run that unlocks resetting.
        // Otherwise they would be able to manipulate the randomness of the prestige system
        // Especially since they are getting a full refund on the prestige points on every reset(Which is something I need to reconsider maybe)
        // Actually, it might be an easy fix, just don't refund their prestige points
        // Because whatever they do end up getting in the prestige tree
        // Will in some way, make the next run easier
        // And in turn they should be able to get for example more workers in the next run, than the previous.
        // This is of course going to require some thorough balancing
        // Like how much more prestige points do I want them to get on every run, on average
        // Like lets say they get their 1st run, 40 prestige points(40 workers)
        // And they only end up getting 53 for example on the next run, they will exponentially slow down in progress then.
        // Because then the run after that (On average, unless they luck out on some rare passive)
        // They might only get 58 workers total. Or worse case scenario Stay the same(53 workers)
        // So I need to implement a way for the players to always make some sort of progress on every run no matter
        // Even if it's just by a worker or two more
        // How do I gaurantee that?
        // Maybe I can keep track of the amount of total resets the player has done, and use that as a small multiplier
        // So it'll be workers * (numberOfResets / 100)
        // So if you have done 3 resets and you have 100 workers on the last run
        // It'll be 100 * (4 / 100) 
        // 100 * 0.04
        // 104, so they will get 104 prestige points, even though they only had 100 workers that last run.

        foreach (var research in Researchable.Researchables)
        {
            float percentageAmount = 0.05f;
            research.Value.ModifyTimeToCompleteResearch(percentageAmount);
        }
    }
}
