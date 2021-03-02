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
    public TMP_Text descriptionText;
}

public class BuildingUI : MonoBehaviour
{
    [SerializeField] public ResourceCost[] _resourceCacheArray;
    
    public ResourceCost[] FillResourceCacheArray()
    {
        return _resourceCacheArray;
    }

    //Okay, so if I have text fields here, I'll have to register them inside each building script, which is fine, but does that mean I'm going to have to run the collector through each text field also then? Most likely I believe.
    //Actually might not be so easy. Because if the values gets passed to the collector the actual text fields won't get changed I think.
    //Maybe just have strings inside the collector or inside the building script. And then have a method here to pass into HandleCollector. And just assign the strings to the text fields that'll be here.
    //The text fields should ideally equal the resourceCacheArray Size, maybe do that in Onvalidate.
    //I can actually maybe just have textFields and strings inside of the REsourceCosts struct.
}
