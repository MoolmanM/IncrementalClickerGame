using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeScript : MonoBehaviour
{
    private Dictionary<ResourceType, float> AfkResources = new Dictionary<ResourceType, float>();

    public GameObject objPrefabEarnedPanel;
    public Transform tformContent;

    void Start()
    {
        GetAfkAmounts();
    }
    private void GetAfkAmounts()
    {
        if (TimeManager.hasPlayedBefore)
        {
            foreach (var resource in Resource.Resources)
            {
                if (resource.Value.IsUnlocked)
                {
                    GameObject newObj = Instantiate(objPrefabEarnedPanel, tformContent);

                    Transform _tformNewObj = newObj.transform;
                    Transform _tformResourceEarned = _tformNewObj.Find("Texts/Text_Earned");
                    Transform _tformResourceMax = _tformNewObj.Find("Texts/Text_Max");
                    Transform _tformImgAdEarned = _tformNewObj.Find("Fill_Area/Fill_Ad_Earned");
                    Transform _tformImgEarned = _tformNewObj.Find("Fill_Area/Fill_Earned");
                    Transform _tformImgPreviousAmount = _tformNewObj.Find("Fill_Area/Fill_Previous_Amount");
                    Transform _tformObjRed = _tformNewObj.Find("Fill_Area/Fill_Red");

                    TMP_Text txtResourceEarned = _tformResourceEarned.GetComponent<TMP_Text>();
                    TMP_Text txtResourceMax = _tformResourceMax.GetComponent<TMP_Text>();
                    Image imgAdEarned = _tformImgAdEarned.GetComponent<Image>();
                    Image imgEarned = _tformImgEarned.GetComponent<Image>();
                    Image imgPreviousAmount = _tformImgPreviousAmount.GetComponent<Image>();
                    GameObject objRed = _tformObjRed.gameObject;

                    float amountEarnedWhileAFK = (float)(TimeManager.difference.TotalSeconds * resource.Value.amountPerSecond);
                    float differenceAmount = resource.Value.storageAmount - amountEarnedWhileAFK;

                    if (resource.Value.amountPerSecond > 0)
                    {
                        if (amountEarnedWhileAFK + resource.Value.amount >= resource.Value.storageAmount)
                        {
                            AfkResources.Add(resource.Key, resource.Value.storageAmount);
                            objRed.SetActive(true);
                        }
                        else
                        {
                            AfkResources.Add(resource.Key, amountEarnedWhileAFK);
                            objRed.SetActive(false);
                        }
                    }

                    txtResourceEarned.text = string.Format("{0} earned: {1:0.00}", resource.Key, amountEarnedWhileAFK);

                    GetCurrentFill(resource.Value.amount + (amountEarnedWhileAFK * 2), resource.Value.storageAmount, imgAdEarned);
                    GetCurrentFill(resource.Value.amount + amountEarnedWhileAFK, resource.Value.storageAmount, imgEarned);
                    GetCurrentFill(resource.Value.amount, resource.Value.storageAmount, imgPreviousAmount);

                    txtResourceMax.text = string.Format("Storage Limit: {0:0.00}", resource.Value.storageAmount.ToString());
                }
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void GetCurrentFill(float amount, float storageAmount, Image imgFill)
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
        imgFill.fillAmount = fillAmount;       
    }
    public void OnCollectButton()
    {
        CollectResources(1);
    }
    public void OnAdCollectButton()
    {
        CollectResources(2);
    }
    private void CollectResources(float multiplier)
    {
        foreach (var resource in AfkResources)
        {
            Debug.Log("Name: " + resource.Key + " Amount: " + Resource.Resources[resource.Key].amount);

            if (resource.Value + Resource.Resources[resource.Key].amount > Resource.Resources[resource.Key].storageAmount)
            {
                Resource.Resources[resource.Key].amount = Resource.Resources[resource.Key].storageAmount;
            }
            else
            {
                Resource.Resources[resource.Key].amount += (resource.Value * multiplier);
            }

            Debug.Log("Name: " + resource.Key + " Amount: " + Resource.Resources[resource.Key].amount);
        }
    }
}
