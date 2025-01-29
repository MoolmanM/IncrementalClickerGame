using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public TMP_Text txtWatts, txtPercentage;
    public Image energyBar;

    public float energyConsumption, energyProduction;

    public float kardashevValue, wattsConsumed;

    public Slider sliderKardashev;
    public TMP_Text textKardashev;
    private float timer = 0.1f;
    public GameObject objIconPanel;
    private bool _hasIntroducedEnergy;

    public void ResetEnergy()
    {
        energyProduction = 0;
        energyConsumption = 0;
        objIconPanel.SetActive(false);
        _hasIntroducedEnergy = false;
    }
    private void Start()
    {
        _hasIntroducedEnergy = PlayerPrefs.GetInt("_hasIntroducedEnergy") == 1 ? true : false;

        if (_hasIntroducedEnergy)
        {
            objIconPanel.SetActive(true);
        }

        energyProduction = PlayerPrefs.GetFloat("TotalEnergyProduction", energyProduction);
        energyConsumption = PlayerPrefs.GetFloat("TotalEnergyConsumption", energyConsumption);

        // Earth's current is 20000000000000
        wattsConsumed = 20000000000000;
        kardashevValue = (Mathf.Log10(wattsConsumed) - 6) / 10;
        sliderKardashev.value = kardashevValue;
        textKardashev.text = string.Format("You are currently {0} on the Kardashev Scale.", kardashevValue);
        UpdateEnergy();
    }
    public void UpdateEnergy()
    {
        if (!_hasIntroducedEnergy && energyProduction > 0)
        {
            _hasIntroducedEnergy = true;
            objIconPanel.SetActive(true);
        }
        float normalRatio = energyConsumption / energyProduction;
        //float percentage = (-normalRatio + 1) * 100;
        energyBar.fillAmount = -normalRatio + 1;
        //txtWatts.text = string.Format("{0}W/{1}W", energyConsumption, energyProduction);
        //txtPercentage.text = string.Format("{0:0.00}%", percentage);
    }
    private void Update()
    {
        // This should only be calculated every time you open up the kardashev scale.
        if ((timer -= Time.deltaTime) <= 0)
        {
            timer = 5f;

            wattsConsumed = 0;
            wattsConsumed = energyProduction;
            kardashevValue = (Mathf.Log10(wattsConsumed) - 6) / 10;
            sliderKardashev.value = kardashevValue;
            textKardashev.text = string.Format("You are currently {0} on the Kardashev Scale.", kardashevValue);
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("_hasIntroducedEnergy", _hasIntroducedEnergy ? 1 : 0);
        PlayerPrefs.SetFloat("TotalEnergyProduction", energyProduction);
        PlayerPrefs.SetFloat("TotalEnergyConsumption", energyConsumption);
    }
}

