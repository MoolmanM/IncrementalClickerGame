using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public struct UiForResource
{
    public TMP_Text txtStorageAmount;
    public TMP_Text txtAmount;
    public TMP_Text txtAmountPerSecond;
}

public enum ResourceType
{
    Food,
    Sticks,
    Stones,
    Knowledge,
    Pelts
}

public class Resource : MonoBehaviour
{
    public static Dictionary<ResourceType, Resource> Resources = new Dictionary<ResourceType, Resource>();

    [System.NonSerialized] public float amount, amountPerSecond, amountGainedWhileAfk;    
    [System.NonSerialized] public int isUnlocked;
    [System.NonSerialized] public UiForResource uiForResource;
    [System.NonSerialized] public GameObject objMainPanel;

    public float storageAmount;
    public ResourceType Type;
    public TMP_Text txtEarned;
    public GameObject objSpacerBelow;

    protected string _perSecondString, _amountString, _storageAmountString, _isUnlockedString;

    protected Transform _tformTxtAmount, _tformTxtAmountPerSecond, _tformTxtStorage, _tformImgbar;
    protected Image _imgBar;
    protected float _timer = 0.1f;

    public virtual void SetInitialValues()
    {
        InitializeObjects();

        if (TimeManager.hasPlayedBefore)
        {
            //Need to make food and sticks 'unlocked' after this.
            amount = PlayerPrefs.GetFloat(_amountString, amount);
            amountPerSecond = PlayerPrefs.GetFloat(_perSecondString, amountPerSecond);
            if (TimeManager.hasPlayedBefore)
            {
                storageAmount = PlayerPrefs.GetFloat(_storageAmountString, storageAmount);
            }
            isUnlocked = PlayerPrefs.GetInt(_isUnlockedString, isUnlocked);
        }

        if (isUnlocked == 1)
        {
            objMainPanel.SetActive(true);
            objSpacerBelow.SetActive(true);
            if (amountPerSecond > 0f)
            {
                TimeManager.GetAFKResource(Type);
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
                txtEarned.text = string.Format("{0}: {1}", Type, "No production just yet."); 
            }
           
        }
        else
        {
            objMainPanel.SetActive(false);
            objSpacerBelow.SetActive(false);
            Debug.Log(Type + ": Resource doesn't exist yet.");
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
    public virtual void UpdateResources()
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
                amount += amountPerSecond / 10;
            }
            
            if (amountPerSecond < 0)
            {
                uiForResource.txtAmountPerSecond.text = string.Format("-{0:0.00}/sec", amountPerSecond);
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
        PlayerPrefs.SetInt(_isUnlockedString, isUnlocked);
    }
}
