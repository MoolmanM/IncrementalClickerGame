using BreakInfinity;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[System.Serializable]
public struct Rarity
{
    public RarityType Type;
    public float randomChance;
    public float passiveCost;
}
public enum RarityType
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
public class Prestige : MonoBehaviour
{
    public static float prestigePoints = 10;
    public GameObject[] nodes;
    public Rarity[] _rarity;
    public static TMP_Text txtPoints;
    private Transform tformTxtPoints;
    public GameObject objPrestige;

    public double testNumber, debugAmount;

    public static List<ResourceType> resourcesUnlockedInPreviousRun = new List<ResourceType>();
    public static List<BuildingType> buildingsUnlockedInPreviousRun = new List<BuildingType>();

    public IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TValue> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while (true)
        {
            yield return values[UnityEngine.Random.Range(0, size)];
        }
    }
    private void GenerateRandomCommonPassive(TMP_Text txtDescription, GameObject objExpand)
    {
        foreach (var value in RandomValues(CommonPassive.CommonPassives).Take(1))
        {
            value.GenerateRandomResource();
            value.GenerateRandomBuilding();
            txtDescription.text = string.Format("{0}", value.description);
            objExpand.GetComponent<Button>().onClick.AddListener(value.ExecutePassive);
        }
    }
    private void GenerateRandomUncommonPassive(TMP_Text txtDescription, GameObject objExpand)
    {
        // Modifying the 'take' amount is just how many values you want to randomize.
        foreach (var value in RandomValues(UncommonPassive.UncommonPassives).Take(1))
        {
            txtDescription.text = string.Format("{0}", value.description);
            objExpand.GetComponent<Button>().onClick.AddListener(value.ExecutePassive);
        }
    }
    private void GenerateRandomRarePassive(TMP_Text txtDescription, GameObject objExpand)
    {
        // Modifying the 'take' amount is just how many values you want to randomize.
        foreach (var value in RandomValues(RarePassive.RarePassives).Take(1))
        {
            txtDescription.text = string.Format("{0}", value.description);
            objExpand.GetComponent<Button>().onClick.AddListener(value.ExecutePassive);
        }
    }
    private void GenerateRandomEpicPassive(TMP_Text txtDescription, GameObject objExpand)
    {
        // Modifying the 'take' amount is just how many values you want to randomize.
        foreach (var value in RandomValues(EpicPassive.EpicPassives).Take(1))
        {
            txtDescription.text = string.Format("{0}", value.description);
            objExpand.GetComponent<Button>().onClick.AddListener(value.ExecutePassive);
        }
    }
    private void GenerateRandomLegendaryPassive(TMP_Text txtDescription, GameObject objExpand)
    {
        // Modifying the 'take' amount is just how many values you want to randomize.
        foreach (var value in RandomValues(LegendaryPassive.LegendaryPassives).Take(1))
        {
            txtDescription.text = string.Format("{0}", value.description);
            objExpand.GetComponent<Button>().onClick.AddListener(value.ExecutePassive);
        }
    }
    private void Start()
    {
        //Debug.Log(new BigDouble(105203122911321275.6).ToString("G1"));
        //Debug.Log(new BigDouble(105203122911321275.6).ToString("E0"));
        //Debug.Log(new BigDouble(105203122911321275.6).ToString("E4"));
        //Debug.Log(new BigDouble(10000).ToString("G1"));

        tformTxtPoints = transform.Find("txtPoints");
        txtPoints = tformTxtPoints.gameObject.GetComponent<TMP_Text>();
        txtPoints.text = string.Format("Prestige Points: {0}", prestigePoints);

        objPrestige.SetActive(false);

        _rarity = new Rarity[5];

        _rarity[0].randomChance = 1f;
        _rarity[0].Type = RarityType.Legendary;
        _rarity[0].passiveCost = 5;

        _rarity[1].randomChance = 3f;
        _rarity[1].Type = RarityType.Epic;
        _rarity[1].passiveCost = 4;

        _rarity[2].randomChance = 10f;
        _rarity[2].Type = RarityType.Rare;
        _rarity[2].passiveCost = 3;

        _rarity[3].randomChance = 35f;
        _rarity[3].Type = RarityType.Uncommon;
        _rarity[3].passiveCost = 2;

        _rarity[4].randomChance = 100f;
        _rarity[4].Type = RarityType.Common;
        _rarity[4].passiveCost = 1;
    }
    [Button]
    private void InitializePassiveTree()
    {
        // Here everything needs to be set back to zero.
        objPrestige.SetActive(true);
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

            for (int p = 0; p < _rarity.Length; p++)
            {
                if (randomNumberGenerated <= _rarity[0].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Legendary;
                    node.Value.passiveCost = _rarity[0].passiveCost;
                    imgNode.color = legendaryColor;

                    GenerateRandomLegendaryPassive(node.Value.txtDescription, node.Value.objExpand);
                    // Execute selecting legendary passives
                    break;
                }
                else if (randomNumberGenerated <= _rarity[1].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Epic;
                    node.Value.passiveCost = _rarity[1].passiveCost;
                    imgNode.color = epicColor;

                    GenerateRandomEpicPassive(node.Value.txtDescription, node.Value.objExpand);
                    break;
                }
                else if (randomNumberGenerated <= _rarity[2].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Rare;
                    node.Value.passiveCost = _rarity[2].passiveCost;
                    imgNode.color = rareColor;

                    GenerateRandomRarePassive(node.Value.txtDescription, node.Value.objExpand);
                    break;
                }
                else if (randomNumberGenerated <= _rarity[3].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Uncommon;
                    node.Value.passiveCost = _rarity[3].passiveCost;
                    imgNode.color = uncommonColor;

                    GenerateRandomUncommonPassive(node.Value.txtDescription, node.Value.objExpand);
                    break;
                }
                else if (randomNumberGenerated <= _rarity[4].randomChance)
                {
                    node.Value.associatedRarityType = RarityType.Common;
                    node.Value.passiveCost = _rarity[4].passiveCost;
                    imgNode.color = commonColor;

                    GenerateRandomCommonPassive(node.Value.txtDescription, node.Value.objExpand);
                    //CommonPassive(node.Value.txtDescription, node.Value.txtCost, node.Value.passiveCost);
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
    [Button]
    private void ResetGame()
    {
        foreach (var building in Building.Buildings)
{
            if (building.Value.isUnlocked)
            {
                buildingsUnlockedInPreviousRun.Add(building.Key);
            }

            building.Value.ResetBuilding();
        }
        foreach (var craftable in Craftable.Craftables)
        {
            craftable.Value.ResetCraftable();
        }
        foreach (var researchable in Researchable.Researchables)
        {
            researchable.Value.ResetResearchable();
        }
        foreach (var worker in Worker.Workers)
        {
            worker.Value.ResetWorker();
        }
        foreach (var resource in Resource.Resources)
        {
            if (resource.Value.isUnlocked)
            {
                resourcesUnlockedInPreviousRun.Add(resource.Key);
            }

            resource.Value.ResetResource();
        }
        Worker.TotalWorkerCount = 0;
        Worker.UnassignedWorkerCount = 0;

        Researchable.researchSimulAllowed = 1;
        Researchable.hasReachedMaxSimulResearch = false;
        Researchable.researchSimulActive = 0;

        Resource.Resources[ResourceType.Food].isUnlocked = true;
        Resource.Resources[ResourceType.Food].objMainPanel.SetActive(true);
        Resource.Resources[ResourceType.Food].objSpacerBelow.SetActive(true);

        Resource.Resources[ResourceType.Lumber].isUnlocked = true;
        Resource.Resources[ResourceType.Lumber].objSpacerBelow.SetActive(true);
        Resource.Resources[ResourceType.Lumber].objMainPanel.SetActive(true);

        PointerNotification.leftAmount = 0;
        PointerNotification.rightAmount = 0;
        PointerNotification.lastLeftAmount = 0;
        PointerNotification.lastRightAmount = 0;
        PointerNotification.HandleLeftAnim();
        PointerNotification.HandleRightAnim();

        TimeManager.ResetSeason();
        //Cancel any ongoing research.

    }
    [Button]
    private void IncreaseNumber()
    {
        testNumber += debugAmount;

        Debug.Log(string.Format("{0} is: {1}",testNumber, NumberToLetter.FormatNumber(testNumber)));
    }
    public void OnDone()
    {
        objPrestige.SetActive(false);
        NodeClass.isFirstPurchase = true;
        NodeClass.neighbourCache = null;
        NodeClass.connectionCache = null;
    }
}
