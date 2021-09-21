using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PassivePair
{
    public RarityType Type;
    public float randomChance;
    public float passiveCost;
}

[System.Serializable]
public struct Common
{
    public string passiveDescription;
    public float randomChance;
}

public enum RarityType
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
// So now I need to generate certain passives per rarity, and also first test how to access the specific rarity from the node.

// they could all implement a Passive interface?
// And have a function in that interface called something like DoPassive

public class Prestige : MonoBehaviour
{
    public static float prestigePoints = 10;
    public GameObject prefabLegendary, prefabEpic, prefabRare, prefabUncommon, prefabCommon;
    public GameObject[] nodes;
    public PassivePair[] passivePair;
    public Common[] commonPassives;
    public static TMP_Text txtPoints;
    private Transform tformTxtPoints;

    // Underneath is just for simulation.
    private int p;
    public int amountOfSimulations;

    public int amountCommon, amountUncommon, amountEpic, amountLegendary;
    private int commonAmount0, commonAmount1, commonAmount2, commonAmount3, commonAmount4;

    private void Start()
    {
        commonPassives = new Common[5];

        // If I really want to do it this way, I'll have to only enable passives that has an effect on the player.
        // Like it would be weird if they could for example increase production of a resource that they won't discover for at least a couple
        // of runs in the future.
        float incrementAmount = 100 / commonPassives.Length;
        commonPassives[0].passiveDescription = "Passive 1";
        commonPassives[0].randomChance = 100 / commonPassives.Length;
        commonPassives[1].passiveDescription = "Passive 2";
        commonPassives[1].randomChance = (100 / commonPassives.Length) + incrementAmount;
        commonPassives[2].passiveDescription = "Passive 3";
        commonPassives[2].randomChance = (100 / commonPassives.Length) + incrementAmount * 2;
        commonPassives[3].passiveDescription = "Passive 4";
        commonPassives[3].randomChance = (100 / commonPassives.Length) + incrementAmount * 3;
        commonPassives[4].passiveDescription = "Passive 5";
        commonPassives[4].randomChance = (100 / commonPassives.Length) + incrementAmount * 4;

        // Surely there has to be an easier way to check if the randomness meets the requirements.

        tformTxtPoints = transform.Find("txtPoints");
        txtPoints = tformTxtPoints.gameObject.GetComponent<TMP_Text>();
        txtPoints.text = string.Format("Prestige Points: {0}", prestigePoints);

        passivePair = new PassivePair[5];

        passivePair[0].randomChance = 1f;
        passivePair[0].Type = RarityType.Legendary;
        passivePair[0].passiveCost = 5;

        passivePair[1].randomChance = 3f;
        passivePair[1].Type = RarityType.Epic;
        passivePair[1].passiveCost = 4;

        passivePair[2].randomChance = 10f;
        passivePair[2].Type = RarityType.Rare;
        passivePair[2].passiveCost = 3;

        passivePair[3].randomChance = 35f;
        passivePair[3].Type = RarityType.Uncommon;
        passivePair[3].passiveCost = 2;

        passivePair[4].randomChance = 100f;
        passivePair[4].Type = RarityType.Common;
        passivePair[4].passiveCost = 1;

        InitializePassiveTree();
    }
    [Button(ButtonSizes.Small)]
    private void InitializePassiveTree()
    {
        // These colors will all of course be replaced with actual graphics/animations.
        // I'm thinking legendary needs to have this moving rainbow animation on it.

        Color commonColor = new Color(0.2830189f, 0.2830189f, 0.2830189f, 1);
        Color uncommonColor = new Color(0.02491992f, 0.754717f, 0.109252f, 1);
        Color rareColor = new Color(0, 0.2846837f, 0.764151f, 1);
        Color epicColor = new Color(0.5033384f, 0.01935742f, 0.8207547f, 1);
        Color legendaryColor = new Color(1, 0.8730429f, 0, 1);

        foreach (var node in NodeClass.Nodes)
        {
            Transform rarityObj = node.Key.transform.Find("Rarity");
            Image imgNode = rarityObj.gameObject.GetComponent<Image>();

            float randomNumberGenerated = UnityEngine.Random.Range(0f, 100f);

            for (int p = 0; p < passivePair.Length; p++)
            {
                if (randomNumberGenerated <= passivePair[0].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Legendary;
                    node.Value.passiveCost = passivePair[0].passiveCost;
                    imgNode.color = legendaryColor;
                    // Execute selecting legendary passives
                    break;
                }
                else if (randomNumberGenerated <= passivePair[1].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Epic;
                    node.Value.passiveCost = passivePair[1].passiveCost;
                    imgNode.color = epicColor;
                    break;
                }
                else if (randomNumberGenerated <= passivePair[2].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Rare;
                    node.Value.passiveCost = passivePair[2].passiveCost;
                    imgNode.color = rareColor;
                    break;
                }
                else if (randomNumberGenerated <= passivePair[3].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Uncommon;
                    node.Value.passiveCost = passivePair[3].passiveCost;
                    imgNode.color = uncommonColor;
                    break;
                }
                else if (randomNumberGenerated <= passivePair[4].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Common;
                    node.Value.passiveCost = passivePair[4].passiveCost;
                    imgNode.color = commonColor;

                    CommonPassive(node.Value.txtDescription, node.Value.txtCost, node.Value.passiveCost);
                    // Execute randomizing common passives. 
                    // Then execute the function asociated with that passive.
                    // Need to give some thought about how I want to execute the function for each passive.
                    // Because it seems a bit silly having a single function for every single passive.
                    // My first thought is having a class for passives and using inheritance.
                    break;
                }
            }

        }
    }
    private void CommonPassive(TMP_Text txtDescription, TMP_Text txtCost, float passiveCost)
    {
        float randomNumber = Random.Range(0f, 100f);

        if (randomNumber <= commonPassives[0].randomChance)
        {
            txtDescription.text = commonPassives[0].passiveDescription;
            txtCost.text = string.Format("Cost: {0}", passiveCost);
            //commonPassives[0].passive.Passive0();
        }
        else if (randomNumber <= commonPassives[1].randomChance)
        {
            txtDescription.text = commonPassives[1].passiveDescription;
            txtCost.text = string.Format("Cost: {0}", passiveCost);
        }
        else if (randomNumber <= commonPassives[2].randomChance)
        {
            txtDescription.text = commonPassives[2].passiveDescription;
            txtCost.text = string.Format("Cost: {0}", passiveCost);
        }
        else if (randomNumber <= commonPassives[3].randomChance)
        {
            txtDescription.text = commonPassives[3].passiveDescription;
            txtCost.text = string.Format("Cost: {0}", passiveCost);
        }
        else
        {
            txtDescription.text = commonPassives[4].passiveDescription;
            txtCost.text = string.Format("Cost: {0}", passiveCost);
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

    [Button(ButtonSizes.Small)]
    private void SimulateCommonPassive()
    {
        for (int i = 0; i < amountOfSimulations; i++)
        {
            bool common0 = false;
            bool common1 = false;
            bool common2 = false;
            bool common3 = false;
            bool common4 = false;

            float randomNumber = Random.Range(0f, 100f);

            for (int u = 0; u < commonPassives.Length; u++)
            {
                if (randomNumber <= commonPassives[0].randomChance)
                {
                    common0 = true;
                }
                else if (randomNumber <= commonPassives[1].randomChance)
                {
                    common1 = true;
                }
                else if (randomNumber <= commonPassives[2].randomChance)
                {
                    common2 = true;
                }
                else if (randomNumber <= commonPassives[3].randomChance)
                {
                    common3 = true;
                }
                else
                {
                    common4 = true;
                }
            }
            if (common0)
            {
                commonAmount0++;
            }
            else if (common1)
            {
                commonAmount1++;
            }
            else if (common2)
            {
                commonAmount2++;
            }
            else if (common3)
            {
                commonAmount3++;
            }
            else if (common4)
            {
                commonAmount4++;
            }
        }

        Debug.Log(string.Format("Percentage of 0: {0:0.00}%, ({1})", (float)commonAmount0 / (float)amountOfSimulations * 100, commonAmount0));
        Debug.Log(string.Format("Percentage of 1: {0:0.00}%, ({1})", (float)commonAmount1 / (float)amountOfSimulations * 100, commonAmount1));
        Debug.Log(string.Format("Percentage of 2: {0:0.00}%, ({1})", (float)commonAmount2 / (float)amountOfSimulations * 100, commonAmount2));
        Debug.Log(string.Format("Percentage of 3: {0:0.00}%, ({1})", (float)commonAmount3 / (float)amountOfSimulations * 100, commonAmount3));
        Debug.Log(string.Format("Percentage of 4: {0:0.00}%, ({1})", (float)commonAmount4 / (float)amountOfSimulations * 100, commonAmount4));
    }
}
