using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWorker : MonoBehaviour
{
    public static uint AutoWorkerAmount, AutoRemainderAmount, TotalWorkerJobs, FinalWorkerCount;
    public static float DecrementAmount, IncrementAmount;

    public static List<WorkerType> workerList = new List<WorkerType>();

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
        foreach(var worker in Worker.Workers)
        {
            uint currentWorkerCount = worker.Value.workerCount;

            if (worker.Value.workerCount <= AutoWorkerAmount & AutoRemainderAmount == 2)
            {
                Worker.Workers[WorkerType.Farmers].workerCount = AutoWorkerAmount + 1;
                Worker.Workers[WorkerType.Woodcutters].workerCount = AutoWorkerAmount + 1;
            }
            else if (worker.Value.workerCount <= AutoWorkerAmount & AutoRemainderAmount == 1)
            {
                Worker.Workers[WorkerType.Farmers].workerCount = AutoWorkerAmount + 1;
            }
            else if (worker.Value.workerCount <= AutoWorkerAmount & AutoRemainderAmount == 0)
            {
                worker.Value.workerCount = AutoWorkerAmount;
            }

            uint wantedWorkerCount = worker.Value.workerCount;

            if (wantedWorkerCount > currentWorkerCount)
            {
                uint differenceAmount = wantedWorkerCount - currentWorkerCount;
                IncrementAmount = (differenceAmount * worker.Value.resourceMultiplier);
                Resource.Resources[worker.Value.resourceTypeToModify].amountPerSecond += IncrementAmount;
            }
            else if (wantedWorkerCount < currentWorkerCount)
            {
                uint differenceAmount = currentWorkerCount - wantedWorkerCount;
                DecrementAmount = (differenceAmount * worker.Value.resourceMultiplier);
                Resource.Resources[worker.Value.resourceTypeToModify].amountPerSecond -= IncrementAmount;
            }
 
            Worker.UnassignedWorkerCount = 0;
            worker.Value.txtHeader.text = string.Format("{0} [{1}]", worker.Value.Type.ToString(), worker.Value.workerCount);
            worker.Value.txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", Worker.UnassignedWorkerCount);
        }              
    }
}
