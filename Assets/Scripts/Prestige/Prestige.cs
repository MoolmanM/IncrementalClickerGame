using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

[System.Serializable]
public struct Rarity
{
    public RarityType Type;
    public float randomChance;
    public float passiveCost;
    public Button buyButton;

    public Rarity(RarityType lastType, int lastPassiveCost, Button lastBuyButton) : this()
    {
        Type = lastType;
        passiveCost = lastPassiveCost;
        buyButton = lastBuyButton;
    }
}
public enum RarityType
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
[System.Serializable]
public struct TestButtons
{
    public Button button;

    public TestButtons(Button lastButton)
{
        button = lastButton;
    }
}

public class Prestige : MonoBehaviour
{
    public static float prestigePoints = 2;
    public static List<ResourceType> resourcesUnlockedInPreviousRun = new List<ResourceType>();
    public static List<BuildingType> buildingsUnlockedInPreviousRun = new List<BuildingType>();
    public static List<WorkerType> workersUnlockedInPreviousRun = new List<WorkerType>();
    public static List<CraftingType> craftablesUnlockedInPreviousRun = new List<CraftingType>();
    public static List<ResearchType> researchablesUnlockedInPreviousRun = new List<ResearchType>();

    public Rarity[] _rarity;
    public GameObject content;
    public GameObject commonPrefab, uncommonPrefab, rarePrefab, epicPrefab, legendaryPrefab;
    public GameObject objOpeningPanel;

    //public Dictionary<RarityType, Button> prestigeBox = new Dictionary<RarityType, Button>();
    public List<Rarity> prestigeBox = new();
    public List<TestButtons> testButtonList = new();
 
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
    public void OnPrestigeChest()
    {
        int amountToRoll = 10;
        InitializeRarityChances(1f, 4f, 10f, 40f);
        GenerateRandomBuff(amountToRoll);
    }
    private void GenerateRandomCommonPassive()
    {
        foreach (var value in RandomValues(CommonPassive.CommonPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(commonPrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            value.InitializePermanentStat();

            if (prestigePoints < 1)
            {
                buyButton.interactable = false;
            }
            
            txtBuyAmount.text = "1";
            txtName.text = value.description;
            prestigeBox.Add(new Rarity(RarityType.Common, 1, buyButton));
            //testButtonList.Add(new TestButtons(buyButton));
            //testButtonList.Add(new TestButtons(buyButton));
            //buyButton.onClick.AddListener(value.InitializePermanentStat);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(1); });
        }
    }
    private void GenerateRandomUncommonPassive()
    {
        foreach (var value in RandomValues(UncommonPassive.UncommonPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(uncommonPrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            if (prestigePoints < 2)
            {
                buyButton.interactable = false;
            }

            txtBuyAmount.text = "2";
            txtName.text = value.description;
            prestigeBox.Add(new Rarity(RarityType.Uncommon, 2, buyButton));
            buyButton.onClick.AddListener(value.InitializePermanentStat);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(2); });
        }
    }
    private void GenerateRandomRarePassive()
    {
        foreach (var value in RandomValues(RarePassive.RarePassives).Take(1))
        {
            GameObject prefabObj = Instantiate(rarePrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();
            Debug.Log(buyButton);
            if (prestigePoints < 3)
            {
                buyButton.interactable = false;
            }

            txtBuyAmount.text = "3";
            txtName.text = value.description;
            prestigeBox.Add(new Rarity(RarityType.Rare, 3, buyButton));
            buyButton.onClick.AddListener(value.InitializePermanentStat);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(3); });
        }
    }
    private void GenerateRandomEpicPassive()
    {
        foreach (var value in RandomValues(EpicPassive.EpicPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(epicPrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            if (prestigePoints < 4)
            {
                buyButton.interactable = false;
            }

            txtBuyAmount.text = "4";
            txtName.text = value.description;
            prestigeBox.Add(new Rarity(RarityType.Epic, 4, buyButton));
            buyButton.onClick.AddListener(value.InitializePermanentStat);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(4); });
        }
    }
    private void GenerateRandomLegendaryPassive()
    {
        foreach (var value in RandomValues(LegendaryPassive.LegendaryPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(legendaryPrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            if (prestigePoints < 5)
            {
                buyButton.interactable = false;
            }

            txtBuyAmount.text = "5";
            txtName.text = value.description;
            prestigeBox.Add(new Rarity(RarityType.Legendary, 5, buyButton));
            buyButton.onClick.AddListener(value.InitializePermanentStat);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(5); });
        }
    }
    public void ClearContent()
    {
        foreach (Transform transformChild in content.GetComponent<Transform>())
        {
            Destroy(transformChild.gameObject);
        }
    }
    private void DeductPrestigePoints(int passiveCost)
    {
        prestigePoints -= passiveCost;

        foreach (var item in prestigeBox)
        {
            if (prestigePoints < item.passiveCost)
            {
                item.buyButton.interactable = false;
            }
        }
    }
    [Button]
    public void StartPrestige()
    {
        OnPrestigeChest();
        objOpeningPanel.SetActive(true);
    }
    [Button]
    public void DebugList()
    {
        foreach (var item in prestigeBox)
        {
            Debug.Log(item.buyButton + " " + item.passiveCost + " " + item.Type);
        }
    }
}
