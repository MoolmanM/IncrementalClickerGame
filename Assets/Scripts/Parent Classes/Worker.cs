using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum WorkerType
{
    Farmers,
    Woodcutters,
    Miners,
    Hunters,
    Scholars
}
[System.Serializable]
public struct ResourcesToModify
{
    public ResourceType resourceTypeToModify;
    public float resourceMultiplier;
    [System.NonSerialized] public float incrementAmount, actualIncrementAmount;
    [System.NonSerialized] public bool hasAssignedEnough, hasAssignedNotEnough, hasInstantiated;
}


public class Worker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Dictionary<WorkerType, Worker> Workers = new Dictionary<WorkerType, Worker>();

    public static uint TotalWorkerCount, UnassignedWorkerCount;
    public static bool isWorkerUnlockedEvent;

    [System.NonSerialized] public GameObject objMainPanel;
    [System.NonSerialized] public TMP_Text txtHeader;
    [System.NonSerialized] public bool isUnlocked, hasSeen = true;
    public ResourcesToModify[] _resourcesToIncrement, _resourcesToDecrement;

    // Make workercount non serialized eventually, for now will use for debugging.
    public uint workerCount;
    public WorkerType Type;
    public GameObject objSpacerBelow;
    public TMP_Text txtAvailableWorkers;

    private Transform _tformTxtHeader, _tformObjMainPanel, _tformTxtDescriptionHeader, _tformTxtDescriptionBody, tformObjTooltip, _tformBtnPlus, _tformBtnMinus;
    private TMP_Text _txtDescriptionHeader, _txtDescriptionBody;
    private GameObject _objTooltip;
    private string _workerString, _previousText;
    protected uint _changeAmount = 1;
    private Button _btnPlus, _btnMinus;
    //private bool buttonPressed;

    public void ResetWorker()
    {
        isUnlocked = false;
        objMainPanel.SetActive(false);
        objSpacerBelow.SetActive(false);
        workerCount = 0;
        hasSeen = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //buttonPressed = true;
        _objTooltip.SetActive(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //buttonPressed = false;
        _objTooltip.SetActive(false);
    }
    protected void UpdateResourceInfo()
    {
        foreach (var resourceToIncrement in _resourcesToIncrement)
        {
            float workerAmountPerSecond = workerCount * resourceToIncrement.resourceMultiplier;
            Resource.Resources[resourceToIncrement.resourceTypeToModify].UpdateResourceInfo(gameObject, workerAmountPerSecond, resourceToIncrement.resourceTypeToModify);
        }

        foreach (var resourceToDecrement in _resourcesToDecrement)
        {
            float workerAmountPerSecond = workerCount * resourceToDecrement.resourceMultiplier;
            Resource.Resources[resourceToDecrement.resourceTypeToModify].UpdateResourceInfo(gameObject, -workerAmountPerSecond, resourceToDecrement.resourceTypeToModify);
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
        _tformBtnMinus = transform.Find("Panel_Main/Button_Minus");
        _tformBtnPlus = transform.Find("Panel_Main/Button_Plus");
        _tformObjMainPanel = transform.Find("Panel_Main");
        _tformTxtDescriptionHeader = transform.Find("Worker_Tooltip/Header");
        _tformTxtDescriptionBody = transform.Find("Worker_Tooltip/Body");
        tformObjTooltip = transform.Find("Worker_Tooltip");

        txtHeader = _tformTxtHeader.GetComponent<TMP_Text>();
        _btnPlus = _tformBtnPlus.GetComponent<Button>();
        _btnMinus = _tformBtnMinus.GetComponent<Button>();
        objMainPanel = _tformObjMainPanel.gameObject;
        _txtDescriptionHeader = _tformTxtDescriptionHeader.GetComponent<TMP_Text>();
        _txtDescriptionBody = _tformTxtDescriptionBody.GetComponent<TMP_Text>();
        _objTooltip = tformObjTooltip.gameObject;

        _btnPlus.onClick.AddListener(OnPlusButton);
        _btnMinus.onClick.AddListener(OnMinusButton);

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
    protected virtual void OnPlusButton()
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
    protected virtual void OnMinusButton()
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
