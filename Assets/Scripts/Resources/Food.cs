using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private float _timer, _timerForResources, _timerForDayIncrement;
    public float foodAmount;

    private void Start()
    {
        _timer = 0f;
        _timerForDayIncrement = 5f;
        _timerForResources = 0.1f;
    }
    private void Update()
    {
        if ((_timerForDayIncrement -= Time.deltaTime) <= 0)
        {
            _timer = _timerForResources;
            foodAmount += (float)0.15;
        }


    }
}