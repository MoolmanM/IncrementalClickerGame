using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunters : Worker
{
    private Worker _worker;
    private bool hasEnoughResources;
    private float timer = 0.1f;

    void Awake()
    {
        _worker = GetComponent<Worker>();
        Workers.Add(Type, _worker);
        SetInitialValues();
    }
    //protected override void OnPlusButton()
    //{
    //    if (UnassignedWorkerCount > 0)
    //    {
    //        if (IncrementSelect.IsOneSelected)
    //        {
    //            _changeAmount = 1;
    //        }
    //        if (IncrementSelect.IsTenSelected)
    //        {
    //            if (UnassignedWorkerCount < 10)
    //            {
    //                _changeAmount = UnassignedWorkerCount;
    //            }
    //            else
    //            {
    //                _changeAmount = 10;
    //            }
    //        }
    //        if (IncrementSelect.IsHundredSelected)
    //        {
    //            if (UnassignedWorkerCount < 100)
    //            {
    //                _changeAmount = UnassignedWorkerCount;
    //            }
    //            else
    //            {
    //                _changeAmount = 100;
    //            }
    //        }
    //        if (IncrementSelect.IsMaxSelected)
    //        {
    //            _changeAmount = UnassignedWorkerCount;
    //        }
    //        UnassignedWorkerCount -= _changeAmount;
    //        workerCount += _changeAmount;
    //        txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
    //        txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", UnassignedWorkerCount);

    //        if (_resourcesToDecrement == null)
    //        {
    //            for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //            {
    //                _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].currentResourceMultiplier;
    //                Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
    //                Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond);
    //            }
    //        }
    //        else
    //        {
    //            if (hasEnoughResources)
    //            {
    //                for (int i = 0; i < _resourcesToDecrement.Length; i++)
    //                {
    //                    _resourcesToDecrement[i].incrementAmount = _changeAmount * _resourcesToDecrement[i].currentResourceMultiplier;
    //                    Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToDecrement[i].incrementAmount;
    //                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond);
    //                }
    //                for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //                {
    //                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].currentResourceMultiplier;
    //                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
    //                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].uiForResource.txtAmountPerSecond.text = string.Format("+{0:0.00}/sec", Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond);
    //                }
    //            }
    //            else
    //            {
    //                Debug.Log("Not enough resources");
    //            }
    //        }
    //    }
    //    UpdateResourceInfo();
    //}
    //public override void OnMinusButton()
    //{
    //    if (workerCount > 0)
    //    {
    //        if (IncrementSelect.IsOneSelected)
    //        {
    //            _changeAmount = 1;
    //        }
    //        if (IncrementSelect.IsTenSelected)
    //        {
    //            if (workerCount < 10)
    //            {
    //                _changeAmount = workerCount;
    //            }
    //            else
    //            {
    //                _changeAmount = 10;
    //            }
    //        }
    //        if (IncrementSelect.IsHundredSelected)
    //        {
    //            if (workerCount < 100)
    //            {
    //                _changeAmount = workerCount;
    //            }
    //            else
    //            {
    //                _changeAmount = 100;
    //            }
    //        }
    //        if (IncrementSelect.IsMaxSelected)
    //        {
    //            _changeAmount = workerCount;
    //        }
    //        UnassignedWorkerCount += _changeAmount;
    //        workerCount -= _changeAmount;
    //        txtHeader.text = string.Format("{0} [{1}]", Type.ToString(), workerCount);
    //        txtAvailableWorkers.text = string.Format("Available Workers: [<color=#FFCBFA>{0}</color>]", UnassignedWorkerCount);

    //        if (_resourcesToDecrement == null)
    //        {
    //            for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //            {
    //                _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].currentResourceMultiplier;
    //                Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
    //            }
    //        }
    //        else
    //        {
    //            if (hasEnoughResources)
    //            {
    //                for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //                {
    //                    _resourcesToIncrement[i].incrementAmount = _changeAmount * _resourcesToIncrement[i].currentResourceMultiplier;
    //                    Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
    //                }
    //                for (int i = 0; i < _resourcesToDecrement.Length; i++)
    //                {
    //                    _resourcesToDecrement[i].incrementAmount = _changeAmount * _resourcesToDecrement[i].currentResourceMultiplier;
    //                    Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond += _resourcesToDecrement[i].incrementAmount;
    //                }
    //            }
    //            else
    //            {
    //                // Not enough resources so nothing should change?, everything should stay at zero I think.
    //            }                                                            
    //        }
    //    }
    //    UpdateResourceInfo();
    //}
    //private void HasEnoughResources()
    //{
    //    // So this isn't that inefficient, since it only runs when a worker has a resource to decrement.
    //    // But I can't think of another solution at this moment.
    //    if (_resourcesToDecrement != null)
    //    {
    //        if ((timer -= Time.deltaTime) <= 0)
    //        {
    //            timer = 0.1f;

    //            for (int i = 0; i < _resourcesToDecrement.Length; i++)
    //            {
    //                if (Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amount <= _resourcesToDecrement[i].currentResourceMultiplier)
    //                {
    //                    hasEnoughResources = false;
    //                }
    //                else
    //                {
    //                    hasEnoughResources = true;
    //                }
    //            }
    //            if (hasEnoughResources)
    //            {
    //                for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //                {
    //                    _resourcesToIncrement[i].hasAssignedNotEnough = false;
    //                }

    //                for (int i = 0; i < _resourcesToDecrement.Length; i++)
    //                {
    //                    _resourcesToDecrement[i].hasAssignedNotEnough = false;
    //                }

    //                for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //                {
    //                    if (!_resourcesToIncrement[i].hasAssignedEnough)
    //                    {
    //                        _resourcesToIncrement[i].incrementAmount = 0;
    //                        _resourcesToIncrement[i].incrementAmount = workerCount * _resourcesToIncrement[i].currentResourceMultiplier;
    //                        Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond += _resourcesToIncrement[i].incrementAmount;
    //                        _resourcesToIncrement[i].hasAssignedEnough = true;
    //                    }
    //                }
    //                for (int i = 0; i < _resourcesToDecrement.Length; i++)
    //                {
    //                    if (!_resourcesToDecrement[i].hasAssignedEnough)
    //                    {
    //                        _resourcesToDecrement[i].incrementAmount = 0;
    //                        _resourcesToDecrement[i].incrementAmount = workerCount * _resourcesToDecrement[i].currentResourceMultiplier;
    //                        Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToDecrement[i].incrementAmount;
    //                        _resourcesToDecrement[i].hasAssignedEnough = true;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //                {
    //                    _resourcesToIncrement[i].hasAssignedEnough = false;
    //                }

    //                for (int i = 0; i < _resourcesToDecrement.Length; i++)
    //                {
    //                    _resourcesToDecrement[i].hasAssignedEnough = false;
    //                }

    //                for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //                {
    //                    if (!_resourcesToIncrement[i].hasAssignedNotEnough)
    //                    {
    //                        _resourcesToIncrement[i].incrementAmount = workerCount * _resourcesToIncrement[i].currentResourceMultiplier;
    //                        Resource.Resources[_resourcesToIncrement[i].resourceTypeToModify].amountPerSecond -= _resourcesToIncrement[i].incrementAmount;
    //                        _resourcesToIncrement[i].incrementAmount = 0;
    //                        _resourcesToIncrement[i].hasAssignedNotEnough = true;
    //                    }
    //                }
    //                for (int i = 0; i < _resourcesToDecrement.Length; i++)
    //                {
    //                    if (!_resourcesToDecrement[i].hasAssignedNotEnough)
    //                    {
    //                        _resourcesToDecrement[i].incrementAmount = workerCount * _resourcesToDecrement[i].currentResourceMultiplier;
    //                        Resource.Resources[_resourcesToDecrement[i].resourceTypeToModify].amountPerSecond += _resourcesToDecrement[i].incrementAmount;
    //                        _resourcesToDecrement[i].incrementAmount = 0;
    //                        _resourcesToDecrement[i].hasAssignedNotEnough = true;
    //                    }
    //                }
    //            }
    //        }

    //        //for (int i = 0; i < _resourcesToDecrement.Length; i++)
    //        //{
    //        //    Debug.Log(_resourcesToDecrement[i].resourceTypeToModify + " " + _resourcesToDecrement[i].incrementAmount);
    //        //}

    //        //for (int i = 0; i < _resourcesToIncrement.Length; i++)
    //        //{
    //        //    Debug.Log(_resourcesToIncrement[i].resourceTypeToModify + " " + _resourcesToIncrement[i].incrementAmount);
    //        //}
    //    }
    //}
    //void Update()
    //{
    //    HasEnoughResources();
    //}
}
