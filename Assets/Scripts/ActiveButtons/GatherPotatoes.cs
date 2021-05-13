using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherPotatoes : MonoBehaviour
{
    public void OnGatherPotatoes()
    {
        Resource._resources[ResourceType.Food].amount++;
    }
}
