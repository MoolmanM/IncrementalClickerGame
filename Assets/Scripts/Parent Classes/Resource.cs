using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct UiForResource
{
    public TMP_Text txtStorageAmount;
    public TMP_Text txtAmount;
    public TMP_Text txtAmountPerSecond;
}

public enum ResourceType
{
    // Might make Energy it's own thing, that will display at the top of the screen.
    // Reason being, energy doesn't really need storage, because there will never be a time where you don't use all of your energy.
    // Or is there, need some more brainstorming.
    // Maybe have another gather button, Gather Stones?
    // How will I handle ores and metals and such?
    // Do I want every single metal? Steel, Iron, Aluminum, Magnesium, Copper, Brass, Bronze, Zinc, Titanium, Tungsten, Adamantium, Nickel, Cobalt, Tin, Lead, Silicon

    Food,
    Lumber,
    Stone,
    Knowledge,
    Pelts,
    Copper,
    Tin,
    Bronze,
    Iron

}

public struct ResourceInfo
{
    public float amountPerSecond;
    public string name;
    public GameObject objModifiedBy;
    //public BuildingType buildingAssociated;
    //public WorkerType workerAssociated;
    public UiForResourceInfo uiForResourceInfo;
}

public struct UiForResourceInfo
{
    public GameObject objMainPanel, objSpacer, objTop, objMid, objBot, objGroup;
    public TMP_Text textInfoName, textInfoAmountPerSecond, textObjName, textObjAPS;
    public Transform tformNewObj, tformInfoName, tformInfoAmountPerSecond;
}

public class Resource : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Dictionary<ResourceType, Resource> Resources = new Dictionary<ResourceType, Resource>();

    public List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();
    public Dictionary<GameObject, ResourceInfo> resourceInfoDict = new Dictionary<GameObject, ResourceInfo>();

    [System.NonSerialized] public GameObject prefabObjGroup, prefabObjTop, prefabObjMid, prefabObjBot;
    [System.NonSerialized] public Transform tformObjTooltipGroup;
    [System.NonSerialized] public GameObject objTooltipGroup;

    [System.NonSerialized] public float amount, amountPerSecond;
    [System.NonSerialized] public bool isUnlocked;
    [System.NonSerialized] public UiForResource uiForResource;
    [System.NonSerialized] public GameObject objMainPanel;
    [System.NonSerialized] public Canvas canvas;
    [System.NonSerialized] public GraphicRaycaster graphicRaycaster;

    public float storageAmount, initialStorageAmount;
    public ResourceType Type;

    protected string _perSecondString, _amountString, _storageAmountString, _isUnlockedString;

    protected Transform _tformTxtAmount, _tformTxtAmountPerSecond, _tformTxtStorage, _tformImgbar;
    protected Image _imgBar;
    protected float _timer = 0.1f;


    public float debugAmountToIncrease;
    //private float initialAmount;

    // Testing seems to be working perfectly so far.
    protected float cachedAmount;

    // New testing
    public float initialAmount;

    public void ResetResource()
    {
        amount = 0;
        amountPerSecond = 0;
        canvas.enabled = false;
        graphicRaycaster.enabled = false;
        isUnlocked = false;
        storageAmount = initialStorageAmount;
        uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);
        StaticMethods.ModifyAPSText(amountPerSecond, uiForResource.txtAmountPerSecond);
        GetCurrentFill();
        Resources[ResourceType.Food].isUnlocked = true;
        Resources[ResourceType.Lumber].isUnlocked = true;
        Resources[ResourceType.Lumber].canvas.enabled = true;
        Resources[ResourceType.Food].canvas.enabled = true;
        Resources[ResourceType.Lumber].graphicRaycaster.enabled = true;
        Resources[ResourceType.Food].graphicRaycaster.enabled = true;
        // Set storage amount back to original storage amount
        // Remove the resourceinfo prefabs?
    }
    private void Start()
    {
        SetStartingResource();
        GetCurrentFill();
    }
    public void UpdateResourceInfo(GameObject obj, float amountPerSecond, ResourceType resourceTypeToModify)
    {
        for (int i = 0; i < resourceInfoList.Count; i++)
        {
            ResourceInfo resourceInfo = resourceInfoList[i];

            if (resourceTypeToModify == Type && resourceInfo.objModifiedBy == obj)
            {
                resourceInfo.amountPerSecond = amountPerSecond;

                if (amountPerSecond == 0)
                {
                    resourceInfo.uiForResourceInfo.objMainPanel.SetActive(false);
                }
                else
                {
                    resourceInfo.uiForResourceInfo.objMainPanel.SetActive(true);
                }

                if (amountPerSecond < 0)
                {
                    resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("<color=#C63434>{0:0.00}/sec</color>", resourceInfo.amountPerSecond);
                }
                else
                {
                    resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", resourceInfo.amountPerSecond);
                }

                resourceInfoList[i] = resourceInfo;
            }
        }
    }
    public void SetInitialAmount(float percentageAmount)
    {
        initialAmount = storageAmount * percentageAmount;
    }
    public void InitializeAmount()
    {
        amount = initialAmount;
    }
    [Button]
    private void DebugIncreaseResource()
    {
        if (debugAmountToIncrease > 0)
        {
            amount += debugAmountToIncrease;
        }
        else
        {
            amount += 20000;
        }

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (resourceInfoList != null && resourceInfoList.Any())
        {
            objTooltipGroup.SetActive(true);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        objTooltipGroup.SetActive(false);
    }
    private void InitializePrefab()
    {
        //prefabResourceInfoPanel = UnityEngine.Resources.Load<GameObject>("ResourceInfo_Prefab/ResourceInfo_Panel");
        //prefabResourceInfoSpacer = UnityEngine.Resources.Load<GameObject>("ResourceInfo_Prefab/Spacer");

        //tformResourceTooltip = transform.Find("Resource_Tooltip");

        //objTooltip = tformResourceTooltip.gameObject;

        #region This is new
        prefabObjTop = UnityEngine.Resources.Load<GameObject>("ResourceInfo_Prefab/Top");
        prefabObjMid = UnityEngine.Resources.Load<GameObject>("ResourceInfo_Prefab/Mid");
        prefabObjBot = UnityEngine.Resources.Load<GameObject>("ResourceInfo_Prefab/Bot");

        tformObjTooltipGroup = transform.Find("Tooltip_Group");

        objTooltipGroup = tformObjTooltipGroup.gameObject;

        #endregion
    }
    public virtual void SetInitialValues()
    {
        InitializeObjects();
        //if (TimeManager.hasPlayedBefore)
        //{
            isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;
            if (isUnlocked)
            {
                amount = PlayerPrefs.GetFloat(_amountString, amount);
                amountPerSecond = PlayerPrefs.GetFloat(_perSecondString, amountPerSecond);
                storageAmount = PlayerPrefs.GetFloat(_storageAmountString, storageAmount);
            }         
        //}
        if (isUnlocked)
        {
            objMainPanel.SetActive(true);
            canvas.enabled = true;
        }
        else
        {
            objMainPanel.SetActive(false);
            canvas.enabled = false;
        }
    }
    private void SetStartingResource()
    {
        // Display amount and amount per second.
        StaticMethods.ModifyAPSText(amountPerSecond, uiForResource.txtAmountPerSecond);
        uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);
    }
    private void InitializeObjects()
    {
        _tformTxtAmount = transform.Find("Header_Panel/Text_Amount");
        _tformTxtAmountPerSecond = transform.Find("Header_Panel/Text_Amount_Per_Second");
        _tformTxtStorage = transform.Find("Header_Panel/Text_Storage");
        _tformImgbar = transform.Find("Storage_Fill_Bar");

        _imgBar = _tformImgbar.GetComponent<Image>();
        uiForResource.txtAmount = _tformTxtAmount.GetComponent<TMP_Text>();
        uiForResource.txtAmountPerSecond = _tformTxtAmountPerSecond.GetComponent<TMP_Text>();
        uiForResource.txtStorageAmount = _tformTxtStorage.GetComponent<TMP_Text>();

        objMainPanel = gameObject;
        canvas = gameObject.GetComponent<Canvas>();
        graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();

        _perSecondString = Type.ToString() + "PS";
        _amountString = Type.ToString() + "A";
        _storageAmountString = Type.ToString() + "Storage";
        _isUnlockedString = Type.ToString() + "Unlocked";

        InitializePrefab();
    }
    public void GetCurrentFill()
    {
        float add = 0;
        float div = 0;
        float fillAmount = 0;

        add = amount;
        div = storageAmount;
        if (add > div)
        {
            add = div;
        }

        fillAmount += add / div;
        _imgBar.fillAmount = fillAmount;
    }
    protected virtual void DeprecatedUpdateResource()
    {
        if (isUnlocked)
        {
            if ((_timer -= Time.deltaTime) <= 0)
            {
                _timer = 0.1f;

                if (amount >= (storageAmount - amountPerSecond))
                {
                    amount = storageAmount;
                }
                else
                {
                    amount += (amountPerSecond / 10);
                }

                uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);

                GetCurrentFill();
            }
        }

    }
    protected virtual void UpdateResource()
    {
        // Can probably check here every tick if amount == the previous cached amount
        // And if it is not equal to the cached variable, then and only then update the text field
        // otherwise you're just calling the code for nothing.
        if (isUnlocked)
        {
            if ((_timer -= Time.deltaTime) <= 0)
            {
                _timer = 0.1f;
                if (amount != storageAmount)
                {
                    if (amount >= (storageAmount - amountPerSecond))
                    {
                        amount = storageAmount;
                    }
                    else
                    {
                        amount += (amountPerSecond / 10);
                    }

                    if (amount != cachedAmount)
                    {
                        uiForResource.txtAmount.text = string.Format("{0:0.00}", NumberToLetter.FormatNumber(amount));
                    }

                    GetCurrentFill();

                    cachedAmount = amount;
                }
                else if (amountPerSecond <= 0.00f)
                {
                    amount += (amountPerSecond / 10);

                    if (amount != cachedAmount)
                    {
                        uiForResource.txtAmount.text = string.Format("{0:0.00}", NumberToLetter.FormatNumber(amount));
                    }
                }
                cachedAmount = amount;
            }
        }

    }
    protected virtual void Update()
    {
        UpdateResource();
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(_amountString, amount);
        PlayerPrefs.SetFloat(_perSecondString, amountPerSecond);
        PlayerPrefs.SetFloat(_storageAmountString, storageAmount);
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
    }
}
