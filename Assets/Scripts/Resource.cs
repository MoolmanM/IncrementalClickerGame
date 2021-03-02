using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct UiForResource
{
    public TMP_Text amount;
    public TMP_Text amountPerSecond;
    public TMP_Text storageAmount;
}

[System.Serializable]
public class Resource : MonoBehaviour
{
    public ResourceType type;
    public float amount;
    public UiForResource uiForResource;

    [Serializable]
    public class ResourcesDictionary : SerializableDictionaryBase<ResourceType, Resource> { }
}
