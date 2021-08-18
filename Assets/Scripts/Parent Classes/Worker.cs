using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum WorkerType
{
    Farmers,
    Woodcutters,
    Miners,
    Hunters,
    Scholars,
    EnergyProducers

    // So do we want hunters, for start. maybe just do for every worker there would be hunts automatically conducted. 
    // So every 500 seconds there's a hunt that lasts 500 seconds. When they leave it costs a certain amount of resources, depending on how many workers was assigned at that time.
    // Maybe have a timer somewhere that your hunters are hunting, maybe in the worker tab.
    // I can grey out the hunter main panel, Change text to "Hunting..." With a progress bar attached like I have in research panel.
    // Maybe just make the progress bar a different color such as orange.
    // And then when they come back you'll get a certain random amount of resources based on some sort of loot pool.
    // food 500 - 1000
    // Leather 0 - 2
    // Bones/tusks?
    // Pelts is the same thing as leather? Should I convert pelts to leather, can pelts be made into something else.
    // Tanning Rack - That seperates the pelt into fur and leather.
    // Should this just be in a new tab workshop where you refine your resources into resources a tier higher? or well in this case from pelt to - leater and fur
    // For example 1 pelt ---> 3 fur and 1 leather.
    // Maybe should have an animals killed variable on the hunts.
    // For each animal 100 - 200 food or whatever and 0 - 2 pelts. Something like that.
    // fur will of course be used for clothing
    // leather can be used for tents as well as various other stuff.
    // Hunter workerType should probably be unlocked when the player has crafted the Stone Spear? 
    // Could have different weapon tiers For example:
    // Wooden Spear - 100 - 200 0 - 2 pelts.
    // Stone Spear 120 - 220 0 - 2 pelts. 
    // Fire Hardended Spear 150 - 250, 0 - 2 pelts.
    // Which is why we should maybe have a weapons tab, but for now lets just do it in the crafting panel.
    // So unlock hunter workertype on the crafting of any weapon type. for in case the player decided to skip other weapon craftings.
}
public struct ResourcesToModify
{
    [System.NonSerialized] public ResourceType resourceTypeToModify;
    [System.NonSerialized] public float resourceMultiplier, incrementAmount, actualIncrementAmount;
    [System.NonSerialized] public bool hasAssignedEnough, hasAssignedNotEnough;
}


public class Worker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Dictionary<WorkerType, Worker> Workers = new Dictionary<WorkerType, Worker>();

    public static uint TotalWorkerCount, UnassignedWorkerCount;
    public static bool isUnlockedEvent;

    [System.NonSerialized] public GameObject objMainPanel;
    [System.NonSerialized] public TMP_Text txtHeader;
    [System.NonSerialized] public bool isUnlocked, hasSeen = true;
    [System.NonSerialized] public ResourcesToModify[] _resourcesToIncrement, _resourcesToDecrement;

    // Make workercount non serialized eventually, for now will use for debugging.
    public uint workerCount;
    public WorkerType Type;
    public GameObject objSpacerBelow;
    public TMP_Text txtAvailableWorkers;

    private Transform _tformTxtHeader, _tformObjMainPanel, _tformTxtDescriptionHeader, _tformTxtDescriptionBody, tformObjTooltip;
    private TMP_Text _txtDescriptionHeader, _txtDescriptionBody;
    private GameObject _objTooltip;
    private string _workerString, _previousText;
    protected uint _changeAmount = 1;
    private bool buttonPressed;

    // This is just for testing
    private bool hasInstantiated;
    private float ContributionAPS;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        _objTooltip.SetActive(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        _objTooltip.SetActive(false);
    }
    private void UpdateResourceInfo()
    {
        foreach (var resource in Resource.Resources)
        {
            foreach (var resourceTypeToIncrement in _resourcesToIncrement)
            {
                if (resource.Key == resourceTypeToIncrement.resourceTypeToModify && !hasInstantiated)
                {
                    resource.Value.resourceInfoList = new List<ResourceInfo>()
                    {
                        new ResourceInfo(){ name = Type.ToString(), amountPerSecond=resourceTypeToIncrement.incrementAmount * workerCount }
                    };

                    hasInstantiated = true;

                    for (int i = 0; i < resource.Value.resourceInfoList.Count; i++)
                    {
                        ResourceInfo resourceInfo = resource.Value.resourceInfoList[i];

                        GameObject newObj = Instantiate(resource.Value.prefabResourceInfo, resource.Value.tformResourceTooltip);

                        Transform _tformNewObj = newObj.transform;
                        Transform _tformInfoName = _tformNewObj.Find("Text_Name");
                        Transform _tformInfoAmountPerSecond = _tformNewObj.Find("Text_AmountPerSecond");

                        resourceInfo.uiForResourceInfo.textInfoName = _tformInfoName.GetComponent<TMP_Text>();
                        resourceInfo.uiForResourceInfo.textInfoAmountPerSecond = _tformInfoAmountPerSecond.GetComponent<TMP_Text>();

                        resourceInfo.uiForResourceInfo.textInfoName.text = Type.ToString();
                        resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", resourceTypeToIncrement.incrementAmount * workerCount);

                        resource.Value.resourceInfoList[i] = resourceInfo;
                    }
                }
                else if (resource.Key == resourceTypeToIncrement.resourceTypeToModify)
                {
                    for (int i = 0; i < resource.Value.resourceInfoList.Count; i++)
                    {
                        ResourceInfo resourceInfo = resource.Value.resourceInfoList[i];
                        resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", resourceTypeToIncrement.incrementAmount * workerCount);
                        resource.Value.resourceInfoList[i] = resourceInfo;
                    }
                }
            }
        }
    }
    private void SetDescriptionText()
    {
        _txtDescriptionHeader.text = string.Format("{0}", Type.ToString());
        foreach (var resourcePlus in _resourcesToIncrement)
        {
            // If there is more than one. I need to make sure it goes to a new line.
            // And if it is a decrement amount, change description text to "decreases"

            _previousText = _txtDescriptionBody.text;

            if (_previousText != "")
            {
                _txtDescriptionBody.text = string.Format("{0}\nIncreases {1} yield by: {2:0.00}", _previousText, resourcePlus.resourceTypeToModify.ToString(), resourcePlus.resourceMultiplier);
            }
            else
            {
                _txtDescriptionBody.text = string.Format("Increases {0} yield by: {1:0.00}", resourcePlus.resourceTypeToModify.ToString(), resourcePlus.resourceMultiplier);
            }
        }
        if (_resourcesToDecrement != null)
        {
            foreach (var resourceMinus in _resourcesToDecrement)
            {
                _previousText = _txtDescriptionBody.text;

                if (_previousText != "")
                {
                    _txtDescriptionBody.text = string.Format("{0}\nDecreases {1} yield by: {2:0.00}", _previousText, resourceMinus.resourceTypeToModify.ToString(), resourceMinus.resourceMultiplier);
                }
                else
                {
                    _txtDescriptionBody.text = string.Format("Decreases {0} yield by: {1:0.00}", resourceMinus.resourceTypeToModify.ToString(), resourceMinus.resourceMultiplier);
                }
            }
        }
    }
    protected void SetInitialValues()
    {
        InitializeObjects();
    }
    protected void InitializeObjects()
    {
        _tformTxtHeader = transform.Find("Panel_Main/Text_Header");
        _tformObjMainPanel = transform.Find("Panel_Main");
        _tformTxtDescriptionHeader = transform.Find("Worker_Tooltip/Header");
        _tformTxtDescriptionBody = transform.Find("Worker_Tooltip/Body");
        tformObjTooltip = transform.Find("Worker_Tooltip");

        txtHeader = _tformTxtHeader.GetComponent<TMP_Text>();
        objMainPanel = _tformObjMainPanel.gameObject;
        _txtDescriptionHeader = _tformTxtDescriptionHeader.GetComponent<TMP_Text>();
        _txtDescriptionBody = _tformTxtDescriptionBody.GetComponent<TMP_Text>();
        _objTooltip = tformObjTooltip.gameObject;

        SetDescriptionText();

        _objTooltip.SetActive(false);

        _workerString = (Type.ToString() + "workerCount");

        workerCount = (uint)PlayerPrefs.GetInt(_workerString, (int)workerCount);
        UnassignedWorkerCount = (uint)PlayerPrefs.GetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        TotalWorkerCount = (uint)PlayerPrefs.GetInt("TotalWorkerCount", (int)TotalWorkerCount);


        txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
        txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", UnassignedWorkerCount);

        if (isUnlocked)
        {
            objMainPanel.SetActive(true);
            objSpacerBelow.SetActive(true);
        }
        else
        {
            objMainPanel.SetActive(false);
            objSpacerBelow.SetActive(false);
        }
        if (AutoToggle.isAutoWorkerOn == 1)
        {
            AutoWorker.CalculateWorkers();
            AutoWorker.AutoAssignWorkers();
        }
    }
    public virtual void OnPlusButton()
    {
        if (UnassignedWorkerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                _changeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (UnassignedWorkerCount < 10)
                {
                    _changeAmount = UnassignedWorkerCount;
                }
                else
                {
                    _changeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (UnassignedWorkerCount < 100)
                {
                    _changeAmount = UnassignedWorkerCount;
                }
                else
                {
                    _changeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                _changeAmount = UnassignedWorkerCount;
            }
            UnassignedWorkerCount -= _changeAmount;
            workerCount += _changeAmount;
            txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
            txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", UnassignedWorkerCount);

            if (_resourcesToDecrement == null)
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].resourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
                }
            }
            else
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].resourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
                }
                for (int i = 0; i < _resourcesToDecrement.Length; i++)
                {
                    _resourcesToDecrement[i].incrementAmount = _changeAmount * _resourcesToDecrement[i].resourceMultiplier;
                    Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToDecrement[i].incrementAmount;
                }
            }

            //incrementAmount = (_changeAmount * resourceMultiplier);
            //Resource.Resources[resourceTypeToModify].amountPerSecond += incrementAmount;
            //ContributionAPS = workerCount * _resourcesToIncrement[i].resourceMultiplier;
            UpdateResourceInfo();
        }
    }
    public virtual void OnMinusButton()
    {
        if (workerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                _changeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (workerCount < 10)
                {
                    _changeAmount = workerCount;
                }
                else
                {
                    _changeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (workerCount < 100)
                {
                    _changeAmount = workerCount;
                }
                else
                {
                    _changeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                _changeAmount = workerCount;
            }
            UnassignedWorkerCount += _changeAmount;
            workerCount -= _changeAmount;
            txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
            txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", UnassignedWorkerCount);

            if (_resourcesToDecrement == null)
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].resourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
                }
            }
            else
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].resourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
                }
                for (int i = 0; i < _resourcesToDecrement.Length; i++)
                {
                    _resourcesToDecrement[i].incrementAmount = _changeAmount * _resourcesToDecrement[i].resourceMultiplier;
                    Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond += _resourcesToDecrement[i].incrementAmount;
                }
            }

            //incrementAmount = (_changeAmount * resourceMultiplier);
            //Resource.Resources[resourceTypeToModify].amountPerSecond -= incrementAmount;
            UpdateResourceInfo();
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        PlayerPrefs.SetInt(_workerString, (int)workerCount);
        PlayerPrefs.SetInt("TotalWorkerCount", (int)TotalWorkerCount);
    }
}
