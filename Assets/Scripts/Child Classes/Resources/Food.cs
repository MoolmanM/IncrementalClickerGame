using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;



public class Food : Resource
{
    private Resource _resource;
    void Awake()
    {
        _resource = GetComponent<Resource>();
        Resources.Add(Type, _resource);
        SetInitialValues();
        isUnlocked = true;
        objMainPanel.SetActive(true);
        canvas.enabled = true;
    }
    protected override void UpdateResource()
    {
        if (isUnlocked)
        {
            if ((_timer -= Time.deltaTime) <= 0)
            {
                _timer = 0.1f;

                if (amount != storageAmount)
                {
                    if (amount >= (storageAmount - amountPerSecond))
                    {
                        amount = storageAmount;
                    }
                    else
                    {
                        amount += (amountPerSecond / 10);
                    }

                    if (amount != cachedAmount)
                    {
                        uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);
                    }
                    if (amount <= 0.00f)
                    {
                        amount = 0;
                    }

                    GetCurrentFill();

                    cachedAmount = amount;
                }
                else if (amountPerSecond <= 0.00f)
                {
                    amount += (amountPerSecond / 10);

                    if (amount != cachedAmount)
                    {
                        uiForResource.txtAmount.text = string.Format("{0:0.00}", amount);
                    }
                }
                cachedAmount = amount;
            }
        }
    }
}


