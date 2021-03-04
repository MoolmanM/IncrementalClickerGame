using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct UiForBuilding
{
    public TMP_Text costNameText;
    public TMP_Text costAmountText;

    //decription Text actually needs to be removed from here because there can only ever be one description per building.
    public TMP_Text descriptionText;
}

public class BuildingUI : MonoBehaviour
{
    [SerializeField] public ResourceCost[] _resourceCacheArray;

    public ResourceCost[] FillResourceCacheArray()
    {
        return _resourceCacheArray;
    }

    
}
