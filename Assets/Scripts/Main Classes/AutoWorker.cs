using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWorker : MonoBehaviour
{
    public static uint AutoWorkerAmount, AutoRemainderAmount, TotalWorkerJobs;

    public static void CalculateWorkers()
    {
        foreach (var actualWorker in Worker.Workers)
        {
            if (actualWorker.Value.isUnlocked == 1)
            {
                AutoWorkerAmount = Worker.TotalWorkerCount / TotalWorkerJobs;
                AutoRemainderAmount = Worker.TotalWorkerCount % TotalWorkerJobs;
            }
        }
         //Debug.Log("Total Jobs: " + TotalWorkerJobs + ", Total Worker Count " + Worker.TotalWorkerCount);
         //Debug.Log("Auto worker amount " + AutoWorkerAmount + ", Auto Remainder Amount " + AutoRemainderAmount);
    }
    public static void AutoAssignWorkers()
    {
        foreach (var actualWorker in Worker.Workers)
        {
            if (actualWorker.Value.isUnlocked == 1)
            {
                uint currentWorkerCount = actualWorker.Value.workerCount;
                              
                actualWorker.Value.workerCount = AutoWorkerAmount;

                if (AutoRemainderAmount == 2)
                {
                    Worker.Workers[WorkerType.Farmers].workerCount = AutoWorkerAmount + 1;
                    Worker.Workers[WorkerType.Woodcutters].workerCount = AutoWorkerAmount + 1;
                }
                else if (AutoRemainderAmount == 1)
                {
                    Worker.Workers[WorkerType.Farmers].workerCount = AutoWorkerAmount + 1;
                }
                
                uint wantedWorkerCount = actualWorker.Value.workerCount;

                if (wantedWorkerCount < currentWorkerCount)
                {
                    uint smallerWorkerCount = currentWorkerCount - wantedWorkerCount;
                    actualWorker.Value.amountToIncreasePerSecondBy = (smallerWorkerCount * actualWorker.Value.resourceMultiplier);
                    Resource.Resources[actualWorker.Value.resourceTypeToModify].amountPerSecond -= actualWorker.Value.amountToIncreasePerSecondBy;
                }
                else if(wantedWorkerCount > currentWorkerCount)
                {
                    uint finalWorkerCount = wantedWorkerCount - currentWorkerCount;
                    actualWorker.Value.amountToIncreasePerSecondBy = (finalWorkerCount * actualWorker.Value.resourceMultiplier);
                    Resource.Resources[actualWorker.Value.resourceTypeToModify].amountPerSecond += actualWorker.Value.amountToIncreasePerSecondBy;
                }
                else if (wantedWorkerCount == currentWorkerCount)
                {
                    Debug.Log("They are equal.");
                    //uint finalWorkerCount = actualWorker.Value.workerCount - currentWorkerCount;
                    //actualWorker.Value.AmountToIncreasePerSecondBy = (finalWorkerCount * actualWorker.Value.ResourceMultiplier);
                    //Resource.Resources[actualWorker.Value.resourceTypeToModify].amountPerSecond += actualWorker.Value.AmountToIncreasePerSecondBy;
                }
                else
                {
                    Debug.Log("Else happened");
                }

                // Seems the main issue with either the idea of using the foreach because it goes through every worker script once even though it just modifies the workercount of one
                // Or it's with the script execution itself. But this seems more like just an underlying issue of the problem above.

                //Debug.Log("Worker Type: " + actualWorker.Key + " ResourceMulti: " + actualWorker.Value.resourceMultiplier + " Worker Count: " + actualWorker.Value.workerCount + " Resource amount per second: " + actualWorker.Value.amountToIncreasePerSecondBy + " Increment Amount: " + actualWorker.Value.amountToIncreasePerSecondBy);
                //Debug.Log("Worker Type: " + actualWorker.Key + " Worker Count " + );
                Worker.UnassignedWorkerCount = 0;
                actualWorker.Value.txtHeader.text = string.Format("{0} [{1}]", actualWorker.Value.Type.ToString(), actualWorker.Value.workerCount);
                actualWorker.Value.txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", Worker.UnassignedWorkerCount);
            }
        }     
    }
}
