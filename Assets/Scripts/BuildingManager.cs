using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static Dictionary<Building, ResourceCost[]> _buildings = new Dictionary<Building, ResourceCost[]>();
    public static Dictionary<Resource, UiForResource> _resources = new Dictionary<Resource, UiForResource>();
    //public static Collector collector;

    public static void Register(Building building, ResourceCost[] resources)
    {
        _buildings.Add(building, resources);
    }

    public static void RegisterResource(Resource resource, UiForResource ui)
    {
        _resources.Add(resource, ui);
    }
}
