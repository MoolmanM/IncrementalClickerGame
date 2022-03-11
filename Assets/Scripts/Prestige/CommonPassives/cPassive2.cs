using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Increase a random Worker's Multiplier by a certain %.
public class cPassive2 : CommonPassive
{
    private CommonPassive _commonPassive;
    private WorkerType workerTypeChosen;
    private float percentageAmount = 0.01f; //1%

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
    }
    private void ChooseRandomWorker()
    {      
        List<WorkerType> workerTypesInCurrentRun = new List<WorkerType>();

        foreach (var worker in Worker.Workers)
        {
            if (worker.Value.isUnlocked)
            {
                workerTypesInCurrentRun.Add(worker.Key);
            }
        }
        if (workerTypesInCurrentRun.Count >= Prestige.workersUnlockedInPreviousRun.Count)
        {
            _index = Random.Range(0, workerTypesInCurrentRun.Count);
            workerTypeChosen = workerTypesInCurrentRun[_index];
        }
        else
        {
            _index = Random.Range(0, Prestige.workersUnlockedInPreviousRun.Count);
            workerTypeChosen = Prestige.workersUnlockedInPreviousRun[_index];
        }

        description = string.Format("Increase the production of {0} by {1}%", Worker.Workers[workerTypeChosen].actualName, percentageAmount*100);
        AddToBoxCache();
    }
    private void AddToBoxCache()
    {
        if (!BoxCache.cachedWorkerMultiplierModified.ContainsKey(workerTypeChosen))
        {
            BoxCache.cachedWorkerMultiplierModified.Add(workerTypeChosen, percentageAmount);
        }
        else
        {
            BoxCache.cachedWorkerMultiplierModified[workerTypeChosen] += percentageAmount;
        }
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        ChooseRandomWorker();
    }
}
