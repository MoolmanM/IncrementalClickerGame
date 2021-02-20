using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static Dictionary<Building, Resource[]> _buildings = new Dictionary<Building, Resource[]>();
    

    public static void Register(Building building, Resource[] resources)
    {
        _buildings.Add(building, resources);
    }
}
