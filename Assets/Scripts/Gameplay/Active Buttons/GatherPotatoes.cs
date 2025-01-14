using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherPotatoes : MonoBehaviour
{
    public void OnGatherPotatoes()
    {
        Resource.Resources[ResourceType.Food].amount++;
        Resource.Resources[ResourceType.Food].trackedAmount++;
        Resource.Resources[ResourceType.Food].uiForResource.txtAmount.text = string.Format("{0:0.00}", NumberToLetter.FormatNumber(Resource.Resources[ResourceType.Food].amount));
    }
}
