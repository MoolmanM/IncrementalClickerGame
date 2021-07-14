using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunters : Worker
{
    private Worker _worker;
    private bool hasEnoughResources;
    private float _timer = 0.1f;

    void Awake()
    {
        _worker = GetComponent<Worker>();
        Workers.Add(Type, _worker);
        SetInitialValues();
        _resourcesToIncrement = new ResourcesToModify[2];

        _resourcesToIncrement[0].resourceTypeToModify = ResourceType.Pelts;
        _resourcesToIncrement[0].resourceMultiplier = 0.004f;

        _resourcesToIncrement[1].resourceTypeToModify = ResourceType.Food;
        _resourcesToIncrement[1].resourceMultiplier = 0.04f;

        // This is only temporary, need to check the conditions and refresh this array whenever I craft woodenspear or stone spear etc.
        // Or any weapon for that matter.
        _resourcesToDecrement = new ResourcesToModify[1];
        _resourcesToDecrement[0].resourceTypeToModify = ResourceType.Sticks;
        _resourcesToDecrement[0].resourceMultiplier = 0.1f;

        if (Craftable.Craftables[CraftingType.WoodenSpear].isUnlocked)
        {
            _resourcesToDecrement = new ResourcesToModify[1];
            _resourcesToDecrement[0].resourceTypeToModify = ResourceType.Sticks;
            _resourcesToDecrement[0].resourceMultiplier = 0.1f;
        }
        else if (Craftable.Craftables[CraftingType.StoneSpear].isUnlocked)
        {
            _resourcesToDecrement = new ResourcesToModify[2];
            _resourcesToDecrement[0].resourceTypeToModify = ResourceType.Sticks;
            _resourcesToDecrement[0].resourceMultiplier = 0.1f;

            _resourcesToDecrement[1].resourceTypeToModify = ResourceType.Stones;
            _resourcesToDecrement[1].resourceMultiplier = 0.1f;
        }

        // Will have to update all these values whenever I craft one of the associated weapons.
        // Shouldn't be that big of a problem, just create a new array for this and the rest should continue to work after that.
    }
    public override void OnPlusButton()
    {
        if (UnassignedWorkerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                _changeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (UnassignedWorkerCount < 10)
                {
                    _changeAmount = UnassignedWorkerCount;
                }
                else
                {
                    _changeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (UnassignedWorkerCount < 100)
                {
                    _changeAmount = UnassignedWorkerCount;
                }
                else
                {
                    _changeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                _changeAmount = UnassignedWorkerCount;
            }
            UnassignedWorkerCount -= _changeAmount;
            workerCount += _changeAmount;
            txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
            txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", UnassignedWorkerCount);

            if (_resourcesToDecrement == null)
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].resourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
                }
            }
            else
            {
                if (hasEnoughResources)
                {
                    for (int i = 0; i < _resourcesToDecrement.Length; i++)
                    {
                        _resourcesToDecrement[i].incrementAmount = _changeAmount * _resourcesToDecrement[i].resourceMultiplier;
                        Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToDecrement[i].incrementAmount;
                    }
                    for (int i = 0; i < _resourcesToIncrement.Length; i++)
                    {
                        _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].resourceMultiplier;
                        Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
                    }
                }
                else
                {
                    Debug.Log("Not enough resources");
                }
            }
        }
    }
    public override void OnMinusButton()
    {
        if (workerCount > 0)
        {
            if (IncrementSelect.IsOneSelected)
            {
                _changeAmount = 1;
            }
            if (IncrementSelect.IsTenSelected)
            {
                if (workerCount < 10)
                {
                    _changeAmount = workerCount;
                }
                else
                {
                    _changeAmount = 10;
                }
            }
            if (IncrementSelect.IsHundredSelected)
            {
                if (workerCount < 100)
                {
                    _changeAmount = workerCount;
                }
                else
                {
                    _changeAmount = 100;
                }
            }
            if (IncrementSelect.IsMaxSelected)
            {
                _changeAmount = workerCount;
            }
            UnassignedWorkerCount += _changeAmount;
            workerCount -= _changeAmount;
            txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
            txtAvailableWorkers.text = string.Format("Available Workers: [{0}]", UnassignedWorkerCount);

            if (_resourcesToDecrement == null)
            {
                for (int i = 0; i < _resourcesToIncrement.Length; i++)
                {
                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].resourceMultiplier;
                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
                }
            }
            else
            {
                if (hasEnoughResources)
                {
                    for (int i = 0; i < _resourcesToIncrement.Length; i++)
                    {
                        _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].resourceMultiplier;
                        Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
                    }
                    for (int i = 0; i < _resourcesToDecrement.Length; i++)
                    {
                        _resourcesToDecrement[i].incrementAmount = _changeAmount * _resourcesToDecrement[i].resourceMultiplier;
                        Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond += _resourcesToDecrement[i].incrementAmount;
                    }
                }
                else
                {
                    // Not enough resources so nothing should change?, everything should stay at zero I think.
                }                                                            
            }
        }
    }
    private void HasEnoughResources()
    {
        // So this isn't that inefficient, since it only runs when a worker has a resource to decrement.
        // But I can't think of another solution at this moment.
        if (_resourcesToDecrement != null)
        {
            if ((_timer -= Time.deltaTime) <= 0)
            {
                _timer = 0.1f;

                for (int i = 0; i < _resourcesToDecrement.Length; i++)
                {
                    if (Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amount <= _resourcesToDecrement[i].resourceMultiplier)
                    {
                        hasEnoughResources = false;
                    }
                    else
                    {
                        hasEnoughResources = true;
                    }
                }
                if (hasEnoughResources)
                {
                    for (int i = 0; i < _resourcesToIncrement.Length; i++)
                    {
                        _resourcesToIncrement[i].hasAssignedNotEnough = false;
                    }

                    for (int i = 0; i < _resourcesToDecrement.Length; i++)
                    {
                        _resourcesToDecrement[i].hasAssignedNotEnough = false;
                    }

                    for (int i = 0; i < _resourcesToIncrement.Length; i++)
                    {
                        if (!_resourcesToIncrement[i].hasAssignedEnough)
                        {
                            _resourcesToIncrement[i].incrementAmount = 0;
                            _resourcesToIncrement[i].incrementAmount = workerCount * _resourcesToIncrement[i].resourceMultiplier;
                            Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
                            _resourcesToIncrement[i].hasAssignedEnough = true;
                        }
                    }
                    for (int i = 0; i < _resourcesToDecrement.Length; i++)
                    {
                        if (!_resourcesToDecrement[i].hasAssignedEnough)
                        {
                            _resourcesToDecrement[i].incrementAmount = 0;
                            _resourcesToDecrement[i].incrementAmount = workerCount * _resourcesToDecrement[i].resourceMultiplier;
                            Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToDecrement[i].incrementAmount;
                            _resourcesToDecrement[i].hasAssignedEnough = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _resourcesToIncrement.Length; i++)
                    {
                        _resourcesToIncrement[i].hasAssignedEnough = false;
                    }

                    for (int i = 0; i < _resourcesToDecrement.Length; i++)
                    {
                        _resourcesToDecrement[i].hasAssignedEnough = false;
                    }

                    for (int i = 0; i < _resourcesToIncrement.Length; i++)
                    {
                        if (!_resourcesToIncrement[i].hasAssignedNotEnough)
                        {
                            _resourcesToIncrement[i].incrementAmount = workerCount * _resourcesToIncrement[i].resourceMultiplier;
                            Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
                            _resourcesToIncrement[i].incrementAmount = 0;
                            _resourcesToIncrement[i].hasAssignedNotEnough = true;
                        }
                    }
                    for (int i = 0; i < _resourcesToDecrement.Length; i++)
                    {
                        if (!_resourcesToDecrement[i].hasAssignedNotEnough)
                        {
                            _resourcesToDecrement[i].incrementAmount = workerCount * _resourcesToDecrement[i].resourceMultiplier;
                            Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond += _resourcesToDecrement[i].incrementAmount;
                            _resourcesToDecrement[i].incrementAmount = 0;
                            _resourcesToDecrement[i].hasAssignedNotEnough = true;
                        }
                    }
                }
            }

            for (int i = 0; i < _resourcesToDecrement.Length; i++)
            {
                Debug.Log(_resourcesToDecrement[i].resourceTypeToModify + " " + _resourcesToDecrement[i].incrementAmount);
            }

            for (int i = 0; i < _resourcesToIncrement.Length; i++)
            {
                Debug.Log(_resourcesToIncrement[i].resourceTypeToModify + " " + _resourcesToIncrement[i].incrementAmount);
            }
        }
    }
    void Update()
    {
        HasEnoughResources();
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<WorkerType, Worker> kvp in Workers)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
}
