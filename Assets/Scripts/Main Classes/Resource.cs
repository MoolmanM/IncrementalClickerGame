using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public struct UiForResource
{
    public TMP_Text TxtStorageAmount;
    public TMP_Text TxtAmount;
    public TMP_Text TxtAmountPerSecond;
}

public enum ResourceType
{
    Food,
    Sticks,
    Stones,
    Knowledge
}

public class Resource : MonoBehaviour
{
    public static Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();

    [System.NonSerialized] public float Amount, AmountPerSecond, AmountGainedWhileAfk;    
    [System.NonSerialized] public int IsUnlocked;
    [System.NonSerialized] public UiForResource _UiForResource;
    [System.NonSerialized] public GameObject objMainPanel;

    public float StorageAmount;
    public ResourceType Type;
    public TMP_Text TxtEarned;
    public GameObject ObjSpacerBelow;

    protected string _perSecondString, _amountString, _storageAmountString, _isUnlockedString;

    protected Transform TformTxtAmount, TformTxtAmountPerSecond, TformTxtStorage, TformImgbar;
    protected Image imgBar;
    protected float _timer = 0.1f;
    protected readonly float maxValue = 0.1f;

    public virtual void SetInitialValues()
    {
        InitializeObjects();

        if (TimeManager.hasPlayedBefore)
        {
            //Need to make food and sticks 'unlocked' after this.
            Amount = PlayerPrefs.GetFloat(_amountString, Amount);
            AmountPerSecond = PlayerPrefs.GetFloat(_perSecondString, AmountPerSecond);
            StorageAmount = PlayerPrefs.GetFloat(_storageAmountString, StorageAmount);
            //IsUnlocked = PlayerPrefs.GetInt(_isUnlockedString, IsUnlocked);
            IsUnlocked = 1;
        }

        if (IsUnlocked == 1)
        {
            objMainPanel.SetActive(true);
            ObjSpacerBelow.SetActive(true);
            if (AmountPerSecond > 0f)
            {
                TimeManager.GetAFKResource(Type);
                _UiForResource.TxtStorageAmount.text = string.Format("{0}", StorageAmount);

                if (AmountPerSecond >= 0)
                {
                    _resources[Type]._UiForResource.TxtAmountPerSecond.text = string.Format("+{0:0.00}/sec", _resources[Type].AmountPerSecond);
                }
                else
                {
                    _resources[Type]._UiForResource.TxtAmountPerSecond.text = string.Format("-{0:0.00}/sec", _resources[Type].AmountPerSecond);
                }
                _resources[Type]._UiForResource.TxtAmount.text = string.Format("{0:0.00}", _resources[Type].Amount);
            }
            else
            {
                TxtEarned.text = string.Format("{0}: {1}", Type, "No production just yet."); 
            }
           
        }
        else
        {
            objMainPanel.SetActive(false);
            ObjSpacerBelow.SetActive(false);
            Debug.Log(Type + ": Resource doesn't exist yet.");
        }
        
    }
    private void InitializeObjects()
    {
        TformTxtAmount = transform.Find("Header_Panel/Text_Amount");
        TformTxtAmountPerSecond = transform.Find("Header_Panel/Text_AmountPerSecond");
        TformTxtStorage = transform.Find("Header_Panel/Text_Storage");
        TformImgbar = transform.Find("ProgressBar");

        imgBar = TformImgbar.GetComponent<Image>();
        _UiForResource.TxtAmount = TformTxtAmount.GetComponent<TMP_Text>();
        _UiForResource.TxtAmountPerSecond = TformTxtAmountPerSecond.GetComponent<TMP_Text>();
        _UiForResource.TxtStorageAmount = TformTxtStorage.GetComponent<TMP_Text>();

        objMainPanel = gameObject;
        objMainPanel.SetActive(false);

        _perSecondString = Type.ToString() + "PS";
        _amountString = Type.ToString() + "A";
        _storageAmountString = Type.ToString() + "Storage";
        _isUnlockedString = Type.ToString() + "Unlocked";
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(_amountString, Amount);
        PlayerPrefs.SetFloat(_perSecondString, AmountPerSecond);
        PlayerPrefs.SetFloat(_storageAmountString, StorageAmount);
        PlayerPrefs.SetInt(_isUnlockedString, IsUnlocked);
    }
    public void GetCurrentFill()
    {
        float add = 0;
        float div = 0;
        float fillAmount = 0;

        add = Amount;
        div = StorageAmount;
        if (add > div)
        {
            add = div;
        }

        fillAmount += add / div;
        imgBar.fillAmount = fillAmount;
    }

    //private void CheckIfUnlockedYet()
    //{
        

    //    foreach (KeyValuePair<CraftingType, Craftable> kvp in Craftable.Craftables)
    //    {
    //        kvp.Value.averageAmount = 0;
    //        float add = 0;
    //        float div = 0;


    //        //float[] amountsRequiredForUnlocking = new float[kvp.Value.resourceCost.Length];
    //        #region GetAverageAmount




    //        //
    //        // Maybe don't use kvp.value.averageAmount. Maybe just make your own float averageAmount here.
    //        //

    //        for (int r = 0; r < kvp.Value.resourceCost.Length; r++)
    //        {
    //            add = Amount;
    //            div = kvp.Value.resourceCost[r].CostAmount;

    //            if (add > div)
    //            {
    //                add = div;
    //            }
    //            kvp.Value.averageAmount += add / div;
    //            Debug.Log(kvp.Key + " " + kvp.Value.resourceCost[r].CostAmount);
    //        }
                
    //            //Debug.Log("Before " + kvp.Key + " " + kvp.Value.averageAmount);
    //            kvp.Value.averageAmount /= kvp.Value.resourceCost.Length;
            
    //            //Debug.Log("After " + kvp.Key + " " + kvp.Value.averageAmount);
    //            //Debug.Log("Before " + kvp.Key + "" + kvp.Value.averageAmount);
    //            //Debug.Log(kvp.Key + " " + kvp.Value.resourceCost.Length);

    //            #endregion

    //            //amountsRequiredForUnlocking[i] = kvp.Value.resourceCost[i].CostAmount * 0.8f;

    //            if (kvp.Value.averageAmount <= 0.8f)
    //            {
    //                //Debug.Log(kvp.Key + " Is unlocked. Amount: " + kvp.Value.averageAmount + " Unlock Status " + kvp.Value.IsUnlocked);
    //                break;
    //            }
    //            else
    //            {
    //                kvp.Value.IsUnlocked = 1;
    //                kvp.Value.ObjMainPanel.SetActive(true);
    //                kvp.Value.ObjSpacerBelow.SetActive(true);
    //                //Debug.Log(kvp.Key + " Is not unlocked. Amount: " + kvp.Value.averageAmount + " Unlock Status " + kvp.Value.IsUnlocked);
    //            }


           
    //        //Debug.Log("After : " + kvp.Key + " Amount: " + kvp.Value.averageAmount);
    //    }
    //}
    public virtual void UpdateResources()
    {

        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = maxValue;

            if (Amount >= (StorageAmount - AmountPerSecond))
            {
                Amount = StorageAmount;
            }
            else
            {
                _resources[Type].Amount += _resources[Type].AmountPerSecond;
            }
            
            if (AmountPerSecond < 0)
            {
                _resources[Type]._UiForResource.TxtAmountPerSecond.text = string.Format("-{0:0.00}/sec", _resources[Type].AmountPerSecond);
            }
            else
            {
                _resources[Type]._UiForResource.TxtAmountPerSecond.text = string.Format("+{0:0.00}/sec", _resources[Type].AmountPerSecond);
            }
            _resources[Type]._UiForResource.TxtAmount.text = string.Format("{0:0.00}", _resources[Type].Amount);

            GetCurrentFill();
            //CheckIfUnlockedYet();
        }
    }
}
