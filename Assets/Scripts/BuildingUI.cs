using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public Resource[] _resourceCacheArray;

    public Resource[] FillResourcesInDictionary()
    {
        return _resourceCacheArray;
    }
}
