using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct PassivePair
{
    public float randomChance;
    public string passiveName;
}

public class Prestige : MonoBehaviour
{
    public GameObject prefabLegendary, prefabEpic, prefabUncommon, prefabCommon;
    public GameObject[] nodes;
    public PassivePair[] passivePair;
    private int p;
    public int amountOfSimulations;

    public int amountCommon, amountUncommon, amountEpic, amountLegendary;

    // Maybe make a massive grid, so you can randomly spawn the elements? 
    // It seems more and more like a very tough thing to do, maybe just have them at static position but have the
    // the nodes themself be random?

    // Okay so I want to simulate a lot of these without creating actual game objects and just having a log tell me how many common, uncommon, epic and legendary were generated.

    private void Start()
    {
        passivePair = new PassivePair[4];

        passivePair[0].randomChance = 2f;
        passivePair[0].passiveName = "Legendary";

        passivePair[1].randomChance = 5f;
        passivePair[1].passiveName = "Epic";

        passivePair[2].randomChance = 35f;
        passivePair[2].passiveName = "Uncommon";

        passivePair[3].randomChance = 55f;
        passivePair[3].passiveName = "Common";

        //GenerateRandomPassive();
        InitializePrefabs();
    }
    [Button(ButtonSizes.Small)]
    private void InitializePrefabs()
    {
        //for (int i = 0; i < nodes.Length; i++)
        //{
        //    float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);

        //    for (int p = 0; p < passivePair.Length; p++)
        //    {
        //        if (randomNumberGenerated <= passivePair[p].randomChance)
        //        {
        //            if (p == 0)
        //            {
        //                Debug.Log("Reached legendary");
        //                Instantiate(prefabLegendary, nodes[i].GetComponent<Transform>());
        //                break;
        //            }
        //            else if (p == 1)
        //            {
        //                Debug.Log("Reached Epic");
        //                Instantiate(prefabEpic, nodes[i].GetComponent<Transform>());
        //                break;
        //            }
        //            else if (p == 2)
        //            {
        //                Debug.Log("Reached Uncommon");
        //                Instantiate(prefabUncommon, nodes[i].GetComponent<Transform>());
        //                break;
        //            }
        //            else if (p == 3)
        //            {
        //                Debug.Log("Reached Common");
        //                Instantiate(prefabCommon, nodes[i].GetComponent<Transform>());
        //                break;
        //            }
        //            else
        //            {
        //                Debug.Log("Reached Common");
        //                Instantiate(prefabCommon, nodes[i].GetComponent<Transform>());
        //                break;
        //            }
        //        }
        //    }
        //}

        for (int i = 0; i < nodes.Length; i++)
        {
            float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);

            for (int p = 0; p < passivePair.Length; p++)
            {
                if (randomNumberGenerated <= passivePair[0].randomChance)
                {
                    Debug.Log("Reached Legendary");
                    Instantiate(prefabLegendary, nodes[i].GetComponent<Transform>());
                    break;
                }
                else if (randomNumberGenerated <= passivePair[1].randomChance)
                {
                    Debug.Log("Reached Epic");
                    Instantiate(prefabEpic, nodes[i].GetComponent<Transform>());
                    break;
                }
                else if (randomNumberGenerated <= passivePair[2].randomChance)
                {
                    Debug.Log("Reached Uncommon");
                    Instantiate(prefabUncommon, nodes[i].GetComponent<Transform>());
                    break;
                }
                else if (randomNumberGenerated <= passivePair[3].randomChance)
                {
                    Debug.Log("Reached Common");
                    Instantiate(prefabCommon, nodes[i].GetComponent<Transform>());
                    break;
                }
                else
                {
                    //Instantiate(prefabCommon, nodes[i].GetComponent<Transform>());
                    //Debug.Log("Reached else");
                    ////break;
                    ///

                    //p = 0;
                }
            }

        }
    }

    [Button(ButtonSizes.Small)]
    private void GenerateRandomPassive()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            while (nodes[i].GetComponent<TMP_Text>().text == "")
            {
                for (int p = 0; p < passivePair.Length; p++)
                {

                    float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);

                    if (randomNumberGenerated <= passivePair[p].randomChance)
                    {
                        Debug.Log(randomNumberGenerated + " This passive: " + passivePair[p].passiveName);
                        nodes[i].GetComponent<TMP_Text>().text = passivePair[p].passiveName;
                    }
                    else if (p == passivePair.Length)
                    {
                        p = 0;
                    }

                }
            }

        }
    }

    [Button(ButtonSizes.Small)]
    private void SimulateRandomPassives()
    {
        amountCommon = 0;
        amountUncommon = 0;
        amountEpic = 0;
        amountLegendary = 0;

        for (int i = 0; i < amountOfSimulations; i++)
        {
            for (int p = 0; p < passivePair.Length; p++)
            {
                float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);

                if (randomNumberGenerated <= passivePair[p].randomChance)
                {
                    if (p == 0)
                    {
                        amountLegendary++;
                    }
                    else if (p == 1)
                    {
                        amountEpic++;
                    }
                    else if (p == 2)
                    {
                        amountUncommon++;
                    }
                    else if (p == 3)
                    {
                        amountCommon++;
                    }
                }
                else if (p == passivePair.Length)
                {
                    p = 0;
                }
            }
        }
        // I'm happy with simulation results.

        // So now i need to decide if I want to randomly spawn nodes on a tree.
        // Or have all the nodes already there and then just assign the passives randomly to these nodes.
        // And of course somewhere in between handle the rarities of these passives.

        // If passives are randomly generated. Make sure everytime the player prestiges, that their previous passives disappear
        // So the player will have one global static amount of 'prestige points', and everytime they prestige, this amount gets incremented.
        // Meaning with every prestige they will have a larger amount of points that they can then spend on the passives.

        // One issue I can think if I randomly spawn passive nodes, what if the very first node in tree.
        // No wait, this shouldn't be an issue as long as the tree doesn't start from one single point.
        // Make it more PoE style.

        // Going to have to designate certain starting areas, or just make it so that where they start
        // Every passive from that point on needs to be connected.

        // Have a prefab object for every rarity. And if the rarity gets rolled,
        // Instantiate the associated prefab.

        // And just like a placeholder gameobject for every node.
        Debug.Log("Legendary Percentage: " + ((float)amountLegendary / (float)amountOfSimulations * 100f) + " Epic Percentage: " + ((float)amountEpic / (float)amountOfSimulations * 100f) + " Uncommon Percentage: " + ((float)amountUncommon / (float)amountOfSimulations * 100f) + " Common Percentage: " + ((float)amountCommon / (float)amountOfSimulations * 100f));
    }
}
