using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneAxe : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();
    }

    void Start()
    {
        // I think it's to early to unlock another building related to woodcutting.
        // Maybe make it so that for every woodcutter they can get 1 log per.... minute? 30 seconds? 1 second? need some more thinking on this.
        SetDescriptionText("Enables collection of logs after woodcutting, maybe even unlocks a new woodcutting related building.");
    }
    protected override void OnCraft()
    {
        bool canPurchase = true;

        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resourceCost[i].currentAmount < resourceCost[i].costAmount)
            {
                canPurchase = false;
                break;
            }
        }

        if (canPurchase)
        {
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
            }

            isCrafted = true;
            Crafted();
            //ModifyWorker();
        }
    }

    //private void ModifyWorker()
    //{
    //    foreach (var kvp in Worker.Workers)
    //    {
    //        if (kvp.Key == WorkerType.Woodcutters)
    //        {
    //            kvp.Value.ModifyMultiplier();
    //        }
    //    }
    //}
}
