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
    public GameObject objMainPanel, objSpacer;
    public TMP_Text textInfoName, textInfoAmountPerSecond;
    public Transform tformNewObj, tformInfoName, tformInfoAmountPerSecond;
}

public class Resource : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static Dictionary<ResourceType, Resource> Resources = new Dictionary<ResourceType, Resource>();
    public List<ResourceInfo> resourceInfoList = new List<ResourceInfo>();
    public Dictionary<GameObject, ResourceInfo> resourceInfoDict = new Dictionary<GameObject, ResourceInfo>();

    [System.NonSerialized] public GameObject prefabResourceInfoPanel, prefabResourceInfoSpacer;
    [System.NonSerialized] public Transform tformResourceTooltip;
    [System.NonSerialized] public GameObject objTooltip;

    [System.NonSerialized] public float amount, amountPerSecond;
    [System.NonSerialized] public bool isUnlocked;
    [System.NonSerialized] public UiForResource uiForResource;
    [System.NonSerialized] public GameObject objMainPanel;

    public float storageAmount, initialStorageAmount;
    public ResourceType Type;
    public GameObject objSpacerBelow;
    public float globalMultiplier = 1f;
    public bool buttonPressed;

    protected string _perSecondString, _amountString, _storageAmountString, _isUnlockedString;

    protected Transform _tformTxtAmount, _tformTxtAmountPerSecond, _tformTxtStorage, _tformImgbar;
    protected Image _imgBar;
    protected float _timer = 0.1f;

    public float debugAmountToIncrease;
    private float initialAmount;

    public void ResetResource()
    {
        amount = 0;
        amountPerSecond = 0;
        objMainPanel.SetActive(false);
        objSpacerBelow.SetActive(false);
        isUnlocked = false;
        storageAmount = initialStorageAmount;
        // Set storage amount back to original storage amount
        // Remove the resourceinfo prefabs?
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
                    resourceInfo.uiForResourceInfo.objSpacer.SetActive(false);
                }
                else
                {
                    resourceInfo.uiForResourceInfo.objMainPanel.SetActive(true);
                    resourceInfo.uiForResourceInfo.objSpacer.SetActive(true);
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
    [Button(ButtonSizes.Small)]
    private void DebugIncreaseResource()
    {
        if (amount > 0)
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
            buttonPressed = true;
            objTooltip.SetActive(true);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        objTooltip.SetActive(false);
    }
    private void InitializePrefab()
    {
        prefabResourceInfoPanel = UnityEngine.Resources.Load<GameObject>("ResourceInfo_Prefab/ResourceInfo_Panel");
        prefabResourceInfoSpacer = UnityEngine.Resources.Load<GameObject>("ResourceInfo_Prefab/Spacer");

        tformResourceTooltip = transform.Find("Resource_Tooltip");

        objTooltip = tformResourceTooltip.gameObject;

        //InitializeResourceInfo();
    }
    public virtual void SetInitialValues()
    {
        InitializeObjects();

        if (TimeManager.hasPlayedBefore)
        {
            //Need to make food and sticks 'unlocked' after this.
            amount = PlayerPrefs.GetFloat(_amountString, amount);
            amountPerSecond = PlayerPrefs.GetFloat(_perSecondString, amountPerSecond);
            storageAmount = PlayerPrefs.GetFloat(_storageAmountString, storageAmount);
            isUnlocked = PlayerPrefs.GetInt(_isUnlockedString) == 1 ? true : false;
        }

        if (isUnlocked)
        {
            objMainPanel.SetActive(true);
            objSpacerBelow.SetActive(true);
            if (amountPerSecond > 0f)
            {
                uiForResource.txtStorageAmount.text = string.Format("{0}", storageAmount);

                if (amountPerSecond >= 0)
                {
                    uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", amountPerSecond);
                }
                else
                {
                    uiForResource.txtAmountPerSecond.text = string.Format("-{0:0.00}/sec", amountPerSecond);
                }
                uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);
            }
            else
            {
                //txtEarned.text = string.Format("{0}: {1}", Type, "No production just yet."); 
            }

        }
        else
        {
            objMainPanel.SetActive(false);
            objSpacerBelow.SetActive(false);
            // Debug.Log(Type + ": Resource doesn't exist yet.");
        }

    }
    private void InitializeObjects()
    {
        _tformTxtAmount = transform.Find("Header_Panel/Text_Amount");
        _tformTxtAmountPerSecond = transform.Find("Header_Panel/Text_AmountPerSecond");
        _tformTxtStorage = transform.Find("Header_Panel/Text_Storage");
        _tformImgbar = transform.Find("ProgressBar");

        _imgBar = _tformImgbar.GetComponent<Image>();
        uiForResource.txtAmount = _tformTxtAmount.GetComponent<TMP_Text>();
        uiForResource.txtAmountPerSecond = _tformTxtAmountPerSecond.GetComponent<TMP_Text>();
        uiForResource.txtStorageAmount = _tformTxtStorage.GetComponent<TMP_Text>();

        objMainPanel = gameObject;
        objMainPanel.SetActive(false);

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
    protected virtual void UpdateResource()
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
                    amount += (amountPerSecond / 10) * globalMultiplier;
                }

                uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);

                GetCurrentFill();
            }
        }

    }
    private void Update()
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
