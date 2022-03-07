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
public struct WorkerResourcesToModify
{
    public ResourceType resourceTypeToModify;
    public float baseResourceMultiplier, currentResourceMultiplier;
    [System.NonSerialized] public float incrementAmount, actualIncrementAmount;
    [System.NonSerialized] public bool hasAssignedEnough, hasAssignedNotEnough, hasInstantiated;
}


public class Worker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Dictionary<WorkerType, Worker> Workers = new Dictionary<WorkerType, Worker>();

    public static uint TotalWorkerCount, UnassignedWorkerCount, AliveCount, DeadCount;
    public static bool isWorkerUnlockedEvent;

    [System.NonSerialized] public GameObject objMainPanel;
    [System.NonSerialized] public Canvas canvas;
    [System.NonSerialized] public GraphicRaycaster graphicRaycaster;
    [System.NonSerialized] public TMP_Text txtHeader;
    [System.NonSerialized] public bool isUnlocked, hasSeen = true, isFirstUnlocked;
    public WorkerResourcesToModify[] _resourcesToIncrement, _resourcesToDecrement;

    // Make workercount non serialized eventually, for now will use for debugging.
    public uint workerCount;
    public WorkerType Type;
    public TMP_Text txtAvailableWorkers;
    public string actualName;

    private Transform _tformTxtHeader, _tformObjMainPanel, _tformTxtDescriptionHeader, _tformTxtDescriptionBody, tformObjTooltip, _tformBtnPlus, _tformBtnMinus;
    private TMP_Text _txtDescriptionHeader, _txtDescriptionBody;
    private GameObject _objTooltip;
    private string _workerString, _previousText, _isUnlockedString;
    protected uint _changeAmount = 1;
    private Button _btnPlus, _btnMinus;

    public void ModifyDescriptionText()
    {
        SetDescriptionText();

        // Check worker decription text, might still need some work.
    }
    public void ResetWorker()
    {
        isUnlocked = false;
        objMainPanel.SetActive(false);
        workerCount = 0;
        hasSeen = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _objTooltip.SetActive(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _objTooltip.SetActive(false);
    }
    protected void UpdateResourceInfo()
    {
        foreach (var resourceToIncrement in _resourcesToIncrement)
        {
            float workerAmountPerSecond = workerCount * resourceToIncrement.currentResourceMultiplier;
            Resource.Resources[resourceToIncrement.resourceTypeToModify].UpdateResourceInfo(gameObject, workerAmountPerSecond, resourceToIncrement.resourceTypeToModify);
        }

        foreach (var resourceToDecrement in _resourcesToDecrement)
        {
            float workerAmountPerSecond = workerCount * resourceToDecrement.currentResourceMultiplier;
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
                _txtDescriptionBody.text = string.Format("{0}\nIncreases {1} yield by: {2:0.00}", _previousText, resourcePlus.resourceTypeToModify.ToString(), resourcePlus.currentResourceMultiplier);
            }
            else
            {
                _txtDescriptionBody.text = string.Format("Increases {0} yield by: {1:0.00}", resourcePlus.resourceTypeToModify.ToString(), resourcePlus.currentResourceMultiplier);
            }
        }
        if (_resourcesToDecrement != null)
        {
            foreach (var resourceMinus in _resourcesToDecrement)
            {
                _previousText = _txtDescriptionBody.text;

                if (_previousText != "")
                {
                    _txtDescriptionBody.text = string.Format("{0}\nDecreases {1} yield by: {2:0.00}", _previousText, resourceMinus.resourceTypeToModify.ToString(), resourceMinus.currentResourceMultiplier);
                }
                else
                {
                    _txtDescriptionBody.text = string.Format("Decreases {0} yield by: {1:0.00}", resourceMinus.resourceTypeToModify.ToString(), resourceMinus.currentResourceMultiplier);
                }
            }
        }
    }
    protected void SetInitialValues()
    {
        InitializeObjects();

        if (isUnlocked)
        {
            objMainPanel.SetActive(true);
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
        }
        else
        {
            objMainPanel.SetActive(false);
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
        }

        if (AutoToggle.isAutoWorkerOn == 1)
        {
            AutoWorker.CalculateWorkers();
            AutoWorker.AutoAssignWorkers();
        }
    }
    protected void InitializeObjects()
    {
        graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        canvas = gameObject.GetComponent<Canvas>();
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
        _isUnlockedString = (Type.ToString() + "isUnlocked");

        workerCount = (uint)PlayerPrefs.GetInt(_workerString, (int)workerCount);
        UnassignedWorkerCount = (uint)PlayerPrefs.GetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        TotalWorkerCount = (uint)PlayerPrefs.GetInt("TotalWorkerCount", (int)TotalWorkerCount);
        isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;


        txtHeader.text = string.Format("{0} [<color=#FFCBFA>{1}</color>]", Type.ToString(), workerCount);
        txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", UnassignedWorkerCount);
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
            txtHeader.text = string.Format("{0} [<color=#FFCBFA>{1}</color>]", actualName.ToString(), workerCount);
            txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", UnassignedWorkerCount);

            if (_resourcesToDecrement == null)
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].currentResourceMultiplier;
                    
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
                    StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
                }
            }
            else
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].currentResourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
                    StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
                }
                for (int i = 0; i < _resourcesToDecrement.Length; i++)
                {
                    _resourcesToDecrement[i].incrementAmount = _changeAmount * _resourcesToDecrement[i].currentResourceMultiplier;
                    Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToDecrement[i].incrementAmount;
                    StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
                    // Should the above not be toDecrement?
                }
            }

            //incrementAmount = (_changeAmount * currentResourceMultiplier);
            //Resource.Resources[resourceTypeToModify].amountPerSecond += incrementAmount;
            //ContributionAPS = workerCount * _resourcesToIncrement[i].currentResourceMultiplier;
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
            // FFCBFA <color=#FFCBFA>{1}</color>
            txtHeader.text = string.Format("{0} [<color=#FFCBFA>{1}</color>]", actualName, workerCount);
            txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", UnassignedWorkerCount);

            if (_resourcesToDecrement == null)
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].currentResourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
                    StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
                }
            }
            else
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].currentResourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
                    StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
                }
                for (int i = 0; i < _resourcesToDecrement.Length; i++)
                {
                    _resourcesToDecrement[i].incrementAmount = _changeAmount * _resourcesToDecrement[i].currentResourceMultiplier;
                    Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond += _resourcesToDecrement[i].incrementAmount;
                    StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
                }
            }

            //incrementAmount = (_changeAmount * currentResourceMultiplier);
            //Resource.Resources[resourceTypeToModify].amountPerSecond -= incrementAmount;
            UpdateResourceInfo();
        }
    }
    public virtual void ModifyMultiplier()
    {

    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        PlayerPrefs.SetInt(_workerString, (int)workerCount);
        PlayerPrefs.SetInt("TotalWorkerCount", (int)TotalWorkerCount);
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
    }
}
