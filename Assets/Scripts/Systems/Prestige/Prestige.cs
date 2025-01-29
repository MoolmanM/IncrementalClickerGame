using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Rarity
{
    public RarityType Type;
    public float randomChance;
    public float passiveCost;
    public Button buyButton;
    public CraftingType craftingType;
    public BuildingType buildingType;
    public ResearchType researchType;
    public WorkerType workerType;
    public Rarity(RarityType lastType, int lastPassiveCost, Button lastBuyButton, CraftingType lastCraftingType, BuildingType lastBuildingType, ResearchType lastResearchType, WorkerType lastWorkerType) : this()
    {
        Type = lastType;
        passiveCost = lastPassiveCost;
        buyButton = lastBuyButton;
        craftingType = lastCraftingType;
        buildingType = lastBuildingType;
        researchType = lastResearchType;
        workerType = lastWorkerType;
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

public class Prestige : MonoBehaviour
{
    public float prestigePoints, cachedPrestigePoints;
    public static List<ResourceType> resourcesUnlockedInPreviousRun = new List<ResourceType>();
    public static List<BuildingType> buildingsUnlockedInPreviousRun = new List<BuildingType>();
    public static List<WorkerType> workersUnlockedInPreviousRun = new List<WorkerType>();
    public static List<CraftingType> craftablesUnlockedInPreviousRun = new List<CraftingType>();
    public static List<ResearchType> researchablesUnlockedInPreviousRun = new List<ResearchType>();

    public PrestigeCache prestigeCache;
    public Rarity[] _rarity;
    public GameObject commonPrefab, uncommonPrefab, rarePrefab, epicPrefab, legendaryPrefab;
    public List<Rarity> prestigeBox = new();
    public TMP_Text txtAvailableWorkers;

    private TMP_Text txtPrestigePoints;

    private GameObject objBtnClickAnywhere;
    private float timeToWait;
    //private GameObject objBtnReset, objBtnAdditionalBox;
    public Transform CanvasMain;
    public GameObject objPrefabBoxOpening;
    private Transform content;

    public Energy energy;

    public IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TValue> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while (true)
        {
            yield return values[UnityEngine.Random.Range(0, size)];
        }
    }
    private void Start()
    {
        
    }
    public void OldGenerateRandomBuff(int amountToRoll)
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
    private IEnumerator GenerateRandomBuff(int amountToRoll)
    {
        timeToWait = 1;
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
            yield return new WaitForSeconds(timeToWait);
        }
        objBtnClickAnywhere.SetActive(false);
        timeToWait = 1;
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
        prestigeBox.Clear();
        int amountToRoll = 10;
        InitializeRarityChances(1f, 4f, 10f, 40f);
        StartCoroutine(GenerateRandomBuff(amountToRoll));          
    }
    public void OnClickAnywhereDuringOpening()
    {
        timeToWait = 0;
    }
    private void GenerateRandomCommonPassive()
    {
        int passiveCost = 1;
        foreach (var value in RandomValues(CommonPassive.CommonPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(commonPrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            value.InitializePrestigeStat();
            CraftingType cacheCraftiingType = value.ReturnCraftingType();
            BuildingType cacheBuildingType = value.ReturnBuildingType();
            ResearchType cacheResearchType = value.ReturnResearchType();
            WorkerType cacheWorkerType = value.ReturnWorkerType();


            if (prestigePoints < passiveCost)
            {
                buyButton.interactable = false;
            }

            
            txtBuyAmount.text = passiveCost.ToString();
            txtName.text = value.description;

            // Is this prestigeBox even needed?
            prestigeBox.Add(new Rarity(RarityType.Common, passiveCost, buyButton, cacheCraftiingType, cacheBuildingType, cacheResearchType, cacheWorkerType));
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonCrafting(cacheCraftiingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonBuilding(cacheBuildingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonResearch(cacheResearchType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonWorker(cacheWorkerType); });
            buyButton.onClick.AddListener(value.InitializePrestigeButton);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(passiveCost, buyButton); });
        }
    }
    private void GenerateRandomUncommonPassive()
    {
        int passiveCost = 2;
        foreach (var value in RandomValues(UncommonPassive.UncommonPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(uncommonPrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            value.InitializePrestigeStat();
            CraftingType cacheCraftiingType = value.ReturnCraftingType();
            BuildingType cacheBuildingType = value.ReturnBuildingType();
            ResearchType cacheResearchType = value.ReturnResearchType();
            WorkerType cacheWorkerType = value.ReturnWorkerType();


            if (prestigePoints < passiveCost)
            {
                buyButton.interactable = false;
            }


            txtBuyAmount.text = passiveCost.ToString();
            txtName.text = value.description;

            // Is this prestigeBox even needed?
            prestigeBox.Add(new Rarity(RarityType.Common, passiveCost, buyButton, cacheCraftiingType, cacheBuildingType, cacheResearchType, cacheWorkerType));
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonCrafting(cacheCraftiingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonBuilding(cacheBuildingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonResearch(cacheResearchType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonWorker(cacheWorkerType); });
            buyButton.onClick.AddListener(value.InitializePrestigeButton);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(passiveCost, buyButton); });
        }
    }
    private void GenerateRandomRarePassive()
    {
        int passiveCost = 3;
        foreach (var value in RandomValues(RarePassive.RarePassives).Take(1))
        {
            GameObject prefabObj = Instantiate(rarePrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            value.InitializePrestigeStat();
            CraftingType cacheCraftiingType = value.ReturnCraftingType();
            BuildingType cacheBuildingType = value.ReturnBuildingType();
            ResearchType cacheResearchType = value.ReturnResearchType();
            WorkerType cacheWorkerType = value.ReturnWorkerType();


            if (prestigePoints < passiveCost)
            {
                buyButton.interactable = false;
            }


            txtBuyAmount.text = passiveCost.ToString();
            txtName.text = value.description;

            // Is this prestigeBox even needed?
            prestigeBox.Add(new Rarity(RarityType.Common, passiveCost, buyButton, cacheCraftiingType, cacheBuildingType, cacheResearchType, cacheWorkerType));
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonCrafting(cacheCraftiingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonBuilding(cacheBuildingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonResearch(cacheResearchType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonWorker(cacheWorkerType); });
            buyButton.onClick.AddListener(value.InitializePrestigeButton);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(passiveCost, buyButton); });
        }
    }
    private void GenerateRandomEpicPassive()
    {
        int passiveCost = 4;
        foreach (var value in RandomValues(EpicPassive.EpicPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(epicPrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            value.InitializePrestigeStat();
            CraftingType cacheCraftiingType = value.ReturnCraftingType();
            BuildingType cacheBuildingType = value.ReturnBuildingType();
            ResearchType cacheResearchType = value.ReturnResearchType();
            WorkerType cacheWorkerType = value.ReturnWorkerType();


            if (prestigePoints < passiveCost)
            {
                buyButton.interactable = false;
            }


            txtBuyAmount.text = passiveCost.ToString();
            txtName.text = value.description;

            // Is this prestigeBox even needed?
            prestigeBox.Add(new Rarity(RarityType.Common, passiveCost, buyButton, cacheCraftiingType, cacheBuildingType, cacheResearchType, cacheWorkerType));
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonCrafting(cacheCraftiingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonBuilding(cacheBuildingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonResearch(cacheResearchType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonWorker(cacheWorkerType); });
            buyButton.onClick.AddListener(value.InitializePrestigeButton);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(passiveCost, buyButton); });
        }
    }
    private void GenerateRandomLegendaryPassive()
    {
        int passiveCost = 5;
        foreach (var value in RandomValues(LegendaryPassive.LegendaryPassives).Take(1))
        {
            GameObject prefabObj = Instantiate(legendaryPrefab, content.GetComponent<Transform>());
            Transform tformTxtBuyAmount = prefabObj.GetComponent<Transform>().Find("Buy_Box/Buy_Amount");
            TMP_Text txtBuyAmount = tformTxtBuyAmount.GetComponent<TMP_Text>();
            Transform tformTxtName = prefabObj.GetComponent<Transform>().Find("Text_Name");
            TMP_Text txtName = tformTxtName.GetComponent<TMP_Text>();
            Button buyButton = prefabObj.GetComponent<Button>();

            value.InitializePrestigeStat();
            CraftingType cacheCraftiingType = value.ReturnCraftingType();
            BuildingType cacheBuildingType = value.ReturnBuildingType();
            ResearchType cacheResearchType = value.ReturnResearchType();
            WorkerType cacheWorkerType = value.ReturnWorkerType();


            if (prestigePoints < passiveCost)
            {
                buyButton.interactable = false;
            }


            txtBuyAmount.text = passiveCost.ToString();
            txtName.text = value.description;

            // Is this prestigeBox even needed?
            prestigeBox.Add(new Rarity(RarityType.Common, passiveCost, buyButton, cacheCraftiingType, cacheBuildingType, cacheResearchType, cacheWorkerType));
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonCrafting(cacheCraftiingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonBuilding(cacheBuildingType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonResearch(cacheResearchType); });
            buyButton.onClick.AddListener(delegate { value.InitializePrestigeButtonWorker(cacheWorkerType); });
            buyButton.onClick.AddListener(value.InitializePrestigeButton);
            buyButton.onClick.AddListener(delegate { DeductPrestigePoints(passiveCost, buyButton); });
        }
    }
    public void ClearContent()
    {
        foreach (Transform transformChild in content.GetComponent<Transform>())
        {
            Destroy(transformChild.gameObject);
        }
    }
    public void OnDonePrestiging()
    {
        prestigeBox.Clear();
    }
    private void DeductPrestigePoints(int passiveCost, Button thisButton)
    {
        if (prestigePoints >= passiveCost)
        {
            prestigePoints -= passiveCost;
            cachedPrestigePoints += passiveCost;
            txtPrestigePoints.text = prestigePoints.ToString();

            foreach (var item in prestigeBox)
            {
                if (prestigePoints < item.passiveCost && !item.buyButton.interactable == false)
                {
                    item.buyButton.interactable = false;
                }
                //else
                //{
                //    item.buyButton.interactable = true;
                //}
            }

            thisButton.interactable = false;
        }              
    }
    public void RefundButton()
    {
        prestigePoints += cachedPrestigePoints;
        prestigeCache.ClearBoxCache();
        cachedPrestigePoints = 0;
        txtPrestigePoints.text = prestigePoints.ToString();

        foreach (var item in prestigeBox)
        {
            if (prestigePoints < item.passiveCost)
            {
                item.buyButton.interactable = false;
            }
            else
            {
                item.buyButton.interactable = true;
            }
        }
    }
    [Button]
    public void OnResetGame()
    {
        foreach (var item in Building.Buildings)
        {
            item.Value.ResetBuilding();
        }
        foreach (var item in Researchable.Researchables)
        {
            item.Value.ResetResearchable();
        }
        foreach (var item in Craftable.Craftables)
        {
            item.Value.ResetCraftable();
        }
        foreach (var item in Worker.Workers)
        {
            item.Value.ResetWorker();
        }
        foreach (var item in Resource.Resources)
        {
            item.Value.ResetResource();
        }

        energy.ResetEnergy();

        //PointerNotification.leftAmount = 0;
        //PointerNotification.rightAmount = 0;
        PointerNotification.HandleLeftAnim();
        PointerNotification.HandleRightAnim();

        Worker.UnassignedWorkerCount += Worker.permCountAddition + Worker.prestigeCountAddition;
        Worker.TotalWorkerCount += Worker.permCountAddition + Worker.prestigeCountAddition;
        Worker.prestigeCountAddition = 0;
        txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", Worker.UnassignedWorkerCount);

        PermanentStats.OnResetGame();

        SaveSystem.ClearPrestigeData();
    }
    [Button]
    public void StartPrestige()
    {
        prestigeCache.DeductValues();
        prestigePoints = Worker.TotalWorkerCount * 5;
        InstantiateBoxOpening();
        OnPrestigeChest();
        
        
        // Here I should completely destory everything in prestigeStats.
        // Then deduct from the values inside the child classes, And then nullify everything inside prestigecache.
        // And then going on as normal should be perfectly fine.
        // Which also means I shouldn't clear box cache of the prestigeCache when 
    }
    public void OnAdditionalPrestigeChest()
    {
        if (prestigePoints >= 2)
        {
            objBtnClickAnywhere.SetActive(true);
            prestigePoints -= 2;
            //prestigeCache.ClearBoxCache();
            cachedPrestigePoints = 0;
            txtPrestigePoints.text = prestigePoints.ToString();
            ClearContent();
            OnPrestigeChest();           
        }
    }
    private void DestroyObj(GameObject obj)
    {
        Destroy(obj);
    }
    private void InstantiateBoxOpening()
    {
        GameObject prefabObj = Instantiate(objPrefabBoxOpening, CanvasMain);
        Transform tformPrefabObj = prefabObj.GetComponent<Transform>();

        Button btnDone = tformPrefabObj.Find("Button_Done").GetComponent<Button>();
        GameObject objReset = tformPrefabObj.Find("Button_Reset").gameObject;
        GameObject objChest = tformPrefabObj.Find("Button_Chest").gameObject;
        GameObject objGold = tformPrefabObj.Find("Status_Gold").gameObject;
        GameObject objGems = tformPrefabObj.Find("Status_Gems").gameObject;
        objBtnClickAnywhere = tformPrefabObj.Find("Button_Click_Anywhere").gameObject;
        content = tformPrefabObj.Find("ScrollRect_OpeningBox/Content");
        Button btnReset = objReset.GetComponent<Button>();
        Button btnChest = objChest.GetComponent<Button>();
        txtPrestigePoints = tformPrefabObj.Find("Status_Gold/Text_Prestige").GetComponent<TMP_Text>();

        objGold.SetActive(true);
        objGems.SetActive(false);
        objReset.SetActive(true);
        objChest.SetActive(true);

        txtPrestigePoints.text = prestigePoints.ToString();
        btnChest.onClick.AddListener(OnAdditionalPrestigeChest);
        btnReset.onClick.AddListener(RefundButton);
        btnDone.onClick.AddListener(prestigeCache.OnDoneButton);
        btnDone.onClick.AddListener(delegate { DestroyObj(prefabObj); });
        objBtnClickAnywhere.GetComponent<Button>().onClick.AddListener(OnClickAnywhereDuringOpening);
    }
}
