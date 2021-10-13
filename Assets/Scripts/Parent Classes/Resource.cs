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
    public BuildingType buildingAssociated;
    public WorkerType workerAssociated;
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
    private void InitializeResourceInfo()
    {

        foreach (var building in Building.Buildings)
        {
            Debug.Log(Type + " " + building.Key);
            foreach (var resourceToIncrement in building.Value.resourcesToIncrement)
            {
                if (resourceToIncrement.resourceTypeToModify == Type)
                {
                    resourceInfoList.Add(new ResourceInfo() { name = building.Value.name.ToString() });
                }
            }
        }

        for (int i = 0; i < resourceInfoList.Count; i++)
        {
            ResourceInfo resourceInfo = resourceInfoList[i];

            resourceInfo.uiForResourceInfo.objMainPanel = Instantiate(prefabResourceInfoPanel, tformResourceTooltip);

            Instantiate(prefabResourceInfoSpacer, tformResourceTooltip);

            resourceInfo.uiForResourceInfo.tformNewObj = resourceInfo.uiForResourceInfo.objMainPanel.transform;
            resourceInfo.uiForResourceInfo.tformInfoName = resourceInfo.uiForResourceInfo.tformNewObj.Find("Text_Name");
            resourceInfo.uiForResourceInfo.tformInfoAmountPerSecond = resourceInfo.uiForResourceInfo.tformNewObj.Find("Text_AmountPerSecond");

            resourceInfo.uiForResourceInfo.textInfoName = resourceInfo.uiForResourceInfo.tformInfoName.GetComponent<TMP_Text>();
            resourceInfo.uiForResourceInfo.textInfoAmountPerSecond = resourceInfo.uiForResourceInfo.tformInfoAmountPerSecond.GetComponent<TMP_Text>();

            resourceInfo.uiForResourceInfo.textInfoName.text = resourceInfo.name;
            resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", resourceInfo.amountPerSecond);

            resourceInfoList[i] = resourceInfo;

            // Okay so now I ONLY want to update the amount per second
            // And if it reaches zero. Or at least when the workercount reaches zero, I need to setactive(false)
            // BUT the problem is, how to know which amount per second to modify.
        }
    }
    public void UpdateResourceInfo(BuildingType buildingType, uint buildingCount, float buildingMultiplier, ResourceType resourceTypeToModify)
    {
        for (int i = 0; i < resourceInfoList.Count; i++)
        {
            ResourceInfo resourceInfo = resourceInfoList[i];

            //Debug.Log("resourceTypeToModify: " + resourceTypeToModify);
            //Debug.Log("Type: " + Type);
            //Debug.Log("resourceInfo.buildingAssociated: " + resourceInfo.buildingAssociated);
            //Debug.Log("buildingType: " + buildingType);

            if (resourceTypeToModify == Type && resourceInfo.buildingAssociated == buildingType)
            {
                resourceInfo.amountPerSecond = buildingCount * buildingMultiplier;
                resourceInfo.uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}/sec", resourceInfo.amountPerSecond);

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
            Debug.Log("Pressed");
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
    protected virtual void UpdateResources()
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
            if (amountPerSecond <= -0.01)
            {
                uiForResource.txtAmountPerSecond.text = string.Format("<color=#C63434>{0:0.00}/sec</color>", amountPerSecond);
            }
            else
            {
                uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", amountPerSecond);
            }
            uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);

            GetCurrentFill();
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(_amountString, amount);
        PlayerPrefs.SetFloat(_perSecondString, amountPerSecond);
        PlayerPrefs.SetFloat(_storageAmountString, storageAmount);
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
    }
    [Button]
    private void DebugList()
    {
        foreach (var infoItem in resourceInfoList)
        {
            Debug.Log(infoItem.name);
        }
    }
}
