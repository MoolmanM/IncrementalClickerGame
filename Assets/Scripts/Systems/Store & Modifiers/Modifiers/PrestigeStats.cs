using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Security.Cryptography;

public class PrestigeStats : MonoBehaviour
{
    public static float prestigeAllWorkerMultiplierAddition;
    public static float prestigeAllBuildingMultiplierAddition;

    public static float prestigeAllCraftablesCostSubtraction;
    public static float prestigeAllResearchablesCostSubtraction;

    public static Dictionary<CraftingType, float> prestigeCraftableCostSubtraction = new();
    public static Dictionary<ResearchType, float> prestigeResearchableCostSubtraction = new();
    public static Dictionary<BuildingType, float> prestigeBuildingCostSubtraction = new();

    public static Dictionary<WorkerType, float> prestigeWorkerMultiplierAddition = new();
    public static Dictionary<BuildingType, float> prestigeBuildingMultiplierAddition = new();

    public static Dictionary<BuildingType, uint> prestigeBuildingCountAddition = new();

    public static uint prestigeWorkerCountAddition;
    public static float prestigeResearchTimeSubtraction;
    public static float prestigeStorageAddition;

    public GameObject modifier, content;

    private string strStorageAddition, strResearchTimeSubtraction, strAllWorkerMultiplierAddition, strAllBuildingMultiplierAddition, strAllCraftablesCostSubtraction, strAllResearchablesCostSubtraction, strWorkerCountAddition;

    private Dictionary<CraftingType, string> strCraftableCostSubtraction = new();
    private Dictionary<ResearchType, string> strResearchableCostSubtraction = new();
    private Dictionary<BuildingType, string> strBuildingCostSubtraction = new();
    private Dictionary<WorkerType, string> strWorkerMultiplierAddition = new();
    private Dictionary<BuildingType, string> strBuildingMultiplierAddition = new();
    private Dictionary<BuildingType, string> strBuildingCountAddition = new();

    public GameObject objGroupBuildings, objGroupCrafting, objGroupWorker, objGroupResearch, objGroupStorage;
    public GameObject objBuildingHeader, objCraftingHeader, objResearchHeader, objStorageHeader, objWorkerHeader;

    public void InstantiateStatList()
    {
        if (prestigeAllBuildingMultiplierAddition > 0.00f || prestigeAllBuildingMultiplierAddition > 0.00f || prestigeBuildingCostSubtraction.Count != 0 ||  prestigeBuildingMultiplierAddition.Count != 0 ||  prestigeBuildingCountAddition.Count != 0)
        {
            objBuildingHeader.SetActive(true);
        }
        else
        {
            objBuildingHeader.SetActive(false);
            objGroupBuildings.SetActive(false);
        }
        AllBuildingMultiplierAddition();
        if (prestigeBuildingCostSubtraction.Count != 0)
        {
            BuildingCostSubtraction();
        }
        if (prestigeBuildingMultiplierAddition.Count != 0)
        {
            BuildingMultiplierAddition();
        }
        if (prestigeBuildingCountAddition.Count != 0)
        {
            BuildingCountAddition();
        }

        if (prestigeAllCraftablesCostSubtraction > 0.00f || prestigeCraftableCostSubtraction.Count != 0)
        {
            objCraftingHeader.SetActive(true);
        }
        else
        {
            objCraftingHeader.SetActive(false);
            objGroupCrafting.SetActive(false);
        }
        AllCraftablesCostSubtraction();
        if (prestigeCraftableCostSubtraction.Count != 0)
        {
            CraftableCostSubtraction();
        }

        if (prestigeAllResearchablesCostSubtraction > 0.00f || prestigeResearchableCostSubtraction.Count != 0 || prestigeResearchTimeSubtraction > 0.00f)
        {
            objResearchHeader.SetActive(true);
        }
        else
        {
            objResearchHeader.SetActive(false);
            objGroupResearch.SetActive(false);
        }
        AllResearchableCostSubtraction();
        if (prestigeResearchableCostSubtraction.Count != 0)
        {
            ResearchableCostSubtraction();
        }
        ResearchTimeSubtraction();

        if (prestigeAllWorkerMultiplierAddition > 0.00f || prestigeWorkerCountAddition > 0 || prestigeWorkerMultiplierAddition.Count != 0)
        {
            objWorkerHeader.SetActive(true);
        }
        else
        {
            objWorkerHeader.SetActive(false);
            objGroupWorker.SetActive(false);
        }
        AllWorkerMultiplierAddition();
        if (prestigeWorkerMultiplierAddition.Count != 0)
        {
            WorkerMultiplierAddition();
        }
        WorkerCountAddition();

        if (prestigeStorageAddition > 0.00f)
        {
            objStorageHeader.SetActive(true);
        }
        else
        {
            objStorageHeader.SetActive(false);
            objGroupStorage.SetActive(false);
        }
        StorageAddition();

        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
    }

    private void ResearchTimeSubtraction()
    {
        if (prestigeResearchTimeSubtraction > 0.00f)
        {
            strResearchTimeSubtraction = string.Format("Research time is reduced by <color=#08F1FF>{0:0.00}%</color>", prestigeResearchTimeSubtraction * 100);

            InstantiateStat(strResearchTimeSubtraction, objGroupResearch);
        }
    }
    private void StorageAddition()
    {
        if (prestigeStorageAddition > 0.00f)
        {
            strStorageAddition = string.Format("Storage limit increased by <color=#08F1FF>{0:0.00}%</color>", prestigeStorageAddition * 100);

            InstantiateStat(strStorageAddition, objGroupStorage);
        }
    }
    private void WorkerCountAddition()
    {
        if (prestigeWorkerCountAddition > 0)
        {
            if (prestigeWorkerCountAddition > 1)
            {
                strWorkerCountAddition = string.Format("Gain <color=#08F1FF>{0}</color> additional workers", prestigeWorkerCountAddition);
            }
            else
            {
                strWorkerCountAddition = string.Format("Gain <color=#08F1FF>1</color> additional worker");
            }

            InstantiateStat(strWorkerCountAddition, objGroupWorker);
        }
    }
    private void BuildingCountAddition()
    {
        foreach (var item in prestigeBuildingCountAddition)
        {
            strBuildingCountAddition.Add(item.Key, "");
            if (strBuildingCountAddition.ContainsKey(item.Key))
            {
                strBuildingCountAddition[item.Key] = string.Format("Gain <color=#08F1FF>{0}</color> additional <color=#E1341E>{1}</color>", item.Value, Building.Buildings[item.Key].ActualName);
            }          
        }
        foreach (var item in strBuildingCountAddition)
        {
            InstantiateStat(item.Value, objGroupBuildings);
        }
    }
    private void BuildingMultiplierAddition()
    {
        foreach (var item in prestigeBuildingMultiplierAddition)
        {
            strBuildingMultiplierAddition.Add(item.Key, "");
            if (strBuildingMultiplierAddition.ContainsKey(item.Key))
            {
                strBuildingMultiplierAddition[item.Key] = string.Format("Increased production of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Building.Buildings[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in strBuildingMultiplierAddition)
        {
            InstantiateStat(item.Value, objGroupBuildings);
        }
    }
    private void WorkerMultiplierAddition()
    {
        foreach (var item in prestigeWorkerMultiplierAddition)
        {
            strWorkerMultiplierAddition.Add(item.Key, "");
            if (strWorkerMultiplierAddition.ContainsKey(item.Key))
            {
                strWorkerMultiplierAddition[item.Key] = string.Format("Increased production of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Worker.Workers[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in strWorkerMultiplierAddition)
        {
            InstantiateStat(item.Value, objGroupWorker);
        }
    }
    private void BuildingCostSubtraction()
    {
        foreach (var item in prestigeBuildingCostSubtraction)
        {
            strBuildingCostSubtraction.Add(item.Key, "");
            if (strBuildingCostSubtraction.ContainsKey(item.Key))
            {
                strBuildingCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Building.Buildings[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in strBuildingCostSubtraction)
        {
            InstantiateStat(item.Value, objGroupBuildings);
        }
    }
    private void ResearchableCostSubtraction()
    {
        foreach (var item in prestigeResearchableCostSubtraction)
        {
            strResearchableCostSubtraction.Add(item.Key, "");
            if (strResearchableCostSubtraction.ContainsKey(item.Key))
            {
                strResearchableCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Researchable.Researchables[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in strResearchableCostSubtraction)
        {
            InstantiateStat(item.Value, objGroupResearch);
        }
    }
    private void CraftableCostSubtraction()
    {
        foreach (var item in prestigeCraftableCostSubtraction)
        {
            strCraftableCostSubtraction.Add(item.Key, "");
            if (strCraftableCostSubtraction.ContainsKey(item.Key))
            {
                strCraftableCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Craftable.Craftables[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in strCraftableCostSubtraction)
        {
            InstantiateStat(item.Value, objGroupCrafting);
        }
    }
    private void AllResearchableCostSubtraction()
    {
        if (prestigeAllResearchablesCostSubtraction > 0.00f)
        {
            strAllResearchablesCostSubtraction = string.Format("All research cost decreased by <color=#08F1FF>{0:0.00}%</color>", prestigeAllResearchablesCostSubtraction * 100);

            InstantiateStat(strAllResearchablesCostSubtraction, objGroupResearch);
        }
    }
    private void AllCraftablesCostSubtraction()
    {
        if (prestigeAllCraftablesCostSubtraction > 0.00f)
        {
            strAllCraftablesCostSubtraction = string.Format("All crafting cost decreased by <color=#08F1FF>{0:0.00}%</color>", prestigeAllCraftablesCostSubtraction * 100);

            InstantiateStat(strAllCraftablesCostSubtraction, objGroupCrafting);
        }
    }
    private void AllWorkerMultiplierAddition()
    {
        if (prestigeAllWorkerMultiplierAddition > 0.00f)
        {
            strAllWorkerMultiplierAddition = string.Format("All workers production increased by <color=#08F1FF>{0:0.00}%</color>", prestigeAllWorkerMultiplierAddition * 100);

            InstantiateStat(strAllWorkerMultiplierAddition, objGroupWorker);
        }
    }
    private void AllBuildingMultiplierAddition()
    {
        if (prestigeAllBuildingMultiplierAddition > 0.00f)
        {
            strAllBuildingMultiplierAddition = string.Format("All buildings production increased by <color=#08F1FF>{0:0.00}%</color>", prestigeAllBuildingMultiplierAddition * 100);

            InstantiateStat(strAllBuildingMultiplierAddition, objGroupBuildings);
        }
    }

    private void InstantiateStat(string strText, GameObject objGroup)
    {
        GameObject prefabObj = Instantiate(modifier, objGroup.GetComponent<Transform>());
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
        foreach (Transform item in objGroupCrafting.GetComponent<Transform>())
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in objGroupBuildings.GetComponent<Transform>())
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in objGroupResearch.GetComponent<Transform>())
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in objGroupWorker.GetComponent<Transform>())
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in objGroupStorage.GetComponent<Transform>())
        {
            Destroy(item.gameObject);
        }

        strCraftableCostSubtraction.Clear();
        strResearchableCostSubtraction.Clear();
        strBuildingCostSubtraction.Clear();
        strWorkerMultiplierAddition.Clear();
        strBuildingMultiplierAddition.Clear();
        strBuildingCountAddition.Clear();

        strStorageAddition = "";
        strResearchTimeSubtraction = "";
        strAllWorkerMultiplierAddition = "";
        strAllBuildingMultiplierAddition = "";
        strAllCraftablesCostSubtraction = "";
        strAllResearchablesCostSubtraction = "";
        strWorkerCountAddition = "";
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("prestigeAllWorkerMultiplierAddition", prestigeAllWorkerMultiplierAddition);
        PlayerPrefs.SetFloat("prestigeAllBuildingMultiplierAddition", prestigeAllBuildingMultiplierAddition);
        PlayerPrefs.SetFloat("prestigeAllCraftablesCostSubtraction", prestigeAllCraftablesCostSubtraction);
        PlayerPrefs.SetFloat("prestigeAllResearchablesCostSubtraction", prestigeAllResearchablesCostSubtraction);
        PlayerPrefs.SetFloat("prestigeResearchTimeSubtraction", prestigeResearchTimeSubtraction);
        PlayerPrefs.SetFloat("prestigeStorageAddition", prestigeStorageAddition);
        PlayerPrefs.SetInt("prestigeWorkerCountAddition", (int)prestigeWorkerCountAddition);

        SaveSystem.SavePrestige(this);
    }
    private void Start()
    {
        prestigeAllWorkerMultiplierAddition = PlayerPrefs.GetFloat("prestigeAllWorkerMultiplierAddition", prestigeAllWorkerMultiplierAddition);
        prestigeAllBuildingMultiplierAddition = PlayerPrefs.GetFloat("prestigeAllBuildingMultiplierAddition", prestigeAllBuildingMultiplierAddition);
        prestigeAllCraftablesCostSubtraction = PlayerPrefs.GetFloat("prestigeAllCraftablesCostSubtraction", prestigeAllCraftablesCostSubtraction);
        prestigeAllResearchablesCostSubtraction = PlayerPrefs.GetFloat("prestigeAllResearchablesCostSubtraction", prestigeAllResearchablesCostSubtraction);
        prestigeResearchTimeSubtraction = PlayerPrefs.GetFloat("prestigeResearchTimeSubtraction", prestigeResearchTimeSubtraction);
        prestigeStorageAddition = PlayerPrefs.GetFloat("prestigeStorageAddition", prestigeStorageAddition);
        prestigeWorkerCountAddition = (uint)PlayerPrefs.GetInt("prestigeWorkerCountAddition", (int)prestigeWorkerCountAddition);

        PlayerDataPrestige data = SaveSystem.LoadPrestigeStats();

        prestigeCraftableCostSubtraction = data.craftableCostSubtraction;
        prestigeResearchableCostSubtraction = data.researchableCostSubtraction;
        prestigeBuildingCostSubtraction = data.buildingCostSubtraction;
        prestigeWorkerMultiplierAddition = data.workerMultiplierAddition;
        prestigeBuildingMultiplierAddition = data.buildingMultiplierAddition;
        prestigeBuildingCountAddition = data.buildingCountAddition;
    }
}
