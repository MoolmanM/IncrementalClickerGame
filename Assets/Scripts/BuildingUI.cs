using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class BuildingUI : MonoBehaviour
{
    [SerializeField] public ResourceCost[] _resourceCacheArray;

    public ResourceCost[] FillResourceCacheArray()
    {
        return _resourceCacheArray;
    }

    
}
