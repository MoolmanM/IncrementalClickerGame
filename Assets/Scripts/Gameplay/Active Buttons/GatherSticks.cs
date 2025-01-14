using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherSticks : MonoBehaviour
{
    public void OnGatherSticks()
    {
        Resource.Resources[ResourceType.Lumber].amount++;
        Resource.Resources[ResourceType.Lumber].trackedAmount++;
        Resource.Resources[ResourceType.Lumber].uiForResource.txtAmount.text = string.Format("{0:0.00}", NumberToLetter.FormatNumber(Resource.Resources[ResourceType.Lumber].amount));
    }
}
