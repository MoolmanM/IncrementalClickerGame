using System.Collections.Generic;
using UnityEngine;

// Increase a random Worker's Multiplier by a certain %.
public class uPassive2 : UncommonPassive
{
    private UncommonPassive _uncommonPassive;
    private WorkerType workerTypeChosen;
    private float permanentAmount = 0.023f, prestigeAmount = 0.115f;

    private void Awake()
    {
        _uncommonPassive = GetComponent<UncommonPassive>();
        UncommonPassives.Add(Type, _uncommonPassive);
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
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Increase the production of worker '{0}' by {1}%", Worker.Workers[workerTypeChosen].actualName, percentageAmount * 100);
    }
    private void AddToBoxCache(float percentageAmount)
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
        ChooseRandomWorker();
        ModifyStatDescription(permanentAmount);
        AddToBoxCache(permanentAmount);
    }
    public override void InitializePrestigeStat()
    {
        ChooseRandomWorker();
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButtonWorker(WorkerType workerType)
    {
        AddToBoxCache(prestigeAmount);
    }
    public override WorkerType ReturnWorkerType()
    {
        return workerTypeChosen;
    }
}
