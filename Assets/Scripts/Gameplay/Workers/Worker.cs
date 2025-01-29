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
    Scholars,
    None
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

    [System.NonSerialized] public GameObject ObjMainPanel;
    [System.NonSerialized] public Canvas Canvas;
    [System.NonSerialized] public GraphicRaycaster GraphicRaycaster;
    [System.NonSerialized] public TMP_Text txtHeader;
    [System.NonSerialized] public bool IsUnlocked, HasSeen = true, IsFirstUnlocked;
    public WorkerResourcesToModify[] _resourcesToIncrement, _resourcesToDecrement;

    // Make workercount non serialized eventually, for now will use for debugging.
    public uint workerCount;
    public WorkerType Type;
    public TMP_Text txtAvailableWorkers;
    public string ActualName;

    private Transform _tformTxtHeader, _tformObjMainPanel, _tformTxtDescriptionHeader, _tformTxtDescriptionBody, tformObjTooltip, _tformBtnPlus, _tformBtnMinus;
    private TMP_Text TxtDescriptionHeader, TxtDescriptionBody;
    private GameObject _objTooltip;
    private string _workerString, _IsUnlockedString;
    protected uint _changeAmount = 1;
    private Button _btnPlus, _btnMinus;

    public string strDescription;

    //Reset variables
    public static uint permCountAddition, prestigeCountAddition;

    public float permAllMultiplierAddition, permMultiplierAddition;

    public float prestigeAllMultiplierAddition, prestigeMultiplierAddition;

    private string _strPermCountAddition, _strPrestigeCountAddition, _strPermAllMultiplierAddition, _strPermMultiplierAddition, _strPrestigeAllMultiplierAddition, _strPrestigeMultiplierAddition;

    public static uint trackedWorkerCount;
    public void ModifyWorkerCount()
    {
        TotalWorkerCount += (prestigeCountAddition + permCountAddition);
        prestigeCountAddition = 0;

        Debug.Log(string.Format("Changed total workers from {0} to {1}", "Hopefully 0", TotalWorkerCount));
    }
    public void ModifyMultiplierForPrestige()
    {
        for (int i = 0; i < _resourcesToIncrement.Length; i++)
        {
            _resourcesToIncrement[i].currentResourceMultiplier = _resourcesToIncrement[i].baseResourceMultiplier;
            float additionAmount = _resourcesToIncrement[i].baseResourceMultiplier * ((prestigeAllMultiplierAddition + permAllMultiplierAddition) + (prestigeMultiplierAddition + permMultiplierAddition));;
            prestigeAllMultiplierAddition = 0;
            prestigeMultiplierAddition = 0;
            _resourcesToIncrement[i].currentResourceMultiplier += additionAmount;
            Debug.Log(string.Format("Changed worker {0}'s resource multi from {1} to {2}", ActualName, _resourcesToIncrement[i].baseResourceMultiplier, _resourcesToIncrement[i].currentResourceMultiplier));
        }
    }
    public void MultiplyIncrementAmount(float multiplierAmount, WorkerType workerType)
    {
        for (int i = 0; i < _resourcesToIncrement.Length; i++)
        {
            //_resourcesToIncrement[i].currentResourceMultiplier = _resourcesToIncrement[i].baseResourceMultiplier;
            //float additionAmount = _resourcesToIncrement[i].baseResourceMultiplier * ((prestigeAllMultiplierAddition + permAllMultiplierAddition) + (prestigeMultiplierAddition + permMultiplierAddition)); ;
            //prestigeAllMultiplierAddition = 0;
            //restigeMultiplierAddition = 0;
            float newAmount = _resourcesToIncrement[i].currentResourceMultiplier * multiplierAmount;
            float diffAmount = newAmount - _resourcesToIncrement[i].currentResourceMultiplier;
            _resourcesToIncrement[i].currentResourceMultiplier = newAmount;
            SetDescriptionText();
            Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += diffAmount * Workers[workerType].workerCount;
            //Debug.Log(string.Format("Changed worker {0}'s resource multi from {1} to {2}", ActualName, _resourcesToIncrement[i].baseResourceMultiplier, _resourcesToIncrement[i].currentResourceMultiplier));
        }
    }
    public void ModifyDescriptionText()
    {
        SetDescriptionText();
    }
    public void ResetWorker()
    {
        //if (_resourcesToDecrement == null)
        //{
        //    for (int i = 0; i < _resourcesToIncrement.Length; i++)
        //    {
        //        _resourcesToIncrement[i].incrementAmount = workerCount * _resourcesToIncrement[i].currentResourceMultiplier;
        //        Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
        //        StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < _resourcesToIncrement.Length; i++)
        //    {
        //        _resourcesToIncrement[i].incrementAmount = workerCount * _resourcesToIncrement[i].currentResourceMultiplier;
        //        Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
        //        StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        //    }
        //    for (int i = 0; i < _resourcesToDecrement.Length; i++)
        //    {
        //        _resourcesToDecrement[i].incrementAmount = workerCount * _resourcesToDecrement[i].currentResourceMultiplier;
        //        Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond += _resourcesToDecrement[i].incrementAmount;
        //        StaticMethods.ModifyAPSText(Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
        //    }
        //}

        IsUnlocked = false;
        ObjMainPanel.SetActive(false);
        Canvas.enabled = false;
        GraphicRaycaster.enabled = false;
        workerCount = 0;
        HasSeen = true;
        TotalWorkerCount = 0;
        UnassignedWorkerCount = 0;
        ModifyMultiplierForPrestige();
        //ModifyWorkerCount();
        SetDescriptionText();
        txtHeader.text = string.Format("{0} [<color=#FFCBFA>{1}</color>]", ActualName.ToString(), workerCount);
        txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", UnassignedWorkerCount);
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
        TxtDescriptionHeader.text = string.Format("{0}", ActualName);
        strDescription = "";

        foreach (var resourcePlus in _resourcesToIncrement)
        {
            strDescription += string.Format("Increases {0} yield by: {1:0.00}\n", resourcePlus.resourceTypeToModify.ToString(), resourcePlus.currentResourceMultiplier);
        }
        if (_resourcesToDecrement != null)
        {
            foreach (var resourceMinus in _resourcesToDecrement)
            {

                strDescription += string.Format("Decreases {0} yield by: {1:0.00}\n", resourceMinus.resourceTypeToModify.ToString(), resourceMinus.currentResourceMultiplier);
            }
        }

        TxtDescriptionBody.text = strDescription;
    }
    protected void SetInitialValues()
    {
        InitializeObjects();

        if (IsUnlocked)
        {
            ObjMainPanel.SetActive(true);
            Canvas.enabled = false;
            GraphicRaycaster.enabled = false;
        }
        else
        {
            ObjMainPanel.SetActive(false);
            Canvas.enabled = false;
            GraphicRaycaster.enabled = false;
        }

        if (AutoToggle.isAutoWorkerOn == 1)
        {
            AutoWorker.CalculateWorkers();
            AutoWorker.AutoAssignWorkers();
        }
    }
    protected void InitializeObjects()
    {
        GraphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
        Canvas = gameObject.GetComponent<Canvas>();
        _tformTxtHeader = transform.Find("Panel_Main/Text_Header");
        _tformBtnMinus = transform.Find("Panel_Main/Button_Minus");
        _tformBtnPlus = transform.Find("Panel_Main/Button_Plus");
        _tformObjMainPanel = transform.Find("Panel_Main");
        _tformTxtDescriptionHeader = transform.Find("Worker_Tooltip/Content/Header");
        _tformTxtDescriptionBody = transform.Find("Worker_Tooltip/Content/Body");
        tformObjTooltip = transform.Find("Worker_Tooltip");

        txtHeader = _tformTxtHeader.GetComponent<TMP_Text>();
        _btnPlus = _tformBtnPlus.GetComponent<Button>();
        _btnMinus = _tformBtnMinus.GetComponent<Button>();
        ObjMainPanel = _tformObjMainPanel.gameObject;
        TxtDescriptionHeader = _tformTxtDescriptionHeader.GetComponent<TMP_Text>();
        TxtDescriptionBody = _tformTxtDescriptionBody.GetComponent<TMP_Text>();
        _objTooltip = tformObjTooltip.gameObject;

        _btnPlus.onClick.AddListener(OnPlusButton);
        _btnMinus.onClick.AddListener(OnMinusButton);

        SetDescriptionText();

        _objTooltip.SetActive(false);

        _workerString = (Type.ToString() + "workerCount");
        _IsUnlockedString = (Type.ToString() + "IsUnlocked");

        AssignPrestigeStrings();

        workerCount = (uint)PlayerPrefs.GetInt(_workerString, (int)workerCount);
        UnassignedWorkerCount = (uint)PlayerPrefs.GetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        TotalWorkerCount = (uint)PlayerPrefs.GetInt("TotalWorkerCount", (int)TotalWorkerCount);
        IsUnlocked = PlayerPrefs.GetInt(_IsUnlockedString) == 1 ? true : false;

        FetchPrestigeValues();


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
            txtHeader.text = string.Format("{0} [<color=#FFCBFA>{1}</color>]", ActualName.ToString(), workerCount);
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
            txtHeader.text = string.Format("{0} [<color=#FFCBFA>{1}</color>]", ActualName, workerCount);
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
    private void AssignPrestigeStrings()
    {
        _strPermCountAddition = Type.ToString() + "permCountAddition";
        _strPermAllMultiplierAddition = Type.ToString() + "permAllMultiplierAddition";
        _strPermMultiplierAddition = Type.ToString() + "permMultiplierAddition";

        _strPrestigeCountAddition = Type.ToString() + "PrestigeCountAddition";
        _strPrestigeAllMultiplierAddition = Type.ToString() + "PrestigeAllMultiplierAddition";
        _strPrestigeMultiplierAddition = Type.ToString() + "PrestigeMultiplierAddition";
    }
    private void SavePrestigeValues()
    {
        PlayerPrefs.SetInt(_strPermCountAddition, (int)permCountAddition);
        PlayerPrefs.SetFloat(_strPermAllMultiplierAddition, permAllMultiplierAddition);
        PlayerPrefs.SetFloat(_strPermMultiplierAddition, permMultiplierAddition);

        PlayerPrefs.SetInt(_strPrestigeCountAddition, (int)prestigeCountAddition);
        PlayerPrefs.SetFloat(_strPrestigeAllMultiplierAddition, prestigeAllMultiplierAddition);
        PlayerPrefs.SetFloat(_strPrestigeMultiplierAddition, prestigeMultiplierAddition);
    }
    private void FetchPrestigeValues()
    {
        permCountAddition = (uint)PlayerPrefs.GetInt(_strPermCountAddition, (int)permCountAddition);
        permAllMultiplierAddition = PlayerPrefs.GetFloat(_strPermAllMultiplierAddition, permAllMultiplierAddition);
        permMultiplierAddition = PlayerPrefs.GetFloat(_strPermMultiplierAddition, permMultiplierAddition);

        prestigeCountAddition = (uint)PlayerPrefs.GetInt(_strPrestigeCountAddition, (int)prestigeCountAddition);
        prestigeAllMultiplierAddition = PlayerPrefs.GetFloat(_strPrestigeAllMultiplierAddition, prestigeAllMultiplierAddition);
        prestigeMultiplierAddition = PlayerPrefs.GetFloat(_strPrestigeMultiplierAddition, prestigeMultiplierAddition);
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("UnassignedWorkerCount", (int)UnassignedWorkerCount);
        PlayerPrefs.SetInt(_workerString, (int)workerCount);
        PlayerPrefs.SetInt("TotalWorkerCount", (int)TotalWorkerCount);
        PlayerPrefs.SetInt(_IsUnlockedString, IsUnlocked ? 1 : 0);

        SavePrestigeValues();
    }
}
