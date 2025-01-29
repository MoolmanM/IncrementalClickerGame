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
    Leather,
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
    public GameObject ObjMainPanel, objSpacer, objTop, objMid, objBot, objGroup;
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
    [System.NonSerialized] public bool IsUnlocked;
    [System.NonSerialized] public UiForResource uiForResource;
    [System.NonSerialized] public GameObject ObjMainPanel;
    [System.NonSerialized] public Canvas Canvas;
    [System.NonSerialized] public GraphicRaycaster GraphicRaycaster;

    public float storageAmount, baseStorageAmount;
    public ResourceType Type;

    protected string _perSecondString, _amountString, _storageAmountString, _IsUnlockedString;

    protected Transform _tformTxtAmount, _tformTxtAmountPerSecond, _tformTxtStorage, _tformImgbar;
    protected Image _imgBar;
    protected float timer = 0.1f;


    public float debugAmountToIncrease;
    //private float initialAmount;

    // Testing seems to be working perfectly so far.
    protected float cachedAmount;
    public float trackedAmount;

    // New testing
    public float initialAmount;

    public void ResetResource()
    {
        amount = 0;
        amountPerSecond = 0;
        Canvas.enabled = false;
        GraphicRaycaster.enabled = false;
        IsUnlocked = false;
        storageAmount = baseStorageAmount;
        uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);
        StaticMethods.ModifyAPSText(amountPerSecond, uiForResource.txtAmountPerSecond);
        GetCurrentFill();
        Resources[ResourceType.Food].IsUnlocked = true;
        Resources[ResourceType.Lumber].IsUnlocked = true;
        Resources[ResourceType.Lumber].Canvas.enabled = true;
        Resources[ResourceType.Food].Canvas.enabled = true;
        Resources[ResourceType.Lumber].GraphicRaycaster.enabled = true;
        Resources[ResourceType.Food].GraphicRaycaster.enabled = true;
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
        // This is for the additional resource info tooltip, not currently using it since I need to update the graphics.

        for (int i = 0; i < resourceInfoList.Count; i++)
        {
            ResourceInfo resourceInfo = resourceInfoList[i];

            if (resourceTypeToModify == Type && resourceInfo.objModifiedBy == obj)
            {
                resourceInfo.amountPerSecond = amountPerSecond;

                if (amountPerSecond == 0)
                {
                    resourceInfo.uiForResourceInfo.ObjMainPanel.SetActive(false);
                }
                else
                {
                    resourceInfo.uiForResourceInfo.ObjMainPanel.SetActive(true);
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
            trackedAmount += debugAmountToIncrease;
        }
        else
        {
            trackedAmount += storageAmount - amount;
            amount += storageAmount - amount;           
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
    private void InitializeTooltipPrefab()
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
        IsUnlocked = PlayerPrefs.GetInt(_IsUnlockedString) == 1 ? true : false;
        if (IsUnlocked)
        {
            amount = PlayerPrefs.GetFloat(_amountString, amount);
            amountPerSecond = PlayerPrefs.GetFloat(_perSecondString, amountPerSecond);
            storageAmount = PlayerPrefs.GetFloat(_storageAmountString, storageAmount);
        }
        //}
        if (IsUnlocked)
        {
            ObjMainPanel.SetActive(true);
            Canvas.enabled = true;
        }
        else
        {
            ObjMainPanel.SetActive(false);
            Canvas.enabled = false;
        }
    }
    private void SetStartingResource()
    {
        // Display amount and amount per second.
        StaticMethods.ModifyAPSText(amountPerSecond, uiForResource.txtAmountPerSecond);
        uiForResource.txtAmount.text = string.Format("{0:0.00}", NumberToLetter.FormatNumber(amount));
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

        ObjMainPanel = gameObject;
        Canvas = gameObject.GetComponent<Canvas>();
        GraphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();

        _perSecondString = Type.ToString() + "PS";
        _amountString = Type.ToString() + "A";
        _storageAmountString = Type.ToString() + "Storage";
        _IsUnlockedString = Type.ToString() + "Unlocked";

        InitializeTooltipPrefab();
    }
    public void GetCurrentFill()
    {
        // Get the fill of the amount vs the storage amount.
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
    protected virtual void UpdateResource()
    {
        // Updates the resource 10 times per second, which is why I divide the amount displayed by 10 so it displays the amount per second for the player.
        if (IsUnlocked)
        {
            if ((timer -= Time.deltaTime) <= 0)
            {
                timer = 0.1f;

                if (amount >= (storageAmount - amountPerSecond))
                {
                    amount = storageAmount;
                }
                else
                {
                    amount += (amountPerSecond / 10);
                    // Tracked amount is used for achievements as well as statistics.
                    trackedAmount += (amountPerSecond / 10);
                }

                // this is to make sure the text field doesn't update when the previous tick had the same amount as the current tick.
                // For example if the previous tick was 1K and the amount only increased by lets say 20, it will stay at 1K.
                if (amount != cachedAmount)
                {
                    uiForResource.txtAmount.text = string.Format("{0:0.00}", NumberToLetter.FormatNumber(amount));
                }

                if (amount != storageAmount)
                {
                    GetCurrentFill();
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
        PlayerPrefs.SetInt(_IsUnlockedString, IsUnlocked ? 1 : 0);
    }

}
