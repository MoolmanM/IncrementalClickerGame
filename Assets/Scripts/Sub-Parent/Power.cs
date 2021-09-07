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

    private GameObject _objPanel, _objBackground, _objFillArea, _objSlider, _objHandle, _objtxtHandleAmount, _objChangeAmount;
    private Transform _tformObjPanel, _tformObjBackground, _tformObjFillArea, _tformObjSlider, _tformTxtHandleAmount, _tformTxtPowerCounter, _tformObjHandle, _tformChangeAmount;
    private TMP_Text _txtHandleAmount, _txtPowerCounter;
    private int _poweredCount;    
   
    protected void InitializePower()
    {
        _tformObjPanel = transform.Find("Panel_Main/Slider_Panel");
        _tformObjBackground = transform.Find("Panel_Main/Slider_Panel/Slider/Background");
        _tformObjFillArea = transform.Find("Panel_Main/Slider_Panel/Slider/Fill_Area");
        _tformObjSlider = transform.Find("Panel_Main/Slider_Panel/Slider");
        _tformTxtHandleAmount = transform.Find("Panel_Main/Slider_Panel/Slider/Handle_Slide_Area/Handle/Text_Handle_Amount");
        _tformTxtPowerCounter = transform.Find("Panel_Main/Power_Counter/Text_Counter");
        _tformObjHandle = transform.Find("Panel_Main/Slider_Panel/Slider/Handle_Slide_Area/Handle");
        //_tformChangeAmount = transform.Find("Panel_Main/Slider_Panel/Text_ChangeAmount");

        //_objChangeAmount = _tformChangeAmount.gameObject;
        _objHandle = _tformObjHandle.gameObject;
        _objPanel = _tformObjPanel.gameObject;
        _objBackground = _tformObjBackground.gameObject;
        _objFillArea = _tformObjFillArea.gameObject;
        _objSlider = _tformObjSlider.gameObject;
        _txtHandleAmount = _tformTxtHandleAmount.GetComponent<TMP_Text>();
        _objtxtHandleAmount = _tformTxtHandleAmount.gameObject;
        _txtPowerCounter = _tformTxtPowerCounter.GetComponent<TMP_Text>();

        _objBackground.SetActive(false);
        _objFillArea.SetActive(false);
        _objPanel.GetComponent<CanvasGroup>().alpha = 0;

        PointerDownHandler.OnPointerDownEvent.AddListener(OnPointerDown);
        PointerUpHandler.OnPointerUpEvent.AddListener(OnPointerUp);
    }
    public void OnPointerDown(PointerEventData data)
    {

        _objtxtHandleAmount.SetActive(true);
        _objSlider.GetComponent<Slider>().minValue = _poweredCount * (-1);
        _objSlider.GetComponent<Slider>().maxValue = _selfCount - _poweredCount;
        _objPanel.GetComponent<CanvasGroup>().alpha = 1;
        _objBackground.SetActive(true);
        _objFillArea.SetActive(true);
    }
    public void OnPointerUp(PointerEventData data)
    {
        _objtxtHandleAmount.SetActive(false);
        RectTransform rectHandle = _objHandle.GetComponent<RectTransform>();
        Slider slider = _objSlider.GetComponent<Slider>();

        _objPanel.GetComponent<CanvasGroup>().alpha = 0;
        _objBackground.SetActive(false);
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
        int sliderValue = (int)_objSlider.GetComponent<Slider>().value;
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
            }
        }
        else
        {
            _poweredCount += sliderValue;
            energy.energyProduction += energyProduction * sliderValue;
            ModifyAmountPerSecond(sliderValue);
        }
        energy.txtEnergy.text = string.Format("{0}W/{1}W", energy.energyConsumption, energy.energyProduction);
    }
    public void UpdateHandleAmount()
    {
        swipe.isDragging = false;
        _txtHandleAmount.text = _objSlider.GetComponent<Slider>().value.ToString();
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
            //buildingContributionAPS = 0;
            //buildingContributionAPS = _selfCount * _resourceMultiplier;
            //UpdateResourceInfo();
        }

        _objTxtHeader.GetComponent<TMP_Text>().text = string.Format("{0} ({1})", _stringOriginalHeader, _selfCount);
    }
    private void ModifyAmountPerSecond(int sliderValue)
    {
        if (sliderValue > 0)
        {
            for (int i = 0; i < resourcesToIncrement.Count; i++)
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].resourceMulitplier * sliderValue;
            }

            for (int i = 0; i < resourcesToDecrement.Count; i++)
            {
                Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= resourcesToDecrement[i].resourceMulitplier * sliderValue;
            }         
        }
        else
        {
            for (int i = 0; i < resourcesToIncrement.Count; i++)
            {
                Resource.Resources[resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += resourcesToIncrement[i].resourceMulitplier * sliderValue;
            }

            for (int i = 0; i < resourcesToDecrement.Count; i++)
            {
                Resource.Resources[resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= resourcesToDecrement[i].resourceMulitplier * sliderValue;
            }
        }
    }
}
