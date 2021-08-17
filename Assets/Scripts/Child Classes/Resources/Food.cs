using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public struct ResourceInfo
{
    public float amountPerSecond;
    public string name;
    public UiForResourceInfo uiForResourceInfo;
}

public struct UiForResourceInfo
{
    public TMP_Text textInfoName;
    public TMP_Text textInfoAmountPerSecond;
}

public class Food : Resource, IPointerDownHandler, IPointerUpHandler
{
    private Resource _resource;

    public GameObject prefabResourceInfo;
    public bool buttonPressed;
    public ResourceInfo[] resourceInfo;
    private Transform _tformResourceTooltip;
    private GameObject _objTooltip;

    public int resourceInfoAmount;

    void Awake()
    {
        _resource = GetComponent<Resource>();
        Resources.Add(Type, _resource);
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked ? 1 : 0);
        SetInitialValues();
        // DisplayConsole();
        InitializePrefab();
    }
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
    protected void UpdateResourceInfo()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = 0.1f;

            for (int i = 0; i < resourceInfo.Length; i++)
            {
                resourceInfo[i].uiForResourceInfo.textInfoAmountPerSecond.text = string.Format("+{0:0.00}", resourceInfo[i].amountPerSecond);
                resourceInfo[i].uiForResourceInfo.textInfoName.text = string.Format("{0}", resourceInfo[i].name.ToString());

                //ShowResourceCostTime(resourceCost[i].uiForResourceCost.textCostAmount, resourceCost[i].currentAmount, resourceCost[i].costAmount, Resource.Resources[resourceCost[i].associatedType].amountPerSecond);
            }
        }
    }
    private void InitializePrefab()
    {
        prefabResourceInfo = UnityEngine.Resources.Load<GameObject>("ResourceInfo_Prefab/ResourceInfo_Panel");

        _tformResourceTooltip = transform.Find("Resource_Tooltip");

        _objTooltip = _tformResourceTooltip.gameObject;

        foreach (var item in Building.Buildings)
        {
            foreach (var typeToModify in item.Value.typesToModify.resourceTypesToModify)
            {
                if (typeToModify == Type)
                {         
                    resourceInfoAmount++;

                    resourceInfo = new ResourceInfo[resourceInfoAmount];

                    resourceInfo[0].name = item.Key.ToString();
                    resourceInfo[0].amountPerSecond = item.Value.buildingContributionAPS;
                }

            }
        }


        for (int i = 0; i < resourceInfo.Length; i++)
        {
            GameObject newObj = Instantiate(prefabResourceInfo, _tformResourceTooltip);

            ////This loop just makes sure that there is a never a body spacer underneath the last element(the last resource cost panel)
            //for (int spacerI = i + 1; spacerI < resourceCost.Length; spacerI++)
            //{
            //    Instantiate(_prefabBodySpacer, _tformBody);
            //}

            Transform _tformNewObj = newObj.transform;
            Transform _tformInfoName = _tformNewObj.Find("Text_Name");
            Transform _tformInfoAmountPerSecond = _tformNewObj.Find("Text_AmountPerSecond");

            resourceInfo[i].uiForResourceInfo.textInfoName = _tformInfoName.GetComponent<TMP_Text>();
            resourceInfo[i].uiForResourceInfo.textInfoAmountPerSecond = _tformInfoAmountPerSecond.GetComponent<TMP_Text>();
        }
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<ResourceType, Resource> kvp in Resources)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    void Update()
    {
        UpdateResources();
        UpdateResourceInfo();
    }
}
