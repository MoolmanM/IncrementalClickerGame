using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static Dictionary<Building, ResourceCost[]> _buildings = new Dictionary<Building, ResourceCost[]>();
    public static Collector collector;

    public static void Register(Building building, ResourceCost[] resources)
    {
        _buildings.Add(building, resources);
    }
}
