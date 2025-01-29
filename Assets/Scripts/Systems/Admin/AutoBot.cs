using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AutoBot : MonoBehaviour
{
    private float timer = 0.1f;
    public int craftUnlockedAmount, craftedAmount;

    private IEnumerator BuyPurchaseable()
    {
        craftUnlockedAmount = 0;
        foreach (var kvp in Craftable.Craftables)
        {
            if (kvp.Value.IsUnlocked)
            {
                craftUnlockedAmount += 1;
            }
        }

        craftUnlockedAmount -= craftedAmount;
        if (craftUnlockedAmount >= 1)
        {
            StartCoroutine(BuyCraftable());
            yield return new WaitForSeconds(0.02f);          
        }
        StartCoroutine(BuyBuilding());
        yield return new WaitForSeconds(0.02f);

        StartCoroutine(BuyResearchable());
        yield return new WaitForSeconds(0.02f);
    }
    private IEnumerator BuyBuilding()
    {
        foreach (var kvp in Building.Buildings)
        {
            if (kvp.Value.IsPurchaseable && kvp.Value.IsUnlocked)
            {
                kvp.Value.OnBuild();

                yield return new WaitForSeconds(0.02f);
                break;
            }
        }
    }
    private IEnumerator BuyResearchable()
    {
        foreach (var kvp in Researchable.Researchables)
        {
            if (kvp.Value.IsPurchaseable && kvp.Value.IsUnlocked && !kvp.Value.isResearched && !kvp.Value._isResearchStarted)
            {
                kvp.Value.OnResearch();
                
                yield return new WaitForSeconds(0.02f);
                break;
            }
        }
    }
    private IEnumerator BuyCraftable()
    {
        foreach (var kvp in Craftable.Craftables)
        {
            if (kvp.Value.IsPurchaseable && kvp.Value.IsUnlocked && !kvp.Value.isCrafted)
            {
                kvp.Value.OnCraft();
                craftedAmount++;
                yield return new WaitForSeconds(0.02f);
                break;
            }
        }
    }

    void Update()
    {
        if ((timer -= Time.deltaTime) <= 0)
        {
            timer = 0.3f;
            StartCoroutine(BuyPurchaseable());

        }
    }
}
