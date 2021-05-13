using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherPotatoes : MonoBehaviour
{
    public void OnGatherPotatoes()
    {
        Resource.Resources[ResourceType.Food].amount++;
    }
}
