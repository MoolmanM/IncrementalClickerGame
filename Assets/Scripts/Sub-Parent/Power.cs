using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Power : Building
{
    public float energyConsumption, energyProduction;
    public Energy energy;
    public Swipe swipe;
    public SliderEvent PointerDownHandler, PointerUpHandler;
    public static bool hasNotEnoughEnergy;

    private GameObject _objPanel, _objSliderBackground, _objFillArea, _objSlider, _objHandle, _objtxtHandleAmount, _objTextPowerCounter;
    private Transform _tformObjPanel, _tformObjSliderBackground, _tformObjFillArea, _tformObjSlider, _tformTxtHandleAmount, _tformTxtPowerCounter, _tformObjHandle;
    private TMP_Text _txtHandleAmount, _txtPowerCounter;
    private Slider _slider;
    private CanvasGroup _canvasGroupPanel;
    private int _poweredCount;

    protected void InitializePower()
    {
        SetInitialValues();

        _tformObjPanel = transform.Find("Panel_Main/Slider_Panel");
        _tformObjSliderBackground = transform.Find("Panel_Main/Slider_Panel/Slider/Background");
        _tformObjFillArea = transform.Find("Panel_Main/Slider_Panel/Slider/Fill_Area");
        _tformObjSlider = transform.Find("Panel_Main/Slider_Panel/Slider");
        _tformTxtHandleAmount = transform.Find("Panel_Main/Slider_Panel/Slider/Handle_Slide_Area/Handle/Text_Handle_Amount");
        _tformTxtPowerCounter = transform.Find("Panel_Main/Text_Power_Counter");
        _tformObjHandle = transform.Find("Panel_Main/Slider_Panel/Slider/Handle_Slide_Area/Handle");

        _objHandle = _tformObjHandle.gameObject;
        _objPanel = _tformObjPanel.gameObject;
        _objSliderBackground = _tformObjSliderBackground.gameObject;
        _objFillArea = _tformObjFillArea.gameObject;
        _objSlider = _tformObjSlider.gameObject;
        _txtHandleAmount = _tformTxtHandleAmount.GetComponent<TMP_Text>();
        _objtxtHandleAmount = _tformTxtHandleAmount.gameObject;
        _objTextPowerCounter = _tformTxtPowerCounter.gameObject;
        _txtPowerCounter = _tformTxtPowerCounter.GetComponent<TMP_Text>();
        _slider = _objSlider.GetComponent<Slider>();
        _canvasGroupPanel = _objPanel.GetComponent<CanvasGroup>();

        _objSliderBackground.SetActive(false);
        _objFillArea.SetActive(false);
        _canvasGroupPanel.alpha = 0;

        //PointerDownHandler.OnPointerDownEvent.AddListener(OnPointerDown);
        //PointerUpHandler.OnPointerUpEvent.AddListener(OnPointerUp);
    }
    public void OnPointerDown(PointerEventData data)
    {
        _objtxtHandleAmount.SetActive(true);
        _slider.minValue = _poweredCount * (-1);
        _slider.maxValue = _selfCount - _poweredCount;
        _canvasGroupPanel.alpha = 1;
        _objSliderBackground.SetActive(true);
        _objFillArea.SetActive(true);
    }
    public void OnPointerUp(PointerEventData data)
    {
        _objtxtHandleAmount.SetActive(false);
        RectTransform rectHandle = _objHandle.GetComponent<RectTransform>();
        Slider slider = _slider;

        _canvasGroupPanel.alpha = 0;
        _objSliderBackground.SetActive(false);
        _objFillArea.SetActive(false);
        HandleEnergy();
        slider.minValue = _poweredCount * (-1);
        slider.maxValue = _selfCount - _poweredCount;
        slider.value = 0;

        if (_poweredCount > 0)
        {
            _txtPowerCounter.text = _poweredCount.ToString();
        }
        else
        {
            _txtPowerCounter.text = "Off";
        }
        rectHandle.anchorMin = new Vector2(0.5f, 0);
        rectHandle.anchorMax = new Vector2(0.5f, 1);
    }
    private void HandleEnergy()
    {
        int sliderValue = (int)_slider.value;
        if (energyConsumption > 0)
        {
            if (sliderValue < 0)
            {
                _poweredCount += sliderValue;
                energy.energyConsumption += energyConsumption * sliderValue;
                ModifyAmountPerSecond(sliderValue);
            }
            else
            {
                if (energyConsumption * (sliderValue + _poweredCount) <= energy.energyProduction)
                {
                    _poweredCount += sliderValue;
                    energy.energyConsumption += energyConsumption * sliderValue;
                    ModifyAmountPerSecond(sliderValue);
                }
                else
                {
                    hasNotEnoughEnergy = true;
                    // Should also include a case where if this happens
                    // Just assign the max amount of consumers before reaching production cap.
                }
            }
        }
        else
        {
            _poweredCount += sliderValue;
            energy.energyProduction += energyProduction * sliderValue;
            ModifyAmountPerSecond(sliderValue);
        }
        energy.UpdateEnergy();
    }
    public void UpdateHandleAmount()
    {
        swipe.isDragging = false;
        _txtHandleAmount.text = _slider.value.ToString();
    }
    public override void OnBuild()
    {
        bool canPurchase = true;

        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resourceCost[i].currentAmount < resourceCost[i].costAmount)
            {
                canPurchase = false;
                break;
            }
        }

        if (canPurchase)
        {
            _selfCount++;
            for (int i = 0; i < resourceCost.Length; i++)
            {
                Resource.Resources[resourceCost[i].associatedType].amount -= resourceCost[i].costAmount;
                resourceCost[i].costAmount *= Mathf.Pow(costMultiplier, _selfCount);
                resourceCost[i].uiForResourceCost.textCostAmount.text = string.Format("{0:0.00}/{1:0.00}", Resource.Resources[resourceCost[i].associatedType].amount, resourceCost[i].costAmount);
            }
        }

        _txtHeader.text = string.Format("{0} ({1})", actualName, _selfCount);
    }
    private void ModifyAmountPerSecond(int sliderValue)
    {
        if (sliderValue > 0)
        {
            for (int i = 0; i < resourcesToIncrement.Count; i++)
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier * sliderValue;

                if (Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond > 0.00f)
                {
                    Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond);
                }
                else
                {
                    Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("{0:0.00}/sec", Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond);
                }
            }

            if (resourcesToDecrement.Count != 0)
            {
                for (int i = 0; i < resourcesToDecrement.Count; i++)
                {
                    Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= resourcesToDecrement[i].currentResourceMultiplier * sliderValue;

                    if (Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond > 0.00f)
                    {
                        Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond);
                    }
                    else
                    {
                        Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("{0:0.00}/sec", Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < resourcesToIncrement.Count; i++)
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].currentResourceMultiplier * sliderValue;

                if (Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond > 0.00f)
                {
                    Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond);
                }
                else
                {
                    Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("{0:0.00}/sec", Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond);
                }
            }

            if (resourcesToDecrement.Count != 0)
            {
                for (int i = 0; i < resourcesToDecrement.Count; i++)
                {
                    Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= resourcesToDecrement[i].currentResourceMultiplier * sliderValue;
                    
                    if (Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond > 0.00f)
                    {
                        Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond);
                    }
                    else
                    {
                        Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("{0:0.00}/sec", Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond);
                    }
                }
            }
        }

        UpdatePowerResourceInfo();
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

}
