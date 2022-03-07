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
    private float _timer = 0.1f;
    public GameObject objIconPanel;
    private bool hasIntroducedEnergy;

    private void Start()
    {
        if (hasIntroducedEnergy)
        {
            objIconPanel.SetActive(true);
        }
        // Earth's current is 20000000000000
        wattsConsumed = 20000000000000;
        kardashevValue = (Mathf.Log10(wattsConsumed) - 6) / 10;
        sliderKardashev.value = kardashevValue;
        textKardashev.text = string.Format("You are currently {0} on the Kardashev Scale.", kardashevValue);
    }
    public void UpdateEnergy()
    {
        if (!hasIntroducedEnergy && energyProduction > 0)
        {
            hasIntroducedEnergy = true;
            objIconPanel.SetActive(true);
        }
        float normalRatio = energyConsumption / energyProduction;
        float percentage = (-normalRatio + 1) * 100;
        energyBar.fillAmount = -normalRatio + 1;
        txtWatts.text = string.Format("{0}W/{1}W", energyConsumption, energyProduction);
        txtPercentage.text = string.Format("{0:0.00}%", percentage);
    }
    private void Update()
    {
        // This should only be calculated every time you open up the kardashev scale.
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = 5f;

            wattsConsumed = 0;
            wattsConsumed = energyProduction;
            kardashevValue = (Mathf.Log10(wattsConsumed) - 6) / 10;
            sliderKardashev.value = kardashevValue;
            textKardashev.text = string.Format("You are currently {0} on the Kardashev Scale.", kardashevValue);
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("hasIntroducedEnergy", hasIntroducedEnergy ? 1 : 0);
    }
}

