using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class PermanentStats : MonoBehaviour
{
    public float allWorkerMultiplierAmount;
    public float allBuildingMultiplierAmount;

    public float allCraftablesCostReduced;
    public float allResearchablesCostReduced;

    public Dictionary<CraftingType, float> craftableCostReduced = new Dictionary<CraftingType, float>();
    public Dictionary<ResearchType, float> researchableCostReduced = new Dictionary<ResearchType, float>();
    public Dictionary<BuildingType, float> buildingCostReduced = new Dictionary<BuildingType, float>();

    public Dictionary<WorkerType, float> workerMultiplierModified = new Dictionary<WorkerType, float>();
    public Dictionary<BuildingType, float> buildingMultiplierModified = new Dictionary<BuildingType, float>();

    public Dictionary<BuildingType, uint> buildingSelfCountModified = new Dictionary<BuildingType, uint>();

    public uint workerCountModified;
    public float researchTimeReductionAmount;
    public float storagePercentageAmount;

    public GameObject commonPrefab, content;

    public void InstantiateStatList()
    {
        if (allWorkerMultiplierAmount > 0)
        {
            InstantiateStat(string.Format("Increase all Workers multiplier by {0:0.00}%", allWorkerMultiplierAmount * 100));
        }
        if (allBuildingMultiplierAmount > 0)
        {
            InstantiateStat(string.Format("Increase all Buildings multiplier by {0:0.00}%", allBuildingMultiplierAmount * 100));
        }
        if (allCraftablesCostReduced > 0)
        {
            InstantiateStat(string.Format("Reduce the cost of all Craftables by {0:0.00}%", allCraftablesCostReduced * 100));
        }
        if (allResearchablesCostReduced > 0)
        {
            InstantiateStat(string.Format("Reduce the cost of all Researchables by {0:0.00}%", allResearchablesCostReduced * 100));
        }

        foreach (var item in craftableCostReduced)
        {
            InstantiateStat(string.Format("Crafting Recipe, {0}'s cost is reduced by: {1:0.00}%", item.Key, item.Value * 100));
        }

        foreach (var item in researchableCostReduced)
        {
            InstantiateStat(string.Format("Research Recipe, {0}'s cost is reduced by: {1:0.00}%", item.Key, item.Value * 100));
        }

        foreach (var item in buildingCostReduced)
        {
            InstantiateStat(string.Format("Research Recipe, {0}'s cost is reduced by: {1:0.00}%", item.Key, item.Value * 100));
        }

        foreach (var item in workerMultiplierModified)
        {
            InstantiateStat(string.Format("{0}'s multiplier is increased by {1:0.00}%", item.Key, item.Value * 100));
        }

        foreach (var item in buildingMultiplierModified)
        {
            InstantiateStat(string.Format("{0} is increased by {1:0.00}%", item.Key, item.Value * 100));
        }

        foreach (var item in Building.Buildings)
        {
            if (item.Value.initialSelfCount > 0)
            {
                InstantiateStat(string.Format("Start each run with an additional {0} {1}'s, when you unlock it", item.Value.initialSelfCount, item.Value.actualName));
                // And then once I create a class for the prestige stats, make it so that it only says:
                // "Start NEXT run with that amount of buildings, since it will change again after the next reset.
            }
        }

        if (workerCountModified > 1)
        {
            InstantiateStat(string.Format("Start each run with {0} additional workers", workerCountModified));
        }
        else
        {
            InstantiateStat(string.Format("Start each run with an additional worker"));
        }

        if (researchTimeReductionAmount > 0)
        {
            InstantiateStat(string.Format("Research time is reduced by: {0:0.00}%", researchTimeReductionAmount * 100));
        }
        if (storagePercentageAmount > 0)
        {
            InstantiateStat(string.Format("Storage limit is increased by: {0:0.00}%", storagePercentageAmount * 100));
        }
    }
    private void InstantiateStat(string strText)
    {
        GameObject prefabObj = Instantiate(commonPrefab, content.GetComponent<Transform>());
        Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
        TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
        txtName.text = strText;
    }
    public void OnStatsWindow()
    {
        InstantiateStatList();
    }
    public void OnLeaveStatsWindow()
    {
        foreach (Transform transformChild in content.GetComponent<Transform>())
        {
            Destroy(transformChild.gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("allWorkerMultiplerAmount", allWorkerMultiplierAmount);
        PlayerPrefs.SetFloat("allBuildingMultiplierAmount", allBuildingMultiplierAmount);
        PlayerPrefs.SetFloat("allCraftablesCostReduced", allCraftablesCostReduced);
        PlayerPrefs.SetFloat("allResearchablesCostReduced", allResearchablesCostReduced);
        PlayerPrefs.SetFloat("researchTimeReductionAmount", researchTimeReductionAmount);
        PlayerPrefs.SetFloat("storagePercentageAmount", storagePercentageAmount);
        PlayerPrefs.SetInt("workerCountModified", (int)workerCountModified);

        SaveSystem.SavePlayer(this);
    }
    private void Start()
    {
        allWorkerMultiplierAmount = PlayerPrefs.GetFloat("allWorkerMultiplerAmount", allWorkerMultiplierAmount);
        allBuildingMultiplierAmount = PlayerPrefs.GetFloat("allBuildingMultiplierAmount", allBuildingMultiplierAmount);
        allCraftablesCostReduced = PlayerPrefs.GetFloat("allCraftablesCostReduced", allCraftablesCostReduced);
        allResearchablesCostReduced = PlayerPrefs.GetFloat("allResearchablesCostReduced", allResearchablesCostReduced);
        researchTimeReductionAmount = PlayerPrefs.GetFloat("researchTimeReductionAmount", researchTimeReductionAmount);
        storagePercentageAmount = PlayerPrefs.GetFloat("storagePercentageAmount", storagePercentageAmount);
        workerCountModified = (uint)PlayerPrefs.GetInt("workerCountModified", (int)workerCountModified);

        PlayerData data = SaveSystem.LoadPlayer();

        craftableCostReduced = data.craftableCostReduced;
        researchableCostReduced = data.researchableCostReduced;
        buildingCostReduced = data.buildingCostReduced;
        workerMultiplierModified = data.workerMultiplierModified;
        buildingMultiplierModified = data.buildingMultiplierModified;
        buildingSelfCountModified = data.buildingSelfCountModified;

        gameObject.SetActive(false);
    }
}
