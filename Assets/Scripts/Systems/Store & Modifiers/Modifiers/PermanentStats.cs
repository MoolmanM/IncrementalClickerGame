using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Security.Cryptography;

public class PermanentStats : MonoBehaviour
{
    public static float nextAllWorkerMultiplierAddition;
    public static float nextAllBuildingMultiplierAddition;

    public static float nextAllCraftablesCostSubtraction;
    public static float nextAllResearchablesCostSubtraction;

    public static Dictionary<CraftingType, float> nextCraftableCostSubtraction = new();
    public static Dictionary<ResearchType, float> nextResearchableCostSubtraction = new();
    public static Dictionary<BuildingType, float> nextBuildingCostSubtraction = new();

    public static Dictionary<WorkerType, float> nextWorkerMultiplierAddition = new();
    public static Dictionary<BuildingType, float> nextBuildingMultiplierAddition = new();

    public static Dictionary<BuildingType, uint> nextBuildingCountAddition = new();


    public static uint nextWorkerCountAddition;
    public static float nextResearchTimeSubtraction;
    public static float nextStorageAddition;

    public static float activeAllWorkerMultiplierAddition;
    public static float activeAllBuildingMultiplierAddition;

    public static float activeAllCraftablesCostSubtraction;
    public static float activeAllResearchablesCostSubtraction;

    public static Dictionary<CraftingType, float> activeCraftableCostSubtraction = new();
    public static Dictionary<ResearchType, float> activeResearchableCostSubtraction = new();
    public static Dictionary<BuildingType, float> activeBuildingCostSubtraction = new();

    public static Dictionary<WorkerType, float> activeWorkerMultiplierAddition = new();
    public static Dictionary<BuildingType, float> activeBuildingMultiplierAddition = new();

    public static Dictionary<BuildingType, uint> activeBuildingCountAddition = new();

    public static uint activeWorkerCountAddition;
    public static float activeResearchTimeSubtraction;
    public static float activeStorageAddition;

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
        //InstantiateStat(string.Format("<color=#F6E19C>Permanent Modifiers:</color>"), content);

        if (activeAllBuildingMultiplierAddition > 0.00f || activeAllBuildingMultiplierAddition > 0.00f || activeBuildingCostSubtraction.Count != 0 || nextBuildingCostSubtraction.Count != 0 || activeBuildingMultiplierAddition.Count != 0 || nextBuildingMultiplierAddition.Count != 0 || activeBuildingCountAddition.Count != 0 || nextBuildingCountAddition.Count != 0)
        {
            objBuildingHeader.SetActive(true);
            //objGroupBuildings.SetActive(true);
        }     
        else
        {
            objBuildingHeader.SetActive(false);
            objGroupBuildings.SetActive(false);
        }
        AllBuildingMultiplierAddition();
        if (activeBuildingCostSubtraction.Count != 0 || nextBuildingCostSubtraction.Count != 0)
        {
            BuildingCostSubtraction();
        }
        if (activeBuildingMultiplierAddition.Count != 0 || nextBuildingMultiplierAddition.Count != 0)
        {
            BuildingMultiplierAddition();
        }
        if (activeBuildingCountAddition.Count != 0 || nextBuildingCountAddition.Count != 0)
        {
            BuildingCountAddition();
        }

        if (activeAllCraftablesCostSubtraction > 0.00f || nextAllCraftablesCostSubtraction > 0.00f || activeCraftableCostSubtraction.Count != 0 || nextCraftableCostSubtraction.Count != 0)
        {
            objCraftingHeader.SetActive(true);
        }
        else
        {
            objCraftingHeader.SetActive(false);
            objGroupCrafting.SetActive(false);
        }
        AllCraftablesCostSubtraction();
        if (activeCraftableCostSubtraction.Count != 0 || nextCraftableCostSubtraction.Count != 0)
        {
            CraftableCostSubtraction();
        }

        if (activeAllResearchablesCostSubtraction > 0.00f || nextAllResearchablesCostSubtraction > 0.00f || activeResearchableCostSubtraction.Count != 0 || nextResearchableCostSubtraction.Count != 0 || activeResearchTimeSubtraction > 0.00f || nextResearchTimeSubtraction > 0.00f)
        {
            objResearchHeader.SetActive(true);
        }
        else
        {
            objResearchHeader.SetActive(false);
            objGroupResearch.SetActive(false);
        }
        AllResearchableCostSubtraction();
        if (activeResearchableCostSubtraction.Count != 0 || nextResearchableCostSubtraction.Count != 0)
        {
            ResearchableCostSubtraction();
        }
        ResearchTimeSubtraction();

        if (activeAllWorkerMultiplierAddition > 0.00f || nextAllWorkerMultiplierAddition > 0.00f || activeWorkerCountAddition > 0 || nextWorkerCountAddition > 0 || activeWorkerMultiplierAddition.Count != 0 || nextWorkerMultiplierAddition.Count != 0)
        {
            objWorkerHeader.SetActive(true);
        }     
        else
        {
            objWorkerHeader.SetActive(false);
            objGroupWorker.SetActive(false);
        }
        AllWorkerMultiplierAddition();
        if (activeWorkerMultiplierAddition.Count != 0 || nextWorkerMultiplierAddition.Count != 0)
        {
            WorkerMultiplierAddition();
        }
        WorkerCountAddition();

        if (activeStorageAddition > 0.00f || nextStorageAddition > 0.00f)
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
        if (activeResearchTimeSubtraction > 0.00f)
        {
            strResearchTimeSubtraction = string.Format("Research time is reduced by <color=#08F1FF>{0:0.00}%</color>", activeResearchTimeSubtraction * 100);
        }
        else if (activeResearchTimeSubtraction == 0.00f && nextResearchTimeSubtraction > 0.00f)
        {
            strResearchTimeSubtraction = string.Format("Research time is reduced by <color=#08F1FF>0</color>");
        }
        if (nextResearchTimeSubtraction > 0.00f)
        {
            strResearchTimeSubtraction += string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", nextResearchTimeSubtraction * 100);
        }
        if (activeResearchTimeSubtraction > 0.00f || nextResearchTimeSubtraction > 0.00f)
        {
            InstantiateStat(strResearchTimeSubtraction, objGroupResearch);
        }
    }
    private void StorageAddition()
    {
        if (activeStorageAddition > 0.00f)
        {
            strStorageAddition = string.Format("Storage limit increased by <color=#08F1FF>{0:0.00}%</color>", activeStorageAddition * 100);
        }
        else if (activeStorageAddition == 0.00f && nextStorageAddition > 0.00f)
        {
            strStorageAddition = string.Format("Storage limit increased by <color=#08F1FF>0</color>");
        }
        if (nextStorageAddition > 0.00f)
        {
            strStorageAddition += string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", nextStorageAddition * 100);
        }
        if (activeStorageAddition > 0.00f || nextStorageAddition > 0.00f)
        {
            InstantiateStat(strStorageAddition, objGroupStorage);
        }
    }
    private void WorkerCountAddition()
    {
        if (activeWorkerCountAddition > 0)
        {
            if (activeWorkerCountAddition > 1)
            {
                strWorkerCountAddition = string.Format("Gain <color=#08F1FF>{0}</color> additional workers", activeWorkerCountAddition);
            }
            else
            {
                strWorkerCountAddition = string.Format("Gain <color=#08F1FF>1</color> additional worker");
            }
        }
        else if (activeWorkerCountAddition == 0 && nextWorkerCountAddition > 0)
        {
            strWorkerCountAddition = string.Format("Gain <color=#08F1FF>0</color> additional workers", nextWorkerCountAddition);
        }
        if (nextWorkerCountAddition > 0)
        {
            strWorkerCountAddition += string.Format("<color=#00C8D4>(+{0})</color>", nextWorkerCountAddition);
        }
        if (activeWorkerCountAddition > 0 || nextWorkerCountAddition > 0)
        {
            InstantiateStat(strWorkerCountAddition, objGroupWorker);
        }
    }
    private void BuildingCountAddition()
    {
        foreach (var item in activeBuildingCountAddition)
        {
            strBuildingCountAddition.Add(item.Key, "");
            if (strBuildingCountAddition.ContainsKey(item.Key))
            {
                strBuildingCountAddition[item.Key] = string.Format("Gain <color=#08F1FF>{0}</color> additional <color=#E1341E>{1}</color>", item.Value, Building.Buildings[item.Key].ActualName);
            }
        }
        foreach (var item in nextBuildingCountAddition)
        {
            if (!strBuildingCountAddition.ContainsKey(item.Key))
            {
                strBuildingCountAddition[item.Key] = string.Format("Gain <color=#08F1FF>0</color><color=#00C8D4>(+{0})</color> additional <color=#E1341E>{1}</color>", item.Value, Building.Buildings[item.Key].ActualName);
            }
            else
            {
                strBuildingCountAddition.Add(item.Key, "");
                strBuildingCountAddition[item.Key] = string.Format("<color=#00C8D4>(+{0})</color>", item.Value);
            }
        }
        foreach (var item in strBuildingCountAddition)
        {
            InstantiateStat(item.Value, objGroupBuildings);
        }
    }
    private void BuildingMultiplierAddition()
    {
        foreach (var item in activeBuildingMultiplierAddition)
        {
            strBuildingMultiplierAddition.Add(item.Key, "");
            if (strBuildingMultiplierAddition.ContainsKey(item.Key))
            {
                strBuildingMultiplierAddition[item.Key] = string.Format("Increased production of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Building.Buildings[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in nextBuildingMultiplierAddition)
        {
            if (!strBuildingMultiplierAddition.ContainsKey(item.Key))
            {
                strBuildingMultiplierAddition[item.Key] = string.Format("Increased production of <color=#E1341E>{0}</color> by <color=#08F1FF>0</color><color=#00C8D4>(+{1:0.00}%)</color>", Building.Buildings[item.Key].ActualName, item.Value * 100);
            }
            else
            {
                strBuildingMultiplierAddition.Add(item.Key, "");
                strBuildingMultiplierAddition[item.Key] = string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", item.Value * 100);
            }
        }
        foreach (var item in strBuildingMultiplierAddition)
        {
            InstantiateStat(item.Value, objGroupBuildings);
        }
    }
    private void WorkerMultiplierAddition()
    {
        foreach (var item in activeWorkerMultiplierAddition)
        {
            strWorkerMultiplierAddition.Add(item.Key, "");
            if (strWorkerMultiplierAddition.ContainsKey(item.Key))
            {
                strWorkerMultiplierAddition[item.Key] = string.Format("Increased production of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Worker.Workers[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in nextWorkerMultiplierAddition)
        {
            if (!strWorkerMultiplierAddition.ContainsKey(item.Key))
            {
                strWorkerMultiplierAddition[item.Key] = string.Format("Increased production of <color=#E1341E>{0}</color> by <color=#08F1FF>0</color><color=#00C8D4>(+{1:0.00}%)</color>", Worker.Workers[item.Key].ActualName, item.Value * 100);
            }
            else
            {
                strWorkerMultiplierAddition.Add(item.Key, "");
                strWorkerMultiplierAddition[item.Key] = string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", item.Value * 100);
            }
        }
        foreach (var item in strWorkerMultiplierAddition)
        {
            InstantiateStat(item.Value, objGroupWorker);
        }
    }
    private void BuildingCostSubtraction()
    {
        foreach (var item in activeBuildingCostSubtraction)
        {
            strBuildingCostSubtraction.Add(item.Key, "");
            if (strBuildingCostSubtraction.ContainsKey(item.Key))
            {
                strBuildingCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Building.Buildings[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in nextBuildingCostSubtraction)
        {
            if (!strBuildingCostSubtraction.ContainsKey(item.Key))
            {
                strBuildingCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>0</color><color=#00C8D4>(+{1:0.00}%)</color>", Building.Buildings[item.Key].ActualName, item.Value * 100);
            }
            else
            {
                strBuildingCostSubtraction.Add(item.Key, "");
                strBuildingCostSubtraction[item.Key] = string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", item.Value * 100);
            }
        }
        foreach (var item in strBuildingCostSubtraction)
        {
            InstantiateStat(item.Value, objGroupBuildings);
        }
    }
    private void ResearchableCostSubtraction()
    {
        foreach (var item in activeResearchableCostSubtraction)
        {
            strResearchableCostSubtraction.Add(item.Key, "");
            if (strResearchableCostSubtraction.ContainsKey(item.Key))
            {
                strResearchableCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Researchable.Researchables[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in nextResearchableCostSubtraction)
        {
            if (!strResearchableCostSubtraction.ContainsKey(item.Key))
            {
                strResearchableCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>0</color><color=#00C8D4>(+{1:0.00}%)</color>", Researchable.Researchables[item.Key].ActualName, item.Value * 100);
            }
            else
            {
                strResearchableCostSubtraction.Add(item.Key, "");
                strResearchableCostSubtraction[item.Key] = string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", item.Value * 100);
            }
        }
        foreach (var item in strResearchableCostSubtraction)
        {
            InstantiateStat(item.Value, objGroupResearch);
        }
    }
    private void CraftableCostSubtraction()
    {
        foreach (var item in activeCraftableCostSubtraction)
        {
            strCraftableCostSubtraction.Add(item.Key, "");
            if (strCraftableCostSubtraction.ContainsKey(item.Key))
            {
                strCraftableCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>{1:0.00}%</color>", Craftable.Craftables[item.Key].ActualName, item.Value * 100);
            }
        }
        foreach (var item in nextCraftableCostSubtraction)
        {
            if (!strCraftableCostSubtraction.ContainsKey(item.Key))
            {
                strCraftableCostSubtraction[item.Key] = string.Format("Decreased cost of <color=#E1341E>{0}</color> by <color=#08F1FF>0</color><color=#00C8D4>(+{1:0.00}%)</color>", Craftable.Craftables[item.Key].ActualName, item.Value * 100);
            }
            else
            {
                strCraftableCostSubtraction.Add(item.Key, "");
                strCraftableCostSubtraction[item.Key] = string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", item.Value * 100);
            }
        }
        foreach (var item in strCraftableCostSubtraction)
        {
            InstantiateStat(item.Value, objGroupCrafting);
        }       
    }
    private void AllResearchableCostSubtraction()
    {
        if (activeAllResearchablesCostSubtraction > 0.00f)
        {
            strAllResearchablesCostSubtraction = string.Format("All research cost decreased by <color=#08F1FF>{0:0.00}%</color>", activeAllResearchablesCostSubtraction * 100);
        }
        else if (activeAllResearchablesCostSubtraction == 0.00f && nextAllResearchablesCostSubtraction > 0.00f)
        {
            strAllResearchablesCostSubtraction = string.Format("All research cost decreased by 0");
        }
        if (nextAllResearchablesCostSubtraction > 0.00f)
        {
            strAllResearchablesCostSubtraction += string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", nextAllResearchablesCostSubtraction * 100);
        }
        if (activeAllResearchablesCostSubtraction > 0.00f || nextAllResearchablesCostSubtraction > 0.00f)
        {
            InstantiateStat(strAllResearchablesCostSubtraction, objGroupResearch);
        }
    }
    private void AllCraftablesCostSubtraction()
    {
        if (activeAllCraftablesCostSubtraction > 0.00f)
        {
            strAllCraftablesCostSubtraction = string.Format("All crafting cost decreased by <color=#08F1FF>{0:0.00}%</color>", activeAllCraftablesCostSubtraction * 100);
        }
        else if (activeAllCraftablesCostSubtraction == 0.00f && nextAllCraftablesCostSubtraction > 0.00f)
        {
            strAllCraftablesCostSubtraction = string.Format("All crafting cost decreased by 0");
        }
        if (nextAllCraftablesCostSubtraction > 0.00f)
        {
            strAllCraftablesCostSubtraction += string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", nextAllCraftablesCostSubtraction * 100);
        }
        if (activeAllCraftablesCostSubtraction > 0.00f || nextAllCraftablesCostSubtraction > 0.00f)
        {
            InstantiateStat(strAllCraftablesCostSubtraction, objGroupCrafting);
        }
    }
    private void AllWorkerMultiplierAddition()
    {
        if (activeAllWorkerMultiplierAddition > 0.00f)
        {
            strAllWorkerMultiplierAddition = string.Format("All workers production increased by <color=#08F1FF>{0:0.00}%</color>", activeAllWorkerMultiplierAddition * 100);
        }
        else if (activeAllWorkerMultiplierAddition == 0.00f && nextAllWorkerMultiplierAddition > 0.00f)
        {
            strAllWorkerMultiplierAddition = string.Format("All workers production increased by 0");
        }
        if (nextAllWorkerMultiplierAddition > 0.00f)
        {
            strAllWorkerMultiplierAddition += string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", nextAllWorkerMultiplierAddition * 100);
        }
        if (activeAllWorkerMultiplierAddition > 0.00f || nextAllWorkerMultiplierAddition > 0.00f)
        {
            InstantiateStat(strAllWorkerMultiplierAddition, objGroupWorker);
        }
    }
    private void AllBuildingMultiplierAddition()
    {
        if (activeAllBuildingMultiplierAddition > 0.00f)
        {
            strAllBuildingMultiplierAddition = string.Format("All buildings production increased by <color=#08F1FF>{0:0.00}%</color>", activeAllBuildingMultiplierAddition * 100);
        }
        else if (activeAllBuildingMultiplierAddition == 0.00f && nextAllBuildingMultiplierAddition > 0.00f)
        {
            strAllBuildingMultiplierAddition = string.Format("All buildings production increased by 0");
        }
        if (nextAllBuildingMultiplierAddition > 0.00f)
        {
            strAllBuildingMultiplierAddition += string.Format("<color=#00C8D4>(+{0:0.00}%)</color>", nextAllBuildingMultiplierAddition * 100);
        }
        if (activeAllBuildingMultiplierAddition > 0.00f || nextAllBuildingMultiplierAddition > 0.00f)
        {
            InstantiateStat(strAllBuildingMultiplierAddition, objGroupBuildings);
        }
    }

    public static void OnResetGame()
    {
        foreach (var item in nextCraftableCostSubtraction)
        {
            activeCraftableCostSubtraction.Add(item.Key, item.Value);
        }
        foreach (var item in nextResearchableCostSubtraction)
        {
            activeResearchableCostSubtraction.Add(item.Key, item.Value);
        }
        foreach (var item in nextBuildingCostSubtraction)
        {
            activeBuildingCostSubtraction.Add(item.Key, item.Value);
        }
        foreach (var item in nextWorkerMultiplierAddition)
        {
            activeWorkerMultiplierAddition.Add(item.Key, item.Value);
        }
        foreach (var item in nextBuildingMultiplierAddition)
        {
            activeBuildingMultiplierAddition.Add(item.Key, item.Value);
        }
        foreach (var item in nextBuildingCountAddition)
        {
            activeBuildingCountAddition.Add(item.Key, item.Value);
        }

        activeStorageAddition = nextStorageAddition;
        activeResearchTimeSubtraction = nextResearchTimeSubtraction;
        activeAllWorkerMultiplierAddition = nextAllWorkerMultiplierAddition;
        activeAllBuildingMultiplierAddition = nextAllBuildingMultiplierAddition;
        activeAllCraftablesCostSubtraction = nextAllCraftablesCostSubtraction;
        activeAllResearchablesCostSubtraction = nextAllResearchablesCostSubtraction;
        activeWorkerCountAddition = nextWorkerCountAddition;

        nextCraftableCostSubtraction.Clear();
        nextResearchableCostSubtraction.Clear();
        nextBuildingCostSubtraction.Clear();
        nextWorkerMultiplierAddition.Clear();
        nextBuildingMultiplierAddition.Clear();
        nextBuildingCountAddition.Clear();

        nextStorageAddition = 0;
        nextResearchTimeSubtraction = 0;
        nextAllWorkerMultiplierAddition = 0;
        nextAllBuildingMultiplierAddition = 0;
        nextAllCraftablesCostSubtraction = 0;
        nextAllResearchablesCostSubtraction = 0;
        nextWorkerCountAddition = 0;
    }

    private void InstantiateStat(string strText, GameObject objGroup)
    {
        //objGroup = Instantiate(modifierGroup, content.GetComponent<Transform>());
        GameObject prefabObj = Instantiate(modifier, objGroup.GetComponent<Transform>());
        //prefabObj.GetComponent<Button>().onClick.AddListener(delegate { DisableGroup(prefabObj); });
        Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
        TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
        txtName.text = strText;
    }
//    private void InstantiateHeader(string strText, GameObject objGroup)
//{
//        //objGroup = Instantiate(modifier, content.GetComponent<Transform>());
//        GameObject prefabObj = Instantiate(modifier, content.GetComponent<Transform>());
//        prefabObj.GetComponent<Button>().onClick.AddListener(delegate { DisableGroup(objGroup); });
//        Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
//        TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
//        txtName.text = strText;
//    }
//    private void DisableGroup(GameObject objGroup)
//    {
//        objGroup.SetActive(false);
//    }
    public void OnStatsWindow()
    {
        InstantiateStatList();
    }
    public void OnLeaveStatsWindow()
    {
        //foreach (Transform transformChild in content.GetComponent<Transform>())
        //{
        //    if (transformChild.name == "Modifier(Clone)")
        //    {
        //        Destroy(transformChild.gameObject);
        //    }
            
        //    //Transform tformModifier = transform.Find("Modifier(Clone)");

        //}

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
        PlayerPrefs.SetFloat("nextAllWorkerMultiplierAddition", nextAllWorkerMultiplierAddition);
        PlayerPrefs.SetFloat("nextAllBuildingMultiplierAddition", nextAllBuildingMultiplierAddition);
        PlayerPrefs.SetFloat("nextAllCraftablesCostSubtraction", nextAllCraftablesCostSubtraction);
        PlayerPrefs.SetFloat("nextAllResearchablesCostSubtraction", nextAllResearchablesCostSubtraction);
        PlayerPrefs.SetFloat("nextResearchTimeSubtraction", nextResearchTimeSubtraction);
        PlayerPrefs.SetFloat("nextStorageAddition", nextStorageAddition);
        PlayerPrefs.SetInt("nextWorkerCountAddition", (int)nextWorkerCountAddition);

        PlayerPrefs.SetFloat("activeAllWorkerMultiplierAddition", activeAllWorkerMultiplierAddition);
        PlayerPrefs.SetFloat("activeAllBuildingMultiplierAddition", activeAllBuildingMultiplierAddition);
        PlayerPrefs.SetFloat("activeAllCraftablesCostSubtraction", activeAllCraftablesCostSubtraction);
        PlayerPrefs.SetFloat("activeAllResearchablesCostSubtraction", activeAllResearchablesCostSubtraction);
        PlayerPrefs.SetFloat("activeResearchTimeSubtraction", activeResearchTimeSubtraction);
        PlayerPrefs.SetFloat("activeStorageAddition", activeStorageAddition);
        PlayerPrefs.SetInt("activeWorkerCountAddition", (int)activeWorkerCountAddition);

        SaveSystem.SavePermanent(this);
    }
    private void Start()
    {
        nextAllWorkerMultiplierAddition = PlayerPrefs.GetFloat("nextAllWorkerMultiplierAddition", nextAllWorkerMultiplierAddition);
        nextAllBuildingMultiplierAddition = PlayerPrefs.GetFloat("nextAllBuildingMultiplierAddition", nextAllBuildingMultiplierAddition);
        nextAllCraftablesCostSubtraction = PlayerPrefs.GetFloat("nextAllCraftablesCostSubtraction", nextAllCraftablesCostSubtraction);
        nextAllResearchablesCostSubtraction = PlayerPrefs.GetFloat("nextAllResearchablesCostSubtraction", nextAllResearchablesCostSubtraction);
        nextResearchTimeSubtraction = PlayerPrefs.GetFloat("nextResearchTimeSubtraction", nextResearchTimeSubtraction);
        nextStorageAddition = PlayerPrefs.GetFloat("nextStorageAddition", nextStorageAddition);
        nextWorkerCountAddition = (uint)PlayerPrefs.GetInt("nextWorkerCountAddition", (int)nextWorkerCountAddition);

        activeAllWorkerMultiplierAddition = PlayerPrefs.GetFloat("activeAllWorkerMultiplierAddition", activeAllWorkerMultiplierAddition);
        activeAllBuildingMultiplierAddition = PlayerPrefs.GetFloat("activeAllBuildingMultiplierAddition", activeAllBuildingMultiplierAddition);
        activeAllCraftablesCostSubtraction = PlayerPrefs.GetFloat("activeAllCraftablesCostSubtraction", activeAllCraftablesCostSubtraction);
        activeAllResearchablesCostSubtraction = PlayerPrefs.GetFloat("activeAllResearchablesCostSubtraction", activeAllResearchablesCostSubtraction);
        activeResearchTimeSubtraction = PlayerPrefs.GetFloat("activeResearchTimeSubtraction", activeResearchTimeSubtraction);
        activeStorageAddition = PlayerPrefs.GetFloat("activeStorageAddition", activeStorageAddition);
        activeWorkerCountAddition = (uint)PlayerPrefs.GetInt("activeWorkerCountAddition", (int)activeWorkerCountAddition);

        PlayerDataPermanent data = SaveSystem.LoadPermanentStats();

        nextCraftableCostSubtraction = data.nextCraftableCostSubtraction;
        nextResearchableCostSubtraction = data.nextResearchableCostSubtraction;
        nextBuildingCostSubtraction = data.nextBuildingCostSubtraction;
        nextWorkerMultiplierAddition = data.nextWorkerMultiplierAddition;
        nextBuildingMultiplierAddition = data.nextBuildingMultiplierAddition;
        nextBuildingCountAddition = data.nextBuildingCountAddition;

        activeCraftableCostSubtraction = data.activeCraftableCostSubtraction;
        activeResearchableCostSubtraction = data.activeResearchableCostSubtraction;
        activeBuildingCostSubtraction = data.activeBuildingCostSubtraction;
        activeWorkerMultiplierAddition = data.activeWorkerMultiplierAddition;
        activeBuildingMultiplierAddition = data.activeBuildingMultiplierAddition;
        activeBuildingCountAddition = data.activeBuildingCountAddition;
    }
}
