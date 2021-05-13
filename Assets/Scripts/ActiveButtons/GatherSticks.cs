using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherSticks : MonoBehaviour
{
    public void OnGatherSticks()
    {
        Resource.Resources[ResourceType.Sticks].amount++;
    }
}
