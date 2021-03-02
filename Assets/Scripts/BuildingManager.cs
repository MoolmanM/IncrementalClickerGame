using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static Dictionary<Building, Resource[]> _buildings = new Dictionary<Building, Resource[]>();
    public static Collector collector;
    public static void Register(Building building, Resource[] resources)
    {
        _buildings.Add(building, resources);
        building.HandleCollector(ref collector);
        Debug.Log(collector.type + " " + collector.buildingIncrementPerSecondAmount);
    }

    

        /*pseudo code here. 
        public struct Collector {
          public int a;
          public int b;
          public int c;
        }

        // building
        public override void HandleCollector(ref Collector collector) {
          collector.a += 5;
          collector.b *= 2;
          // idfk
        }

        // master
        Collector collector;

        foreach building:
          building.HandleCollector(ref collector);

        // do something with the data at the end
        */
}
