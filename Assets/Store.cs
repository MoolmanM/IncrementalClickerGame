using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.UI;

// ODDS

// Common 80%
// Uncommon 17.5%   <97.5%
// Rare 1.8%        >2.5%
// Epic 0.6%
// Legendary 0.1%

// Common 72.5%
// Uncommon 22.5%   <95%
// Rare 3.7%        >5%
// Epic 1%
// Legendary 0.3%

// Common 60%
// Uncommon 30%     <90%
// Rare 6%          >10%
// Epic 3%
// Legendary 1%

// Common 30%
// Uncommon 50%     <80%
// Rare 11.25%      >20%
// Epic 6.25%
// Legendary 2.5%

// Common 15%
// Uncommon 55%     <70%
// Rare 12.5%       >30%
// Epic 7.5%
// Legendary 10%

// IDEA FOR PRESTIGE
// Instead of creating auto bot to pick buffs for you
// Just back it so that the player who doesn't want to choose can just press a button and it automatically buys chests 
// And doesn't need him to pick any of the buffs.

// Create gameobject for each rarity?
// Generate one common stat?
// What should I call these things, additionally what should I call the prestige ones then.
// Stats, Traits? Passives?

// I need to have a list of buffs for each rarity
// Then when you buy a box with 10 buffs, you have to spawn the 10 buffs, then determine the rarities that you'll get within those 10
// Then within those rarities you need to randomly roll which buff exactly you'll get.
public class Store : MonoBehaviour
{
    public int gemsAmount = 500;
    public TMP_Text txtGems;
    public Rarity[] _rarity;
    private float commonAmount, uncommonAmount, rareAmount, epicAmount, legendaryAmount;
    public int testAmount;
    public GameObject content;
    public GameObject commonPrefab, uncommonPrefab, rarePrefab, epicPrefab, legendaryPrefab;

    public void Start()
    {
        txtGems.text = gemsAmount.ToString();
    }
    public IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TValue> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while (true)
        {
            yield return values[UnityEngine.Random.Range(0, size)];
        }
    }
    public void GenerateRandomBuff(int amountToRoll)
    {
        for (int i = 0; i < amountToRoll; i++)
        {
            float randomNumberGenerated = Random.Range(0f, 100f);

            for (int p = 0; p < _rarity.Length; p++)
            {
                if (randomNumberGenerated <= _rarity[0].randomChance)
                {
                    GenerateRandomLegendaryPassive();
                    // By generate I only mean fill in the text field of the buff page
                    // And then have the buy button next to it.
                    // Then probably add every buff the player bought to a list
                    // And then after confirming their purchases they all will run their function
                    // That's for prestige
                    // For shop it will happen instantly.
                    break;
                }
                else if (randomNumberGenerated <= _rarity[1].randomChance)
                {
                    GenerateRandomEpicPassive();
                    break;
                }
                else if (randomNumberGenerated <= _rarity[2].randomChance)
                {
                    GenerateRandomRarePassive();
                    break;
                }
                else if (randomNumberGenerated <= _rarity[3].randomChance)
                {
                    GenerateRandomUncommonPassive();
                    break;
                }
                else if (randomNumberGenerated <= _rarity[4].randomChance)
                {
                    GenerateRandomCommonPassive();
                    break;
                }
            }

        }
    }
    public void InitializeRarityChances(float legendaryChance, float epicChance, float rareChance, float uncommonChance)
    {
        _rarity = new Rarity[5];

        _rarity[0].randomChance = legendaryChance;
        _rarity[0].Type = RarityType.Legendary;
        _rarity[0].passiveCost = 5;

        _rarity[1].randomChance = epicChance;
        _rarity[1].Type = RarityType.Epic;
        _rarity[1].passiveCost = 4;

        _rarity[2].randomChance = rareChance;
        _rarity[2].Type = RarityType.Rare;
        _rarity[2].passiveCost = 3;

        _rarity[3].randomChance = uncommonChance;
        _rarity[3].Type = RarityType.Uncommon;
        _rarity[3].passiveCost = 2;

        _rarity[4].randomChance = 100f;
        _rarity[4].Type = RarityType.Common;
        _rarity[4].passiveCost = 1;
    }
    public void OnCommonChest()
    {
        int amountToRoll = 2;
        InitializeRarityChances(0.1f, 0.7f, 2.5f, 20f);      
        GenerateRandomBuff(amountToRoll);
    }
    public void OnUncommonChest()
    {
        int amountToRoll = 4;
        InitializeRarityChances(0.3f, 1.3f, 5f, 27.5f);      
        GenerateRandomBuff(amountToRoll);
    }
    public void OnRareChest()
    {
        int amountToRoll = 6;
        InitializeRarityChances(1f, 4f, 10f, 40f);           
        GenerateRandomBuff(amountToRoll);
    }
    public void OnEpicChest()
    {
        int amountToRoll = 8;
        InitializeRarityChances(2.5f, 8.75f, 20f, 70f);      
        GenerateRandomBuff(amountToRoll);
    }
    public void OnLegendaryChest()
    {
        int amountToRoll = 10;
        InitializeRarityChances(10f, 17.5f, 30f, 85f);
        GenerateRandomBuff(amountToRoll);
    }
    private void GenerateRandomCommonPassive()
    {
        foreach (var value in RandomValues(CommonPassive.CommonPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(commonPrefab, content.GetComponent<Transform>());
            value.InitializePermanentStat();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            txtName.text = value.description;
        }
    }
    private void GenerateRandomUncommonPassive()
    {
        // Modifying the 'take' amount is just how many values you want to randomize.
        foreach (var value in RandomValues(UncommonPassive.UncommonPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(uncommonPrefab, content.GetComponent<Transform>());
            value.InitializePermanentStat();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            txtName.text = value.description;
        }
    }
    private void GenerateRandomRarePassive()
    {
        // Modifying the 'take' amount is just how many values you want to randomize.
        foreach (var value in RandomValues(RarePassive.RarePassives).Take(1))
        {
            GameObject prefabObj = Instantiate(rarePrefab, content.GetComponent<Transform>());
            value.InitializePermanentStat();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            txtName.text = value.description;
        }
    }
    private void GenerateRandomEpicPassive()
    {
        // Okay so now each passive that gets generated needs to be assigned to one of the text fields.
        // It also needs to instantiate the correct one i.e the correct color, as well as animation
        // I will worry about attaching the correct animation at a later date.

        // Modifying the 'take' amount is just how many values you want to randomize.
        foreach (var value in RandomValues(EpicPassive.EpicPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(epicPrefab, content.GetComponent<Transform>());
            value.InitializePermanentStat();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            txtName.text = value.description;
        }
    }
    private void GenerateRandomLegendaryPassive()
    {
        // Modifying the 'take' amount is just how many values you want to randomize.
        foreach (var value in RandomValues(LegendaryPassive.LegendaryPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(legendaryPrefab, content.GetComponent<Transform>());
            value.InitializePermanentStat();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            txtName.text = value.description;
        }
    }
    public void ClearContent()
    {
        foreach (Transform transformChild in content.GetComponent<Transform>())
        {
            Destroy(transformChild.gameObject);
        }
    }
    public void Test()
    {
        // Common 60%
        // Uncommon 30%
        // Rare 6%
        // Epic 3%
        // Legendary 1%
        commonAmount = 0;
        uncommonAmount = 0;
        rareAmount = 0;
        epicAmount = 0;
        legendaryAmount = 0;

        //_rarity = null;
        _rarity = new Rarity[5];

        _rarity[0].randomChance = 1f;
        _rarity[0].Type = RarityType.Legendary;
        _rarity[0].passiveCost = 5;

        _rarity[1].randomChance = 4f;
        _rarity[1].Type = RarityType.Epic;
        _rarity[1].passiveCost = 4;

        _rarity[2].randomChance = 10f;
        _rarity[2].Type = RarityType.Rare;
        _rarity[2].passiveCost = 3;

        _rarity[3].randomChance = 40f;
        _rarity[3].Type = RarityType.Uncommon;
        _rarity[3].passiveCost = 2;

        _rarity[4].randomChance = 100f;
        _rarity[4].Type = RarityType.Common;
        _rarity[4].passiveCost = 1;

        for (int i = 0; i < testAmount; i++)
        {
            float randomNumberGenerated = Random.Range(0f, 100f);

            for (int p = 0; p < _rarity.Length; p++)
            {
                if (randomNumberGenerated <= _rarity[0].randomChance)
                {
                    legendaryAmount++;
                    break;
                }
                else if (randomNumberGenerated <= _rarity[1].randomChance)
                {
                    epicAmount++;
                    break;
                }
                else if (randomNumberGenerated <= _rarity[2].randomChance)
                {
                    rareAmount++;
                    break;
                }
                else if (randomNumberGenerated <= _rarity[3].randomChance)
                {
                    uncommonAmount++;
                    break;
                }
                else if (randomNumberGenerated <= _rarity[4].randomChance)
                {
                    commonAmount++;
                    break;
                }
            }

        }

        float commonPercentage = (commonAmount / testAmount) * 100;
        float uncommonPercentage = (uncommonAmount / testAmount) * 100;
        float rarePercentage = (rareAmount / testAmount) * 100;
        float epicPercentage = (epicAmount / testAmount) * 100;
        float legendaryPercentage = (legendaryAmount / testAmount) * 100;

        Debug.Log(string.Format("Common: {0:0.00}%", commonPercentage));
        Debug.Log(string.Format("Uncommon: {0:0.00}%", uncommonPercentage));
        Debug.Log(string.Format("Rare: {0:0.00}%", rarePercentage));
        Debug.Log(string.Format("Epic: {0:0.00}%", epicPercentage));
        Debug.Log(string.Format("Legendary: {0:0.00}%", legendaryPercentage));
    }
}
