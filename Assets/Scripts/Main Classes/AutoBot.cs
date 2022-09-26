using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBot : MonoBehaviour
{
    private float _timer = 0.1f;

    private void BuyPurcahseable()
    {
        //foreach (var kvp in Researchable.Researchables)
        //{
        //    if (kvp.Value.isPurchaseable && kvp.Value.isUnlocked)
        //    {
        //        kvp.Value.OnResearch();
        //    }
        //}

        //foreach (var kvp in Craftable.Craftables)
        //{
        //    if (kvp.Value.isPurchaseable && kvp.Value.isUnlocked)
        //    {
        //        kvp.Value.OnCraft();
        //    }
        //}

        foreach (var kvp in Building.Buildings)
        {
            if (kvp.Value.isPurchaseable && kvp.Value.isUnlocked)
            {
                kvp.Value.OnBuild();
            }
        }
    }
    void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = 0.1f;

            BuyPurcahseable();
        }
    }
}
