using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Power : Building, IPointerDownHandler, IPointerUpHandler
{
    public static bool hasLessThanZeroAPS;

    public float energyConsumption, energyProduction;
    public Energy energy;
    public PopupEvent PointerDownHandler, PointerUpHandler;
    public static bool hasNotEnoughEnergy;
    public Transform CanvasMain;

    private uint _poweredCount;
    private GameObject _objPopupCircle;
    private Button _btnClickAnywhere;
    private TMP_Text _txtPowerCounter, _txtChangeAmount;
    private float timeButtonHeld;
    private bool buttonHeld, popupCircleActive;

    public override void ResetBuilding()
    {
        base.ResetBuilding();

        energyProduction = 0;
        energyConsumption = 0;
    }
    protected void InitializePower()
    {
        SetInitialValues();

        Transform tformtxtPowerCounter = transform.Find("Panel_Main/Text_Power_Counter");

        _txtPowerCounter = tformtxtPowerCounter.GetComponent<TMP_Text>();

        _poweredCount = (uint)PlayerPrefs.GetInt(ActualName + "PoweredCount", (int)_poweredCount);

        _txtPowerCounter.text = _poweredCount.ToString();
    }
    public override void OnBuild()
    {
        bool canPurchase = true;

        for (int i = 0; i < ResourceCost.Length; i++)
        {
            if (ResourceCost[i].CurrentAmount < ResourceCost[i].CostAmount)
            {
                canPurchase = false;
                break;
            }
        }

        if (canPurchase)
        {
            _selfCount++;
            for (int i = 0; i < ResourceCost.Length; i++)
            {
                Resource.Resources[ResourceCost[i].AssociatedType].amount -= ResourceCost[i].CostAmount;
                ResourceCost[i].CostAmount *= Mathf.Pow(costMultiplier, _selfCount);
                ResourceCost[i].UiForResourceCost.CostAmountText.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[ResourceCost[i].AssociatedType].amount, ResourceCost[i].CostAmount);
            }
        }

        TxtHeader.text = string.Format("{0} ({1})", ActualName, _selfCount);
    }
    private void ModifyAmountPerSecond(int changeValue)
    {
        if (changeValue > 0)
        {
            if (resourcesToDecrement.Count != 0)
            {
                for (int i = 0; i < resourcesToDecrement.Count; i++)
                {

                    Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= resourcesToDecrement[i].currentResourceMultiplier * changeValue;

                    StaticMethods.ModifyAPSText(Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
                }
            }
            for (int i = 0; i < resourcesToIncrement.Count; i++)
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier * changeValue;

                StaticMethods.ModifyAPSText(Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
            }
        }
        else
        {
            for (int i = 0; i < resourcesToIncrement.Count; i++)
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier * changeValue;

                StaticMethods.ModifyAPSText(Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
            }

            if (resourcesToDecrement.Count != 0)
            {
                for (int i = 0; i < resourcesToDecrement.Count; i++)
                {
                    Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= resourcesToDecrement[i].currentResourceMultiplier * changeValue;

                    StaticMethods.ModifyAPSText(Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond, Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond);
                }
            }
        }

        //UpdatePowerResourceInfo();
    }
    protected void UpdatePowerResourceInfo()
    {
        foreach (var resourceToIncrement in resourcesToIncrement)
        {
            float buildingAmountPerSecond = resourceToIncrement.currentResourceMultiplier * _poweredCount;
            Resource.Resources[resourceToIncrement.resourceTypeToModify].UpdateResourceInfo(gameObject, buildingAmountPerSecond, resourceToIncrement.resourceTypeToModify);
        }

        foreach (var resourceToDecrement in resourcesToDecrement)
        {
            float buildingAmountPerSecond = resourceToDecrement.currentResourceMultiplier * _poweredCount;
            Resource.Resources[resourceToDecrement.resourceTypeToModify].UpdateResourceInfo(gameObject, -buildingAmountPerSecond, resourceToDecrement.resourceTypeToModify);
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (!popupCircleActive)
        {
            buttonHeld = true;
        }
    }
    public void OnPointerUp(PointerEventData data)
    {
        buttonHeld = false;
        timeButtonHeld = 0;
    }
    public void OnClickAnywhere()
    {
        HandleEnergy();
        PopupCircleButtons.changeAmount = 0;
        _txtPowerCounter.text = _poweredCount.ToString();
        Destroy(_objPopupCircle);
        popupCircleActive = false;
    }
    private void InitializePopupCircleButtons()
    {
        Vector2 vectorCircle = new Vector2(ObjProgressCircle.transform.position.x, ObjProgressCircle.transform.position.y);
        GameObject prefabCirclePanel = Resources.Load<GameObject>("Popup_Circle_Prefab/Popup_Circle_Panel");

        _objPopupCircle = Instantiate(prefabCirclePanel, vectorCircle, new Quaternion(0, 0, 0, 0), CanvasMain);
        Transform tformBtnPlus1 = _objPopupCircle.transform.Find("Plus/Button_+1");
        Transform tformBtnPlus10 = _objPopupCircle.transform.Find("Plus/Button_+10");
        Transform tformBtnPlus100 = _objPopupCircle.transform.Find("Plus/Button_+100");
        Transform tformBtnPlusMax = _objPopupCircle.transform.Find("Plus/Button_+Max");
        Transform tformBtnMinus1 = _objPopupCircle.transform.Find("Minus/Button_-1");
        Transform tformBtnMinus10 = _objPopupCircle.transform.Find("Minus/Button_-10");
        Transform tformBtnMinus100 = _objPopupCircle.transform.Find("Minus/Button_-100");
        Transform tformBtnMinusMax = _objPopupCircle.transform.Find("Minus/Button_-Max");
        Transform tformBtnClickAnywhere = _objPopupCircle.transform.Find("Button_ClickAnywhere");
        Transform tformTxtChangeAmount = _objPopupCircle.transform.Find("Text_ChangeAmount");

        Button btnPlus1 = tformBtnPlus1.GetComponent<Button>();
        Button btnPlus10 = tformBtnPlus10.GetComponent<Button>();
        Button btnPlus100 = tformBtnPlus100.GetComponent<Button>();
        Button btnPlusMax = tformBtnPlusMax.GetComponent<Button>();
        Button btnMinus1 = tformBtnMinus1.GetComponent<Button>();
        Button btnMinus10 = tformBtnMinus10.GetComponent<Button>();
        Button btnMinus100 = tformBtnMinus100.GetComponent<Button>();
        Button btnMinusMax = tformBtnMinusMax.GetComponent<Button>();
        _btnClickAnywhere = tformBtnClickAnywhere.GetComponent<Button>();
        _txtChangeAmount = tformTxtChangeAmount.GetComponent<TMP_Text>();

        btnPlus1.onClick.AddListener(delegate { PopupCircleButtons.OnButtonPlus1(_selfCount, _poweredCount, _txtChangeAmount); });
        btnPlus10.onClick.AddListener(delegate { PopupCircleButtons.OnButtonPlus10(_selfCount, _poweredCount, _txtChangeAmount); });
        btnPlus100.onClick.AddListener(delegate { PopupCircleButtons.OnButtonPlus100(_selfCount, _poweredCount, _txtChangeAmount); });
        btnPlusMax.onClick.AddListener(delegate { PopupCircleButtons.OnButtonPlusMax(_selfCount, _poweredCount, _txtChangeAmount); });
        btnMinus1.onClick.AddListener(delegate { PopupCircleButtons.OnButtonMinus1(_poweredCount, _txtChangeAmount); });
        btnMinus10.onClick.AddListener(delegate { PopupCircleButtons.OnButtonMinus10(_poweredCount, _txtChangeAmount); });
        btnMinus100.onClick.AddListener(delegate { PopupCircleButtons.OnButtonMinus100(_poweredCount, _txtChangeAmount); });
        btnMinusMax.onClick.AddListener(delegate { PopupCircleButtons.OnButtonMinusMax((int)_selfCount, _poweredCount, _txtChangeAmount); });
        _btnClickAnywhere.onClick.AddListener(OnClickAnywhere);
    }
    private void HandleEnergy()
    {
        if (energyConsumption > 0)
        {
            if (PopupCircleButtons.changeAmount < 0)
            {
                _poweredCount += (uint)PopupCircleButtons.changeAmount;
                energy.energyConsumption += energyConsumption * PopupCircleButtons.changeAmount;
                ModifyAmountPerSecond(PopupCircleButtons.changeAmount);
            }
            else
            {
                // here I need to check if the amount that we are decreasing that specific resource by should not be lower than zero.
                if (resourcesToDecrement.Count != 0)
                {
                    for (int i = 0; i < resourcesToDecrement.Count; i++)
                    {
                        if (Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond - resourcesToDecrement[i].currentResourceMultiplier * PopupCircleButtons.changeAmount > 0)
                        {
                            if (energyConsumption * (PopupCircleButtons.changeAmount + _poweredCount) <= energy.energyProduction)
                            {
                                _poweredCount += (uint)PopupCircleButtons.changeAmount;
                                energy.energyConsumption += energyConsumption * PopupCircleButtons.changeAmount;
                                ModifyAmountPerSecond(PopupCircleButtons.changeAmount);
                            }
                            else
                            {
                                hasNotEnoughEnergy = true;
                                // Should also include a case where if this happens
                                // Just assign the max amount of consumers before reaching production cap.
                            }
                        }
                        else
                        {
                            hasLessThanZeroAPS = true;
                        }
                    }
                }
            }
        }
        else
        {
            if (resourcesToDecrement.Count != 0)
            {
                for (int i = 0; i < resourcesToDecrement.Count; i++)
                {
                    if (Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond - resourcesToDecrement[i].currentResourceMultiplier * PopupCircleButtons.changeAmount > 0)
                    {
                        _poweredCount += (uint)PopupCircleButtons.changeAmount;
                        energy.energyProduction += energyProduction * PopupCircleButtons.changeAmount;
                        ModifyAmountPerSecond(PopupCircleButtons.changeAmount);
                    }
                    else
                    {
                        hasLessThanZeroAPS = true;
                    }
                }
            }
        }
        energy.UpdateEnergy();
    }
    protected override void Update()
    {
        if ((timer -= Time.deltaTime) <= 0)
        {
            timer = maxValue;
            CheckIfPurchaseable();
            UpdateResourceCostTexts();
        }

        if (buttonHeld)
        {
            timeButtonHeld += Time.deltaTime;

            // If you hold the button for this amount of time it will pop up the circle
            // This is just to prevent misclicks. So the intentions of the player must be intentional.
            if (timeButtonHeld >= 0.3f)
            {
                InitializePopupCircleButtons();
                buttonHeld = false;
                popupCircleActive = true;
                timeButtonHeld = 0;
            }
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(ActualName + "PoweredCount", (int)_poweredCount);
    }
}
