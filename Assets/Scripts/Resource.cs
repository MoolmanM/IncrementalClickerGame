using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct UiForResource
{
    public TMP_Text storageAmount;
    public TMP_Text amount;
    public TMP_Text amountPerSecond;
}

public enum ResourceType
{
    Food,
    Stones,
    Sticks
}

public class Resource : MonoBehaviour
{
    public static Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();

    [System.NonSerialized] public float amount;
    [System.NonSerialized] public float amountPerSecond;

    public UiForResource uiForResource;
    public ResourceType type;
}
