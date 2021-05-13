using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherSticks : MonoBehaviour
{
    public void OnGatherSticks()
    {
        Resource._resources[ResourceType.Sticks].amount++;
    }
}
